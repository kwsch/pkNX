using System.IO;
using pkNX.Game;
using pkNX.Structures;
using pkNX.WinForms.Subforms;

namespace pkNX.WinForms.Controls;

internal class EditorSV : EditorBase
{
    protected override GameManagerSV ROM { get; }
    private GameData Data => ROM.Data;
    protected internal EditorSV(GameManagerSV rom) => ROM = rom;

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
        using var form = new MapViewer9(ROM);
        form.ShowDialog();
    }

    public void EditMasterDump()
    {
        using var md = new DumperSV(ROM);
        md.ShowDialog();
    }
}
