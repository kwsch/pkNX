using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.WinForms.Subforms;

namespace pkNX.WinForms.Controls;

internal class EditorPLA : EditorBase
{
    protected internal EditorPLA(GameManager rom) : base(rom) { }

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

    public void EditTrainers()
    {
        var folder = ROM.GetFilteredFolder(GameFile.TrainerData).FilePath;
        var files = Directory.GetFiles(folder);
        var data = files.Select(FlatBufferConverter.DeserializeFrom<TrData8a>).ToArray();
        var names = files.Select(Path.GetFileNameWithoutExtension).ToArray();
        var cache = new DataCache<TrData8a>(data);
        using var form = new GenericEditor<TrData8a>(cache, names, "Trainers", canSave: false);
        form.ShowDialog();
    }

    public void NotWorking_EditItems()
    {
        var obj = ROM.GetFilteredFolder(GameFile.ItemStats, z => new FileInfo(z).Length == 36);
        var cache = new DataCache<Item>(obj)
        {
            Create = Item.FromBytes,
            Write = item => item.Write(),
        };
        using var form = new GenericEditor<Item>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor");
        form.ShowDialog();
        if (!form.Modified)
            cache.CancelEdits();
        else
            cache.Save();
    }

    public void EditSpawns()
    {
        var residentpak = ROM.GetFile(GameFile.Resident)[0];
        var resident = new GFPack(residentpak);
        using var form = new MapViewer8a(ROM, resident);
        form.ShowDialog();
    }

    public void EditMoves()
    {
        var obj = ROM[GameFile.MoveStats]; // folder
        var cache = new DataCache<Waza8a>(obj)
        {
            Create = FlatBufferConverter.DeserializeFrom<Waza8a>,
            Write = FlatBufferConverter.SerializeFrom,
        };
        using var form = new GenericEditor<Waza8a>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }

        cache.Save();
        ROM.Data.MoveData.ClearAll(); // force reload if used again
    }

    public void EditItems()
    {
        var obj = ROM[GameFile.ItemStats]; // mini
        var data = obj[0];
        var items = Item8a.GetArray(data);
        var cache = new DataCache<Item8a>(items);
        using var form = new GenericEditor<Item8a>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }

        obj[0] = Item8a.SetArray(items, data);
    }

    public void EditSymbolBehave()
    {
        var obj = ROM.GetFile(GameFile.SymbolBehave);
        var data = obj[0];
        var root = FlatBufferConverter.DeserializeFrom<PokeAIArchive8a>(data);
        var cache = new DataCache<PokeAI8a>(root.Table);
        var names = root.Table.Select(z => $"{z.Species}{(z.Form != 0 ? $"-{z.Form}" : "")}").ToArray();
        using var form = new GenericEditor<PokeAI8a>(cache, names, "Symbol Behavior Editor", canSave: false);
        form.ShowDialog();
        if (!form.Modified)
            return;
        obj[0] = FlatBufferConverter.SerializeFrom(root);
    }

    public void EditMasterDump()
    {
        using var md = new DumperPLA((GameManagerPLA)ROM);
        md.ShowDialog();
    }
}
