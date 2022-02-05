using System;
using System.Diagnostics;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms
{
    public partial class DumperPLA : Form
    {
        private readonly GameDumperPLA Dumper;

        public DumperPLA(GameManagerPLA rom)
        {
            InitializeComponent();
            Dumper = new GameDumperPLA(rom);
        }

        private void B_OpenFolder_Click(object sender, EventArgs e) => Process.Start("explorer.exe", Dumper.DumpFolder);

        #region Tab 1
        private void B_ParsePersonal_Click(object sender, EventArgs e)
        {
            Dumper.DumpPersonal();
            System.Media.SystemSounds.Asterisk.Play();
        }
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

        private void B_PokeDrops_Click(object sender, EventArgs e)
        {
            Dumper.DumpDrops();
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

        private void B_Placement_Click(object sender, EventArgs e)
        {
            Dumper.DumpPlacement();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_Resident_Click(object sender, EventArgs e)
        {
            Dumper.DumpResident();
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

        private void B_PKEvo_Click(object sender, EventArgs e)
        {
            Dumper.DumpEvolutionBinary();
            System.Media.SystemSounds.Asterisk.Play();
        }
        #endregion

        #region Tab 3
        private void B_GetOutbreak_Click(object sender, EventArgs e)
        {
            Dumper.DumpOutbreak();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_DumpScriptCommands(object sender, EventArgs e)
        {
            Dumper.DumpScriptID();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_GetDex_Click(object sender, EventArgs e)
        {
            Dumper.DumpDex();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_MoveShop_Click(object sender, EventArgs e)
        {
            Dumper.DumpMoveShop();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void B_DumpHash_Click(object sender, EventArgs e)
        {
            Dumper.DumpAHTB();
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
