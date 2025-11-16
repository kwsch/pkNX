using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.WinForms.Subforms;
using System.IO;
using System.Windows.Forms;

namespace pkNX.WinForms.Controls;

internal class Editor9a : EditorBase
{
    protected override GameManager9a ROM { get; }
    private GameData Data => ROM.Data;

    protected internal Editor9a(GameManager9a rom)
    {
        ROM = rom;
        CheckOodleDllPresence();
    }

    private static void CheckOodleDllPresence()
    {
        const string file = $"{Oodle.OodleLibraryPath}.dll";
        var dir = Application.StartupPath;
        var path = Path.Combine(dir, file);
        if (!File.Exists(path))
            WinFormsUtil.Alert($"{file} not found in the executable folder", "Some decompression functions may cause errors.");
    }

    [EditorCallable(EditorCategory.Dialog)]
    public void EditCommon()
    {
        var text = ROM.GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");
        var config = new TextConfig(ROM.Game);
        var tc = new TextContainer(text, config);
        using var form = new TextEditor(tc, TextEditor.TextEditorMode.Common);
        form.ShowDialog();
        if (!form.Modified)
            text.CancelEdits();
    }

    [EditorCallable(EditorCategory.Dialog)]
    public void EditScript()
    {
        var text = ROM.GetFilteredFolder(GameFile.StoryText, z => Path.GetExtension(z) == ".dat");
        var config = new TextConfig(ROM.Game);
        var tc = new TextContainer(text, config);
        using var form = new TextEditor(tc, TextEditor.TextEditorMode.Script);
        form.ShowDialog();
        if (!form.Modified)
            text.CancelEdits();
    }

    [EditorCallable(EditorCategory.Field)]
    public void EditMapViewer()
    {
        using var form = new MapViewer9a(ROM, "English");
        form.ShowDialog();
    }

    public void EditMasterDump()
    {
        using var md = new Dumper9a(ROM);
        md.ShowDialog();
    }
}
