using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using pkNX.Sprites;
using pkNX.Structures;
using pkNX.WinForms.Properties;
using EditorBase = pkNX.WinForms.Controls.EditorBase;

namespace pkNX.WinForms
{
    public partial class Main : Form
    {
        private int Language
        {
            get => CB_Lang.SelectedIndex;
            set => CB_Lang.SelectedIndex = value;
        }

        private EditorBase? Editor;

        public Main()
        {
            InitializeComponent();

            CB_Lang.SelectedIndex = Settings.Default.Language;
            if (!string.IsNullOrWhiteSpace(Settings.Default.GamePath))
                OpenPath(Settings.Default.GamePath);

            DragDrop += (s, e) =>
            {
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (var f in files)
                    OpenPath(f);
            };
            DragEnter += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Editor == null)
                return;

            Editor.Close();
            EditUtil.SaveSettings(Editor.Game);
            Settings.Default.Language = CB_Lang.SelectedIndex;
            Settings.Default.GamePath = TB_Path.Text;
            Settings.Default.Save();
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

            string val = string.Empty;
            try { val = Clipboard.GetText(); }
            catch { }
            if (int.TryParse(val, out int seed))
            {
                Util.Rand = new Random(seed);
                WinFormsUtil.Alert($"Reseeded RNG to seed: {seed}");
                return;
            }
            WinFormsUtil.Alert("Unable to set seed.");
        }

        private void OpenPath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    OpenFolder(path);
                else
                    OpenFile(path);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                WinFormsUtil.Error($"Failed to open -- {path}", ex.Message);
            }
        }

        private static void OpenFile(string path)
        {
            var result = FileRipper.TryOpenFile(path);
            if (result.Code != RipResultCode.Success)
            {
                WinFormsUtil.Alert("Invalid file loaded." + Environment.NewLine + $"Unable to recognize data: {result.Code}.", path);
                return;
            }

            System.Media.SystemSounds.Asterisk.Play();
            Process.Start("explorer.exe", result.ResultPath);
        }

        private void OpenFolder(string path)
        {
            var editor = EditorBase.GetEditor(path, Language);
            if (editor == null)
            {
                WinFormsUtil.Alert("Invalid folder loaded." + Environment.NewLine + "Unable to recognize game data.", path);
            }
            else
            {
                try
                {
                    editor.Initialize();
                    LoadROM(editor);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    WinFormsUtil.Error("Failed to initialize ROM data." + Environment.NewLine + "Please ensure your dump is correctly set up, with updated patches merged in (if applicable).", ex.Message, ex.StackTrace);
                }
            }
        }

        private void LoadROM(EditorBase editor)
        {
            Editor = editor;
            var ctrl = Editor.GetControls(120, 30);
            FLP_Controls.Controls.Clear();
            foreach (var c in ctrl)
                FLP_Controls.Controls.Add(c);

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
    }
}
