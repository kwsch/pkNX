using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures;
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

    private void PG_Encounters_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
    {
        object obj = e.NewSelection.Value;
        bool enable = obj is EncounterSlot8a;
        B_CloneTableEntry.Enabled = enable;
        B_ConfigureAsAlpha.Enabled = enable;
        B_RemoveCondition.Enabled = enable;
    }

    private void B_CloneTableEntry_Click(object sender, EventArgs e)
    {
        object obj = PG_Encounters.SelectedGridItem.Value;
        if (obj is EncounterSlot8a slotToClone)
        {
            var encounterTable = (EncounterTable8a)PG_Encounters.SelectedObject;
            encounterTable.Table = encounterTable.Table.Concat(new[] { (EncounterSlot8a)slotToClone.Clone() }).ToArray();
            PG_Encounters.Refresh();
        }
    }

    private void B_ConfigureAsAlpha_Click(object sender, EventArgs e)
    {
        object obj = PG_Encounters.SelectedGridItem.Value;
        if (obj is EncounterSlot8a slotToEdit)
        {
            slotToEdit.BaseProbability = 1;
            slotToEdit.Field_09 = true;
            slotToEdit.Field_10 = true;
            slotToEdit.Oybn.Field_02 = true;
            slotToEdit.Oybn.Field_03 = true;
            slotToEdit.Oybn.Oybn1 = true;
            slotToEdit.NumPerfectIvs = 3;

            PG_Encounters.Refresh();
        }
    }

    private void B_RemoveCondition_Click(object sender, EventArgs e)
    {
        object obj = PG_Encounters.SelectedGridItem.Value;
        if (obj is EncounterSlot8a slotToEdit)
        {
            slotToEdit.Eligibility.ConditionID = Condition8a.None;
            slotToEdit.Eligibility.ConditionTypeID = ConditionType8a.None;

            PG_Encounters.Refresh();
        }
    }
}
