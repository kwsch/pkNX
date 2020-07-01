using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;

namespace pkNX.WinForms.Controls
{
    internal class EditorSWSH : EditorBase
    {
        protected internal EditorSWSH(GameManager rom) : base(rom) { }

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
                ReadClass = data => new TrainerClass8(data),
                ReadPoke = data => new TrainerPoke8(data),
                ReadTrainer = data => new TrainerData8(data),
                ReadTeam = TrainerPoke8.ReadTeam,
                WriteTeam = TrainerPoke8.WriteTeam,
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
                TMHM = Legal.TMHM_SWSH,
            };
            using var form = new PokeDataUI(editor, ROM);
            form.ShowDialog();
            if (!form.Modified)
                editor.CancelEdits();
            else
                editor.Save();
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

        public void NotWorking_EditMoves()
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

        public void NotWorking_EditShinyRate()
        {
            var path = Path.Combine(ROM.PathExeFS, "main");
            var data = FileMitm.ReadAllBytes(path);
            var nso = new NSO(data);

            var shiny = new ShinyRateSWSH(nso.DecompressedText);
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

        public void NotWorking_EditTM()
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

        public void NotWorking_EditTypeChart()
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

        public void EditWild_SW() => PopWildEdit("k");
        public void EditWild_SH() => PopWildEdit("t");

        private void PopWildEdit(string file)
        {
            IFileContainer fp = ROM.GetFile(GameFile.NestData);
            var data_table = new GFPack(fp[0]);
            var sdo = data_table.GetDataFileName($"encount_symbol_{file}.bin");
            var hdo = data_table.GetDataFileName($"encount_{file}.bin");
            var s = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(sdo);
            var h = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(hdo);
            while (s.EncounterTables[0].SubTables.Length != 9)
            {
                s = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(sdo);
                h = FlatBufferConverter.DeserializeFrom<EncounterArchive8>(hdo);
            }

            using var form = new SSWE(ROM, s, h);
            form.ShowDialog();
            if (!form.Modified)
                return;

            var sd = FlatBufferConverter.SerializeFrom(s);
            var hd = FlatBufferConverter.SerializeFrom(h);
            data_table.SetDataFileName($"encount_symbol_{file}.bin", sd);
            data_table.SetDataFileName($"encount_{file}.bin", hd);

            fp[0] = data_table.Write();
        }

        public void EditRaids()
        {
            IFileContainer fp = ROM.GetFile(GameFile.NestData);
            var data_table = new GFPack(fp[0]);
            const string nest = "nest_hole_encount.bin";
            var nest_encounts = FlatBufferConverter.DeserializeFrom<EncounterNest8Archive>(data_table.GetDataFileName(nest));

            var arr = nest_encounts.Tables;
            var cache = new DataCache<EncounterNest8Table>(arr);
            var games = new[] {"Sword", "Shield"};
            var names = arr.Select((z, i) => $"{games[z.GameVersion - 1]} - {i / 2}").ToArray();

            void Randomize()
            {
                var pt = ROM.Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                var frand = new FormRandomizer(ROM.Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in arr)
                {
                    foreach (var p in t.Entries)
                    {
                        p.Species = srand.GetRandomSpecies(p.Species);
                        p.AltForm = frand.GetRandomForme(p.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                        p.Ability = 4; // "A4" -- 1, 2, or H
                        p.Gender = 0; // random
                        p.IsGigantamax = false; // don't allow gmax flag on non-gmax species
                    }
                }
            }

            using var form = new GenericEditor<EncounterNest8Table>(cache, names, "Max Raid Battles Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                return;
            var data = FlatBufferConverter.SerializeFrom(nest_encounts);
            data_table.SetDataFileName(nest, data);
            fp[0] = data_table.Write();
        }

        public void EditRaidRewards()
        {
            IFileContainer fp = ROM.GetFile(GameFile.NestData);
            var data_table = new GFPack(fp[0]);
            const string nest = "nest_hole_drop_rewards.bin";
            byte[] originalData = data_table.GetDataFileName(nest);
            var nest_drops = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(originalData);

            var arr = nest_drops.Tables;
            var cache = new DataCache<NestHoleReward8Table>(arr);
            var names = arr.Select(z => $"{z.TableID}").ToArray();

            void Randomize()
            {
                int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Info.Game);
                foreach (var t in arr)
                {
                    foreach (var i in t.Entries)
                        i.ItemID = (uint)PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                }
            }

            using var form = new GenericEditor<NestHoleReward8Table>(cache, names, "Raid Rewards Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                return;
            var data = FlatBufferConverter.SerializeFrom(nest_drops);
            data_table.SetDataFileName(nest, data);
            fp[0] = data_table.Write();
        }

        public void EditRBonusRewards()
        {
            IFileContainer fp = ROM.GetFile(GameFile.NestData);
            var data_table = new GFPack(fp[0]);
            const string nest = "nest_hole_bonus_rewards.bin";
            var nest_bonus = FlatBufferConverter.DeserializeFrom<NestHoleReward8Archive>(data_table.GetDataFileName(nest));

            var arr = nest_bonus.Tables;
            var cache = new DataCache<NestHoleReward8Table>(arr);
            var names = arr.Select(z => $"{z.TableID}").ToArray();

            void Randomize()
            {
                int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Info.Game);
                foreach (var t in arr)
                {
                    foreach (var i in t.Entries)
                        i.ItemID = (uint)PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                }
            }

            using var form = new GenericEditor<NestHoleReward8Table>(cache, names, "Raid Bonus Rewards Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                return;
            var data = FlatBufferConverter.SerializeFrom(nest_bonus);
            data_table.SetDataFileName(nest, data);
            fp[0] = data_table.Write();
        }

        public void EditStatic()
        {
            var arc = ROM.GetFile(GameFile.EncounterStatic);
            var data = arc[0];
            var objs = FlatBufferConverter.DeserializeFrom<EncounterStatic8Archive>(data);

            var encounters = objs.Table;
            var names = Enumerable.Range(0, encounters.Length).Select(z => $"{z:000}").ToArray();
            var cache = new DirectCache<EncounterStatic8>(encounters);

            void Randomize()
            {
                int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
                var pt = ROM.Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                var frand = new FormRandomizer(ROM.Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in encounters)
                {
                    if (t.Species >= Species.Zacian && t.Species <= Species.Eternatus) // Eternatus crashes when changed, keep Zacian and Zamazenta to make final boss battle fair
                        continue;
                    t.Species = (Species)srand.GetRandomSpecies((int)t.Species);
                    t.AltForm = frand.GetRandomForme((int)t.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                    t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = Nature.Random25;
                    t.Gender = FixedGender.Random;
                    t.ShinyLock = Shiny.Random;
                    t.Move0 = t.Move1 = t.Move2 = t.Move3 = 0;
                    if (t.IV_Hp != -4)
                        t.IV_Hp = t.IV_Atk = t.IV_Def = t.IV_SpAtk = t.IV_SpDef = t.IV_Spe = -1;
                }
            }

            using var form = new GenericEditor<EncounterStatic8>(cache, names, "Static Encounter Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        public void EditGift()
        {
            var arc = ROM.GetFile(GameFile.EncounterGift);
            var data = arc[0];
            var objs = FlatBufferConverter.DeserializeFrom<EncounterGift8Archive>(data);

            var gifts = objs.Table;
            var names = Enumerable.Range(0, gifts.Length).Select(z => $"{z:000}").ToArray();
            var cache = new DirectCache<EncounterGift8>(gifts);

            void Randomize()
            {
                int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
                var pt = ROM.Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                var frand = new FormRandomizer(ROM.Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in gifts)
                {
                    // swap gmax gifts for other gmax capable species
                    if (t.CanGigantamax)
                    {
                        t.Species = (Species)Legal.GigantamaxForms[Randomization.Util.Random.Next(Legal.GigantamaxForms.Length)];
                        t.AltForm = t.Species == Species.Pikachu || t.Species == Species.Meowth ? 0 : frand.GetRandomForme((int)t.Species, false, false, false, false, ROM.Data.PersonalData.Table); // Pikachu & Meowth altforms can't gmax
                    }
                    else
                    {
                        t.Species = (Species)srand.GetRandomSpecies((int)t.Species);
                        t.AltForm = frand.GetRandomForme((int)t.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                    }

                    t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.Ball = (Ball)Randomization.Util.Random.Next(1, 15); // packed bit, only allows for 15 balls
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = Nature.Random25;
                    t.Gender = FixedGender.Random;
                    t.ShinyLock = Shiny.Random;
                    if (t.IV_Hp != -4)
                        t.IV_Hp = t.IV_Atk = t.IV_Def = t.IV_SpAtk = t.IV_SpDef = t.IV_Spe = -1;
                }
            }

            using var form = new GenericEditor<EncounterGift8>(cache, names, "Gift Pokémon Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        public void EditTrade()
        {
            var arc = ROM.GetFile(GameFile.EncounterTrade);
            var data = arc[0];
            var objs = FlatBufferConverter.DeserializeFrom<EncounterTrade8Archive>(data);

            var trades = objs.Table;
            var names = Enumerable.Range(0, trades.Length).Select(z => $"{z:000}").ToArray();
            var cache = new DirectCache<EncounterTrade8>(trades);

            void Randomize()
            {
                int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
                var pt = ROM.Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, ROM.Data.PersonalData);
                var frand = new FormRandomizer(ROM.Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in trades)
                {
                    // what you receive
                    t.Species = (Species)srand.GetRandomSpecies((int)t.Species);
                    t.AltForm = frand.GetRandomForme((int)t.Species, false, false, true, true, ROM.Data.PersonalData.Table);
                    t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.Ball = (Ball)Randomization.Util.Random.Next(1, 15); // packed bit, only allows for 15 balls
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = Nature.Random25;
                    t.Gender = FixedGender.Random;
                    t.ShinyLock = Shiny.Random;
                    t.Relearn1 = t.Relearn2 = t.Relearn3 = t.Relearn4 = 0;
                    if (t.IV_Hp != -4)
                        t.IV_Hp = t.IV_Atk = t.IV_Def = t.IV_SpAtk = t.IV_SpDef = t.IV_Spe = -1;

                    // what you trade
                    t.RequiredSpecies = (Species)srand.GetRandomSpecies((int)t.RequiredSpecies);
                    t.RequiredForm = frand.GetRandomForme((int)t.RequiredSpecies, false, false, true, true, ROM.Data.PersonalData.Table);
                    t.RequiredNature = Nature.Random25; // any
                }
            }

            using var form = new GenericEditor<EncounterTrade8>(cache, names, "In-Game Trades Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        private bool PopEdit<T>(T[] data, string title, string[] names) where T : class
        {
            var cache = new DirectCache<T>(data);
            using var form = new GenericEditor<T>(cache, names, title);
            form.ShowDialog();
            return form.Modified;
        }

        public void EditMasterDump()
        {
            using var md = new DumperSWSH((GameManagerSWSH)ROM);
            md.ShowDialog();
        }
    }
}
