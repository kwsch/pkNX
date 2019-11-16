using System;
using System.Diagnostics;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms
{
    public partial class DumperSWSH : Form
    {
        private readonly GameDumperSWSH Dumper;

        public DumperSWSH(GameManagerSWSH rom)
        {
            InitializeComponent();
            Dumper = new GameDumperSWSH(rom);
        }

        private void B_OpenFolder_Click(object sender, EventArgs e) => Process.Start("explorer.exe", Dumper.DumpFolder);

        #region Tab 1
        private void B_ParsePKMDetails_Click(object sender, EventArgs e)
        {
            Dumper.DumpPokeInfo();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_DumpTrainers_Click(object sender, EventArgs e)
        {
            Dumper.DumpTrainers();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Wild_Click(object sender, EventArgs e)
        {
            Dumper.DumpWilds();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Static_Click(object sender, EventArgs e)
        {
            Dumper.DumpStatic();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Gift_Click(object sender, EventArgs e)
        {
            Dumper.DumpGifts();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Nest_Click(object sender, EventArgs e)
        {
            Dumper.DumpNestEntries();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Trade_Click(object sender, EventArgs e)
        {
            Dumper.DumpTrades();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_ItemInfo_Click(object sender, EventArgs e)
        {
            Dumper.DumpItems();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Moves_Click(object sender, EventArgs e)
        {
            Dumper.DumpMoves();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_BattleTower_Click(object sender, EventArgs e)
        {
            Dumper.DumpBattleTower();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Distribution_Nests_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;
            var path = fbd.SelectedPath;
            Dumper.DumpDistributionNestEntries(path);
            System.Media.SystemSounds.Asterisk.Play();
        }
        #endregion

        #region Tab 2
        private void B_PKText_Click(object sender, EventArgs e)
        {
            Dumper.DumpStrings();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_PKLearn_Click(object sender, EventArgs e)
        {
            Dumper.DumpLearnsetBinary();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_PKEggMove_Click(object sender, EventArgs e)
        {
            Dumper.DumpEggBinary();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_PKEvo_Click(object sender, EventArgs e)
        {
            Dumper.DumpEvolutionBinary();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_PKForms_Click(object sender, EventArgs e)
        {
            Dumper.DumpFormNames();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_PKRibbon_Click(object sender, EventArgs e)
        {
            Dumper.DumpRibbonNames();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Memories_Click(object sender, EventArgs e)
        {
            Dumper.DumpMemoryStrings();
            System.Media.SystemSounds.Asterisk.Play();
        }
        #endregion

        #region Tab 3
        private void B_GetDummiedMoveInfo_Click(object sender, EventArgs e)
        {
            Dumper.DumpDummiedMoves();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_DumpHash_Click(object sender, EventArgs e)
        {
            Dumper.DumpAHTB();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_GalarDex_Click(object sender, EventArgs e)
        {
            Dumper.DumpGalarDex();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_FlavorText_Click(object sender, EventArgs e)
        {
            Dumper.DumpFlavorText();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_EggMove_Click(object sender, EventArgs e)
        {
            Dumper.DumpEggEntries();
            System.Media.SystemSounds.Asterisk.Play();
        }
        #endregion
    }
}
