using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using FontAwesome.Sharp;
using PKHeX.Drawing.PokeSprite;

using pkNX.Game;
using pkNX.Structures;
using pkNX.WinForms.Controls;
using Button = System.Windows.Controls.Button;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;

namespace pkNX.WinForms;

public readonly struct EditorButtonData
{
    public static readonly int Width = 140;
    public static readonly int Height = 58;
    public static readonly Thickness Margin = new(3, 3, 3, 3);

    public static readonly int LayoutWidth = Width + (int)Margin.Left + (int)Margin.Right;
    public static readonly int LayoutHeight = Height + (int)Margin.Top + (int)Margin.Bottom;

    public string Title { get; init; }
    public IconChar? Icon { get; init; }
    public Action<object, RoutedEventArgs> OnClick { get; init; }
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
        nameof(Categories), typeof(EditorButtonData[]), typeof(MainWindow), new PropertyMetadata(Array.Empty<EditorButtonData>()));

    private static readonly string[] SupportedLanguages = {
        "カタカナ",
        "漢字",
        "English",
        "Français",
        "Italiano",
        "Deutsch",
        "Español",
        "한국",
        "汉字简化方案",
        "漢字簡化方案"
    };

    public EditorButtonData[] Categories
    {
        get { return (EditorButtonData[])GetValue(CategoriesProperty); }
        set { SetValue(CategoriesProperty, value); }
    }

    public ProgramSettings Settings { get; }

    private int Language
    {
        get => CB_Lang.SelectedIndex;
        set => CB_Lang.SelectedIndex = value;
    }

    private EditorBase? Editor;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        CB_Lang.ItemsSource = SupportedLanguages;

        SpriteName.AllowShinySprite = true;
        SpriteBuilderUtil.SpriterPreference = SpriteBuilderPreference.ForceSprites;

        Settings = ProgramSettings.LoadSettings();
        CB_Lang.SelectedIndex = Settings.Language;
        Menu_DisplayAdvanced.IsChecked = Settings.DisplayAdvanced;

        if (!string.IsNullOrWhiteSpace(Settings.GamePath) && Directory.Exists(Settings.GamePath))
            OpenPath(Settings.GamePath, Settings.GameOverride);

        Drop += (s, e) =>
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
                e.Effects = DragDropEffects.Copy;
        };
    }

    private void ChangeLanguage(object sender, EventArgs e)
    {
        if (Editor == null)
            return;

        if (Editor.Game.GetGeneration() < 7 && Language > 7 && !GameVersion.GG.Contains(Editor.Game))
        {
            WinFormsUtil.Alert("Selected Language is not available for this game", "Defaulting to English.");
            CB_Lang.SelectedIndex = 2;
            return;
        }
        Editor.Language = Language;
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
            // Rethrow to have a better debugging experience
            if (Debugger.IsAttached)
                throw;

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
        var loadResult = EditorBase.GetEditor(path, Language, gameOverride);
        var (editor, result) = loadResult;

        while (result == GameLoadResult.RomfsSelected)
        {
            var errorMsg = result.GetErrorMsg();

            var parentPath = Directory.GetParent(path)?.FullName ?? string.Empty;
            if (string.IsNullOrEmpty(parentPath))
                return;

            var prompt = $"Clicking 'OK' will allow pkNX to create a working directory in the following folder:\n\"{parentPath}\"";

            if (WinFormsUtil.Prompt(MessageBoxButton.OKCancel, errorMsg, prompt) != MessageBoxResult.OK)
                return;

            (editor, result) = EditorBase.GetEditor(parentPath, Language, gameOverride);
        }

        if (editor == null)
        {
            var msg = "An error occurred while loading the game files." + Environment.NewLine + result.GetErrorMsg();
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
            // Rethrow to have a better debugging experience
            if (Debugger.IsAttached)
                throw;

            var msg = "Failed to initialize ROM data." + Environment.NewLine +
                      "Please ensure your dump is correctly set up, with updated patches merged in (if applicable).";
            var stack = ex.StackTrace ?? string.Empty;
            WinFormsUtil.Error(msg, ex.Message, stack);
        }
    }

    private void LoadEditorButtons(EditorCategory category = EditorCategory.None)
    {
        List<EditorButtonData> categories = new();
        if (category == EditorCategory.None)
        {
            foreach (var c in (EditorCategory[])Enum.GetValues(typeof(EditorCategory)))
            {
                if (c == EditorCategory.None)
                    continue;

                if (Editor!.CountControlsForCategory(c) == 0)
                    continue;

                categories.Add(new()
                {
                    Icon = c.GetIcon(),
                    Title = c.ToString(),
                    OnClick = (_, _) => LoadEditorButtons(c)
                });
            }
        }
        else
        {
            // Create back button
            categories.Add(new()
            {
                Title = "Back",
                Icon = IconChar.Reply,
                OnClick = (_, _) => LoadEditorButtons()
            });
        }

        categories.AddRange(Editor!.GetControls(category, Settings.DisplayAdvanced).OrderBy(x => x.Title));

        Categories = categories.ToArray();

        AdjustWindowSize();

        CenterWindowOnScreen();
    }

    private void CenterWindowOnScreen()
    {
        var helper = new WindowInteropHelper(this);
        var screen = Screen.FromHandle(helper.Handle);
        var area = screen.WorkingArea;

        var source = PresentationSource.FromVisual(this);
        var dpi = source?.CompositionTarget?.TransformFromDevice.M11 ?? 1.0;

        Left = dpi * area.Left + (dpi * area.Width - Width) / 2;
        Top = dpi * area.Top + (dpi * area.Height - Height) / 2;
    }

    private void AdjustWindowSize()
    {
        int wp = EditorButtonData.LayoutWidth;
        int hp = EditorButtonData.LayoutHeight;
        int area = wp * hp;
        var count = Categories.Length;
        // Resize form dimensions then center to screen, so that all buttons are shown.
        var totalArea = count * area;
        var squareSide = Math.Sqrt(totalArea);
        var columns = (int)Math.Ceiling(squareSide / wp) + 1;
        var rows = (count / columns) + 1;

        double containerHorizontalMargin = SV_Controls.Padding.Left + SV_Controls.Padding.Right;
        double containerVerticalMargin = SV_Controls.Padding.Top + SV_Controls.Padding.Bottom;

        double titleHeight = (SystemParameters.WindowCaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight) * 1.5;
        double verticalBorderWidth = Math.Ceiling((SystemParameters.ResizeFrameVerticalBorderWidth + SystemParameters.FixedFrameVerticalBorderWidth) * 2) + 4;

        Width = containerHorizontalMargin + columns * wp + verticalBorderWidth;
        Height = SP_Header.Height + containerVerticalMargin + rows * hp + titleHeight;

        UpdateLayout();

        // Adjust for scrollbar width
        //if (SV_Controls.ComputedVerticalScrollBarVisibility == Visibility.Visible)
        //    DP_Content.Width += (int)Math.Ceiling(SystemParameters.VerticalScrollBarWidth);
    }

    private void LoadROM(EditorBase editor)
    {
        Editor?.Close(); // Clean exit prior editor
        Editor = editor;

        LoadEditorButtons();

        Title = $"{nameof(pkNX)} - {Editor.Game}";
        L_Path.Content = Editor.Location;
        Menu_Current.IsEnabled = true;
        EditUtil.LoadSettings(Editor.Game);
        EditUtil.SaveSettings(Editor.Game);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private async void Window_Closing(object sender, CancelEventArgs e)
    {
        if (Editor == null)
            return;

        Editor.Close();
        EditUtil.SaveSettings(Editor.Game);
        Settings.Language = CB_Lang.SelectedIndex;
        Settings.GamePath = (string)L_Path.Content;
        Settings.GameOverride = Editor.Game;
        await ProgramSettings.SaveSettings(Settings);
    }

    private void Menu_Current_Click(object sender, EventArgs e)
    {
        if (Directory.Exists((string)L_Path.Content))
            Process.Start("explorer.exe", (string)L_Path.Content);
    }

    private void Menu_SetRNGSeed_Click(object sender, EventArgs e)
    {
        var result = WinFormsUtil.Prompt(MessageBoxButton.YesNo, "Reseed RNG?",
            "If yes, copy the 32 bit (not hex) integer seed to the clipboard before hitting Yes.");
        if (MessageBoxResult.Yes != result)
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

    private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        using System.Windows.Forms.FolderBrowserDialog fbd = new();
        fbd.Description = "Select a folder containing the game files.";
        fbd.UseDescriptionForTitle = true;
        fbd.ShowNewFolderButton = false;

        if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            OpenPath(fbd.SelectedPath);
    }

    private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Editor?.Save();
    }

    private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        //if (ModifierKeys == Key.Control) // triggered via hotkey
        {
            if (MessageBoxResult.Yes != WinFormsUtil.Prompt(MessageBoxButton.YesNo, $"Quit {nameof(pkNX)}?"))
                return;
        }
        Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        EditorButtonData data = (EditorButtonData)(sender as Button)!.DataContext;
        data.OnClick.Invoke(sender, e);
    }

    private async void Menu_DisplayAdvanced_Click(object sender, RoutedEventArgs e)
    {
        Menu_DisplayAdvanced.IsChecked = !Menu_DisplayAdvanced.IsChecked;
        Settings.DisplayAdvanced = Menu_DisplayAdvanced.IsChecked;
        await ProgramSettings.SaveSettings(Settings);

        // Force reload of editor buttons
        LoadEditorButtons();
    }

    private void Menu_EditWorkspaceSettings_Click(object sender, RoutedEventArgs e)
    {
        var form = new Settings();
        form.ShowDialog();
        Editor.SetupVFS();
    }
}
