using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Controls;

public partial class EncounterTableEditor8a : UserControl
{
    public EncounterTable8a[] Tables = Array.Empty<EncounterTable8a>();

    public EncounterTableEditor8a() => InitializeComponent();

    public void LoadTable(EncounterTable8a[] table, string path)
    {
        Tables = table;
        if (table.Length == 0)
        {
            Visible = false;
            return;
        }

        Visible = true;
        L_ConfigName.Text = path;

        var items = table.Select(z => new ComboItem(z.TableName.Replace("\"", ""), z)).ToArray();
        CB_Encounters.DisplayMember = nameof(ComboItem.Text);
        CB_Encounters.ValueMember = nameof(ComboItem.Value);
        CB_Encounters.DataSource = new BindingSource(items, null);

        CB_Encounters.SelectedIndex = 0;
    }

    private class ComboItem
    {
        public ComboItem(string text, EncounterTable8a value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; }
        public EncounterTable8a Value { get; }
    }

    private void CB_Encounters_SelectedIndexChanged(object sender, EventArgs e)
    {
        PG_Encounters.SelectedObject = (EncounterTable8a)CB_Encounters.SelectedValue;
    }

    private void B_NoShinyLocks_Click(object sender, EventArgs e)
    {
        int ctr = 0;
        foreach (var table in Tables)
        {
            foreach (var slot in table.Table)
            {
                if (slot.ShinyLock != ShinyType8a.Never)
                    continue;
                slot.ShinyLock = ShinyType8a.Random;
                ctr++;
            }
        }

        WinFormsUtil.Alert($"Removed {ctr} locked slots.");
        CB_Encounters_SelectedIndexChanged(sender, e);
    }
}
