using System;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers;
using PKHeX.Drawing.PokeSprite;

namespace pkNX.WinForms;

public partial class EncounterList : UserControl
{
    public EncounterList()
    {
        InitializeComponent();
    }

    public bool OverworldSpawn
    {
        set => NUD_Count.Visible = L_Count.Visible = value;
    }

    public bool ShowForm
    {
        set => dgv.Columns[FormColumn]!.Visible = value;
    }

    private const string FormColumn = nameof(FormColumn);

    public void Initialize()
    {
        var dgvPicture = new DataGridViewImageColumn
        {
            HeaderText = "Sprite",
            DisplayIndex = 0,
            Width = SpriteUtil.Spriter.Width + 2,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
            ReadOnly = true,
        };
        var padding = SpriteUtil.Spriter.Height > 40
            ? new Padding(0, (SpriteUtil.Spriter.Height / 2) - 8, 0, 0)
            : new Padding(0);
        var dgvSpecies = new DataGridViewComboBoxColumn
        {
            HeaderText = "Species",
            DisplayIndex = 1,
            Width = 135,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Padding = padding },
            FlatStyle = FlatStyle.Flat,
        };
        var dgvForm = new DataGridViewTextBoxColumn
        {
            Name = FormColumn,
            HeaderText = "Form",
            DisplayIndex = 2,
            Width = 45,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
        };
        var dgvPercent = new DataGridViewTextBoxColumn
        {
            HeaderText = "Chance",
            DisplayIndex = 3,
            Width = 52,
            DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
        };

        dgvSpecies.Items.AddRange(SpeciesNames);

        dgv.Columns.Add(dgvPicture);
        dgv.Columns.Add(dgvSpecies);
        dgv.Columns.Add(dgvForm);
        dgv.Columns.Add(dgvPercent);

        dgv.CellValueChanged += (s, e) =>
        {
            if (e.ColumnIndex == 0)
                return;
            UpdateRowImage(e.RowIndex);
        };
        dgv.Columns[0].DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
    }

    private void UpdateRowImage(int row)
    {
        var r = dgv.Rows[row];
        var cells = r.Cells;
        int sp = Array.IndexOf(SpeciesNames, cells[1].Value);
        var formstr = cells[2].Value?.ToString() ?? "0";
        if (!int.TryParse(formstr, out var form) || (uint) form > 100)
            cells[2].Value = 0;
        if (!int.TryParse(cells[2].Value?.ToString(), out var rate) || (uint)rate > 100)
            cells[3].Value = 0;

        cells[0].Value = SpriteUtil.GetSprite((ushort)sp, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never);
    }

    private EncounterSlot7b[]? Slots;
    public static string[] SpeciesNames { private get; set; }= Array.Empty<string>();

    public void LoadSlots(EncounterSlot7b[] slots)
    {
        Slots = slots;

        dgv.Rows.Clear();
        dgv.Rows.Add(slots.Length);
        // Fill Entries
        for (int i = 0; i < slots.Length; i++)
        {
            var row = dgv.Rows[i];
            row.Cells[1].Value = SpeciesNames[slots[i].Species];
            row.Cells[2].Value = slots[i].Form;
            row.Cells[3].Value = slots[i].Probability;
            row.Height = SpriteUtil.Spriter.Height + 2;
        }

        dgv.CancelEdit();
    }

    public void SaveCurrent()
    {
        if (Slots == null)
            return;
        for (int i = 0; i < Slots.Length; i++)
        {
            SaveRow(i, Slots[i]);
        }
    }

    private void SaveRow(int row, EncounterSlot7b s)
    {
        var cells = dgv.Rows[row].Cells;
        int sp = Array.IndexOf(SpeciesNames, cells[1].Value ?? SpeciesNames[0]);
        string formstr = cells[2].Value?.ToString() ?? "0";
        _ = short.TryParse(formstr, out var form);
        string probstr = cells[3].Value?.ToString() ?? "0";
        _ = int.TryParse(probstr, out var prob);

        if (sp == 0)
        {
            s.Species = s.Probability = 0;
            s.Form = 0;
            return;
        }

        s.Species = sp;
        s.Form = form;
        s.Probability = prob;
    }

    private void CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        if (dgv.IsCurrentCellDirty)
            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
}
