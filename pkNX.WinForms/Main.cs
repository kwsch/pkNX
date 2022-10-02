using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Sprites;
using pkNX.Structures;
using EditorBase = pkNX.WinForms.Controls.EditorBase;

namespace pkNX.WinForms;

public partial class Main : Form
{
    private const int ButtonWidth = 120;
    private const int ButtonHeight = 40;
    private const int ButtonPadding = 3;

    public static readonly string ProgramSettingsPath = Path.Combine(Application.StartupPath, "settings.json");
    public ProgramSettings Settings { get; }

    private int Language
    {
        get => CB_Lang.SelectedIndex;
        set => CB_Lang.SelectedIndex = value;
    }

    private EditorBase? Editor;

    public Main()
    {
        InitializeComponent();

        Settings = SettingsSerializer.GetSettings<ProgramSettings>(ProgramSettingsPath).Result;
        CB_Lang.SelectedIndex = Settings.Language;
        if (!string.IsNullOrWhiteSpace(Settings.GamePath))
            OpenPath(Settings.GamePath, Settings.GameOverride);

        DragDrop += (s, e) =>
        {
            var files = (string[]?)e.Data?.GetData(DataFormats.FileDrop);
            if (files is null)
                return;
            foreach (var f in files)
                OpenPath(f);
        };
        DragEnter += (s, e) =>
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
                e.Effect = DragDropEffects.Copy;
        };
    }

    private void ChangeLanguage(object sender, EventArgs e)
    {
        Menu_Options.DropDown.Close();
        if (Editor == null)
            return;

        if (Editor.Game.GetGeneration() < 7 && Language > 7)
        {
            WinFormsUtil.Alert("Selected Language is not available for this game", "Defaulting to English.");
            CB_Lang.SelectedIndex = 2;
            return;
        }
        Editor.Language = Language;
    }

    private void Menu_Open_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        if (fbd.ShowDialog() == DialogResult.OK)
            OpenPath(fbd.SelectedPath);
    }

    private async void Main_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Editor == null)
            return;

        Editor.Close();
        EditUtil.SaveSettings(Editor.Game);
        Settings.Language = CB_Lang.SelectedIndex;
        Settings.GamePath = TB_Path.Text;
        Settings.GameOverride = Editor.Game;
        await SettingsSerializer.SaveSettings(Settings, ProgramSettingsPath);
    }

    private void Menu_Exit_Click(object sender, EventArgs e)
    {
        if (ModifierKeys == Keys.Control) // triggered via hotkey
        {
            if (DialogResult.Yes != WinFormsUtil.Prompt(MessageBoxButtons.YesNo, $"Quit {nameof(pkNX)}?"))
                return;
        }
        Close();
    }

    private void Menu_SetRNGSeed_Click(object sender, EventArgs e)
    {
        var result = WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Reseed RNG?",
            "If yes, copy the 32 bit (not hex) integer seed to the clipboard before hitting Yes.");
        if (DialogResult.Yes != result)
            return;

        string val = GetClipboardTextString();
        if (int.TryParse(val, out int seed))
        {
            Util.Rand = new Random(seed);
            WinFormsUtil.Alert($"Reseeded RNG to seed: {seed}");
            return;
        }
        WinFormsUtil.Alert("Unable to set seed.");
    }

    private static string GetClipboardTextString()
    {
        try
        {
            return Clipboard.GetText();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to read clipboard text: {ex.Message}");
            return string.Empty;
        }
    }

    private void OpenPath(string path, GameVersion gameOverride = GameVersion.Any)
    {
        try
        {
            if (Directory.Exists(path))
                OpenFolder(path, gameOverride);
            else
                OpenFile(path);
        }
        catch (Exception ex)
        {
            WinFormsUtil.Error($"Failed to open -- {path}", ex.Message);
        }
    }

    private static void OpenFile(string path)
    {
        var result = FileRipper.TryOpenFile(path);
        if (result.Code != RipResultCode.Success || result.ResultPath is not { } resultPath)
        {
            WinFormsUtil.Alert("Invalid file loaded." + Environment.NewLine + $"Unable to recognize data: {result.Code}.", path);
            return;
        }

        System.Media.SystemSounds.Asterisk.Play();
        Process.Start("explorer.exe", resultPath);
    }

    private void OpenFolder(string path, GameVersion gameOverride)
    {
        var editor = EditorBase.GetEditor(path, Language, gameOverride);
        if (editor == null)
        {
            var msg = "Invalid folder loaded." + Environment.NewLine + "Unable to recognize game data.";
            WinFormsUtil.Alert(msg, path);
            return;
        }

        try
        {
            editor.Initialize();
            LoadROM(editor);
        }
        catch (Exception ex)
        {
            var msg = "Failed to initialize ROM data." + Environment.NewLine +
                      "Please ensure your dump is correctly set up, with updated patches merged in (if applicable).";
            var stack = ex.StackTrace ?? string.Empty;
            WinFormsUtil.Error(msg, ex.Message, stack);
        }
    }

    private void LoadEditorButtons(EditorCategory category = EditorCategory.None)
    {
        FLP_Controls.SuspendLayout();
        FLP_Controls.Controls.Clear();

        if (category == EditorCategory.None)
        {
            foreach (var c in (EditorCategory[])Enum.GetValues(typeof(EditorCategory)))
            {
                if (c == EditorCategory.None)
                    continue;

                if (Editor!.CountControlsForCategory(c) == 0)
                    continue;

                FLP_Controls.Controls.Add(CreateCategoryButton(c));
            }
        }
        else
        {
            // Create back button
            FLP_Controls.Controls.Add(CreateCategoryButton(EditorCategory.None));
        }

        AddEditorButtonsForCategory(category);

        AdjustWindowSize();
    }

    private void AddEditorButtonsForCategory(EditorCategory category)
    {
        var ctrls = Editor!.GetControls(ButtonWidth, ButtonHeight, category).OrderBy(x => x.Text);

        foreach (var ctrl in ctrls)
            FLP_Controls.Controls.Add(ctrl);
    }

    public Button CreateCategoryButton(EditorCategory category)
    {
        var b = new Button
        {
            Width = ButtonWidth,
            Height = ButtonHeight,
            Name = $"B_OpenCategory{category}",
            Text = ((category == EditorCategory.None) ? "Back" : $"Show {category} Editors"),
        };
        b.Click += (s, e) =>
        {
            LoadEditorButtons(category);
        };
        return b;
    }

    private void AdjustWindowSize()
    {
        const int wp = ButtonWidth + (2 * ButtonPadding);
        const int hp = ButtonHeight + (2 * ButtonPadding);
        const int area = wp * hp;
        var count = FLP_Controls.Controls.Count;
        // Resize form dimensions then center to screen, so that all buttons are shown.
        var totalArea = count * area;
        var squareSide = Math.Sqrt(totalArea);
        var columns = (int)Math.Ceiling(squareSide / wp) + 1;
        var rows = (count / columns) + 2;

        Size newClientSize = new(columns * wp, FLP_Controls.Location.Y + (rows * hp));

        // Only grow in size
        if (ClientSize.Width < newClientSize.Width || ClientSize.Height < newClientSize.Height)
            ClientSize = newClientSize;

        FLP_Controls.ResumeLayout();

        // Adjust for scrollbar width
        if (FLP_Controls.VerticalScroll.Visible)
            ClientSize = new(newClientSize.Width + SystemInformation.VerticalScrollBarWidth, newClientSize.Height);
    }

    private void LoadROM(EditorBase editor)
    {
        Editor = editor;

        // Force client size to zero to make sure the windows size is adjusted properly the first time
        ClientSize = Size.Empty;

        LoadEditorButtons();

        CenterToScreen();

        Text = $"{nameof(pkNX)} - {Editor.Game}";
        TB_Path.Text = Editor.Location;
        Menu_Current.Enabled = true;
        EditUtil.LoadSettings(Editor.Game);
        EditUtil.SaveSettings(Editor.Game);
        SpriteUtil.Initialize();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void Menu_Current_Click(object sender, EventArgs e)
    {
        if (Directory.Exists(TB_Path.Text))
            Process.Start("explorer.exe", TB_Path.Text);
    }

    private void Menu_Save_Click(object sender, EventArgs e)
    {
        Editor?.Save();
    }
}
