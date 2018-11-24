namespace pkNX.WinForms
{
    partial class GGWE
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CB_Location = new System.Windows.Forms.ComboBox();
            this.NUD_RankMin = new System.Windows.Forms.NumericUpDown();
            this.NUD_RankMax = new System.Windows.Forms.NumericUpDown();
            this.L_Rank = new System.Windows.Forms.Label();
            this.TC_Tables = new System.Windows.Forms.TabControl();
            this.Tab_Ground = new System.Windows.Forms.TabPage();
            this.EL_Ground = new pkNX.WinForms.EncounterList();
            this.Tab_Water = new System.Windows.Forms.TabPage();
            this.EL_Water = new pkNX.WinForms.EncounterList();
            this.Tab_Old = new System.Windows.Forms.TabPage();
            this.EL_Old = new pkNX.WinForms.EncounterList();
            this.Tab_Good = new System.Windows.Forms.TabPage();
            this.EL_Good = new pkNX.WinForms.EncounterList();
            this.Tab_Super = new System.Windows.Forms.TabPage();
            this.EL_Super = new pkNX.WinForms.EncounterList();
            this.Tab_Sky = new System.Windows.Forms.TabPage();
            this.EL_Sky = new pkNX.WinForms.EncounterList();
            this.Tab_Rand = new System.Windows.Forms.TabPage();
            this.L_SpawnDuration = new System.Windows.Forms.Label();
            this.L_SpawnCount = new System.Windows.Forms.Label();
            this.L_SpawnRate = new System.Windows.Forms.Label();
            this.CHK_FillEmpty = new System.Windows.Forms.CheckBox();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.PG_Species = new System.Windows.Forms.PropertyGrid();
            this.NUD_ModDuration = new System.Windows.Forms.NumericUpDown();
            this.NUD_ModRate = new System.Windows.Forms.NumericUpDown();
            this.NUD_ModCount = new System.Windows.Forms.NumericUpDown();
            this.B_ModDuration = new System.Windows.Forms.Button();
            this.B_ModRate = new System.Windows.Forms.Button();
            this.B_ModSpawn = new System.Windows.Forms.Button();
            this.L_Hash = new System.Windows.Forms.Label();
            this.B_Save = new System.Windows.Forms.Button();
            this.CHK_WildMega = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMax)).BeginInit();
            this.TC_Tables.SuspendLayout();
            this.Tab_Ground.SuspendLayout();
            this.Tab_Water.SuspendLayout();
            this.Tab_Old.SuspendLayout();
            this.Tab_Good.SuspendLayout();
            this.Tab_Super.SuspendLayout();
            this.Tab_Sky.SuspendLayout();
            this.Tab_Rand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModCount)).BeginInit();
            this.SuspendLayout();
            //
            // CB_Location
            //
            this.CB_Location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Location.FormattingEnabled = true;
            this.CB_Location.Location = new System.Drawing.Point(12, 12);
            this.CB_Location.Name = "CB_Location";
            this.CB_Location.Size = new System.Drawing.Size(207, 21);
            this.CB_Location.TabIndex = 3;
            this.CB_Location.SelectedIndexChanged += new System.EventHandler(this.CB_Location_SelectedIndexChanged);
            //
            // NUD_RankMin
            //
            this.NUD_RankMin.Location = new System.Drawing.Point(195, 52);
            this.NUD_RankMin.Name = "NUD_RankMin";
            this.NUD_RankMin.Size = new System.Drawing.Size(51, 20);
            this.NUD_RankMin.TabIndex = 7;
            //
            // NUD_RankMax
            //
            this.NUD_RankMax.Location = new System.Drawing.Point(252, 52);
            this.NUD_RankMax.Name = "NUD_RankMax";
            this.NUD_RankMax.Size = new System.Drawing.Size(51, 20);
            this.NUD_RankMax.TabIndex = 8;
            //
            // L_Rank
            //
            this.L_Rank.AutoSize = true;
            this.L_Rank.Location = new System.Drawing.Point(192, 36);
            this.L_Rank.Name = "L_Rank";
            this.L_Rank.Size = new System.Drawing.Size(120, 13);
            this.L_Rank.TabIndex = 9;
            this.L_Rank.Text = "Trainer Rank Min / Max";
            //
            // TC_Tables
            //
            this.TC_Tables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TC_Tables.Controls.Add(this.Tab_Ground);
            this.TC_Tables.Controls.Add(this.Tab_Water);
            this.TC_Tables.Controls.Add(this.Tab_Old);
            this.TC_Tables.Controls.Add(this.Tab_Good);
            this.TC_Tables.Controls.Add(this.Tab_Super);
            this.TC_Tables.Controls.Add(this.Tab_Sky);
            this.TC_Tables.Controls.Add(this.Tab_Rand);
            this.TC_Tables.Location = new System.Drawing.Point(12, 78);
            this.TC_Tables.Name = "TC_Tables";
            this.TC_Tables.SelectedIndex = 0;
            this.TC_Tables.Size = new System.Drawing.Size(321, 426);
            this.TC_Tables.TabIndex = 10;
            //
            // Tab_Ground
            //
            this.Tab_Ground.Controls.Add(this.EL_Ground);
            this.Tab_Ground.Location = new System.Drawing.Point(4, 22);
            this.Tab_Ground.Name = "Tab_Ground";
            this.Tab_Ground.Size = new System.Drawing.Size(313, 400);
            this.Tab_Ground.TabIndex = 0;
            this.Tab_Ground.Text = "Ground";
            this.Tab_Ground.UseVisualStyleBackColor = true;
            //
            // EL_Ground
            //
            this.EL_Ground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Ground.Location = new System.Drawing.Point(0, 0);
            this.EL_Ground.Name = "EL_Ground";
            this.EL_Ground.Size = new System.Drawing.Size(313, 400);
            this.EL_Ground.TabIndex = 14;
            //
            // Tab_Water
            //
            this.Tab_Water.Controls.Add(this.EL_Water);
            this.Tab_Water.Location = new System.Drawing.Point(4, 22);
            this.Tab_Water.Name = "Tab_Water";
            this.Tab_Water.Size = new System.Drawing.Size(313, 400);
            this.Tab_Water.TabIndex = 1;
            this.Tab_Water.Text = "Water";
            this.Tab_Water.UseVisualStyleBackColor = true;
            //
            // EL_Water
            //
            this.EL_Water.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Water.Location = new System.Drawing.Point(0, 0);
            this.EL_Water.Name = "EL_Water";
            this.EL_Water.Size = new System.Drawing.Size(313, 400);
            this.EL_Water.TabIndex = 13;
            //
            // Tab_Old
            //
            this.Tab_Old.Controls.Add(this.EL_Old);
            this.Tab_Old.Location = new System.Drawing.Point(4, 22);
            this.Tab_Old.Name = "Tab_Old";
            this.Tab_Old.Size = new System.Drawing.Size(313, 400);
            this.Tab_Old.TabIndex = 2;
            this.Tab_Old.Text = "Old";
            this.Tab_Old.UseVisualStyleBackColor = true;
            //
            // EL_Old
            //
            this.EL_Old.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Old.Location = new System.Drawing.Point(0, 0);
            this.EL_Old.Name = "EL_Old";
            this.EL_Old.Size = new System.Drawing.Size(313, 400);
            this.EL_Old.TabIndex = 13;
            //
            // Tab_Good
            //
            this.Tab_Good.Controls.Add(this.EL_Good);
            this.Tab_Good.Location = new System.Drawing.Point(4, 22);
            this.Tab_Good.Name = "Tab_Good";
            this.Tab_Good.Size = new System.Drawing.Size(313, 400);
            this.Tab_Good.TabIndex = 3;
            this.Tab_Good.Text = "Good";
            this.Tab_Good.UseVisualStyleBackColor = true;
            //
            // EL_Good
            //
            this.EL_Good.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Good.Location = new System.Drawing.Point(0, 0);
            this.EL_Good.Name = "EL_Good";
            this.EL_Good.Size = new System.Drawing.Size(313, 400);
            this.EL_Good.TabIndex = 13;
            //
            // Tab_Super
            //
            this.Tab_Super.Controls.Add(this.EL_Super);
            this.Tab_Super.Location = new System.Drawing.Point(4, 22);
            this.Tab_Super.Name = "Tab_Super";
            this.Tab_Super.Size = new System.Drawing.Size(313, 400);
            this.Tab_Super.TabIndex = 4;
            this.Tab_Super.Text = "Super";
            this.Tab_Super.UseVisualStyleBackColor = true;
            //
            // EL_Super
            //
            this.EL_Super.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Super.Location = new System.Drawing.Point(0, 0);
            this.EL_Super.Name = "EL_Super";
            this.EL_Super.Size = new System.Drawing.Size(313, 400);
            this.EL_Super.TabIndex = 12;
            //
            // Tab_Sky
            //
            this.Tab_Sky.Controls.Add(this.EL_Sky);
            this.Tab_Sky.Location = new System.Drawing.Point(4, 22);
            this.Tab_Sky.Name = "Tab_Sky";
            this.Tab_Sky.Size = new System.Drawing.Size(313, 400);
            this.Tab_Sky.TabIndex = 5;
            this.Tab_Sky.Text = "Sky";
            this.Tab_Sky.UseVisualStyleBackColor = true;
            //
            // EL_Sky
            //
            this.EL_Sky.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EL_Sky.Location = new System.Drawing.Point(0, 0);
            this.EL_Sky.Name = "EL_Sky";
            this.EL_Sky.Size = new System.Drawing.Size(313, 400);
            this.EL_Sky.TabIndex = 12;
            //
            // Tab_Rand
            //
            this.Tab_Rand.Controls.Add(this.CHK_WildMega);
            this.Tab_Rand.Controls.Add(this.L_SpawnDuration);
            this.Tab_Rand.Controls.Add(this.L_SpawnCount);
            this.Tab_Rand.Controls.Add(this.L_SpawnRate);
            this.Tab_Rand.Controls.Add(this.CHK_FillEmpty);
            this.Tab_Rand.Controls.Add(this.B_RandAll);
            this.Tab_Rand.Controls.Add(this.PG_Species);
            this.Tab_Rand.Controls.Add(this.NUD_ModDuration);
            this.Tab_Rand.Controls.Add(this.NUD_ModRate);
            this.Tab_Rand.Controls.Add(this.NUD_ModCount);
            this.Tab_Rand.Controls.Add(this.B_ModDuration);
            this.Tab_Rand.Controls.Add(this.B_ModRate);
            this.Tab_Rand.Controls.Add(this.B_ModSpawn);
            this.Tab_Rand.Location = new System.Drawing.Point(4, 22);
            this.Tab_Rand.Name = "Tab_Rand";
            this.Tab_Rand.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Rand.Size = new System.Drawing.Size(313, 400);
            this.Tab_Rand.TabIndex = 6;
            this.Tab_Rand.Text = "Rand";
            this.Tab_Rand.UseVisualStyleBackColor = true;
            //
            // L_SpawnDuration
            //
            this.L_SpawnDuration.AutoSize = true;
            this.L_SpawnDuration.Location = new System.Drawing.Point(202, 77);
            this.L_SpawnDuration.Name = "L_SpawnDuration";
            this.L_SpawnDuration.Size = new System.Drawing.Size(47, 13);
            this.L_SpawnDuration.TabIndex = 11;
            this.L_SpawnDuration.Text = "seconds";
            //
            // L_SpawnCount
            //
            this.L_SpawnCount.AutoSize = true;
            this.L_SpawnCount.Location = new System.Drawing.Point(202, 13);
            this.L_SpawnCount.Name = "L_SpawnCount";
            this.L_SpawnCount.Size = new System.Drawing.Size(112, 13);
            this.L_SpawnCount.TabIndex = 10;
            this.L_SpawnCount.Text = "Max overworld models";
            //
            // L_SpawnRate
            //
            this.L_SpawnRate.AutoSize = true;
            this.L_SpawnRate.Location = new System.Drawing.Point(202, 45);
            this.L_SpawnRate.Name = "L_SpawnRate";
            this.L_SpawnRate.Size = new System.Drawing.Size(15, 13);
            this.L_SpawnRate.TabIndex = 9;
            this.L_SpawnRate.Text = "%";
            //
            // CHK_FillEmpty
            //
            this.CHK_FillEmpty.AutoSize = true;
            this.CHK_FillEmpty.Location = new System.Drawing.Point(154, 108);
            this.CHK_FillEmpty.Name = "CHK_FillEmpty";
            this.CHK_FillEmpty.Size = new System.Drawing.Size(96, 17);
            this.CHK_FillEmpty.TabIndex = 8;
            this.CHK_FillEmpty.Text = "Fill Empty Slots";
            this.CHK_FillEmpty.UseVisualStyleBackColor = true;
            //
            // B_RandAll
            //
            this.B_RandAll.Location = new System.Drawing.Point(6, 110);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(142, 26);
            this.B_RandAll.TabIndex = 7;
            this.B_RandAll.Text = "Randomize All Species";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            //
            // PG_Species
            //
            this.PG_Species.Location = new System.Drawing.Point(0, 142);
            this.PG_Species.Name = "PG_Species";
            this.PG_Species.Size = new System.Drawing.Size(313, 258);
            this.PG_Species.TabIndex = 6;
            this.PG_Species.ToolbarVisible = false;
            //
            // NUD_ModDuration
            //
            this.NUD_ModDuration.Location = new System.Drawing.Point(149, 75);
            this.NUD_ModDuration.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NUD_ModDuration.Name = "NUD_ModDuration";
            this.NUD_ModDuration.Size = new System.Drawing.Size(47, 20);
            this.NUD_ModDuration.TabIndex = 5;
            this.NUD_ModDuration.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            //
            // NUD_ModRate
            //
            this.NUD_ModRate.Location = new System.Drawing.Point(149, 43);
            this.NUD_ModRate.Name = "NUD_ModRate";
            this.NUD_ModRate.Size = new System.Drawing.Size(47, 20);
            this.NUD_ModRate.TabIndex = 4;
            this.NUD_ModRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // NUD_ModCount
            //
            this.NUD_ModCount.Location = new System.Drawing.Point(149, 11);
            this.NUD_ModCount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NUD_ModCount.Name = "NUD_ModCount";
            this.NUD_ModCount.Size = new System.Drawing.Size(47, 20);
            this.NUD_ModCount.TabIndex = 3;
            this.NUD_ModCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // B_ModDuration
            //
            this.B_ModDuration.Location = new System.Drawing.Point(6, 70);
            this.B_ModDuration.Name = "B_ModDuration";
            this.B_ModDuration.Size = new System.Drawing.Size(142, 26);
            this.B_ModDuration.TabIndex = 2;
            this.B_ModDuration.Text = "Modify All Spawn Duration";
            this.B_ModDuration.UseVisualStyleBackColor = true;
            this.B_ModDuration.Click += new System.EventHandler(this.B_ModDuration_Click);
            //
            // B_ModRate
            //
            this.B_ModRate.Location = new System.Drawing.Point(6, 38);
            this.B_ModRate.Name = "B_ModRate";
            this.B_ModRate.Size = new System.Drawing.Size(142, 26);
            this.B_ModRate.TabIndex = 1;
            this.B_ModRate.Text = "Modify All Spawn Rate";
            this.B_ModRate.UseVisualStyleBackColor = true;
            this.B_ModRate.Click += new System.EventHandler(this.B_ModRate_Click);
            //
            // B_ModSpawn
            //
            this.B_ModSpawn.Location = new System.Drawing.Point(6, 6);
            this.B_ModSpawn.Name = "B_ModSpawn";
            this.B_ModSpawn.Size = new System.Drawing.Size(142, 26);
            this.B_ModSpawn.TabIndex = 0;
            this.B_ModSpawn.Text = "Modify All Spawn Count";
            this.B_ModSpawn.UseVisualStyleBackColor = true;
            this.B_ModSpawn.Click += new System.EventHandler(this.B_ModCount_Click);
            //
            // L_Hash
            //
            this.L_Hash.AutoSize = true;
            this.L_Hash.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Hash.Location = new System.Drawing.Point(13, 52);
            this.L_Hash.Name = "L_Hash";
            this.L_Hash.Size = new System.Drawing.Size(77, 14);
            this.L_Hash.TabIndex = 11;
            this.L_Hash.Text = "L_ZoneHash";
            //
            // B_Save
            //
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(268, 0);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 12;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            //
            // CHK_WildMega
            //
            this.CHK_WildMega.AutoSize = true;
            this.CHK_WildMega.Location = new System.Drawing.Point(154, 122);
            this.CHK_WildMega.Name = "CHK_WildMega";
            this.CHK_WildMega.Size = new System.Drawing.Size(82, 17);
            this.CHK_WildMega.TabIndex = 12;
            this.CHK_WildMega.Text = "Wild Megas";
            this.CHK_WildMega.UseVisualStyleBackColor = true;
            //
            // GGWE
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 516);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Hash);
            this.Controls.Add(this.TC_Tables);
            this.Controls.Add(this.L_Rank);
            this.Controls.Add(this.NUD_RankMax);
            this.Controls.Add(this.NUD_RankMin);
            this.Controls.Add(this.CB_Location);
            this.MaximizeBox = false;
            this.Name = "GGWE";
            this.Text = "Wild Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMax)).EndInit();
            this.TC_Tables.ResumeLayout(false);
            this.Tab_Ground.ResumeLayout(false);
            this.Tab_Water.ResumeLayout(false);
            this.Tab_Old.ResumeLayout(false);
            this.Tab_Good.ResumeLayout(false);
            this.Tab_Super.ResumeLayout(false);
            this.Tab_Sky.ResumeLayout(false);
            this.Tab_Rand.ResumeLayout(false);
            this.Tab_Rand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_ModCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CB_Location;
        private System.Windows.Forms.NumericUpDown NUD_RankMin;
        private System.Windows.Forms.NumericUpDown NUD_RankMax;
        private System.Windows.Forms.Label L_Rank;
        private System.Windows.Forms.TabControl TC_Tables;
        private System.Windows.Forms.TabPage Tab_Ground;
        private System.Windows.Forms.TabPage Tab_Water;
        private System.Windows.Forms.TabPage Tab_Old;
        private System.Windows.Forms.TabPage Tab_Good;
        private System.Windows.Forms.TabPage Tab_Super;
        private EncounterList EL_Super;
        private System.Windows.Forms.TabPage Tab_Sky;
        private EncounterList EL_Sky;
        private EncounterList EL_Ground;
        private EncounterList EL_Water;
        private EncounterList EL_Old;
        private EncounterList EL_Good;
        private System.Windows.Forms.Label L_Hash;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.TabPage Tab_Rand;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.PropertyGrid PG_Species;
        private System.Windows.Forms.NumericUpDown NUD_ModDuration;
        private System.Windows.Forms.NumericUpDown NUD_ModRate;
        private System.Windows.Forms.NumericUpDown NUD_ModCount;
        private System.Windows.Forms.Button B_ModDuration;
        private System.Windows.Forms.Button B_ModRate;
        private System.Windows.Forms.Button B_ModSpawn;
        private System.Windows.Forms.CheckBox CHK_FillEmpty;
        private System.Windows.Forms.Label L_SpawnDuration;
        private System.Windows.Forms.Label L_SpawnCount;
        private System.Windows.Forms.Label L_SpawnRate;
        private System.Windows.Forms.CheckBox CHK_WildMega;
    }
}