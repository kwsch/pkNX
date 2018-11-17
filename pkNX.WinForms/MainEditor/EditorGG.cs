using System.IO;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls
{
    internal class EditorGG : EditorBase
    {
        protected internal EditorGG(GameManager rom) : base(rom) { }

        public void EditTrainers()
        {
            WinFormsUtil.Alert("Not implemented yet.");
        }

        public void EditCommon()
        {
            var text = ROM.GetFile(GameFile.GameText);
            ((FolderContainer) text).Initialize(z => Path.GetExtension(z) == ".dat");
            var config = new TextConfig(ROM.Game);
            var tc = new TextContainer(text, config);
            var editor = new TextEditor(tc, TextEditor.TextEditorMode.Common);
            editor.ShowDialog();
        }

        public void EditScript()
        {
            var text = ROM.GetFile(GameFile.StoryText);
            ((FolderContainer)text).Initialize(z => Path.GetExtension(z) == ".dat");
            var config = new TextConfig(ROM.Game);
            var tc = new TextContainer(text, config);
            var editor = new TextEditor(tc, TextEditor.TextEditorMode.Script);
            editor.ShowDialog();
        }
    }
}