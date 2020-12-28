using System;
using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms
{
    public sealed partial class ShinyRate : Form
    {
        private readonly ShinyRateInfo Data;
        private readonly bool Loaded;

        public ShinyRate(ShinyRateInfo info)
        {
            InitializeComponent();
            Data = info;
            // load initial state
            RB_Always.Enabled = Data.AllowAlways;
            if (Data.IsFixed)
            {
                RB_Fixed.Checked = true;
                NUD_Rerolls.Value = Data.GetFixedRate();
            }
            if (Data.IsAlways)
                RB_Always.Checked = true;
            if (Data.IsDefault)
                RB_Default.Checked = true;

            // force update labels
            ChangePercent(this, EventArgs.Empty);
            ChangeRerollCount(this, EventArgs.Empty);
            Loaded = true;
        }

        public bool Modified { get; set; }

        private void B_Save_Click(object sender, EventArgs e)
        {
            Modified = true;
            Close();
        }

        private void ChangeSelection(object sender, EventArgs e)
        {
            GB_Rerolls.Enabled = GB_RerollHelper.Enabled = sender == RB_Fixed;
            if (!Loaded)
                return;
            if (sender == RB_Default)
                Data.SetDefault();
            else if (sender == RB_Always)
                Data.SetAlwaysShiny();
            else
                Data.SetFixedRate((int)NUD_Rerolls.Value);
        }

        private void ChangeRerollCount(object sender, EventArgs e)
        {
            if (Loaded && RB_Fixed.Checked)
                Data.SetFixedRate((int)NUD_Rerolls.Value);

            int count = (int)NUD_Rerolls.Value;
            const int bc = 4096;
            var pct = 1 - Math.Pow((float)(bc - 1) / bc, count);
            L_Overall.Text = $"~{pct:P}";
        }

        private void ChangePercent(object sender, EventArgs e)
        {
            var pct = NUD_Rate.Value;
            const int bc = 4096;

            var inv = (int)Math.Log(1 - ((float)pct / 100), (float)(bc - 1) / bc);
            if (pct == 0)
                pct = 0.00001m; // arbitrary nonzero
            L_RerollCount.Text = $"Count: {inv:0} = 1:{(int)(1 / (pct / 100))}";
        }
    }
}
