namespace pkNX.WinForms
{
    partial class DumperSV
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
            this.TC_Options = new System.Windows.Forms.TabControl();
            this.Tab_General = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.B_ParsePKMDetails = new System.Windows.Forms.Button();
            this.B_Raid = new System.Windows.Forms.Button();
            this.B_PKText = new System.Windows.Forms.Button();
            this.B_Trainers = new System.Windows.Forms.Button();
            this.B_Encount = new System.Windows.Forms.Button();
            this.B_DumpMoves = new System.Windows.Forms.Button();
            this.B_Cooking = new System.Windows.Forms.Button();
            this.Tab_Misc = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.B_DumpHash = new System.Windows.Forms.Button();
            this.B_DumpArc = new System.Windows.Forms.Button();
            this.B_DumpSpecific = new System.Windows.Forms.Button();
            this.B_Misc = new System.Windows.Forms.Button();
            this.Tab_Future = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.B_DistributionRaids = new System.Windows.Forms.Button();
            this.B_OpenFolder = new System.Windows.Forms.Button();
            this.B_DumpPath = new System.Windows.Forms.Button();
            this.TC_Options.SuspendLayout();
            this.Tab_General.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.Tab_Misc.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.Tab_Future.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // TC_Options
            // 
            this.TC_Options.Controls.Add(this.Tab_General);
            this.TC_Options.Controls.Add(this.Tab_Misc);
            this.TC_Options.Controls.Add(this.Tab_Future);
            this.TC_Options.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TC_Options.Location = new System.Drawing.Point(0, 0);
            this.TC_Options.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TC_Options.Name = "TC_Options";
            this.TC_Options.SelectedIndex = 0;
            this.TC_Options.Size = new System.Drawing.Size(484, 321);
            this.TC_Options.TabIndex = 0;
            // 
            // Tab_General
            // 
            this.Tab_General.Controls.Add(this.flowLayoutPanel1);
            this.Tab_General.Location = new System.Drawing.Point(4, 24);
            this.Tab_General.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_General.Name = "Tab_General";
            this.Tab_General.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_General.Size = new System.Drawing.Size(476, 293);
            this.Tab_General.TabIndex = 0;
            this.Tab_General.Text = "General";
            this.Tab_General.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.B_ParsePKMDetails);
            this.flowLayoutPanel1.Controls.Add(this.B_Raid);
            this.flowLayoutPanel1.Controls.Add(this.B_PKText);
            this.flowLayoutPanel1.Controls.Add(this.B_Trainers);
            this.flowLayoutPanel1.Controls.Add(this.B_Encount);
            this.flowLayoutPanel1.Controls.Add(this.B_DumpMoves);
            this.flowLayoutPanel1.Controls.Add(this.B_Cooking);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(468, 287);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // B_ParsePKMDetails
            // 
            this.B_ParsePKMDetails.Location = new System.Drawing.Point(4, 3);
            this.B_ParsePKMDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_ParsePKMDetails.Name = "B_ParsePKMDetails";
            this.B_ParsePKMDetails.Size = new System.Drawing.Size(144, 63);
            this.B_ParsePKMDetails.TabIndex = 1;
            this.B_ParsePKMDetails.Text = "Personal Stats";
            this.B_ParsePKMDetails.UseVisualStyleBackColor = true;
            this.B_ParsePKMDetails.Click += new System.EventHandler(this.B_ParsePKMDetails_Click);
            // 
            // B_Raid
            // 
            this.B_Raid.Location = new System.Drawing.Point(156, 3);
            this.B_Raid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Raid.Name = "B_Raid";
            this.B_Raid.Size = new System.Drawing.Size(144, 63);
            this.B_Raid.TabIndex = 16;
            this.B_Raid.Text = "Raids";
            this.B_Raid.UseVisualStyleBackColor = true;
            this.B_Raid.Click += new System.EventHandler(this.B_Raid_Click);
            // 
            // B_PKText
            // 
            this.B_PKText.Location = new System.Drawing.Point(308, 3);
            this.B_PKText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_PKText.Name = "B_PKText";
            this.B_PKText.Size = new System.Drawing.Size(144, 63);
            this.B_PKText.TabIndex = 17;
            this.B_PKText.Text = "Text Files";
            this.B_PKText.UseVisualStyleBackColor = true;
            this.B_PKText.Click += new System.EventHandler(this.B_PKText_Click);
            // 
            // B_Trainers
            // 
            this.B_Trainers.Location = new System.Drawing.Point(4, 72);
            this.B_Trainers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Trainers.Name = "B_Trainers";
            this.B_Trainers.Size = new System.Drawing.Size(144, 63);
            this.B_Trainers.TabIndex = 18;
            this.B_Trainers.Text = "Trainers";
            this.B_Trainers.UseVisualStyleBackColor = true;
            this.B_Trainers.Click += new System.EventHandler(this.B_Trainers_Click);
            // 
            // B_Encount
            // 
            this.B_Encount.Location = new System.Drawing.Point(156, 72);
            this.B_Encount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Encount.Name = "B_Encount";
            this.B_Encount.Size = new System.Drawing.Size(144, 63);
            this.B_Encount.TabIndex = 19;
            this.B_Encount.Text = "Encounters";
            this.B_Encount.UseVisualStyleBackColor = true;
            this.B_Encount.Click += new System.EventHandler(this.B_Encount_Click);
            // 
            // B_DumpMoves
            // 
            this.B_DumpMoves.Location = new System.Drawing.Point(308, 72);
            this.B_DumpMoves.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DumpMoves.Name = "B_DumpMoves";
            this.B_DumpMoves.Size = new System.Drawing.Size(144, 63);
            this.B_DumpMoves.TabIndex = 20;
            this.B_DumpMoves.Text = "Moves";
            this.B_DumpMoves.UseVisualStyleBackColor = true;
            this.B_DumpMoves.Click += new System.EventHandler(this.B_DumpMoves_Click);
            // 
            // B_Cooking
            // 
            this.B_Cooking.Location = new System.Drawing.Point(4, 141);
            this.B_Cooking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Cooking.Name = "B_Cooking";
            this.B_Cooking.Size = new System.Drawing.Size(144, 63);
            this.B_Cooking.TabIndex = 21;
            this.B_Cooking.Text = "Cooking";
            this.B_Cooking.UseVisualStyleBackColor = true;
            this.B_Cooking.Click += new System.EventHandler(this.B_Cooking_Click);
            // 
            // Tab_Misc
            // 
            this.Tab_Misc.Controls.Add(this.flowLayoutPanel3);
            this.Tab_Misc.Location = new System.Drawing.Point(4, 24);
            this.Tab_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Misc.Name = "Tab_Misc";
            this.Tab_Misc.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Misc.Size = new System.Drawing.Size(476, 293);
            this.Tab_Misc.TabIndex = 2;
            this.Tab_Misc.Text = "Misc";
            this.Tab_Misc.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.B_DumpHash);
            this.flowLayoutPanel3.Controls.Add(this.B_DumpArc);
            this.flowLayoutPanel3.Controls.Add(this.B_DumpSpecific);
            this.flowLayoutPanel3.Controls.Add(this.B_Misc);
            this.flowLayoutPanel3.Controls.Add(this.B_DumpPath);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(4, 3);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(468, 287);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // B_DumpHash
            // 
            this.B_DumpHash.Location = new System.Drawing.Point(4, 3);
            this.B_DumpHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DumpHash.Name = "B_DumpHash";
            this.B_DumpHash.Size = new System.Drawing.Size(144, 63);
            this.B_DumpHash.TabIndex = 14;
            this.B_DumpHash.Text = "Hash Tables";
            this.B_DumpHash.UseVisualStyleBackColor = true;
            this.B_DumpHash.Click += new System.EventHandler(this.B_DumpHash_Click);
            // 
            // B_DumpArc
            // 
            this.B_DumpArc.Location = new System.Drawing.Point(156, 3);
            this.B_DumpArc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DumpArc.Name = "B_DumpArc";
            this.B_DumpArc.Size = new System.Drawing.Size(144, 63);
            this.B_DumpArc.TabIndex = 18;
            this.B_DumpArc.Text = "Archives";
            this.B_DumpArc.UseVisualStyleBackColor = true;
            this.B_DumpArc.Click += new System.EventHandler(this.B_DumpArc_Click);
            // 
            // B_DumpSpecific
            // 
            this.B_DumpSpecific.Location = new System.Drawing.Point(308, 3);
            this.B_DumpSpecific.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DumpSpecific.Name = "B_DumpSpecific";
            this.B_DumpSpecific.Size = new System.Drawing.Size(144, 63);
            this.B_DumpSpecific.TabIndex = 19;
            this.B_DumpSpecific.Text = "Specific Packed";
            this.B_DumpSpecific.UseVisualStyleBackColor = true;
            this.B_DumpSpecific.Click += new System.EventHandler(this.B_DumpSpecific_Click);
            // 
            // B_Misc
            // 
            this.B_Misc.Location = new System.Drawing.Point(4, 72);
            this.B_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Misc.Name = "B_Misc";
            this.B_Misc.Size = new System.Drawing.Size(144, 63);
            this.B_Misc.TabIndex = 20;
            this.B_Misc.Text = "Misc";
            this.B_Misc.UseVisualStyleBackColor = true;
            this.B_Misc.Click += new System.EventHandler(this.B_DumpMisc_Click);
            // 
            // Tab_Future
            // 
            this.Tab_Future.Controls.Add(this.flowLayoutPanel4);
            this.Tab_Future.Location = new System.Drawing.Point(4, 24);
            this.Tab_Future.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Future.Name = "Tab_Future";
            this.Tab_Future.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Future.Size = new System.Drawing.Size(476, 293);
            this.Tab_Future.TabIndex = 3;
            this.Tab_Future.Text = "Future";
            this.Tab_Future.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.B_DistributionRaids);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(4, 3);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(468, 287);
            this.flowLayoutPanel4.TabIndex = 3;
            // 
            // B_DistributionRaids
            // 
            this.B_DistributionRaids.Location = new System.Drawing.Point(4, 3);
            this.B_DistributionRaids.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DistributionRaids.Name = "B_DistributionRaids";
            this.B_DistributionRaids.Size = new System.Drawing.Size(144, 63);
            this.B_DistributionRaids.TabIndex = 14;
            this.B_DistributionRaids.Text = "Distribution Raids";
            this.B_DistributionRaids.UseVisualStyleBackColor = true;
            this.B_DistributionRaids.Click += new System.EventHandler(this.B_DistributionRaids_Click);
            // 
            // B_OpenFolder
            // 
            this.B_OpenFolder.Location = new System.Drawing.Point(376, -1);
            this.B_OpenFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_OpenFolder.Name = "B_OpenFolder";
            this.B_OpenFolder.Size = new System.Drawing.Size(88, 27);
            this.B_OpenFolder.TabIndex = 2;
            this.B_OpenFolder.Text = "Open Folder";
            this.B_OpenFolder.UseVisualStyleBackColor = true;
            this.B_OpenFolder.Click += new System.EventHandler(this.B_OpenFolder_Click);
            // 
            // B_DumpPath
            // 
            this.B_DumpPath.Location = new System.Drawing.Point(156, 72);
            this.B_DumpPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_DumpPath.Name = "B_DumpPath";
            this.B_DumpPath.Size = new System.Drawing.Size(144, 63);
            this.B_DumpPath.TabIndex = 21;
            this.B_DumpPath.Text = "Specific File...";
            this.B_DumpPath.UseVisualStyleBackColor = true;
            this.B_DumpPath.Click += new System.EventHandler(this.B_DumpPath_Click);
            // 
            // DumperSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 321);
            this.Controls.Add(this.B_OpenFolder);
            this.Controls.Add(this.TC_Options);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "DumperSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVDump";
            this.TC_Options.ResumeLayout(false);
            this.Tab_General.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.Tab_Misc.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.Tab_Future.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TC_Options;
        private System.Windows.Forms.TabPage Tab_General;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button B_ParsePKMDetails;
        private System.Windows.Forms.TabPage Tab_Misc;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button B_DumpHash;
        private System.Windows.Forms.Button B_OpenFolder;
        private System.Windows.Forms.TabPage Tab_Future;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button B_DistributionRaids;
        private System.Windows.Forms.Button B_Raid;
        private System.Windows.Forms.Button B_DumpArc;
        private System.Windows.Forms.Button B_DumpSpecific;
        private System.Windows.Forms.Button B_PKText;
        private System.Windows.Forms.Button B_Trainers;
        private System.Windows.Forms.Button B_Misc;
        private System.Windows.Forms.Button B_Encount;
        private System.Windows.Forms.Button B_DumpMoves;
        private System.Windows.Forms.Button B_Cooking;
        private System.Windows.Forms.Button B_DumpPath;
    }
}
