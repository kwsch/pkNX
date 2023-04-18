using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PKHeX.Core;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SWSH;
using Ball = pkNX.Structures.Ball;
using FixedGender = pkNX.Structures.FixedGender;
using GameData = pkNX.Game.GameData;
using GameVersion = pkNX.Structures.GameVersion;
using Legal = pkNX.Structures.Legal;
using Nature = pkNX.Structures.Nature;
using Shiny = pkNX.Structures.Shiny;
using Species = pkNX.Structures.Species;

namespace pkNX.WinForms.Controls;

internal class EditorSWSH : EditorBase
{
    protected override GameManagerSWSH ROM { get; }
    private GameData Data => ROM.Data;
    protected internal EditorSWSH(GameManagerSWSH rom) => ROM = rom;

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
            TrainerData = ROM.GetFilteredFolder(GameFile.TrainerSpecData),
            TrainerPoke = ROM.GetFilteredFolder(GameFile.TrainerSpecPoke),
            TrainerClass = ROM.GetFilteredFolder(GameFile.TrainerSpecClass),
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
            TR = Legal.TR_SWSH,
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

    public void EditShinyRate()
    {
        if (ROM.PathExeFS == null)
        {
            WinFormsUtil.Alert("ExeFS not detected.");
            return;
        }

        var path = Path.Combine(ROM.PathExeFS, "main");
        if (!File.Exists(path))
        {
            WinFormsUtil.Alert("Not able to find `main` file in ExeFS.");
            return;
        }

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
            0x6F, 0x63, 0x6F, 0x6C, 0x45, 0x00,
        };
        int ofs = CodePattern.IndexOfBytes(nso.DecompressedRO, pattern);
        if (ofs < 0)
        {
            WinFormsUtil.Alert("Not able to find type chart data in ExeFS.");
            return;
        }
        ofs += pattern.Length + 0x24; // 0x5B4C0C in lgpe 1.0 RO

        var cdata = new byte[18 * 18];
        var types = ROM.GetStrings(TextName.TypeNames);
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
        if (ROM.Game == GameVersion.SWSH)
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
        var s = FlatBufferConverter.DeserializeFrom<EncounterArchive>(sdo);
        var h = FlatBufferConverter.DeserializeFrom<EncounterArchive>(hdo);
        while (s.EncounterTables[0].SubTables.Count != 9)
        {
            s = FlatBufferConverter.DeserializeFrom<EncounterArchive>(sdo);
            h = FlatBufferConverter.DeserializeFrom<EncounterArchive>(hdo);
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
        var nest_encounts = FlatBufferConverter.DeserializeFrom<EncounterNestArchive>(data_table.GetDataFileName(nest));

        var arr = nest_encounts.Table;
        var cache = new DataCache<EncounterNestTable>(arr);
        var games = new[] { "Sword", "Shield" };
        var names = arr.Select((z, i) => $"{games[z.GameVersion - 1]} - {i / 2}").ToArray();

        void Randomize()
        {
            var pt = Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
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
                    p.Form = frand.GetRandomForm(p.Species, false, spec.AllowRandomFusions, ROM.Info.Generation, Data.PersonalData.Table);
                    p.Ability = 4; // "A4" -- 1, 2, or H
                    p.Gender = 0; // random
                    p.IsGigantamax = false; // don't allow gmax flag on non-gmax species
                }
            }
        }

        using var form = new GenericEditor<EncounterNestTable>(cache, names, "Max Raid Battles Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            return;
        var data = nest_encounts.SerializeFrom();
        data_table.SetDataFileName(nest, data);
        fp[0] = data_table.Write();
    }

    public void EditRaidRewards()
    {
        IFileContainer fp = ROM.GetFile(GameFile.NestData);
        var data_table = new GFPack(fp[0]);
        const string nest = "nest_hole_drop_rewards.bin";
        byte[] originalData = data_table.GetDataFileName(nest);
        var nest_drops = FlatBufferConverter.DeserializeFrom<NestHoleRewardArchive>(originalData);

        var arr = nest_drops.Table;
        var cache = new DataCache<NestHoleRewardTable>(arr);
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

        using var form = new GenericEditor<NestHoleRewardTable>(cache, names, "Raid Rewards Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            return;
        var data = nest_drops.SerializeFrom();
        data_table.SetDataFileName(nest, data);
        fp[0] = data_table.Write();
    }

    public void EditRBonusRewards()
    {
        IFileContainer fp = ROM.GetFile(GameFile.NestData);
        var data_table = new GFPack(fp[0]);
        const string nest = "nest_hole_bonus_rewards.bin";
        var nest_bonus = FlatBufferConverter.DeserializeFrom<NestHoleRewardArchive>(data_table.GetDataFileName(nest));

        var arr = nest_bonus.Table;
        var cache = new DataCache<NestHoleRewardTable>(arr);
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

        using var form = new GenericEditor<NestHoleRewardTable>(cache, names, "Raid Bonus Rewards Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            return;
        var data = nest_bonus.SerializeFrom();
        data_table.SetDataFileName(nest, data);
        fp[0] = data_table.Write();
    }

    public void EditStatic()
    {
        var arc = ROM.GetFile(GameFile.EncounterTableStatic);
        var data = arc[0];
        var objs = FlatBufferConverter.DeserializeFrom<EncounterStaticArchive>(data);

        var encounters = objs.Table;
        var names = Enumerable.Range(0, encounters.Count).Select(z => $"{z:000}").ToArray();
        var cache = new DirectCache<Structures.FlatBuffers.SWSH.EncounterStatic>(encounters);

        void Randomize()
        {
            int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
            var pt = Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
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
                t.Form = (byte)frand.GetRandomForm(t.Species, false, spec.AllowRandomFusions, ROM.Info.Generation, Data.PersonalData.Table);
                t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                t.Nature = (int)Nature.Random25;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = (int)Shiny.Random;
                t.Moves = new[] { 0, 0, 0, 0 };
                if (t.IVHP != -4 && t.IVs.Any(z => z != 31))
                    t.IVs = new[] { -1, -1, -1, -1, -1, -1 };
            }
        }

        using var form = new GenericEditor<Structures.FlatBuffers.SWSH.EncounterStatic>(cache, names, "Static Encounter Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            arc.CancelEdits();
        else
            arc[0] = objs.SerializeFrom();
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
            var table = shop.Single;
            var names = table.Select((z, _) => $"{(ShopSWSH.TryGetValue(z.Hash, out var shopName) ? shopName : z.Hash.ToString("X"))}").ToArray();
            var cache = new DirectCache<SingleShop>(table);
            using var form = new GenericEditor<SingleShop>(cache, names, $"{nameof(SingleShop)} Editor", Randomize);
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
                    var items = shopDefinition.Inventories.Items;
                    for (int i = 0; i < items.Count; i++)
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
            var table = shop.Multi;
            var names = table.Select((z, _) => $"{(ShopSWSH.TryGetValue(z.Hash, out var shopName) ? shopName : z.Hash.ToString("X"))}").ToArray();
            var cache = new DirectCache<MultiShop>(table);
            using var form = new GenericEditor<MultiShop>(cache, names, $"{nameof(MultiShop)} Editor", Randomize);
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
                        for (int i = 0; i < items.Count; i++)
                            items[i] = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                    }
                }
            }
        }
        arc[0] = shop.SerializeFrom();
    }


    public readonly Dictionary<ulong, string> ShopSWSH = new()
    {
        // Galar
        { 0x1F3FF031A3A24490, "Poké Mart [0 Badges, Before Catching Tutorial]" },
        { 0x8E308F85B43038B4, "Motostoke [Upper Tier, TMs]" },
        { 0x8E309085B4303A67, "Hammerlocke [West, TMs]" },
        { 0x8E309185B4303C1A, "Hammerlocke [East, TMs]" },
        { 0x8E309285B4303DCD, "Wyndon [North, TMs]" },
        { 0x8E308B85B43031E8, "Battle Tower [TMs]" },
        { 0xCBD67969D873539B, "Motostoke [Lower Tier, Miscellaneous]" },
        { 0xCBD67869D87351E8, "Hammerlocke [South, Miscellaneous]" },
        { 0xCBD67B69D8735701, "Wyndon [South, Miscellaneous]" },
        { 0x04D7046DA09D3C78, "Hulbury [Herb Shop]" },
        { 0x4B2F9E98DDCB0707, "Hulbury [Incense Shop]" },
        { 0xE379CDF67A297070, "Wedgehurst [Berry Shop]" },
        { 0x3FD7A44219BF30BB, "Hammerlocke [South, BP Shop]" },
        { 0x3FD7A34219BF2F08, "Battle Tower [Battle Items]" },
        { 0x3FD7A64219BF3421, "Battle Tower [Nature Mints]" },
        // 0xD1BEA92EAAE52B5A -- Wishing Piece + X Items... unused?

        // Wild Area
        // next 84 tables are all Ingredient Sellers with similar inventories that rotate daily; don't bother labeling (why hardcode everything GF?)
        { 0xD1BEAA2EAAE52D0D, "Watt Trader 1 [Net Ball]" },
        { 0xD1BEA72EAAE527F4, "Watt Trader 1 [Dive Ball]" },
        { 0xD1BEA82EAAE529A7, "Watt Trader 1 [Nest Ball]" },
        { 0xD1BEA52EAAE5248E, "Watt Trader 1 [Repeat Ball]" },
        { 0xD1BEA62EAAE52641, "Watt Trader 1 [Timer Ball]" },
        { 0xD1BEA32EAAE52128, "Watt Trader 1 [Luxury Ball]" },
        { 0xD1BEA42EAAE522DB, "Watt Trader 1 [Dusk Ball]" },
        { 0xD1BEA12EAAE51DC2, "Watt Trader 1 [Heal Ball]" },
        { 0xD1BEA22EAAE51F75, "Watt Trader 1 [Quick Ball]" },

        { 0xD1C20F2EAAE80E83, "Watt Trader 2 [Net Ball]" },
        { 0xD1C20E2EAAE80CD0, "Watt Trader 2 [Dive Ball]" },
        { 0xD1C2112EAAE811E9, "Watt Trader 2 [Nest Ball]" },
        { 0xD1C2102EAAE81036, "Watt Trader 2 [Repeat Ball]" },
        { 0xD1C2132EAAE8154F, "Watt Trader 2 [Timer Ball]" },
        { 0xD1C2122EAAE8139C, "Watt Trader 2 [Luxury Ball]" },
        { 0xD1C2152EAAE818B5, "Watt Trader 2 [Dusk Ball]" },
        { 0xD1C2142EAAE81702, "Watt Trader 2 [Heal Ball]" },
        { 0xD1C2172EAAE81C1B, "Watt Trader 2 [Quick Ball]" },

        { 0xD1C2162EAAE81A68, "Watt Trader 3 [Net Ball]" },
        { 0xD1B79D2EAADEF848, "Watt Trader 3 [Dive Ball]" },
        { 0xD1B79E2EAADEF9FB, "Watt Trader 3 [Nest Ball]" },
        { 0xD1B79F2EAADEFBAE, "Watt Trader 3 [Repeat Ball]" },
        { 0xD1B7A02EAADEFD61, "Watt Trader 3 [Timer Ball]" },
        { 0xD1B7A12EAADEFF14, "Watt Trader 3 [Luxury Ball]" },
        { 0xD1B7A22EAADF00C7, "Watt Trader 3 [Dusk Ball]" },
        { 0xD1B7A32EAADF027A, "Watt Trader 3 [Heal Ball]" },
        { 0xD1B7A42EAADF042D, "Watt Trader 3 [Quick Ball]" },

        { 0xD1B7952EAADEEAB0, "Watt Trader 4 [Net Ball]" },
        { 0xD1B7962EAADEEC63, "Watt Trader 4 [Dive Ball]" },
        { 0xD1BB232EAAE211D1, "Watt Trader 4 [Nest Ball]" },
        { 0xD1BB222EAAE2101E, "Watt Trader 4 [Repeat Ball]" },
        { 0xD1BB212EAAE20E6B, "Watt Trader 4 [Timer Ball]" },
        { 0xD1BB202EAAE20CB8, "Watt Trader 4 [Luxury Ball]" },
        { 0xD1BB272EAAE2189D, "Watt Trader 4 [Dusk Ball]" },
        { 0xD1BB262EAAE216EA, "Watt Trader 4 [Heal Ball]" },
        { 0xD1BB252EAAE21537, "Watt Trader 4 [Quick Ball]" },

        { 0xD1BB242EAAE21384, "Watt Trader 5 [Net Ball]" },
        { 0xD1BB1B2EAAE20439, "Watt Trader 5 [Dive Ball]" },
        { 0xD1BB1A2EAAE20286, "Watt Trader 5 [Nest Ball]" },
        { 0xD1CC212EAAF0819E, "Watt Trader 5 [Repeat Ball]" },
        { 0xD1CC222EAAF08351, "Watt Trader 5 [Timer Ball]" },
        { 0xD1CC1F2EAAF07E38, "Watt Trader 5 [Luxury Ball]" },
        { 0xD1CC202EAAF07FEB, "Watt Trader 5 [Dusk Ball]" },
        { 0xD1CC252EAAF0886A, "Watt Trader 5 [Heal Ball]" },
        { 0xD1CC262EAAF08A1D, "Watt Trader 5 [Quick Ball]" },

        { 0xD1CC232EAAF08504, "Watt Trader 6 [Net Ball]" },
        { 0xD1CC242EAAF086B7, "Watt Trader 6 [Dive Ball]" },
        { 0xD1CC192EAAF07406, "Watt Trader 6 [Repeat Ball]" },
        { 0xD1CC1A2EAAF075B9, "Watt Trader 6 [Quick Ball]" },
        { 0xD1CFA72EAAF39B27, "Watt Trader 6 [Heal Ball]" },

        // Isle of Armor
        { 0x5870C0165650F6A5, "Fields of Honor [Berry Shop]" },

        // Crown Tundra
        { 0x81DA6390A03C7E3F, "Freezington [Peddler]" },
        { 0x813C350B0B777943, "Snowslide Slope [Today's Highlight, TR00-TR09]" },
        { 0x813C360B0B777AF6, "Snowslide Slope [Today's Highlight, TR10-TR19]" },
        { 0x813C370B0B777CA9, "Snowslide Slope [Today's Highlight, TR20-TR29]" },
        { 0x813C380B0B777E5C, "Snowslide Slope [Today's Highlight, TR30-TR39]" },
        { 0x813C390B0B77800F, "Snowslide Slope [Today's Highlight, TR40-TR49]" },
        { 0x813C3A0B0B7781C2, "Snowslide Slope [Today's Highlight, TR50-TR59]" },
        { 0x813C3B0B0B778375, "Snowslide Slope [Today's Highlight, TR60-TR69]" },
        { 0x813C3C0B0B778528, "Snowslide Slope [Today's Highlight, TR70-TR79]" },
        { 0x813C3D0B0B7786DB, "Snowslide Slope [Today's Highlight, TR80-TR89]" },
        { 0x813F3A0B0B79B799, "Snowslide Slope [Today's Highlight, TR90-TR99]" },
        { 0xF49C86F8683842BF, "Max Lair [Dynite Ore Trader]" },
    };

    public void EditMoves()
    {
        var obj = ROM[GameFile.MoveStats]; // mini
        var cache = new DataCache<Waza>(obj)
        {
            Create = FlatBufferConverter.DeserializeFrom<Waza>,
            Write = FlatBufferConverter.SerializeFrom,
        };
        using var form = new GenericEditor<Waza>(cache, ROM.GetStrings(TextName.MoveNames), "Move Editor");
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
        var rentals = FlatBufferConverter.DeserializeFrom<RentalArchive>(data);
        var cache = new DataCache<Rental>(rentals.Table);
        var names = rentals.Table.Select((z, i) => $"{i:000} {z.Hash1:X16}").ToArray();
        using var form = new GenericEditor<Rental>(cache, names, "Rental Editor");
        form.ShowDialog();
        if (!form.Modified)
        {
            cache.CancelEdits();
            return;
        }
        obj[0] = rentals.SerializeFrom();
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
        var arc = ROM.GetFile(GameFile.EncounterTableGift);
        var data = arc[0];
        var objs = FlatBufferConverter.DeserializeFrom<EncounterGiftArchive>(data);

        var gifts = objs.Table;
        var names = Enumerable.Range(0, gifts.Count).Select(z => $"{z:000}").ToArray();
        var cache = new DirectCache<Structures.FlatBuffers.SWSH.EncounterGift>(gifts);

        void Randomize()
        {
            int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
            var pt = Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
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
                    t.Form = (byte)(t.Species is (int)Species.Pikachu or (int)Species.Meowth ? 0 : frand.GetRandomForm(t.Species, false, false, ROM.Info.Generation, Data.PersonalData.Table)); // Pikachu & Meowth altforms can't gmax
                }
                else
                {
                    t.Species = srand.GetRandomSpecies(t.Species);
                    t.Form = (byte)frand.GetRandomForm(t.Species, false, spec.AllowRandomFusions, ROM.Info.Generation, Data.PersonalData.Table);
                }

                t.Ability = Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                t.Ball = (Ball)Randomization.Util.Random.Next(1, Structures.FlatBuffers.SWSH.EncounterGift.BallToItem.Length);
                t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                t.Nature = (int)Nature.Random25;
                t.Gender = (byte)FixedGender.Random;
                t.ShinyLock = (int)Shiny.Random;
                if (t.IVHP != -4 && t.IVs.Any(z => z != 31))
                    t.IVs = new[] { -1, -1, -1, -1, -1, -1 };
            }
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
            var obj = FlatBufferConverter.DeserializeFrom<PlacementZoneArchive>(table);
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

        using var form = new GenericEditor<Structures.FlatBuffers.SWSH.EncounterGift>(cache, names, "Gift Pokémon Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
        {
            arc.CancelEdits();
        }
        else
        {
            UpdateStarters(); // update placement critter data to match new randomized species
            arc[0] = objs.SerializeFrom();
        }
    }

    public void EditTrade()
    {
        var arc = ROM.GetFile(GameFile.EncounterTableTrade);
        var data = arc[0];
        var objs = FlatBufferConverter.DeserializeFrom<EncounterTradeArchive>(data);

        var trades = objs.Table;
        var names = Enumerable.Range(0, trades.Count).Select(z => $"{z:000}").ToArray();
        var cache = new DirectCache<Structures.FlatBuffers.SWSH.EncounterTrade>(trades);

        void Randomize()
        {
            int[] PossibleHeldItems = Legal.GetRandomItemList(ROM.Game);
            var pt = Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();

            var spec = EditUtil.Settings.Species;
            var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            var frand = new FormRandomizer(Data.PersonalData);
            srand.Initialize(spec, ban);
            foreach (var t in trades)
            {
                // what you receive
                t.Species = srand.GetRandomSpecies(t.Species);
                t.Form = (byte)frand.GetRandomForm(t.Species, false, spec.AllowRandomFusions, ROM.Info.Generation, Data.PersonalData.Table);
                t.AbilityNumber = (byte)Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                t.Ball = (Ball)Randomization.Util.Random.Next(1, Structures.FlatBuffers.SWSH.EncounterTrade.BallToItem.Length);
                t.HeldItem = PossibleHeldItems[Randomization.Util.Random.Next(PossibleHeldItems.Length)];
                t.Nature = (int)Nature.Random25;
                t.Gender = (int)FixedGender.Random;
                t.ShinyLock = (int)Shiny.Random;
                t.Relearn1 = 0;
                if (t.IVHP != -4 && t.IVs.Any(z => z != 31))
                    t.IVs = new[] { -1, -1, -1, -1, -1, -1 };

                // what you trade
                t.RequiredSpecies = srand.GetRandomSpecies(t.RequiredSpecies);
                t.RequiredForm = (byte)frand.GetRandomForm(t.RequiredSpecies, false, false, ROM.Info.Generation, Data.PersonalData.Table);
                t.RequiredNature = (int)Nature.Random25; // any
            }
        }

        using var form = new GenericEditor<Structures.FlatBuffers.SWSH.EncounterTrade>(cache, names, "In-Game Trades Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            arc.CancelEdits();
        else
            arc[0] = objs.SerializeFrom();
    }

    public void EditDynamaxAdv()
    {
        var arc = ROM.GetFile(GameFile.DynamaxDens);
        var data = arc[0];
        var objs = FlatBufferConverter.DeserializeFrom<EncounterUndergroundArchive>(data);

        var table = objs.Table;
        var names = Enumerable.Range(0, table.Count).Select(z => $"{z:000}").ToArray();
        var cache = new DirectCache<EncounterUnderground>(table);

        void Randomize()
        {
            var pt = Data.PersonalData;
            int[] ban = pt.Table.Take(ROM.Info.MaxSpeciesID + 1)
                .Select((z, i) => new { Species = i, Present = ((IPersonalInfoSWSH)z).IsPresentInGame })
                .Where(z => !z.Present).Select(z => z.Species).ToArray();

            var spec = EditUtil.Settings.Species;
            var srand = new SpeciesRandomizer(ROM.Info, Data.PersonalData);
            var frand = new FormRandomizer(Data.PersonalData);
            srand.Initialize(spec, ban);
            foreach (var t in table)
            {
                // what you receive
                t.Species = srand.GetRandomSpecies(t.Species);
                t.Form = (byte)frand.GetRandomForm(t.Species, false, spec.AllowRandomFusions, ROM.Info.Generation, Data.PersonalData.Table);
                t.Ability = (uint)Randomization.Util.Random.Next(1, 4); // 1, 2, or H
                t.Move0 = t.Move1 = t.Move2 = t.Move3 = 0;
            }
        }

        using var form = new GenericEditor<EncounterUnderground>(cache, names, "Dynamax Adventures Encounter Editor", Randomize);
        form.ShowDialog();
        if (!form.Modified)
            arc.CancelEdits();
        else
            arc[0] = objs.SerializeFrom();
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
        obj[0] = root.SerializeFrom();

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
        using var md = new DumperSWSH(ROM);
        md.ShowDialog();
    }

    public void EditPlacement()
    {
        var arc = ROM.GetFile(GameFile.Placement);
        var placement = new GFPack(arc[0]);
        var area_names = new AHTB(placement.GetDataFileName("AreaNameHashTable.tbl")).ToDictionary();

        List<PlacementZoneArchive> areas = new();
        List<string> names = new();
        foreach (var area in area_names)
        {
            var areaName = area.Value;
            var fileName = $"{areaName}.bin";
            if (placement.GetIndexFileName(fileName) < 0)
                continue;

            var bin = placement.GetDataFileName(fileName);
            var data = FlatBufferConverter.DeserializeFrom<PlacementZoneArchive>(bin);

            names.Add(fileName);
            areas.Add(data);
        }

        var arr = areas.ToArray();
        var nameArr = names.ToArray();
        var cache = new DataCache<PlacementZoneArchive>(arr);
        var form = new GenericEditor<PlacementZoneArchive>(cache, nameArr, "Placement", canSave: true);
        form.ShowDialog();
        if (!form.Modified)
            return;

        // Stuff files back into the gfpak and save
        for (int i = 0; i < arr.Length; i++)
        {
            var obj = arr[i];
            var bin = obj.SerializeFrom();
            placement.SetDataFileName(nameArr[i], bin);
        }
        arc[0] = placement.Write();
    }
}
