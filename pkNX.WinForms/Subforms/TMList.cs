using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Randomization;

namespace pkNX.WinForms
{
    public sealed partial class TMList : Form
    {
        private readonly int[] AllowedMoves;
        private readonly string[] MoveNames;
        private readonly ushort[] OriginalMoves;

        public TMList(ushort[] moves, int[] allowed, string[] movenames)
        {
            InitializeComponent();
            MoveNames = EditorUtil.SanitizeMoveList(movenames);
            AllowedMoves = allowed;
            SetupDGV(MoveNames);
            LoadMoves(moves);
            OriginalMoves = moves;
        }

        public bool Modified { get; set; }
        public ushort[] FinalMoves { get; private set; }

        private void SetupDGV(string[] list)
        {
            dgvTM.Columns.Clear();
            DataGridViewColumn dgvIndex = new DataGridViewTextBoxColumn();
            {
                dgvIndex.HeaderText = "Index";
                dgvIndex.DisplayIndex = 0;
                dgvIndex.Width = 45;
                dgvIndex.ReadOnly = true;
                dgvIndex.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvIndex.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            DataGridViewComboBoxColumn dgvMove = new DataGridViewComboBoxColumn();
            {
                dgvMove.HeaderText = "Move";
                dgvMove.DisplayIndex = 1;
                foreach (string t in list)
                    dgvMove.Items.Add(t); // add only the Names

                dgvMove.Width = 133;
                dgvMove.FlatStyle = FlatStyle.Flat;
                dgvIndex.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgvTM.Columns.Add(dgvIndex);
            dgvTM.Columns.Add(dgvMove);
        }

        public void LoadMoves(ushort[] tmlist)
        {
            dgvTM.Rows.Clear();

            for (int i = 0; i < tmlist.Length; i++)
            {
                dgvTM.Rows.Add();
                dgvTM.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvTM.Rows[i].Cells[1].Value = MoveNames[tmlist[i]];
            }
        }

        public ushort[] SaveMoves()
        {
            ushort[] moves = new ushort[dgvTM.RowCount];
            for (int i = 0; i < moves.Length; i++)
                moves[i] = (ushort)Array.IndexOf(MoveNames, dgvTM.Rows[i].Cells[1].Value);

            return moves;
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            Modified = true;
            FinalMoves = SaveMoves();
            Close();
        }

        private void B_RTM_Click(object sender, EventArgs e)
        {
            var moves = GetRandomMoves();
            LoadMoves(moves);
            System.Media.SystemSounds.Asterisk.Play();
        }

        private ushort[] GetRandomMoves()
        {
            var allowed = AllowedMoves.Select(z => (ushort)z).Except(new ushort[] { 0 }).ToArray();
            var rand = new GenericRandomizer<ushort>(allowed);
            return rand.GetMany(OriginalMoves.Length);
        }
    }
}
