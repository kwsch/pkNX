using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms;

public partial class DumperSV : Form
{
    private readonly GameManagerSV Game;
    private readonly GameDumperSV Dumper;

    public DumperSV(GameManagerSV rom)
    {
        InitializeComponent();
        Dumper = new GameDumperSV(Game = rom);
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

    private void B_Raid_Click(object sender, EventArgs e)
    {
        Dumper.DumpRaid();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Trainers_Click(object sender, EventArgs e)
    {
        Dumper.DumpTrainers();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DumpMoves_Click(object sender, EventArgs e)
    {
        Dumper.DumpMoves();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_Cooking_Click(object sender, EventArgs e)
    {
        Dumper.DumpCooking();
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

    private void B_DumpPath_Click(object sender, EventArgs e)
    {
        bool Valid(string path) => Game.HasFile(path);
        using var f = new TextInput(Valid);
        f.ShowDialog();
        if (f.DialogResult != DialogResult.OK)
            return; // cancel
        var path = f.Result;
        if (string.IsNullOrWhiteSpace(path))
            return; // silent quit

        if (!Game.HasFile(path))
        {
            WinFormsUtil.Alert("File not found in the game.");
            return;
        }

        var data = Game.GetPackedFile(path);
        // Prompt for a path to export, then save file.
        using var sfd = new SaveFileDialog
        {
            FileName = Path.GetFileName(path),
            Filter = "All Files (*.*)|*.*",
        };

        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        File.WriteAllBytes(sfd.FileName, data);
        System.Media.SystemSounds.Asterisk.Play();
    }

    #endregion

    #region Tab - BCAT
    private void B_DistributionRaids_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        if (fbd.ShowDialog() != DialogResult.OK)
            return;
        var path = fbd.SelectedPath;
        Dumper.DumpDistributionRaids(path);
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void B_DeliveryOutbreaks_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        if (fbd.ShowDialog() != DialogResult.OK)
            return;
        var path = fbd.SelectedPath;
        Dumper.DumpDeliveryOutbreaks(path);
        System.Media.SystemSounds.Asterisk.Play();
    }
    #endregion

    private void B_Encount_Click(object sender, EventArgs e)
    {
        Dumper.DumpEncounters();
        System.Media.SystemSounds.Asterisk.Play();
    }
}
