using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures;
using Util = pkNX.Randomization.Util;

namespace pkNX.WinForms.Controls;

public partial class StatEditor : UserControl
{
    public StatEditor()
    {
        InitializeComponent();
        tb_iv = [TB_HPIV, TB_ATKIV, TB_DEFIV, TB_SPEIV, TB_SPAIV, TB_SPDIV];
        tb_ev = [TB_HPEV, TB_ATKEV, TB_DEFEV, TB_SPEEV, TB_SPAEV, TB_SPDEV];
        tb_av = [TB_HPAV, TB_ATKAV, TB_DEFAV, TB_SPEAV, TB_SPAAV, TB_SPDAV];
        labarray = [Label_ATK, Label_DEF, Label_SPE, Label_SPA, Label_SPD];
    }

    public void Initialize(string[] types)
    {
        UpdatingFields = true;
        foreach (var t in types.Skip(1))
            CB_HPType.Items.Add(t);
        UpdatingFields = false;
    }

    public IPersonalTable? Personal { private get; set; }
    public bool UpdatingFields;
    public StatPKM PKM { get; set; } = new TrainerPoke7b();

    private readonly MaskedTextBox[] tb_iv;
    private readonly MaskedTextBox[] tb_ev;
    private readonly MaskedTextBox[] tb_av;
    private readonly Label[] labarray;

    public void UpdateStats()
    {
        if (UpdatingFields)
            return;

        UpdatingFields = true;
        for (int i = 0; i < 6; i++)
        {
            if (Util.ToInt32(tb_iv[i].Text) > 31)
                tb_iv[i].Text = "31";
            if (Util.ToInt32(tb_ev[i].Text) > 255)
                tb_ev[i].Text = "255";
            if (Util.ToInt32(tb_av[i].Text) > 200)
                tb_av[i].Text = "200";
        }
        UpdatingFields = false;

        var pt = Personal;
        if (pt == null)
            throw new NullReferenceException("Personal table hasn't been initialized.");

        var pi = pt.GetFormEntry((ushort)PKM.Species, (byte)PKM.Form);
        var stats = PKM.GetStats(pi);

        Stat_HP.Text = stats[0].ToString();
        Stat_ATK.Text = stats[1].ToString();
        Stat_DEF.Text = stats[2].ToString();
        Stat_SPA.Text = stats[4].ToString();
        Stat_SPD.Text = stats[5].ToString();
        Stat_SPE.Text = stats[3].ToString();

        TB_IVTotal.Text = tb_iv.Sum(z => Util.ToInt32(z.Text)).ToString();
        TB_EVTotal.Text = tb_ev.Sum(z => Util.ToInt32(z.Text)).ToString();
        if (PKM is IAwakened s)
            TB_AVTotal.Text = s.AwakeningSum().ToString();

        var showAV = PKM is IAwakened;
        Label_AVs.Visible = TB_AVTotal.Visible = FLP_HPType.Visible = showAV;
        foreach (var mtb in tb_av)
            mtb.Visible = showAV;
        Label_EVs.Visible = TB_EVTotal.Visible = FLP_Dynamax.Visible = !showAV;
        foreach (var mtb in tb_ev)
            mtb.Visible = !showAV;

        // Recolor the Stat Labels based on boosted stats.
        RecolorStatLabels();
        UpdatingFields = true;
        CB_HPType.SelectedIndex = PKM.HiddenPowerType;
        UpdatingFields = false;
    }

    private void RecolorStatLabels()
    {
        int incr = (PKM.Nature / 5);
        int decr = (PKM.Nature % 5);
        // Reset Label Colors
        foreach (Label label in labarray)
            label.ResetForeColor();

        // Set Colored StatLabels only if Nature isn't Neutral
        if (incr != decr)
        {
            labarray[incr].ForeColor = Color.Red;
            labarray[decr].ForeColor = Color.Blue;
        }
    }

    public void LoadStats(StatPKM pkm)
    {
        PKM = pkm;

        UpdatingFields = true;
        for (int i = 0; i < 6; i++)
            tb_iv[i].Text = pkm.GetIV(i).ToString("00");
        for (int i = 0; i < 6; i++)
            tb_ev[i].Text = pkm.GetEV(i).ToString("00");
        if (PKM is IAwakened a)
        {
            for (int i = 0; i < 6; i++)
                tb_av[i].Text = a.GetAV(i).ToString("00");
        }
        CB_HPType.SelectedIndex = PKM.HiddenPowerType;
        UpdatingFields = false;
        UpdateStats();
    }

    private void UpdateIV(object sender, EventArgs e)
    {
        if (UpdatingFields || sender is not MaskedTextBox t)
            return;
        var index = Array.IndexOf(tb_iv, t);
        if (index < 0)
            return;
        int value = Math.Min(31, Util.ToInt32(t.Text));
        PKM.SetIV(index, value);
        UpdatingFields = true;
        CB_HPType.SelectedIndex = PKM.HiddenPowerType;
        UpdatingFields = false;
        UpdateStats();
    }

    private void UpdateEV(object sender, EventArgs e)
    {
        if (UpdatingFields || sender is not MaskedTextBox t)
            return;
        var index = Array.IndexOf(tb_ev, t);
        if (index < 0)
            return;
        int value = Math.Min(252, Util.ToInt32(t.Text));
        PKM.SetEV(index, value);
        UpdateStats();
    }

    private void UpdateAV(object sender, EventArgs e)
    {
        if (UpdatingFields || sender is not MaskedTextBox t || PKM is not IAwakened a)
            return;
        var index = Array.IndexOf(tb_av, t);
        if (index < 0)
            return;
        int value = Math.Min(200, Util.ToInt32(t.Text));
        a.SetAV(index, value);
        UpdateStats();
    }

    private void ChangeHPType(object sender, EventArgs e)
    {
        if (UpdatingFields)
            return;
        PKM.SetHPIVs(CB_HPType.SelectedIndex);
        UpdateStats();
    }
}
