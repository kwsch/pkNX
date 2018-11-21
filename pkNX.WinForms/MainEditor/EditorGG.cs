using System.IO;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls
{
    internal class EditorGG : EditorBase
    {
        protected internal EditorGG(GameManager rom) : base(rom) { }

        public void EditCommon()
        {
            var text = ROM.GetFilteredFolder(GameFile.GameText, z => Path.GetExtension(z) == ".dat");
            var config = new TextConfig(ROM.Game);
            var tc = new TextContainer(text, config);
            var form = new TextEditor(tc, TextEditor.TextEditorMode.Common);
            form.ShowDialog();
            if (!form.Modified)
                text.CancelEdits();
        }

        public void EditScript()
        {
            var text = ROM.GetFilteredFolder(GameFile.StoryText, z => Path.GetExtension(z) == ".dat");
            var config = new TextConfig(ROM.Game);
            var tc = new TextContainer(text, config);
            var form = new TextEditor(tc, TextEditor.TextEditorMode.Script);
            form.ShowDialog();
            if (!form.Modified)
                text.CancelEdits();
        }

        public void EditTrainers()
        {
            var editor = new TrainerEditor
            {
                ReadClass = data => new TrainerClass7b(data),
                ReadPoke = data => new TrainerPoke7b(data),
                ReadTrainer = data => new TrainerData7b(data),
                ReadTeam = TrainerPoke7b.ReadTeam,
                WriteTeam = TrainerPoke7b.WriteTeam,
                TrainerData = ROM.GetFilteredFolder(GameFile.TrainerData),
                TrainerPoke = ROM.GetFilteredFolder(GameFile.TrainerPoke),
                TrainerClass = ROM.GetFilteredFolder(GameFile.TrainerClass),
            };
            editor.Initialize();
            var form = new BTTE(ROM, editor);
            form.ShowDialog();
            if (!form.Modified)
                editor.CancelEdits();
            else
                editor.Save();
        }

        public void EditPokémon()
        {
            var editor = new PokeEditor
            {
                Evolve = ROM.Data.EvolutionData,
                Learn = ROM.Data.LevelUpData,
                Mega = ROM.Data.MegaEvolutionData,
                Personal = ROM.Data.PersonalData,

            };
            var form = new PokeDataUI(editor, ROM);
            form.ShowDialog();
            if (!form.Modified)
                editor.CancelEdits();
            else
                editor.Save();
        }
    }
}