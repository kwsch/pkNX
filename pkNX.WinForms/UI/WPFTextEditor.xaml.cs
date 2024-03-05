using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using pkNX.Containers;
using pkNX.Containers.VFS;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms;

/// <summary>
/// Interaction logic for WPFTextEditor.xaml
/// </summary>
public partial class WPFTextEditor
{
    public record TextEditorEntry : INotifyPropertyChanged
    {
        private string _variable;

        public int Line { get; init; }
        public string Variable
        {
            get => _variable;
            set
            {
                _variable = value;
                Hash = FnvHash.HashFnv1a_64(_variable);
                OnPropertyChanged();
                OnPropertyChanged(nameof(Hash));
            }
        }
        public ulong Hash { get; private set; }
        public string Text { get; set; }

        public bool IsReadOnly { get; }

        public TextEditorEntry(int line, string variable, ulong hash, string text, bool isLastRow = false)
        {
            Line = line;
            _variable = variable;
            Hash = hash;
            Text = text;
            IsReadOnly = isLastRow;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public ObservableCollection<TextEditorEntry> Entries { get; } = [];

    public enum TextEditorMode
    {
        Common,
        Script,
    }

    private readonly TextEditorMode Mode;

    public bool Modified { get; set; }
    public int LoadedFileIndex { get; set; } = -1;

    public IReadOnlyList<string> FileNames { get; init; }
    private IReadOnlyList<VirtualFile> TextFiles { get; }
    private IReadOnlyList<VirtualFile> TableFiles { get; }

    public readonly TextConfig? Config;

    public WPFTextEditor(TextEditorMode mode, TextConfig? config = null)
    {
        Mode = mode;
        Config = config;

        var path = GamePath.GetDirectoryPath(mode == TextEditorMode.Common ? GameFile.GameText : GameFile.StoryText);
        var files = VirtualFileSystem.Current.GetFiles(path);

        var fileNames = new List<string>();
        var textFiles = new List<VirtualFile>();
        var variableFiles = new List<VirtualFile>();

        foreach (var file in files)
        {
            var (name, ext) = file.GetFileNameAndExtension();

            switch (ext)
            {
                case "dat":
                    textFiles.Add(file);
                    fileNames.Add(name); // Note: We should only add the file name once. Since each .dat file has a .tbl file with the same name, we can just do this once here.
                    break;
                case "tbl":
                    variableFiles.Add(file);
                    break;
            }
        }

        FileNames = fileNames;
        TableFiles = variableFiles;
        TextFiles = textFiles;

        InitializeComponent();
        var settings = ProgramSettings.LoadSettings();
        CHK_ShowHashes.IsChecked = settings.DisplayAdvanced;

        CB_Entry.SelectedIndex = 0;
    }

    private bool ImportTextFiles(string fileName)
    {
        static bool ValidateSeparatorIndex(int nextSeparator, int lineNumber, ReadOnlySpan<char> line)
        {
            if (nextSeparator != -1)
                return true;

            WinFormsUtil.Error($"Invalid Line @ {lineNumber}, expected '|', but was not found near '{line}'");
            return false;
        }

        bool forceImport = false;

        if (Path.GetFileNameWithoutExtension(fileName) != FileNames[LoadedFileIndex])
        {
            if (WinFormsUtil.Warn(MessageBoxButton.YesNo,
                "The filename does not match the currently selected file. Using the wrong data will result in game crashes!",
                "Import anyway?") != MessageBoxResult.Yes)
            {
                return false;
            }
            forceImport = true;
        }

        string[] fileText = File.ReadAllLines(fileName, Encoding.Unicode);

        List<TextEditorEntry> entries = [];

        for (int i = 0; i < fileText.Length; i++)
        {
            ReadOnlySpan<char> line = fileText[i].AsSpan();

            // Check each line and make sure it starts with '|', if not that line should be appended to the text value of the last line
            if (!line.StartsWith("|"))
            {
                var lastEntry = entries[^1];
                entries[^1] = lastEntry with { Text = lastEntry.Text + "\\n" + line.ToString() };
                continue;
            }

            line = line[1..]; // Skip the first '|'

            int nextSeparator = line.IndexOf('|');
            if (!ValidateSeparatorIndex(nextSeparator, i, line))
                return false;

            string variable = line[..nextSeparator].ToString();
            line = line[(nextSeparator + 1)..]; // Skip over the variable and the second '|'

            nextSeparator = line.IndexOf('|');
            if (!ValidateSeparatorIndex(nextSeparator, i, line))
                return false;

            // Skip the first two characters '0x'
            if (!ulong.TryParse(line[2..nextSeparator], NumberStyles.HexNumber, null, out var hash))
            {
                hash = FnvHash.HashFnv1a_64(variable);
            }

            line = line[(nextSeparator + 1)..]; // Skip over the hash and the third '|'

            string text = line.ToString(); // The rest of the line is the text

            entries.Add(new TextEditorEntry(entries.Count, variable, hash, text));
        }

        if (!forceImport && Entries.Count != entries.Count)
        {
            if (WinFormsUtil.Prompt(MessageBoxButton.YesNo,
                $"The number of lines imported ({entries.Count}), does not match the number of lines in editor ({Entries.Count}). This might be okay if you intend to add additional entries.",
                "Import anyway?") != MessageBoxResult.Yes)
            {
                return false;
            }
        }

        Entries.Clear();
        foreach (var entry in entries)
            Entries.Add(entry);

        return true;
    }

    public void ExportTextFile(string fileName, bool replaceNewline)
    {
        using var ms = new MemoryStream();
        ms.Write([0xFF, 0xFE], 0, 2); // Write Unicode BOM
        using (TextWriter tw = new StreamWriter(ms, new UnicodeEncoding()))
        {
            foreach (var entry in Entries)
            {
                var textString = entry.Text;

                if (replaceNewline)
                {
                    textString = textString.Replace("\\n", "\n");
                }

                tw.WriteLine($"|{entry.Variable}|0x{entry.Hash:X16}|{textString}");
            }
        }
        File.WriteAllBytes(fileName, ms.ToArray());
    }

    public void InsertEntry(int index)
    {
        if (Entries.Count == 0 || index < 0 || index >= Entries.Count - 1)
        {
            Entries.Add(new TextEditorEntry(Entries.Count, "", 0, ""));
            DG_Text.SelectedIndex = Entries.Count - 1;
            return;
        }

        bool ctrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) > 0;

        if (index != 0 && !ctrlPressed)
        {
            if (WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Inserting in between rows will shift all subsequent lines.", "Continue? (Hold ctrl to ignore prompt)") != MessageBoxResult.Yes)
                return;
        }
        // Insert new Row after current row.
        int nextLine = index + 1;
        Entries.Insert(nextLine, new TextEditorEntry(nextLine, "", 0, ""));
        DG_Text.SelectedIndex = nextLine;

        for (int i = nextLine + 1; i < Entries.Count; i++)
            Entries[i] = Entries[i] with { Line = i };
    }

    public void RemoveEntry(int index)
    {
        if (Entries.Count == 0 || index < 0 || index >= Entries.Count)
        {
            return;
        }

        bool ctrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) > 0;

        if (index < Entries.Count - 1 && !ctrlPressed)
        {
            if (WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Deleting a row above other lines will shift all subsequent lines.", "Continue? (Hold ctrl to ignore prompt)") != MessageBoxResult.Yes)
                return;
        }

        Entries.RemoveAt(index);

        for (int i = index; i < Entries.Count; i++)
            Entries[i] = Entries[i] with { Line = i };
    }

    public void LoadSelectedFile()
    {
        LoadedFileIndex = CB_Entry.SelectedIndex;
        Entries.Clear();

        var textFile = TextFiles[LoadedFileIndex];
        var tblFile = TableFiles[LoadedFileIndex];

        var lines = new TextFile(textFile.ReadAllBytes(), Config).Lines;
        var tbl = new AHTB(tblFile.Open());

        // The table has 1 more entry than the dat to show when the table ends
        if (tbl.Entries.Length != lines.Length + 1)
        {
            var result = WinFormsUtil.Warn(MessageBoxButton.YesNo, "Data corruption detected!", "The number of labels in the table does not match the number of lines in the text file.", "Do you wish to restore the original file?");
            if (result != MessageBoxResult.Yes)
                return;

            textFile.Delete(DeleteMode.TopMostWriteableLayer);
            tblFile.Delete(DeleteMode.TopMostWriteableLayer);

            lines = new TextFile(textFile.ReadAllBytes(), Config).Lines; // Reopen the original file
            tbl = new AHTB(tblFile.Open()); // Reopen the original file

            Debug.Assert(tbl.Entries.Length == lines.Length + 1);
        }

        for (int i = 0; i < tbl.Count; ++i)
        {
            var label = tbl.Entries[i];
            bool isLast = i == tbl.Count - 1;
            var text = isLast ? "" : lines[i];
            Entries.Add(new TextEditorEntry(i, label.Name, label.Hash, text, isLast));
        }
    }

    public void SaveCurrentFile()
    {
        if (!Modified)
            return;

        var textBytes = TextFile.GetBytes(Entries.SkipLast(1).Select(x => x.Text), Config);
        TextFiles[LoadedFileIndex].WriteAllBytes(textBytes);

        AHTB tbl = new(Entries.ToDictionary(x => x.Hash, y => y.Variable));
        TableFiles[LoadedFileIndex].WriteAllBytes(tbl);

        Modified = false;
    }

    public void PromptSaveCurrentFile(CancelEventArgs e)
    {
        if (!Modified)
            return;

        var result = WinFormsUtil.Prompt(MessageBoxButton.YesNoCancel, "Would you like to save your changes?");
        switch (result)
        {
            case MessageBoxResult.Cancel:
                e.Cancel = true;
                break;
            case MessageBoxResult.Yes:
                SaveCurrentFile();
                break;
        }
    }

    private void CB_Entry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CB_Entry.SelectedIndex == LoadedFileIndex)
            return;

        CancelEventArgs args = new();
        PromptSaveCurrentFile(args);

        if (args.Cancel)
        {
            CB_Entry.SelectedIndex = LoadedFileIndex;
            return;
        }

        LoadSelectedFile();
    }

    private void B_AddLine_Click(object sender, RoutedEventArgs e)
    {
        InsertEntry(DG_Text.SelectedIndex);
        DG_Text.ScrollIntoView(DG_Text.SelectedItem);
    }

    private void B_RemoveLine_Click(object sender, RoutedEventArgs e)
    {
        RemoveEntry(DG_Text.SelectedIndex);
    }

    private void B_Export_Click(object sender, RoutedEventArgs e)
    {
        if (Entries.Count == 0)
            return;

        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
        {
            BatchExport();
            return;
        }

        using var dump = new System.Windows.Forms.SaveFileDialog();
        dump.Filter = @"Text File|*.txt";
        dump.FileName = FileNames[LoadedFileIndex] + ".txt";
        if (dump.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;

        var result = WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Would you like to unescape newline characters?");
        bool newline = result == MessageBoxResult.Yes;
        string path = dump.FileName;
        ExportTextFile(path, newline);

        System.Media.SystemSounds.Asterisk.Play();
    }

    private void BatchExport()
    {
        using var dumpFolder = new System.Windows.Forms.FolderBrowserDialog();
        dumpFolder.Description = "Select export folder";
        dumpFolder.UseDescriptionForTitle = true;
        dumpFolder.ShowNewFolderButton = true;

        if (dumpFolder.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;

        // If not directory is empty, prompt to replace all files
        if (Directory.EnumerateFiles(dumpFolder.SelectedPath).Any())
        {
            var replaceResult = WinFormsUtil.Prompt(MessageBoxButton.YesNoCancel, "The selected folder is not empty. Would you like to continue and replace all text files?");
            if (replaceResult == MessageBoxResult.Cancel)
                return;
        }

        var result = WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Would you like to unescape newline characters?");
        bool newline = result == MessageBoxResult.Yes;

        foreach (var fileName in FileNames)
        {
            string path = Path.Combine(dumpFolder.SelectedPath, fileName + ".txt");
            ExportTextFile(path, newline);
        }

        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Import_Click(object sender, RoutedEventArgs e)
    {
        using var dump = new System.Windows.Forms.OpenFileDialog();
        dump.Filter = @"Text File|*.txt";
        dump.FileName = FileNames[LoadedFileIndex] + ".txt";
        if (dump.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;

        string path = dump.FileName;
        if (!ImportTextFiles(path))
            return;

        System.Media.SystemSounds.Asterisk.Play();
    }

    private void DG_Text_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.EditAction == DataGridEditAction.Cancel)
            return;

        var entry = (TextEditorEntry)e.Row.Item;
        var text = ((TextBox)e.EditingElement).Text;

        switch (e.Column.DisplayIndex)
        {
            case 1:
                Modified |= entry.Variable != text;
                break;
            case 3:
                Modified |= entry.Text != text;
                break;
        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        PromptSaveCurrentFile(e);
    }

    private void B_Save_Click(object sender, RoutedEventArgs e)
    {
        SaveCurrentFile();
    }
}
