using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Controls
{
    internal class EditorSWSH : EditorBase
    {
        private GameData Data => ((GameManagerSWSH)ROM).Data;
        protected internal EditorSWSH(GameManagerSWSH rom) : base(rom) { }

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
            using var form = new BTTE(Data, editor, ROM);
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
                Evolve = Data.EvolutionData,
                Learn = Data.LevelUpData,
                Mega = Data.MegaEvolutionData,
                Personal = Data.PersonalData,
                TMHM = Legal.TMHM_SWSH,
            };
            using var form = new PokeDataUI(editor, ROM, Data);
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
            var allowed = Legal.GetAllowedMoves(ROM.Game, Data.MoveData.Length);
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

        public void EditWild()
        {
            if (ROM.PathExeFS == null)
            {
                var dr = WinFormsUtil.Prompt(MessageBoxButtons.YesNoCancel, "No ExeFS data found. Please choose which game's encounter tables you wish to edit.", "Yes for Sword, No for Shield.");
                if (dr == DialogResult.Cancel)
                    return;
                PopWildEdit(dr == DialogResult.Yes ? "k" : "t");
            }
            else
            {
                PopWildEdit(ROM.Game == GameVersion.SW ? "k" : "t");
            }
        }

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

            using var form = new SSWE((GameManagerSWSH)ROM, s, h);
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

            var arr = nest_encounts.Table;
            var cache = new DataCache<EncounterNest8Table>(arr);
            var games = new[] {"Sword", "Shield"};
            var names = arr.Select((z, i) => $"{games[z.GameVersion - 1]} - {i / 2}").ToArray();

            void Randomize()
            {
                var pt = Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
                var frand = new FormRandomizer(Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in arr)
                {
                    foreach (var p in t.Entries)
                    {
                        p.Species = srand.GetRandomSpecies(p.Species);
                        p.Form = frand.GetRandomForme(p.Species, false, false, true, true, Data.PersonalData.Table);
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

            var arr = nest_drops.Table;
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

            var arr = nest_bonus.Table;
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
                var pt = Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
                var frand = new FormRandomizer(Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in encounters)
                {
                    if (t.Species is >= (int)Species.Zacian and <= (int)Species.Eternatus) // Eternatus crashes when changed, keep Zacian and Zamazenta to make final boss battle fair
                        continue;
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, Data.PersonalData.Table);
                    t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = (int)Nature.Random25;
                    t.Gender = (int)FixedGender.Random;
                    t.ShinyLock = (int)Shiny.Random;
                    t.Moves = new[] { 0, 0, 0, 0 };
                    if (t.IV_HP != -4 && t.IVs.Any(z => z != 31))
                        t.IVs = new[] {-1,-1,-1,-1,-1,-1};
                }
            }

            using var form = new GenericEditor<EncounterStatic8>(cache, names, "Static Encounter Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        public void EditShop1() => EditShop(false);

        public void EditShop2() => EditShop(true);

        private void EditShop(bool shop2)
        {
            var arc = ROM.GetFile(GameFile.Shops);
            var data = arc[0];
            int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
            var shop = FlatBufferConverter.DeserializeFrom<ShopInventory>(data);
            if (!shop2)
            {
                var table = shop.Shop1;
                var names = table.Select((z, _) => $"{(z.SWSH.TryGetValue(z.Hash, out var shopName) ? shopName : z.Hash.ToString("X"))}").ToArray();
                var cache = new DirectCache<Shop1>(table);
                using var form = new GenericEditor<Shop1>(cache, names, $"{nameof(Shop1)} Editor", Randomize);
                form.ShowDialog();
                if (!form.Modified)
                {
                    arc.CancelEdits();
                    return;
                }

                void Randomize()
                {
                    foreach (var shopDefinition in table)
                    {
                        var items = shopDefinition.Inventory.Items;
                        for (int i = 0; i < items.Length; i++)
                        {
                            if (Legal.Pouch_TMHM_SM.Contains((ushort)items[i]) || items[i] == 1230) // skip TMs
                                continue;
                            items[i] = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                        }
                    }
                }
            }
            else
            {
                var table = shop.Shop2;
                var names = table.Select((z, _) => $"{(z.SWSH.TryGetValue(z.Hash, out var shopName) ? shopName : z.Hash.ToString("X"))}").ToArray();
                var cache = new DirectCache<Shop2>(table);
                using var form = new GenericEditor<Shop2>(cache, names, $"{nameof(Shop2)} Editor", Randomize);
                form.ShowDialog();
                if (!form.Modified)
                {
                    arc.CancelEdits();
                    return;
                }

                void Randomize()
                {
                    foreach (var shopDefinition in table)
                    {
                        foreach (var inv in shopDefinition.Inventories)
                        {
                            var items = inv.Items;
                            for (int i = 0; i < items.Length; i++)
                                items[i] = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                        }
                    }
                }
            }
            arc[0] = FlatBufferConverter.SerializeFrom(shop);
        }

        public void EditMoves()
        {
            var obj = ROM[GameFile.MoveStats]; // mini
            var cache = new DataCache<Waza8>(obj)
            {
                Create = FlatBufferConverter.DeserializeFrom<Waza8>,
                Write = FlatBufferConverter.SerializeFrom,
            };
            using var form = new GenericEditor<Waza8>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
            form.ShowDialog();
            if (!form.Modified)
            {
                cache.CancelEdits();
                return;
            }

            cache.Save();
            Data.MoveData.ClearAll(); // force reload if used again
        }

        public void EditRental()
        {
            var obj = ROM[GameFile.Rentals];
            var data = obj[0];
            var rentals = FlatBufferConverter.DeserializeFrom<Rental8Archive>(data);
            var cache = new DataCache<Rental8>(rentals.Table);
            var names = rentals.Table.Select((z, i) => $"{i:000} {z.Hash1:X16}").ToArray();
            using var form = new GenericEditor<Rental8>(cache, names, "Rental Editor");
            form.ShowDialog();
            if (!form.Modified)
            {
                cache.CancelEdits();
                return;
            }
            obj[0] = FlatBufferConverter.SerializeFrom(rentals);
        }

        public void EditItems()
        {
            var obj = ROM[GameFile.ItemStats]; // mini
            var data = obj[0];
            var items = Item8.GetArray(data);
            var cache = new DataCache<Item8>(items);
            using var form = new GenericEditor<Item8>(cache, ROM.GetStrings(TextName.ItemNames), "Item Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
            {
                cache.CancelEdits();
                return;
            }

            void Randomize()
            {
                var tradeEvos = new[] { 221, 226, 227, 233, 235, 252, 321, 322, 323, 324, 325, 573, 646, 647 };
                foreach (var item in items)
                {
                    if (item.ItemSprite == -1 || !tradeEvos.Contains(item.ItemID))
                        continue;

                    item.Boost0 = 8; // evo stone
                    item.EffectField = 6; // use effect
                    item.CanUseOnPokemon = true;
                }
            }

            obj[0] = Item8.SetArray(items, data);
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
                var pt = Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
                var frand = new FormRandomizer(Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in gifts)
                {
                    // swap gmax gifts and kubfu for other gmax capable species
                    if (t.CanGigantamax || t.Species == (int)Species.Kubfu)
                    {
                        t.Species = Legal.GigantamaxForms[Randomization.Util.Random.Next(Legal.GigantamaxForms.Length)];
                        t.Form = (byte)(t.Species is (int)Species.Pikachu or (int)Species.Meowth ? 0 : frand.GetRandomForme(t.Species, false, false, false, false, Data.PersonalData.Table)); // Pikachu & Meowth altforms can't gmax
                    }
                    else
                    {
                        t.Species = srand.GetRandomSpecies(t.Species);
                        t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, Data.PersonalData.Table);
                    }

                    t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.Ball = (Ball)Randomization.Util.Random.Next(1, EncounterGift8.BallToItem.Length);
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = (int)Nature.Random25;
                    t.Gender = (byte)FixedGender.Random;
                    t.ShinyLock = (int)Shiny.Random;
                    if (t.IV_HP != -4 && t.IVs.Any(z => z != 31))
                        t.IVs = new[] {-1,-1,-1,-1,-1,-1};
                }

                UpdateStarters(); // update placement critter data to match new randomized species
            }

            void UpdateStarters()
            {
                var container = ROM.GetFile(GameFile.Placement);
                var placement = new GFPack(container[0]);

                // a_r0501_i0101.bin for Toxel
                // a_bt0101.bin for Type: Null
                // a_wr0201_i0101.bin for Bulbasaur, Squirtle, Porygon, and Kubfu
                // a_wr0301_i0401.bin for Cosmog
                // a_d0901.bin for Poipole
                const string file = "a_0101.bin";
                var table = placement.GetDataFileName(file);
                var obj = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(table);
                var critters = obj.Table[0].Critters;

                // Grookey
                critters[3].Species = (uint)gifts[0].Species;
                critters[3].Form = gifts[0].Form;

                // Scorbunny
                critters[1].Species = (uint)gifts[3].Species;
                critters[1].Form = gifts[3].Form;

                // Sobble
                critters[2].Species = (uint)gifts[4].Species;
                critters[2].Form = gifts[4].Form;

                var bin = FlatBufferConverter.SerializeFrom(obj);
                placement.SetDataFileName(file, bin);
                container[0] = placement.Write();
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
                var pt = Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new {Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame})
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
                var frand = new FormRandomizer(Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in trades)
                {
                    // what you receive
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, Data.PersonalData.Table);
                    t.AbilityNumber = (byte)Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.Ball = (Ball)Randomization.Util.Random.Next(1, EncounterTrade8.BallToItem.Length);
                    t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    t.Nature = (int)Nature.Random25;
                    t.Gender = (int)FixedGender.Random;
                    t.ShinyLock = (int)Shiny.Random;
                    t.Relearn1 = 0;
                    if (t.IV_HP != -4 && t.IVs.Any(z => z != 31))
                        t.IVs = new[] {-1,-1,-1,-1,-1,-1};

                    // what you trade
                    t.RequiredSpecies = srand.GetRandomSpecies(t.RequiredSpecies);
                    t.RequiredForm = (byte)frand.GetRandomForme(t.RequiredSpecies, false, false, true, true, Data.PersonalData.Table);
                    t.RequiredNature = (int)Nature.Random25; // any
                }
            }

            using var form = new GenericEditor<EncounterTrade8>(cache, names, "In-Game Trades Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        public void EditDynamaxAdv()
        {
            var arc = ROM.GetFile(GameFile.DynamaxDens);
            var data = arc[0];
            var objs = FlatBufferConverter.DeserializeFrom<EncounterUnderground8Archive>(data);

            var table = objs.Table;
            var names = Enumerable.Range(0, table.Length).Select(z => $"{z:000}").ToArray();
            var cache = new DirectCache<EncounterUnderground8>(table);

            void Randomize()
            {
                var pt = Data.PersonalData;
                int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                    .Select((z, i) => new { Species = i, Present = ((PersonalInfoSWSH)z).IsPresentInGame })
                    .Where(z => !z.Present).Select(z => z.Species).ToArray();

                var spec = EditUtil.Settings.Species;
                var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
                var frand = new FormRandomizer(Data.PersonalData);
                srand.Initialize(spec, ban);
                foreach (var t in table)
                {
                    // what you receive
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = (byte)frand.GetRandomForme(t.Species, false, false, true, true, Data.PersonalData.Table);
                    t.Ability = (uint)Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                    t.Move0 = t.Move1 = t.Move2 = t.Move3 = 0;
                }
            }

            using var form = new GenericEditor<EncounterUnderground8>(cache, names, "Dynamax Adventures Encounter Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                arc.CancelEdits();
            else
                arc[0] = FlatBufferConverter.SerializeFrom(objs);
        }

        public void EditSymbolBehave()
        {
            bool altRand = Control.ModifierKeys == Keys.Alt;
            var obj = ROM.GetFile(GameFile.SymbolBehave);
            var data = obj[0];
            var root = FlatBufferConverter.DeserializeFrom<SymbolBehaveRoot>(data);
            var cache = new DataCache<SymbolBehave>(root.Table);
            var names = root.Table.Select(z => $"{z.Species}{(z.Form != 0 ? $"-{z.Form}" : "")}").ToArray();
            using var form = new GenericEditor<SymbolBehave>(cache, names, "Symbol Behavior Editor", Randomize);
            form.ShowDialog();
            if (!form.Modified)
                return;
            obj[0] = FlatBufferConverter.SerializeFrom(root);

            void Randomize()
            {
                var mode = altRand
                    ? "WaterDash" // Sharpedo dash homing -- good luck running!
                    : "Anawohoru"; // Diglett - Disappear when approached, pop out elsewhere
                foreach (var t in root.Table)
                    t.Behavior = mode;
            }
        }

        public void EditMasterDump()
        {
            using var md = new DumperSWSH((GameManagerSWSH)ROM);
            md.ShowDialog();
        }

        public void EditPlacement()
        {
            var arc = ROM.GetFile(GameFile.Placement);
            var placement = new GFPack(arc[0]);
            var area_names = new AHTB(placement.GetDataFileName("AreaNameHashTable.tbl")).ToDictionary();

            List<PlacementArea8Archive> areas = new();
            List<string> names = new();
            foreach (var area in area_names)
            {
                var areaName = area.Value;
                var fileName = $"{areaName}.bin";
                if (placement.GetIndexFileName(fileName) < 0)
                    continue;

                var bin = placement.GetDataFileName(fileName);
                var data = FlatBufferConverter.DeserializeFrom<PlacementArea8Archive>(bin);

                names.Add(fileName);
                areas.Add(data);
            }

            var arr = areas.ToArray();
            var nameArr = names.ToArray();
            var cache = new DataCache<PlacementArea8Archive>(arr);
            var form = new GenericEditor<PlacementArea8Archive>(cache, nameArr, "Placement", canSave: true);
            form.ShowDialog();
            if (!form.Modified)
                return;

            // Stuff files back into the gfpak and save
            for (int i = 0; i < arr.Length; i++)
            {
                var obj = arr[i];
                var bin = FlatBufferConverter.SerializeFrom(obj);
                placement.SetDataFileName(nameArr[i], bin);
            }
            arc[0] = placement.Write();
        }
    }
}
