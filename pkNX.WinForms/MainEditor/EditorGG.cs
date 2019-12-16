﻿using System;
using System.IO;
using System.Linq;
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
            using var form = new BTTE(ROM, editor);
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
                TMHM = Legal.TMHM_GG,
            };
            using var form = new PokeDataUI(editor, ROM);
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
            using var form = new GenericEditor<Item>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor");
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
            using var form = new GenericEditor<Move7>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
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
                    t.Form = Legal.GetRandomForme(t.Species, false, true, false, ROM.Data.PersonalData);
                }
            }

            using var form = new GenericEditor<EncounterGift7b>(cache, names, "Gift Editor", Randomize);
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
                    t.Form = Legal.GetRandomForme(t.Species, false, true, false, ROM.Data.PersonalData);
                    t.RequiredSpecies = srand.GetRandomSpecies(t.RequiredSpecies);
                }
            }

            using var form = new GenericEditor<EncounterTrade7b>(cache, names, "Trade Editor", Randomize);
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
                    t.Form = Legal.GetRandomForme(t.Species, false, true, false, ROM.Data.PersonalData);
                }
            }

            using var form = new GenericEditor<EncounterStatic7b>(cache, names, "Static Encounter Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                file.CancelEdits();
            else
                file[0] = objs.SelectMany(z => z.Write()).ToArray();
        }

        public void EditWild_GP() => PopWildEdit("encount_data_p.bin");
        public void EditWild_GE() => PopWildEdit("encount_data_e.bin");

        private void PopWildEdit(string file)
        {
            var winner = Path.Combine(ROM.PathRomFS, "bin", "field", "param", "encount", file);
            var obj = FlatBufferConverter.DeserializeFrom<EncounterArchive7b>(winner);

            using var form = new GGWE(ROM, obj);
            form.ShowDialog();
            var result = form.Result;
            if (string.IsNullOrWhiteSpace(result))
                return;

            var data = FlatBufferConverter.SerializeFrom(obj);
            FileMitm.WriteAllBytes(winner, data);
        }

        public void EditShinyRate()
        {
            var path = Path.Combine(ROM.PathExeFS, "main");
            var data = FileMitm.ReadAllBytes(path);
            var nso = new NSO(data);

            var shiny = new ShinyRateGG(nso.DecompressedText);
            if (!shiny.IsEditable)
            {
                WinFormsUtil.Alert("Not able to find shiny rate logic in ExeFS.");
                return;
            }

            using var editor = new ShinyRate(shiny);
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
                WinFormsUtil.Alert("Not able to find tm data in ExeFS.");
                return;
            }

            var moves = list.GetMoves();
            var allowed = Legal.GetAllowedMoves(ROM.Game, ROM.Data.MoveData.Length);
            var names = ROM.GetStrings(TextName.MoveNames);
            using var editor = new TMList(moves, allowed, names);
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

            byte[] pattern = // N2nn3pia9transport18UnreliableProtocolE
            {
                0x4E, 0x32, 0x6E, 0x6E, 0x33, 0x70, 0x69, 0x61, 0x39, 0x74, 0x72, 0x61, 0x6E, 0x73, 0x70, 0x6F, 0x72,
                0x74, 0x31, 0x38, 0x55, 0x6E, 0x72, 0x65, 0x6C, 0x69, 0x61, 0x62, 0x6C, 0x65, 0x50, 0x72, 0x6F, 0x74,
                0x6F, 0x63, 0x6F, 0x6C, 0x45, 0x00
            };
            int ofs = CodePattern.IndexOfBytes(nso.DecompressedRO, pattern);
            if (ofs < 0)
            {
                WinFormsUtil.Alert("Not able to find type chart data in ExeFS.");
                return;
            }
            ofs += pattern.Length + 0x24; // 0x5B4C0C in lgpe 1.0 RO

            var cdata = new byte[18 * 18];
            var types = ROM.GetStrings(TextName.Types);
            Array.Copy(nso.DecompressedRO, ofs, cdata, 0, cdata.Length);
            var chart = new TypeChartEditor(cdata);
            using var editor = new TypeChart(chart, types);
            editor.ShowDialog();
            if (!editor.Modified)
                return;

            chart.Data.CopyTo(nso.DecompressedRO, ofs);
            data = nso.Write();
            FileMitm.WriteAllBytes(path, data);
        }
    }
}