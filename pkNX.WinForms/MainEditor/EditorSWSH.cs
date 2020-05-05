using System;
using System.IO;
using System.Linq;
using pkNX.Containers;
using pkNX.Game;
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

        public void EditRaid()
        {
            IFileContainer fp = ROM.GetFile(GameFile.NestData);
            var data_table = new GFPack(fp[0]);
            const string nest = "nest_hole_encount.bin";
            var nest_encounts = FlatBufferConverter.DeserializeFrom<EncounterNest8Archive>(data_table.GetDataFileName(nest));

            var arr = nest_encounts.Tables;
            var cache = new DataCache<EncounterNest8Table>(arr);
            var games = new[] {"Sword", "Shield"};
            var names = arr.Select((z, i) => $"{games[z.GameVersion - 1]} - {i / 2}").ToArray();
            using var form = new GenericEditor<EncounterNest8Table>(cache, names, "Raid Encounters");
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
            var names = arr.Select((z, i) => $"{z.TableID}").ToArray();
            using var form = new GenericEditor<NestHoleReward8Table>(cache, names, "Raid Rewards");
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
            var names = arr.Select((z, i) => $"{z.TableID}").ToArray();
            using var form = new GenericEditor<NestHoleReward8Table>(cache, names, "RBonus Rewards");
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
            if (CreateGenericEditor<EncounterStatic8Archive, EncounterStatic8>(ref data, "Static Encounters"))
                arc[0] = data;
        }

        public void EditGift()
        {
            var arc = ROM.GetFile(GameFile.EncounterGift);
            var data = arc[0];
            if (CreateGenericEditor<EncounterGift8Archive, EncounterGift8>(ref data, "Gift Encounters"))
                arc[0] = data;
        }

        public void EditTrade()
        {
            var arc = ROM.GetFile(GameFile.EncounterTrade);
            var data = arc[0];
            if (CreateGenericEditor<EncounterTrade8Archive, EncounterTrade8>(ref data, "Trade Encounters"))
                arc[0] = data;
        }

        private bool CreateGenericEditor<TA, T1>(ref byte[] data, string title, string[] names = null) where TA : IFlatBufferArchive<T1> where T1 : class
        {
            var objs = FlatBufferConverter.DeserializeFrom<TA>(data);
            if (names == null)
                names = Enumerable.Range(1, objs.Table.Length).Select(z => z.ToString()).ToArray();
            var result = PopEdit(objs.Table, title, names);
            if (result)
                data = FlatBufferConverter.SerializeFrom(objs);
            return result;
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
