using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.WinForms.Controls;

public partial class PlacementSpawnerEditor8a : UserControl
{
    public IList<PlacementSpawner> Spawners = Array.Empty<PlacementSpawner>();

    public PlacementSpawnerEditor8a() => InitializeComponent();

    public void LoadTable(IList<PlacementSpawner> table, string path)
    {
        Spawners = table;
        if (table.Count == 0)
        {
            Visible = false;
            return;
        }

        Visible = true;
        L_ConfigName.Text = path;

        var items = table.Select(z => new ComboItem<PlacementSpawner>(z.NameSummary.Replace("\"", ""), z)).ToArray();
        CB_Encounters.DisplayMember = nameof(ComboItem<PlacementSpawner>.Text);
        CB_Encounters.ValueMember = nameof(ComboItem<PlacementSpawner>.Value);
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
        if (CB_Encounters.SelectedValue is not PlacementSpawner spawner)
            throw new ArgumentException(nameof(CB_Encounters.SelectedValue));
        PG_Spawner.SelectedObject = spawner;
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
