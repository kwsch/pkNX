using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.WinForms.Controls;

public partial class LandmarkEditor8a : UserControl
{
    public IList<LandmarkItemSpawn> Spawners = [];

    public LandmarkEditor8a() => InitializeComponent();

    public void LoadTable(IList<LandmarkItemSpawn> table, string path)
    {
        Spawners = table;
        if (table.Count == 0)
        {
            Visible = false;
            return;
        }

        Visible = true;
        L_ConfigName.Text = path;

        var items = table.Select(z => new ComboItem(z.NameSummary.Replace("\"", ""), z)).ToArray();
        CB_Encounters.DisplayMember = nameof(ComboItem.Text);
        CB_Encounters.ValueMember = nameof(ComboItem.Value);
        CB_Encounters.DataSource = new BindingSource(items, string.Empty);

        CB_Encounters.SelectedIndex = 0;
    }

    private class ComboItem(string text, LandmarkItemSpawn value)
    {
        public string Text { get; } = text;
        public LandmarkItemSpawn Value { get; } = value;
    }

    private void CB_Encounters_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CB_Encounters.SelectedValue is not LandmarkItemSpawn spawner)
            throw new ArgumentException(nameof(CB_Encounters.SelectedValue));
        PG_Encounters.SelectedObject = spawner;
    }

    private void B_HighEncounterChance_Click(object sender, EventArgs e)
    {
        const int chance = 75;
        foreach (var spawner in Spawners)
        {
            spawner.ActivationRate = chance;
        }

        WinFormsUtil.Alert($"Changed all encounter chance to {chance}%.");
        CB_Encounters_SelectedIndexChanged(sender, e);
    }
}
