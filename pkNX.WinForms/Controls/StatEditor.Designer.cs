namespace pkNX.WinForms.Controls
{
    partial class StatEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FLP_Stats = new System.Windows.Forms.FlowLayoutPanel();
            this.FLP_StatHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.FLP_StatsHeaderRight = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_IVs = new System.Windows.Forms.Label();
            this.Label_EVs = new System.Windows.Forms.Label();
            this.Label_AVs = new System.Windows.Forms.Label();
            this.Label_Stats = new System.Windows.Forms.Label();
            this.FLP_HP = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_HP = new System.Windows.Forms.Label();
            this.TB_HPIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_HPEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_HPAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_HP = new System.Windows.Forms.MaskedTextBox();
            this.FLP_Atk = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_ATK = new System.Windows.Forms.Label();
            this.TB_ATKIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_ATKEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_ATKAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_ATK = new System.Windows.Forms.MaskedTextBox();
            this.FLP_Def = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_DEF = new System.Windows.Forms.Label();
            this.TB_DEFIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_DEFEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_DEFAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_DEF = new System.Windows.Forms.MaskedTextBox();
            this.FLP_SpA = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_SPA = new System.Windows.Forms.Label();
            this.TB_SPAIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPAEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPAAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_SPA = new System.Windows.Forms.MaskedTextBox();
            this.FLP_SpD = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_SPD = new System.Windows.Forms.Label();
            this.TB_SPDIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPDEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPDAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_SPD = new System.Windows.Forms.MaskedTextBox();
            this.FLP_Spe = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_SPE = new System.Windows.Forms.Label();
            this.TB_SPEIV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPEEV = new System.Windows.Forms.MaskedTextBox();
            this.TB_SPEAV = new System.Windows.Forms.MaskedTextBox();
            this.Stat_SPE = new System.Windows.Forms.MaskedTextBox();
            this.FLP_StatsTotal = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_Total = new System.Windows.Forms.Label();
            this.TB_IVTotal = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.TB_EVTotal = new System.Windows.Forms.TextBox();
            this.FLP_HPType = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_HiddenPowerPrefix = new System.Windows.Forms.Label();
            this.CB_HPType = new System.Windows.Forms.ComboBox();
            this.FLP_Stats.SuspendLayout();
            this.FLP_StatHeader.SuspendLayout();
            this.FLP_StatsHeaderRight.SuspendLayout();
            this.FLP_HP.SuspendLayout();
            this.FLP_Atk.SuspendLayout();
            this.FLP_Def.SuspendLayout();
            this.FLP_SpA.SuspendLayout();
            this.FLP_SpD.SuspendLayout();
            this.FLP_Spe.SuspendLayout();
            this.FLP_StatsTotal.SuspendLayout();
            this.FLP_HPType.SuspendLayout();
            this.SuspendLayout();
            //
            // FLP_Stats
            //
            this.FLP_Stats.Controls.Add(this.FLP_StatHeader);
            this.FLP_Stats.Controls.Add(this.FLP_HP);
            this.FLP_Stats.Controls.Add(this.FLP_Atk);
            this.FLP_Stats.Controls.Add(this.FLP_Def);
            this.FLP_Stats.Controls.Add(this.FLP_SpA);
            this.FLP_Stats.Controls.Add(this.FLP_SpD);
            this.FLP_Stats.Controls.Add(this.FLP_Spe);
            this.FLP_Stats.Controls.Add(this.FLP_StatsTotal);
            this.FLP_Stats.Controls.Add(this.FLP_HPType);
            this.FLP_Stats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLP_Stats.Location = new System.Drawing.Point(0, 0);
            this.FLP_Stats.Name = "FLP_Stats";
            this.FLP_Stats.Size = new System.Drawing.Size(230, 212);
            this.FLP_Stats.TabIndex = 451;
            //
            // FLP_StatHeader
            //
            this.FLP_StatHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_StatHeader.Controls.Add(this.FLP_StatsHeaderRight);
            this.FLP_StatHeader.Location = new System.Drawing.Point(0, 0);
            this.FLP_StatHeader.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_StatHeader.Name = "FLP_StatHeader";
            this.FLP_StatHeader.Size = new System.Drawing.Size(230, 22);
            this.FLP_StatHeader.TabIndex = 122;
            //
            // FLP_StatsHeaderRight
            //
            this.FLP_StatsHeaderRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_StatsHeaderRight.Controls.Add(this.Label_IVs);
            this.FLP_StatsHeaderRight.Controls.Add(this.Label_EVs);
            this.FLP_StatsHeaderRight.Controls.Add(this.Label_AVs);
            this.FLP_StatsHeaderRight.Controls.Add(this.Label_Stats);
            this.FLP_StatsHeaderRight.Location = new System.Drawing.Point(62, 0);
            this.FLP_StatsHeaderRight.Margin = new System.Windows.Forms.Padding(62, 0, 0, 0);
            this.FLP_StatsHeaderRight.Name = "FLP_StatsHeaderRight";
            this.FLP_StatsHeaderRight.Size = new System.Drawing.Size(138, 21);
            this.FLP_StatsHeaderRight.TabIndex = 123;
            //
            // Label_IVs
            //
            this.Label_IVs.Location = new System.Drawing.Point(0, 0);
            this.Label_IVs.Margin = new System.Windows.Forms.Padding(0);
            this.Label_IVs.Name = "Label_IVs";
            this.Label_IVs.Size = new System.Drawing.Size(30, 21);
            this.Label_IVs.TabIndex = 29;
            this.Label_IVs.Text = "IVs";
            this.Label_IVs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // Label_EVs
            //
            this.Label_EVs.Location = new System.Drawing.Point(30, 0);
            this.Label_EVs.Margin = new System.Windows.Forms.Padding(0);
            this.Label_EVs.Name = "Label_EVs";
            this.Label_EVs.Size = new System.Drawing.Size(32, 21);
            this.Label_EVs.TabIndex = 27;
            this.Label_EVs.Text = "EVs";
            this.Label_EVs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // Label_AVs
            //
            this.Label_AVs.Location = new System.Drawing.Point(62, 0);
            this.Label_AVs.Margin = new System.Windows.Forms.Padding(0);
            this.Label_AVs.Name = "Label_AVs";
            this.Label_AVs.Size = new System.Drawing.Size(35, 21);
            this.Label_AVs.TabIndex = 30;
            this.Label_AVs.Text = "AVs";
            this.Label_AVs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // Label_Stats
            //
            this.Label_Stats.Location = new System.Drawing.Point(97, 0);
            this.Label_Stats.Margin = new System.Windows.Forms.Padding(0);
            this.Label_Stats.Name = "Label_Stats";
            this.Label_Stats.Size = new System.Drawing.Size(35, 21);
            this.Label_Stats.TabIndex = 28;
            this.Label_Stats.Text = "Stats";
            this.Label_Stats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // FLP_HP
            //
            this.FLP_HP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_HP.Controls.Add(this.Label_HP);
            this.FLP_HP.Controls.Add(this.TB_HPIV);
            this.FLP_HP.Controls.Add(this.TB_HPEV);
            this.FLP_HP.Controls.Add(this.TB_HPAV);
            this.FLP_HP.Controls.Add(this.Stat_HP);
            this.FLP_HP.Location = new System.Drawing.Point(0, 22);
            this.FLP_HP.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_HP.Name = "FLP_HP";
            this.FLP_HP.Size = new System.Drawing.Size(230, 21);
            this.FLP_HP.TabIndex = 123;
            //
            // Label_HP
            //
            this.Label_HP.Location = new System.Drawing.Point(0, 0);
            this.Label_HP.Margin = new System.Windows.Forms.Padding(0);
            this.Label_HP.Name = "Label_HP";
            this.Label_HP.Size = new System.Drawing.Size(65, 21);
            this.Label_HP.TabIndex = 19;
            this.Label_HP.Text = "HP:";
            this.Label_HP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_HPIV
            //
            this.TB_HPIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_HPIV.Location = new System.Drawing.Point(65, 0);
            this.TB_HPIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_HPIV.Mask = "00";
            this.TB_HPIV.Name = "TB_HPIV";
            this.TB_HPIV.Size = new System.Drawing.Size(22, 20);
            this.TB_HPIV.TabIndex = 46;
            this.TB_HPIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_HPIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_HPEV
            //
            this.TB_HPEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_HPEV.Location = new System.Drawing.Point(93, 0);
            this.TB_HPEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_HPEV.Mask = "000";
            this.TB_HPEV.Name = "TB_HPEV";
            this.TB_HPEV.Size = new System.Drawing.Size(28, 20);
            this.TB_HPEV.TabIndex = 47;
            this.TB_HPEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_HPEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_HPAV
            //
            this.TB_HPAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_HPAV.Location = new System.Drawing.Point(127, 0);
            this.TB_HPAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_HPAV.Mask = "000";
            this.TB_HPAV.Name = "TB_HPAV";
            this.TB_HPAV.Size = new System.Drawing.Size(28, 20);
            this.TB_HPAV.TabIndex = 49;
            this.TB_HPAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_HPAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_HP
            //
            this.Stat_HP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_HP.Enabled = false;
            this.Stat_HP.Location = new System.Drawing.Point(161, 0);
            this.Stat_HP.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_HP.Mask = "00000";
            this.Stat_HP.Name = "Stat_HP";
            this.Stat_HP.PromptChar = ' ';
            this.Stat_HP.Size = new System.Drawing.Size(37, 20);
            this.Stat_HP.TabIndex = 48;
            this.Stat_HP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_Atk
            //
            this.FLP_Atk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_Atk.Controls.Add(this.Label_ATK);
            this.FLP_Atk.Controls.Add(this.TB_ATKIV);
            this.FLP_Atk.Controls.Add(this.TB_ATKEV);
            this.FLP_Atk.Controls.Add(this.TB_ATKAV);
            this.FLP_Atk.Controls.Add(this.Stat_ATK);
            this.FLP_Atk.Location = new System.Drawing.Point(0, 43);
            this.FLP_Atk.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_Atk.Name = "FLP_Atk";
            this.FLP_Atk.Size = new System.Drawing.Size(230, 21);
            this.FLP_Atk.TabIndex = 124;
            //
            // Label_ATK
            //
            this.Label_ATK.Location = new System.Drawing.Point(0, 0);
            this.Label_ATK.Margin = new System.Windows.Forms.Padding(0);
            this.Label_ATK.Name = "Label_ATK";
            this.Label_ATK.Size = new System.Drawing.Size(65, 21);
            this.Label_ATK.TabIndex = 20;
            this.Label_ATK.Text = "Atk:";
            this.Label_ATK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_ATKIV
            //
            this.TB_ATKIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_ATKIV.Location = new System.Drawing.Point(65, 0);
            this.TB_ATKIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_ATKIV.Mask = "00";
            this.TB_ATKIV.Name = "TB_ATKIV";
            this.TB_ATKIV.Size = new System.Drawing.Size(22, 20);
            this.TB_ATKIV.TabIndex = 47;
            this.TB_ATKIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_ATKIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_ATKEV
            //
            this.TB_ATKEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_ATKEV.Location = new System.Drawing.Point(93, 0);
            this.TB_ATKEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_ATKEV.Mask = "000";
            this.TB_ATKEV.Name = "TB_ATKEV";
            this.TB_ATKEV.Size = new System.Drawing.Size(28, 20);
            this.TB_ATKEV.TabIndex = 48;
            this.TB_ATKEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_ATKEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_ATKAV
            //
            this.TB_ATKAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_ATKAV.Location = new System.Drawing.Point(127, 0);
            this.TB_ATKAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_ATKAV.Mask = "000";
            this.TB_ATKAV.Name = "TB_ATKAV";
            this.TB_ATKAV.Size = new System.Drawing.Size(28, 20);
            this.TB_ATKAV.TabIndex = 50;
            this.TB_ATKAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_ATKAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_ATK
            //
            this.Stat_ATK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_ATK.Enabled = false;
            this.Stat_ATK.Location = new System.Drawing.Point(161, 0);
            this.Stat_ATK.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_ATK.Mask = "00000";
            this.Stat_ATK.Name = "Stat_ATK";
            this.Stat_ATK.PromptChar = ' ';
            this.Stat_ATK.Size = new System.Drawing.Size(37, 20);
            this.Stat_ATK.TabIndex = 49;
            this.Stat_ATK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_Def
            //
            this.FLP_Def.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_Def.Controls.Add(this.Label_DEF);
            this.FLP_Def.Controls.Add(this.TB_DEFIV);
            this.FLP_Def.Controls.Add(this.TB_DEFEV);
            this.FLP_Def.Controls.Add(this.TB_DEFAV);
            this.FLP_Def.Controls.Add(this.Stat_DEF);
            this.FLP_Def.Location = new System.Drawing.Point(0, 64);
            this.FLP_Def.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_Def.Name = "FLP_Def";
            this.FLP_Def.Size = new System.Drawing.Size(230, 21);
            this.FLP_Def.TabIndex = 125;
            //
            // Label_DEF
            //
            this.Label_DEF.Location = new System.Drawing.Point(0, 0);
            this.Label_DEF.Margin = new System.Windows.Forms.Padding(0);
            this.Label_DEF.Name = "Label_DEF";
            this.Label_DEF.Size = new System.Drawing.Size(65, 21);
            this.Label_DEF.TabIndex = 21;
            this.Label_DEF.Text = "Def:";
            this.Label_DEF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_DEFIV
            //
            this.TB_DEFIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_DEFIV.Location = new System.Drawing.Point(65, 0);
            this.TB_DEFIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_DEFIV.Mask = "00";
            this.TB_DEFIV.Name = "TB_DEFIV";
            this.TB_DEFIV.Size = new System.Drawing.Size(22, 20);
            this.TB_DEFIV.TabIndex = 48;
            this.TB_DEFIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_DEFIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_DEFEV
            //
            this.TB_DEFEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_DEFEV.Location = new System.Drawing.Point(93, 0);
            this.TB_DEFEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_DEFEV.Mask = "000";
            this.TB_DEFEV.Name = "TB_DEFEV";
            this.TB_DEFEV.Size = new System.Drawing.Size(28, 20);
            this.TB_DEFEV.TabIndex = 49;
            this.TB_DEFEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_DEFEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_DEFAV
            //
            this.TB_DEFAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_DEFAV.Location = new System.Drawing.Point(127, 0);
            this.TB_DEFAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_DEFAV.Mask = "000";
            this.TB_DEFAV.Name = "TB_DEFAV";
            this.TB_DEFAV.Size = new System.Drawing.Size(28, 20);
            this.TB_DEFAV.TabIndex = 51;
            this.TB_DEFAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_DEFAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_DEF
            //
            this.Stat_DEF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_DEF.Enabled = false;
            this.Stat_DEF.Location = new System.Drawing.Point(161, 0);
            this.Stat_DEF.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_DEF.Mask = "00000";
            this.Stat_DEF.Name = "Stat_DEF";
            this.Stat_DEF.PromptChar = ' ';
            this.Stat_DEF.Size = new System.Drawing.Size(37, 20);
            this.Stat_DEF.TabIndex = 50;
            this.Stat_DEF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_SpA
            //
            this.FLP_SpA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_SpA.Controls.Add(this.Label_SPA);
            this.FLP_SpA.Controls.Add(this.TB_SPAIV);
            this.FLP_SpA.Controls.Add(this.TB_SPAEV);
            this.FLP_SpA.Controls.Add(this.TB_SPAAV);
            this.FLP_SpA.Controls.Add(this.Stat_SPA);
            this.FLP_SpA.Location = new System.Drawing.Point(0, 85);
            this.FLP_SpA.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_SpA.Name = "FLP_SpA";
            this.FLP_SpA.Size = new System.Drawing.Size(230, 21);
            this.FLP_SpA.TabIndex = 126;
            //
            // Label_SPA
            //
            this.Label_SPA.Location = new System.Drawing.Point(0, 0);
            this.Label_SPA.Margin = new System.Windows.Forms.Padding(0);
            this.Label_SPA.Name = "Label_SPA";
            this.Label_SPA.Size = new System.Drawing.Size(65, 21);
            this.Label_SPA.TabIndex = 51;
            this.Label_SPA.Text = "SpA:";
            this.Label_SPA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_SPAIV
            //
            this.TB_SPAIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPAIV.Location = new System.Drawing.Point(65, 0);
            this.TB_SPAIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_SPAIV.Mask = "00";
            this.TB_SPAIV.Name = "TB_SPAIV";
            this.TB_SPAIV.Size = new System.Drawing.Size(22, 20);
            this.TB_SPAIV.TabIndex = 49;
            this.TB_SPAIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPAIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_SPAEV
            //
            this.TB_SPAEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPAEV.Location = new System.Drawing.Point(93, 0);
            this.TB_SPAEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPAEV.Mask = "000";
            this.TB_SPAEV.Name = "TB_SPAEV";
            this.TB_SPAEV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPAEV.TabIndex = 50;
            this.TB_SPAEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPAEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_SPAAV
            //
            this.TB_SPAAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPAAV.Location = new System.Drawing.Point(127, 0);
            this.TB_SPAAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPAAV.Mask = "000";
            this.TB_SPAAV.Name = "TB_SPAAV";
            this.TB_SPAAV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPAAV.TabIndex = 53;
            this.TB_SPAAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPAAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_SPA
            //
            this.Stat_SPA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_SPA.Enabled = false;
            this.Stat_SPA.Location = new System.Drawing.Point(161, 0);
            this.Stat_SPA.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_SPA.Mask = "00000";
            this.Stat_SPA.Name = "Stat_SPA";
            this.Stat_SPA.PromptChar = ' ';
            this.Stat_SPA.Size = new System.Drawing.Size(37, 20);
            this.Stat_SPA.TabIndex = 52;
            this.Stat_SPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_SpD
            //
            this.FLP_SpD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_SpD.Controls.Add(this.Label_SPD);
            this.FLP_SpD.Controls.Add(this.TB_SPDIV);
            this.FLP_SpD.Controls.Add(this.TB_SPDEV);
            this.FLP_SpD.Controls.Add(this.TB_SPDAV);
            this.FLP_SpD.Controls.Add(this.Stat_SPD);
            this.FLP_SpD.Location = new System.Drawing.Point(0, 106);
            this.FLP_SpD.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_SpD.Name = "FLP_SpD";
            this.FLP_SpD.Size = new System.Drawing.Size(230, 21);
            this.FLP_SpD.TabIndex = 127;
            //
            // Label_SPD
            //
            this.Label_SPD.Location = new System.Drawing.Point(0, 0);
            this.Label_SPD.Margin = new System.Windows.Forms.Padding(0);
            this.Label_SPD.Name = "Label_SPD";
            this.Label_SPD.Size = new System.Drawing.Size(65, 21);
            this.Label_SPD.TabIndex = 23;
            this.Label_SPD.Text = "SpD:";
            this.Label_SPD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_SPDIV
            //
            this.TB_SPDIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPDIV.Location = new System.Drawing.Point(65, 0);
            this.TB_SPDIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_SPDIV.Mask = "00";
            this.TB_SPDIV.Name = "TB_SPDIV";
            this.TB_SPDIV.Size = new System.Drawing.Size(22, 20);
            this.TB_SPDIV.TabIndex = 5;
            this.TB_SPDIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPDIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_SPDEV
            //
            this.TB_SPDEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPDEV.Location = new System.Drawing.Point(93, 0);
            this.TB_SPDEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPDEV.Mask = "000";
            this.TB_SPDEV.Name = "TB_SPDEV";
            this.TB_SPDEV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPDEV.TabIndex = 11;
            this.TB_SPDEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPDEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_SPDAV
            //
            this.TB_SPDAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPDAV.Location = new System.Drawing.Point(127, 0);
            this.TB_SPDAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPDAV.Mask = "000";
            this.TB_SPDAV.Name = "TB_SPDAV";
            this.TB_SPDAV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPDAV.TabIndex = 50;
            this.TB_SPDAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPDAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_SPD
            //
            this.Stat_SPD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_SPD.Enabled = false;
            this.Stat_SPD.Location = new System.Drawing.Point(161, 0);
            this.Stat_SPD.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_SPD.Mask = "00000";
            this.Stat_SPD.Name = "Stat_SPD";
            this.Stat_SPD.PromptChar = ' ';
            this.Stat_SPD.Size = new System.Drawing.Size(37, 20);
            this.Stat_SPD.TabIndex = 49;
            this.Stat_SPD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_Spe
            //
            this.FLP_Spe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_Spe.Controls.Add(this.Label_SPE);
            this.FLP_Spe.Controls.Add(this.TB_SPEIV);
            this.FLP_Spe.Controls.Add(this.TB_SPEEV);
            this.FLP_Spe.Controls.Add(this.TB_SPEAV);
            this.FLP_Spe.Controls.Add(this.Stat_SPE);
            this.FLP_Spe.Location = new System.Drawing.Point(0, 127);
            this.FLP_Spe.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_Spe.Name = "FLP_Spe";
            this.FLP_Spe.Size = new System.Drawing.Size(230, 21);
            this.FLP_Spe.TabIndex = 128;
            //
            // Label_SPE
            //
            this.Label_SPE.Location = new System.Drawing.Point(0, 0);
            this.Label_SPE.Margin = new System.Windows.Forms.Padding(0);
            this.Label_SPE.Name = "Label_SPE";
            this.Label_SPE.Size = new System.Drawing.Size(65, 21);
            this.Label_SPE.TabIndex = 24;
            this.Label_SPE.Text = "Spe:";
            this.Label_SPE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_SPEIV
            //
            this.TB_SPEIV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPEIV.Location = new System.Drawing.Point(65, 0);
            this.TB_SPEIV.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_SPEIV.Mask = "00";
            this.TB_SPEIV.Name = "TB_SPEIV";
            this.TB_SPEIV.Size = new System.Drawing.Size(22, 20);
            this.TB_SPEIV.TabIndex = 51;
            this.TB_SPEIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPEIV.TextChanged += new System.EventHandler(this.UpdateIV);
            //
            // TB_SPEEV
            //
            this.TB_SPEEV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPEEV.Location = new System.Drawing.Point(93, 0);
            this.TB_SPEEV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPEEV.Mask = "000";
            this.TB_SPEEV.Name = "TB_SPEEV";
            this.TB_SPEEV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPEEV.TabIndex = 52;
            this.TB_SPEEV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPEEV.TextChanged += new System.EventHandler(this.UpdateEV);
            //
            // TB_SPEAV
            //
            this.TB_SPEAV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SPEAV.Location = new System.Drawing.Point(127, 0);
            this.TB_SPEAV.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_SPEAV.Mask = "000";
            this.TB_SPEAV.Name = "TB_SPEAV";
            this.TB_SPEAV.Size = new System.Drawing.Size(28, 20);
            this.TB_SPEAV.TabIndex = 54;
            this.TB_SPEAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_SPEAV.TextChanged += new System.EventHandler(this.UpdateAV);
            //
            // Stat_SPE
            //
            this.Stat_SPE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stat_SPE.Enabled = false;
            this.Stat_SPE.Location = new System.Drawing.Point(161, 0);
            this.Stat_SPE.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Stat_SPE.Mask = "00000";
            this.Stat_SPE.Name = "Stat_SPE";
            this.Stat_SPE.PromptChar = ' ';
            this.Stat_SPE.Size = new System.Drawing.Size(37, 20);
            this.Stat_SPE.TabIndex = 53;
            this.Stat_SPE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_StatsTotal
            //
            this.FLP_StatsTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_StatsTotal.Controls.Add(this.Label_Total);
            this.FLP_StatsTotal.Controls.Add(this.TB_IVTotal);
            this.FLP_StatsTotal.Controls.Add(this.textBox1);
            this.FLP_StatsTotal.Controls.Add(this.TB_EVTotal);
            this.FLP_StatsTotal.Location = new System.Drawing.Point(0, 148);
            this.FLP_StatsTotal.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_StatsTotal.Name = "FLP_StatsTotal";
            this.FLP_StatsTotal.Size = new System.Drawing.Size(172, 21);
            this.FLP_StatsTotal.TabIndex = 129;
            //
            // Label_Total
            //
            this.Label_Total.Location = new System.Drawing.Point(0, 0);
            this.Label_Total.Margin = new System.Windows.Forms.Padding(0);
            this.Label_Total.Name = "Label_Total";
            this.Label_Total.Size = new System.Drawing.Size(65, 21);
            this.Label_Total.TabIndex = 25;
            this.Label_Total.Text = "Total:";
            this.Label_Total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // TB_IVTotal
            //
            this.TB_IVTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_IVTotal.Enabled = false;
            this.TB_IVTotal.Location = new System.Drawing.Point(65, 0);
            this.TB_IVTotal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.TB_IVTotal.MaxLength = 3;
            this.TB_IVTotal.Name = "TB_IVTotal";
            this.TB_IVTotal.Size = new System.Drawing.Size(22, 20);
            this.TB_IVTotal.TabIndex = 44;
            this.TB_IVTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // textBox1
            //
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(93, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(28, 20);
            this.textBox1.TabIndex = 45;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // TB_EVTotal
            //
            this.TB_EVTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_EVTotal.Enabled = false;
            this.TB_EVTotal.Location = new System.Drawing.Point(127, 0);
            this.TB_EVTotal.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.TB_EVTotal.MaxLength = 3;
            this.TB_EVTotal.Name = "TB_EVTotal";
            this.TB_EVTotal.Size = new System.Drawing.Size(28, 20);
            this.TB_EVTotal.TabIndex = 43;
            this.TB_EVTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // FLP_HPType
            //
            this.FLP_HPType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.FLP_HPType.Controls.Add(this.Label_HiddenPowerPrefix);
            this.FLP_HPType.Controls.Add(this.CB_HPType);
            this.FLP_HPType.Location = new System.Drawing.Point(0, 169);
            this.FLP_HPType.Margin = new System.Windows.Forms.Padding(0);
            this.FLP_HPType.Name = "FLP_HPType";
            this.FLP_HPType.Size = new System.Drawing.Size(172, 21);
            this.FLP_HPType.TabIndex = 130;
            //
            // Label_HiddenPowerPrefix
            //
            this.Label_HiddenPowerPrefix.Location = new System.Drawing.Point(0, 0);
            this.Label_HiddenPowerPrefix.Margin = new System.Windows.Forms.Padding(0);
            this.Label_HiddenPowerPrefix.Name = "Label_HiddenPowerPrefix";
            this.Label_HiddenPowerPrefix.Size = new System.Drawing.Size(102, 21);
            this.Label_HiddenPowerPrefix.TabIndex = 29;
            this.Label_HiddenPowerPrefix.Text = "Hidden Power:";
            this.Label_HiddenPowerPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // CB_HPType
            //
            this.CB_HPType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_HPType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_HPType.DropDownWidth = 80;
            this.CB_HPType.FormattingEnabled = true;
            this.CB_HPType.Location = new System.Drawing.Point(102, 0);
            this.CB_HPType.Margin = new System.Windows.Forms.Padding(0);
            this.CB_HPType.Name = "CB_HPType";
            this.CB_HPType.Size = new System.Drawing.Size(70, 21);
            this.CB_HPType.TabIndex = 44;
            this.CB_HPType.SelectedIndexChanged += new System.EventHandler(this.ChangeHPType);
            //
            // StatEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FLP_Stats);
            this.Name = "StatEditor";
            this.Size = new System.Drawing.Size(230, 212);
            this.FLP_Stats.ResumeLayout(false);
            this.FLP_StatHeader.ResumeLayout(false);
            this.FLP_StatsHeaderRight.ResumeLayout(false);
            this.FLP_HP.ResumeLayout(false);
            this.FLP_HP.PerformLayout();
            this.FLP_Atk.ResumeLayout(false);
            this.FLP_Atk.PerformLayout();
            this.FLP_Def.ResumeLayout(false);
            this.FLP_Def.PerformLayout();
            this.FLP_SpA.ResumeLayout(false);
            this.FLP_SpA.PerformLayout();
            this.FLP_SpD.ResumeLayout(false);
            this.FLP_SpD.PerformLayout();
            this.FLP_Spe.ResumeLayout(false);
            this.FLP_Spe.PerformLayout();
            this.FLP_StatsTotal.ResumeLayout(false);
            this.FLP_StatsTotal.PerformLayout();
            this.FLP_HPType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FLP_Stats;
        private System.Windows.Forms.FlowLayoutPanel FLP_StatHeader;
        private System.Windows.Forms.FlowLayoutPanel FLP_StatsHeaderRight;
        private System.Windows.Forms.Label Label_IVs;
        private System.Windows.Forms.Label Label_EVs;
        private System.Windows.Forms.Label Label_Stats;
        private System.Windows.Forms.FlowLayoutPanel FLP_HP;
        private System.Windows.Forms.Label Label_HP;
        private System.Windows.Forms.MaskedTextBox TB_HPIV;
        private System.Windows.Forms.MaskedTextBox TB_HPEV;
        private System.Windows.Forms.MaskedTextBox Stat_HP;
        private System.Windows.Forms.FlowLayoutPanel FLP_Atk;
        private System.Windows.Forms.Label Label_ATK;
        private System.Windows.Forms.MaskedTextBox TB_ATKIV;
        private System.Windows.Forms.MaskedTextBox TB_ATKEV;
        private System.Windows.Forms.MaskedTextBox Stat_ATK;
        private System.Windows.Forms.FlowLayoutPanel FLP_Def;
        private System.Windows.Forms.Label Label_DEF;
        private System.Windows.Forms.MaskedTextBox TB_DEFIV;
        private System.Windows.Forms.MaskedTextBox TB_DEFEV;
        private System.Windows.Forms.MaskedTextBox Stat_DEF;
        private System.Windows.Forms.FlowLayoutPanel FLP_SpA;
        private System.Windows.Forms.Label Label_SPA;
        private System.Windows.Forms.MaskedTextBox TB_SPAIV;
        private System.Windows.Forms.MaskedTextBox TB_SPAEV;
        private System.Windows.Forms.MaskedTextBox Stat_SPA;
        private System.Windows.Forms.FlowLayoutPanel FLP_SpD;
        private System.Windows.Forms.Label Label_SPD;
        private System.Windows.Forms.MaskedTextBox TB_SPDIV;
        private System.Windows.Forms.MaskedTextBox TB_SPDEV;
        private System.Windows.Forms.MaskedTextBox Stat_SPD;
        private System.Windows.Forms.FlowLayoutPanel FLP_Spe;
        private System.Windows.Forms.Label Label_SPE;
        private System.Windows.Forms.MaskedTextBox TB_SPEIV;
        private System.Windows.Forms.MaskedTextBox TB_SPEEV;
        private System.Windows.Forms.MaskedTextBox Stat_SPE;
        private System.Windows.Forms.FlowLayoutPanel FLP_StatsTotal;
        private System.Windows.Forms.Label Label_Total;
        private System.Windows.Forms.TextBox TB_IVTotal;
        private System.Windows.Forms.TextBox TB_EVTotal;
        private System.Windows.Forms.FlowLayoutPanel FLP_HPType;
        private System.Windows.Forms.Label Label_HiddenPowerPrefix;
        private System.Windows.Forms.ComboBox CB_HPType;
        private System.Windows.Forms.Label Label_AVs;
        private System.Windows.Forms.MaskedTextBox TB_HPAV;
        private System.Windows.Forms.MaskedTextBox TB_ATKAV;
        private System.Windows.Forms.MaskedTextBox TB_DEFAV;
        private System.Windows.Forms.MaskedTextBox TB_SPAAV;
        private System.Windows.Forms.MaskedTextBox TB_SPDAV;
        private System.Windows.Forms.MaskedTextBox TB_SPEAV;
        private System.Windows.Forms.TextBox textBox1;
    }
}
