using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms;

public partial class Dumper9a : Form
{
    private readonly GameManager9a Game;
    private readonly GameDumper9a Dumper;

    public Dumper9a(GameManager9a rom)
    {
        InitializeComponent();
        Dumper = new GameDumper9a(Game = rom);
    }

    private void B_OpenFolder_Click(object sender, EventArgs e) => Process.Start("explorer.exe", Dumper.DumpFolder);

    #region Tab 1
    private void B_ParsePKMDetails_Click(object sender, EventArgs e)
    {
        Dumper.DumpPersonal();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_PKText_Click(object sender, EventArgs e)
    {
        Dumper.DumpStrings();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Trainers_Click(object sender, EventArgs e)
    {
        Dumper.DumpTrainers();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpMovesAbilities_Click(object sender, EventArgs e)
    {
        Dumper.DumpMoves();
        Dumper.DumpAbilities();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpItems_Click(object sender, EventArgs e)
    {
        Dumper.DumpItems();
        Dumper.DumpFieldItems();
        Dumper.DumpHiddenItems();
        Dumper.DumpMegaCrystals();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpPokeData_Click(object sender, EventArgs e)
    {
        Dumper.DumpPokemonData();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpConfig_Click(object sender, EventArgs e)
    {
        Dumper.DumpConfig();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpEncountData_Click(object sender, EventArgs e)
    {
        Dumper.DumpEncountData();
        System.Media.SystemSounds.Asterisk.Play();
    }
    #endregion

    #region Tab 2
    private void B_DumpHash_Click(object sender, EventArgs e)
    {
        Dumper.DumpAHTB();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpArc_Click(object sender, EventArgs e)
    {
        Dumper.DumpArchives();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpSpecific_Click(object sender, EventArgs e)
    {
        Dumper.DumpHashReflectionBFBS();
        Dumper.DumpSpecific();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpMisc_Click(object sender, EventArgs e)
    {
        Dumper.DumpMisc();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpDimension_Click(object sender, EventArgs e)
    {
        Dumper.DumpDimension();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpScrubbed_Click(object sender, EventArgs e)
    {
        Dumper.DumpScrubbedInfo();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpPath_Click(object sender, EventArgs e)
    {
        using var f = new TextInput(Game.HasFile);
        while (true)
        {
            f.ShowDialog();
            if (f.DialogResult != DialogResult.OK)
                return; // cancel
            var path = f.Result;
            if (string.IsNullOrWhiteSpace(path))
                return; // silent quit

            if (!Game.HasFile(path))
            {
                WinFormsUtil.Alert("File not found in the game.");
                continue;
            }

            var data = Game.GetPackedFile(path);
            var ext = Path.GetExtension(path);
            if (ext.EndsWith("bfbs"))
                Dumper.RipBFBS(path);

            // Prompt for a path to export, then save file.
            using var sfd = new SaveFileDialog();
            sfd.FileName = Path.GetFileName(path);
            sfd.Filter = "All Files (*.*)|*.*";

            if (sfd.ShowDialog() != DialogResult.OK)
                continue;
            File.WriteAllBytes(sfd.FileName, data);
            System.Media.SystemSounds.Asterisk.Play();
            break;
        }
    }

    private void B_DumpFromTextFile_Click(object sender, EventArgs e)
    {
        const string fileName = "filepaths.txt";
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        if (!File.Exists(path))
        {
            WinFormsUtil.Alert($"{fileName} not found in the current directory.");
            return;
        }

        Dumper.DumpFromTextFile(path);
        System.Media.SystemSounds.Asterisk.Play();
    }

    #endregion

    #region Tab - Future
    // Placeholder for future dumpers
    #endregion

    private void B_Encount_Click(object sender, EventArgs e)
    {
        Dumper.DumpEncounters();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Mega_Click(object sender, EventArgs e)
    {
        Dumper.DumpMegaInfo();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Locations_Click(object sender, EventArgs e)
    {
        Dumper.DumpLocationNames();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_SearchPattern_Click(object sender, EventArgs e)
    {
        Dumper.ScanPatterns();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Triggers_Click(object sender, EventArgs e)
    {
        Dumper.DumpEventTriggers();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DonutLocalization_Click(object sender, EventArgs e)
    {
        Dumper.DumpDonutData();
        System.Media.SystemSounds.Asterisk.Play();
    }
}
