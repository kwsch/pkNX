namespace pkNX.WinForms
{
    partial class SWSHE
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
            this.Tab_Normal = new System.Windows.Forms.TabPage();
            this.Tab_Overcast = new System.Windows.Forms.TabPage();
            this.Tab_Raining = new System.Windows.Forms.TabPage();
            this.Tab_Thunder = new System.Windows.Forms.TabPage();
            this.Tab_Sun = new System.Windows.Forms.TabPage();
            this.Tab_Rand = new System.Windows.Forms.TabPage();
            this.CHK_ForceType = new System.Windows.Forms.CheckBox();
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
            this.SL_0 = new pkNX.WinForms.EncounterList8();
            this.SL_1 = new pkNX.WinForms.EncounterList8();
            this.SL_2 = new pkNX.WinForms.EncounterList8();
            this.SL_3 = new pkNX.WinForms.EncounterList8();
            this.SL_5 = new pkNX.WinForms.EncounterList8();
            this.SL_4 = new pkNX.WinForms.EncounterList8();
            this.SL_6 = new pkNX.WinForms.EncounterList8();
            this.SL_7 = new pkNX.WinForms.EncounterList8();
            this.SL_8 = new pkNX.WinForms.EncounterList8();
            this.SL_9 = new pkNX.WinForms.EncounterList8();
            this.SL_10 = new pkNX.WinForms.EncounterList8();
            this.B_Dump = new System.Windows.Forms.Button();
            this.Tab_Snowing = new System.Windows.Forms.TabPage();
            this.Tab_Snowstorm = new System.Windows.Forms.TabPage();
            this.Tab_Sandstorm = new System.Windows.Forms.TabPage();
            this.Tab_Fog = new System.Windows.Forms.TabPage();
            this.Tab_Shaking = new System.Windows.Forms.TabPage();
            this.Tab_Fishing = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMax)).BeginInit();
            this.TC_Tables.SuspendLayout();
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
            this.CB_Location.Size = new System.Drawing.Size(190, 21);
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
            this.L_Rank.Size = new System.Drawing.Size(84, 13);
            this.L_Rank.TabIndex = 9;
            this.L_Rank.Text = "Level Min / Max";
            // 
            // TC_Tables
            // 
            this.TC_Tables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TC_Tables.Controls.Add(this.Tab_Normal);
            this.TC_Tables.Controls.Add(this.Tab_Overcast);
            this.TC_Tables.Controls.Add(this.Tab_Raining);
            this.TC_Tables.Controls.Add(this.Tab_Thunder);
            this.TC_Tables.Controls.Add(this.Tab_Sun);
            this.TC_Tables.Controls.Add(this.Tab_Snowing);
            this.TC_Tables.Controls.Add(this.Tab_Snowstorm);
            this.TC_Tables.Controls.Add(this.Tab_Sandstorm);
            this.TC_Tables.Controls.Add(this.Tab_Fog);
            this.TC_Tables.Controls.Add(this.Tab_Shaking);
            this.TC_Tables.Controls.Add(this.Tab_Fishing);
            this.TC_Tables.Controls.Add(this.Tab_Rand);
            this.TC_Tables.Location = new System.Drawing.Point(12, 78);
            this.TC_Tables.Name = "TC_Tables";
            this.TC_Tables.SelectedIndex = 0;
            this.TC_Tables.Size = new System.Drawing.Size(359, 426);
            this.TC_Tables.TabIndex = 10;
            // 
            // Tab_Normal
            // 
            this.Tab_Normal.Location = new System.Drawing.Point(4, 22);
            this.Tab_Normal.Name = "Tab_Normal";
            this.Tab_Normal.Size = new System.Drawing.Size(351, 400);
            this.Tab_Normal.TabIndex = 0;
            this.Tab_Normal.Text = "Normal";
            // 
            // Tab_Overcast
            // 
            this.Tab_Overcast.Location = new System.Drawing.Point(4, 22);
            this.Tab_Overcast.Name = "Tab_Overcast";
            this.Tab_Overcast.Size = new System.Drawing.Size(351, 400);
            this.Tab_Overcast.TabIndex = 1;
            this.Tab_Overcast.Text = "Overcast";
            // 
            // Tab_Raining
            // 
            this.Tab_Raining.Location = new System.Drawing.Point(4, 22);
            this.Tab_Raining.Name = "Tab_Raining";
            this.Tab_Raining.Size = new System.Drawing.Size(351, 400);
            this.Tab_Raining.TabIndex = 2;
            this.Tab_Raining.Text = "Raining";
            // 
            // Tab_Thunder
            // 
            this.Tab_Thunder.Location = new System.Drawing.Point(4, 22);
            this.Tab_Thunder.Name = "Tab_Thunder";
            this.Tab_Thunder.Size = new System.Drawing.Size(351, 400);
            this.Tab_Thunder.TabIndex = 3;
            this.Tab_Thunder.Text = "Thunder";
            // 
            // Tab_Sun
            // 
            this.Tab_Sun.Location = new System.Drawing.Point(4, 22);
            this.Tab_Sun.Name = "Tab_Sun";
            this.Tab_Sun.Size = new System.Drawing.Size(351, 400);
            this.Tab_Sun.TabIndex = 4;
            this.Tab_Sun.Text = "Intense Sun";
            // 
            // Tab_Rand
            // 
            this.Tab_Rand.Controls.Add(this.CHK_ForceType);
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
            this.Tab_Rand.Size = new System.Drawing.Size(351, 400);
            this.Tab_Rand.TabIndex = 6;
            this.Tab_Rand.Text = "Rand";
            this.Tab_Rand.UseVisualStyleBackColor = true;
            // 
            // CHK_ForceType
            // 
            this.CHK_ForceType.Checked = true;
            this.CHK_ForceType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_ForceType.Location = new System.Drawing.Point(182, 105);
            this.CHK_ForceType.Name = "CHK_ForceType";
            this.CHK_ForceType.Size = new System.Drawing.Size(125, 31);
            this.CHK_ForceType.TabIndex = 13;
            this.CHK_ForceType.Text = "Force At Least 1 Water/Grass Viridian";
            this.CHK_ForceType.UseVisualStyleBackColor = true;
            // 
            // L_SpawnDuration
            // 
            this.L_SpawnDuration.AutoSize = true;
            this.L_SpawnDuration.Location = new System.Drawing.Point(202, 77);
            this.L_SpawnDuration.Name = "L_SpawnDuration";
            this.L_SpawnDuration.Size = new System.Drawing.Size(47, 13);
            this.L_SpawnDuration.TabIndex = 11;
            this.L_SpawnDuration.Text = "seconds";
            this.L_SpawnDuration.Visible = false;
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
            this.CHK_FillEmpty.Location = new System.Drawing.Point(80, 105);
            this.CHK_FillEmpty.Name = "CHK_FillEmpty";
            this.CHK_FillEmpty.Size = new System.Drawing.Size(83, 31);
            this.CHK_FillEmpty.TabIndex = 8;
            this.CHK_FillEmpty.Text = "Fill All Empty Slots";
            this.CHK_FillEmpty.UseVisualStyleBackColor = true;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Location = new System.Drawing.Point(6, 102);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(68, 34);
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
            this.NUD_ModDuration.Visible = false;
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
            this.B_ModDuration.Visible = false;
            // 
            // B_ModRate
            // 
            this.B_ModRate.Location = new System.Drawing.Point(6, 38);
            this.B_ModRate.Name = "B_ModRate";
            this.B_ModRate.Size = new System.Drawing.Size(142, 26);
            this.B_ModRate.TabIndex = 1;
            this.B_ModRate.Text = "Modify All Spawn Rate";
            this.B_ModRate.UseVisualStyleBackColor = true;
            // 
            // B_ModSpawn
            // 
            this.B_ModSpawn.Location = new System.Drawing.Point(6, 6);
            this.B_ModSpawn.Name = "B_ModSpawn";
            this.B_ModSpawn.Size = new System.Drawing.Size(142, 26);
            this.B_ModSpawn.TabIndex = 0;
            this.B_ModSpawn.Text = "Modify All Spawn Count";
            this.B_ModSpawn.UseVisualStyleBackColor = true;
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
            this.B_Save.Location = new System.Drawing.Point(306, 0);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 12;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // SL_0
            // 
            this.SL_0.Location = new System.Drawing.Point(0, 0);
            this.SL_0.Name = "SL_0";
            this.SL_0.Size = new System.Drawing.Size(300, 320);
            this.SL_0.TabIndex = 0;
            // 
            // SL_1
            // 
            this.SL_1.Location = new System.Drawing.Point(0, 0);
            this.SL_1.Name = "SL_1";
            this.SL_1.Size = new System.Drawing.Size(300, 320);
            this.SL_1.TabIndex = 0;
            // 
            // SL_2
            // 
            this.SL_2.Location = new System.Drawing.Point(0, 0);
            this.SL_2.Name = "SL_2";
            this.SL_2.Size = new System.Drawing.Size(300, 320);
            this.SL_2.TabIndex = 0;
            // 
            // SL_3
            // 
            this.SL_3.Location = new System.Drawing.Point(0, 0);
            this.SL_3.Name = "SL_3";
            this.SL_3.Size = new System.Drawing.Size(300, 320);
            this.SL_3.TabIndex = 0;
            // 
            // SL_5
            // 
            this.SL_5.Location = new System.Drawing.Point(0, 0);
            this.SL_5.Name = "SL_5";
            this.SL_5.Size = new System.Drawing.Size(300, 320);
            this.SL_5.TabIndex = 0;
            // 
            // SL_4
            // 
            this.SL_4.Location = new System.Drawing.Point(0, 0);
            this.SL_4.Name = "SL_4";
            this.SL_4.Size = new System.Drawing.Size(300, 320);
            this.SL_4.TabIndex = 0;
            // 
            // SL_6
            // 
            this.SL_6.Location = new System.Drawing.Point(0, 0);
            this.SL_6.Name = "SL_6";
            this.SL_6.Size = new System.Drawing.Size(300, 320);
            this.SL_6.TabIndex = 0;
            // 
            // SL_7
            // 
            this.SL_7.Location = new System.Drawing.Point(0, 0);
            this.SL_7.Name = "SL_7";
            this.SL_7.Size = new System.Drawing.Size(300, 320);
            this.SL_7.TabIndex = 0;
            // 
            // SL_8
            // 
            this.SL_8.Location = new System.Drawing.Point(0, 0);
            this.SL_8.Name = "SL_8";
            this.SL_8.Size = new System.Drawing.Size(300, 320);
            this.SL_8.TabIndex = 0;
            // 
            // SL_9
            // 
            this.SL_9.Location = new System.Drawing.Point(0, 0);
            this.SL_9.Name = "SL_9";
            this.SL_9.Size = new System.Drawing.Size(300, 320);
            this.SL_9.TabIndex = 0;
            // 
            // SL_10
            // 
            this.SL_10.Location = new System.Drawing.Point(0, 0);
            this.SL_10.Name = "SL_10";
            this.SL_10.Size = new System.Drawing.Size(300, 320);
            this.SL_10.TabIndex = 0;
            // 
            // B_Dump
            // 
            this.B_Dump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Dump.Location = new System.Drawing.Point(225, 0);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(75, 23);
            this.B_Dump.TabIndex = 13;
            this.B_Dump.Text = "Dump";
            this.B_Dump.UseVisualStyleBackColor = true;
            // 
            // Tab_Snowing
            // 
            this.Tab_Snowing.Location = new System.Drawing.Point(4, 22);
            this.Tab_Snowing.Name = "Tab_Snowing";
            this.Tab_Snowing.Size = new System.Drawing.Size(351, 400);
            this.Tab_Snowing.TabIndex = 7;
            this.Tab_Snowing.Text = "Snowing";
            this.Tab_Snowing.UseVisualStyleBackColor = true;
            // 
            // Tab_Snowstorm
            // 
            this.Tab_Snowstorm.Location = new System.Drawing.Point(4, 22);
            this.Tab_Snowstorm.Name = "Tab_Snowstorm";
            this.Tab_Snowstorm.Size = new System.Drawing.Size(351, 400);
            this.Tab_Snowstorm.TabIndex = 8;
            this.Tab_Snowstorm.Text = "Snowstorm";
            this.Tab_Snowstorm.UseVisualStyleBackColor = true;
            // 
            // Tab_Sandstorm
            // 
            this.Tab_Sandstorm.Location = new System.Drawing.Point(4, 22);
            this.Tab_Sandstorm.Name = "Tab_Sandstorm";
            this.Tab_Sandstorm.Size = new System.Drawing.Size(351, 400);
            this.Tab_Sandstorm.TabIndex = 9;
            this.Tab_Sandstorm.Text = "Sandstorm";
            this.Tab_Sandstorm.UseVisualStyleBackColor = true;
            // 
            // Tab_Fog
            // 
            this.Tab_Fog.Location = new System.Drawing.Point(4, 22);
            this.Tab_Fog.Name = "Tab_Fog";
            this.Tab_Fog.Size = new System.Drawing.Size(351, 400);
            this.Tab_Fog.TabIndex = 10;
            this.Tab_Fog.Text = "Fog";
            this.Tab_Fog.UseVisualStyleBackColor = true;
            // 
            // Tab_Shaking
            // 
            this.Tab_Shaking.Location = new System.Drawing.Point(4, 22);
            this.Tab_Shaking.Name = "Tab_Shaking";
            this.Tab_Shaking.Size = new System.Drawing.Size(351, 400);
            this.Tab_Shaking.TabIndex = 11;
            this.Tab_Shaking.Text = "Shaking Trees";
            this.Tab_Shaking.UseVisualStyleBackColor = true;
            // 
            // Tab_Fishing
            // 
            this.Tab_Fishing.Location = new System.Drawing.Point(4, 22);
            this.Tab_Fishing.Name = "Tab_Fishing";
            this.Tab_Fishing.Size = new System.Drawing.Size(351, 400);
            this.Tab_Fishing.TabIndex = 12;
            this.Tab_Fishing.Text = "Fishing";
            this.Tab_Fishing.UseVisualStyleBackColor = true;
            // 
            // SWSHE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 516);
            this.Controls.Add(this.B_Dump);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.L_Hash);
            this.Controls.Add(this.TC_Tables);
            this.Controls.Add(this.L_Rank);
            this.Controls.Add(this.NUD_RankMax);
            this.Controls.Add(this.NUD_RankMin);
            this.Controls.Add(this.CB_Location);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SWSHE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wild Editor";
            this.Load += new System.EventHandler(this.SWSHE_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_RankMax)).EndInit();
            this.TC_Tables.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage Tab_Normal;
        private System.Windows.Forms.TabPage Tab_Overcast;
        private System.Windows.Forms.TabPage Tab_Raining;
        private System.Windows.Forms.TabPage Tab_Thunder;
        private System.Windows.Forms.TabPage Tab_Sun;
        private EncounterList8 SL_0;
        private EncounterList8 SL_1;
        private EncounterList8 SL_2;
        private EncounterList8 SL_3;
        private EncounterList8 SL_4;
        private EncounterList8 SL_5;
        private EncounterList8 SL_6;
        private EncounterList8 SL_7;
        private EncounterList8 SL_8;
        private EncounterList8 SL_9;
        private EncounterList8 SL_10;
        private System.Windows.Forms.Label L_Hash;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.TabPage Tab_Rand;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.PropertyGrid PG_Species;
        private System.Windows.Forms.NumericUpDown NUD_ModRate;
        private System.Windows.Forms.NumericUpDown NUD_ModCount;
        private System.Windows.Forms.Button B_ModRate;
        private System.Windows.Forms.Button B_ModSpawn;
        private System.Windows.Forms.CheckBox CHK_FillEmpty;
        private System.Windows.Forms.Label L_SpawnCount;
        private System.Windows.Forms.Label L_SpawnRate;
        private System.Windows.Forms.CheckBox CHK_ForceType;
        private System.Windows.Forms.Label L_SpawnDuration;
        private System.Windows.Forms.NumericUpDown NUD_ModDuration;
        private System.Windows.Forms.Button B_ModDuration;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.TabPage Tab_Snowing;
        private System.Windows.Forms.TabPage Tab_Snowstorm;
        private System.Windows.Forms.TabPage Tab_Sandstorm;
        private System.Windows.Forms.TabPage Tab_Fog;
        private System.Windows.Forms.TabPage Tab_Shaking;
        private System.Windows.Forms.TabPage Tab_Fishing;
    }
}