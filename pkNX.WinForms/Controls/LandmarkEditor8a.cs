using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Controls;

public partial class LandmarkEditor8a : UserControl
{
    public LandmarkItemSpawn8a[] Spawners = Array.Empty<LandmarkItemSpawn8a>();

    public LandmarkEditor8a() => InitializeComponent();

    public void LoadTable(LandmarkItemSpawn8a[] table, string path)
    {
        Spawners = table;
        if (table.Length == 0)
        {
            Visible = false;
            return;
        }

        Visible = true;
        L_ConfigName.Text = path;

        var items = table.Select(z => new ComboItem(z.NameSummary.Replace("\"", ""), z)).ToArray();
        CB_Encounters.DisplayMember = nameof(ComboItem.Text);
        CB_Encounters.ValueMember = nameof(ComboItem.Value);
        CB_Encounters.DataSource = new BindingSource(items, null);

        CB_Encounters.SelectedIndex = 0;
    }

    private class ComboItem
    {
        public ComboItem(string text, LandmarkItemSpawn8a value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; }
        public LandmarkItemSpawn8a Value { get; }
    }

    private void CB_Encounters_SelectedIndexChanged(object sender, EventArgs e)
    {
        PG_Encounters.SelectedObject = (LandmarkItemSpawn8a)CB_Encounters.SelectedValue;
    }

    private void B_HighEncounterChance_Click(object sender, EventArgs e)
    {
        const int chance = 75;
        foreach (var spawner in Spawners)
        {
            spawner.Field_04 = chance;
        }

        WinFormsUtil.Alert($"Changed all encounter chance to {chance}%.");
        CB_Encounters_SelectedIndexChanged(sender, e);
    }
}
