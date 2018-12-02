using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
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

        public void EditItems()
        {
            var obj = ROM.GetFilteredFolder(GameFile.ItemStats, z => new FileInfo(z).Length == 36);
            var cache = new DataCache<Item>(obj)
            {
                Create = Item.FromBytes,
                Write = item => item.Write(),
            };
            var form = new GenericEditor<Item>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor");
            form.ShowDialog();
            if (!form.Modified)
                cache.CancelEdits();
            else
                cache.Save();
        }

        public void EditMoves()
        {
            var obj = ROM[GameFile.MoveStats]; // mini
            var cache = new DataCache<Move7>(obj)
            {
                Create = data => new Move7(data),
                Write = move => move.Write(),
            };
            var form = new GenericEditor<Move7>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
            form.ShowDialog();
            if (!form.Modified)
                cache.CancelEdits();
            else
                cache.Save();
        }

        public void EditGift()
        {
            var file = ROM[GameFile.EncounterGift];
            var data = file[0];
            var objs = data.GetArray(z => new EncounterGift7b(z), EncounterGift7b.SIZE); // binary
            var names = Enumerable.Range(0, objs.Length).Select(z => $"{z:00}").ToArray();
            var cache = new DirectCache<EncounterGift7b>(objs);

            void Randomize()
            {
                var spec = EditUtil.Settings.Species;
                spec.Gen2 = spec.Gen3 = spec.Gen4 = spec.Gen5 = spec.Gen6 = spec.Gen7 = false;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                srand.Initialize(spec);
                foreach (var t in objs)
                {
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = Legal.GetRandomForme(t.Species, false, true, ROM.Data.PersonalData);
                }
            }

            var form = new GenericEditor<EncounterGift7b>(cache, names, "Gift Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                file.CancelEdits();
            else
                file[0] = objs.SelectMany(z => z.Write()).ToArray();
        }

        public void EditTrade()
        {
            var file = ROM[GameFile.EncounterTrade];
            var data = file[0];
            var objs = data.GetArray(z => new EncounterTrade7b(z), EncounterTrade7b.SIZE); // binary
            var names = Enumerable.Range(0, objs.Length).Select(z => $"{z:00}").ToArray();
            var cache = new DirectCache<EncounterTrade7b>(objs);

            void Randomize()
            {
                var spec = EditUtil.Settings.Species;
                spec.Gen2 = spec.Gen3 = spec.Gen4 = spec.Gen5 = spec.Gen6 = spec.Gen7 = false;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                srand.Initialize(spec);
                foreach (var t in objs)
                {
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = Legal.GetRandomForme(t.Species, false, true, ROM.Data.PersonalData);
                    t.RequiredSpecies = srand.GetRandomSpecies(t.RequiredSpecies);
                }
            }

            var form = new GenericEditor<EncounterTrade7b>(cache, names, "Trade Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                file.CancelEdits();
            else
                file[0] = objs.SelectMany(z => z.Write()).ToArray();
        }

        public void EditStatic()
        {
            var file = ROM[GameFile.EncounterStatic];
            var data = file[0];
            var objs = data.GetArray(z => new EncounterStatic7b(z), EncounterStatic7b.SIZE); // binary
            var names = Enumerable.Range(0, objs.Length).Select(z => $"{z:00}").ToArray();
            var cache = new DirectCache<EncounterStatic7b>(objs);

            void Randomize()
            {
                var spec = EditUtil.Settings.Species;
                spec.Gen2 = spec.Gen3 = spec.Gen4 = spec.Gen5 = spec.Gen6 = spec.Gen7 = false;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                srand.Initialize(spec);
                for (int i = 2; i < objs.Length; i++) // skip starters
                {
                    var t = objs[i];
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = Legal.GetRandomForme(t.Species, false, true, ROM.Data.PersonalData);
                }
            }

            var form = new GenericEditor<EncounterStatic7b>(cache, names, "Static Encounter Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                file.CancelEdits();
            else
                file[0] = objs.SelectMany(z => z.Write()).ToArray();
        }

        public void EditWild()
        {
            var ofd = new OpenFileDialog { Filter = "json files (*.json)|*.json|All files (*.*)|*.*" };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            var path = ofd.FileName;
            if (!File.Exists(path))
                return;

            if (Path.GetExtension(path) != ".json" || new FileInfo(path).Length > 350_000)
            {
                WinFormsUtil.Alert("Not an expected json file.");
                return;
            }
            var json = File.ReadAllText(path);
            var form = new GGWE(ROM, json);
            form.ShowDialog();
            var result = form.Result;
            if (string.IsNullOrWhiteSpace(result))
                return; // no save

            var sfd = new SaveFileDialog { Filter = "json files (*.json)|*.json|All files (*.*)|*.*" };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            path = sfd.FileName;
            File.WriteAllText(path, result);
        }

        public void EditShinyRate()
        {
            var path = Path.Combine(ROM.PathExeFS, "main");
            var data = FileMitm.ReadAllBytes(path);
            var nso = new NSO(data);

            var shiny = new ShinyRateGG(nso.DecompressedText);
            if (!shiny.IsEditable)
            {
                WinFormsUtil.Alert("Not able to find shiny rate logic in exefs.");
                return;
            }

            var editor = new ShinyRate(shiny);
            editor.ShowDialog();
            if (!editor.Modified)
                return;

            nso.DecompressedText = shiny.Data;
            FileMitm.WriteAllBytes(path, nso.Write());
        }

        public void EditTM()
        {
            var path = Path.Combine(ROM.PathExeFS, "main");
            var data = FileMitm.ReadAllBytes(path);
            var list = new TMEditorGG(data);
            if (!list.Valid)
            {
                WinFormsUtil.Alert("Not able to find tm data in exefs.");
                return;
            }

            var moves = list.GetMoves();
            var allowed = Legal.GetAllowedMoves(ROM.Game, ROM.Data.MoveData.Length);
            var names = ROM.GetStrings(TextName.MoveNames);
            var editor = new TMList(moves, allowed, names);
            editor.ShowDialog();
            if (!editor.Modified)
                return;

            list.SetMoves(editor.FinalMoves);
            data = list.Write();
            FileMitm.WriteAllBytes(path, data);
        }

        public void EditTypeChart()
        {
            var path = Path.Combine(ROM.PathExeFS, "main");
            var data = FileMitm.ReadAllBytes(path);
            var nso = new NSO(data);

            byte[] pattern = {0x84, 0x5A, 0xA4, 0xFF};
            int ofs = CodePattern.IndexOfBytes(nso.DecompressedRO, pattern);
            if (ofs < 0)
            {
                WinFormsUtil.Alert("Not able to find type chart data in exefs.");
                return;
            }
            ofs += pattern.Length;

            var cdata = new byte[18 * 18];
            var types = ROM.GetStrings(TextName.Types);
            Array.Copy(nso.DecompressedRO, ofs, cdata, 0, cdata.Length);
            var chart = new TypeChartEditor(cdata);
            var editor = new TypeChart(chart, types);
            editor.ShowDialog();
            if (!editor.Modified)
                return;

            chart.Data.CopyTo(nso.DecompressedRO, ofs);
            data = nso.Write();
            FileMitm.WriteAllBytes(path, data);
        }
    }
}