using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Controls;

public partial class PlacementSpawnerEditor8a : UserControl
{
    public PlacementSpawner8a[] Spawners = Array.Empty<PlacementSpawner8a>();

    public PlacementSpawnerEditor8a() => InitializeComponent();

    public void LoadTable(PlacementSpawner8a[] table, string path)
    {
        Spawners = table;
        if (table.Length == 0)
        {
            Visible = false;
            return;
        }

        Visible = true;
        L_ConfigName.Text = path;

        var items = table.Select(z => new ComboItem<PlacementSpawner8a>(z.NameSummary.Replace("\"", ""), z)).ToArray();
        CB_Encounters.DisplayMember = nameof(ComboItem<PlacementSpawner8a>.Text);
        CB_Encounters.ValueMember = nameof(ComboItem<PlacementSpawner8a>.Value);
        CB_Encounters.DataSource = new BindingSource(items, null);

        CB_Encounters.SelectedIndex = 0;
    }

    private class ComboItem<T>
    {
        public ComboItem(string text, T value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; }
        public T Value { get; }
    }

    private void CB_Encounters_SelectedIndexChanged(object sender, EventArgs e)
    {
        PG_Spawner.SelectedObject = (PlacementSpawner8a)CB_Encounters.SelectedValue;
    }

    private void B_MaxSpawnCountRange_Click(object sender, EventArgs e)
    {
        const int count = 8;
        foreach (var spawner in Spawners)
        {
            spawner.MinSpawnCount = spawner.MaxSpawnCount = count;
        }

        WinFormsUtil.Alert($"Set all to have {count} entities spawned.");
        CB_Encounters_SelectedIndexChanged(sender, e);
    }
}
