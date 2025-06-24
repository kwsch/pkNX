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
            TC_Options = new System.Windows.Forms.TabControl();
            Tab_General = new System.Windows.Forms.TabPage();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            B_ParsePKMDetails = new System.Windows.Forms.Button();
            B_Raid = new System.Windows.Forms.Button();
            B_PKText = new System.Windows.Forms.Button();
            B_Trainers = new System.Windows.Forms.Button();
            B_Encount = new System.Windows.Forms.Button();
            B_DumpMovesAbilities = new System.Windows.Forms.Button();
            B_Cooking = new System.Windows.Forms.Button();
            Tab_Misc = new System.Windows.Forms.TabPage();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            B_DumpHash = new System.Windows.Forms.Button();
            B_DumpArc = new System.Windows.Forms.Button();
            B_DumpSpecific = new System.Windows.Forms.Button();
            B_Misc = new System.Windows.Forms.Button();
            B_DumpPath = new System.Windows.Forms.Button();
            Tab_Future = new System.Windows.Forms.TabPage();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            B_DistributionRaids = new System.Windows.Forms.Button();
            B_DeliveryOutbreaks = new System.Windows.Forms.Button();
            B_OpenFolder = new System.Windows.Forms.Button();
            B_KitakamiPoints = new System.Windows.Forms.Button();
            TC_Options.SuspendLayout();
            Tab_General.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            Tab_Misc.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            Tab_Future.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // TC_Options
            // 
            TC_Options.Controls.Add(Tab_General);
            TC_Options.Controls.Add(Tab_Misc);
            TC_Options.Controls.Add(Tab_Future);
            TC_Options.Dock = System.Windows.Forms.DockStyle.Fill;
            TC_Options.Location = new System.Drawing.Point(0, 0);
            TC_Options.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TC_Options.Name = "TC_Options";
            TC_Options.SelectedIndex = 0;
            TC_Options.Size = new System.Drawing.Size(484, 364);
            TC_Options.TabIndex = 0;
            // 
            // Tab_General
            // 
            Tab_General.Controls.Add(flowLayoutPanel1);
            Tab_General.Location = new System.Drawing.Point(4, 26);
            Tab_General.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_General.Name = "Tab_General";
            Tab_General.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_General.Size = new System.Drawing.Size(476, 334);
            Tab_General.TabIndex = 0;
            Tab_General.Text = "General";
            Tab_General.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(B_ParsePKMDetails);
            flowLayoutPanel1.Controls.Add(B_Raid);
            flowLayoutPanel1.Controls.Add(B_PKText);
            flowLayoutPanel1.Controls.Add(B_Trainers);
            flowLayoutPanel1.Controls.Add(B_Encount);
            flowLayoutPanel1.Controls.Add(B_DumpMovesAbilities);
            flowLayoutPanel1.Controls.Add(B_Cooking);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(468, 328);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // B_ParsePKMDetails
            // 
            B_ParsePKMDetails.Location = new System.Drawing.Point(4, 3);
            B_ParsePKMDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_ParsePKMDetails.Name = "B_ParsePKMDetails";
            B_ParsePKMDetails.Size = new System.Drawing.Size(144, 71);
            B_ParsePKMDetails.TabIndex = 1;
            B_ParsePKMDetails.Text = "Personal Stats";
            B_ParsePKMDetails.UseVisualStyleBackColor = true;
            B_ParsePKMDetails.Click += B_ParsePKMDetails_Click;
            // 
            // B_Raid
            // 
            B_Raid.Location = new System.Drawing.Point(156, 3);
            B_Raid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Raid.Name = "B_Raid";
            B_Raid.Size = new System.Drawing.Size(144, 71);
            B_Raid.TabIndex = 16;
            B_Raid.Text = "Raids";
            B_Raid.UseVisualStyleBackColor = true;
            B_Raid.Click += B_Raid_Click;
            // 
            // B_PKText
            // 
            B_PKText.Location = new System.Drawing.Point(308, 3);
            B_PKText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_PKText.Name = "B_PKText";
            B_PKText.Size = new System.Drawing.Size(144, 71);
            B_PKText.TabIndex = 17;
            B_PKText.Text = "Text Files";
            B_PKText.UseVisualStyleBackColor = true;
            B_PKText.Click += B_PKText_Click;
            // 
            // B_Trainers
            // 
            B_Trainers.Location = new System.Drawing.Point(4, 80);
            B_Trainers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Trainers.Name = "B_Trainers";
            B_Trainers.Size = new System.Drawing.Size(144, 71);
            B_Trainers.TabIndex = 18;
            B_Trainers.Text = "Trainers";
            B_Trainers.UseVisualStyleBackColor = true;
            B_Trainers.Click += B_Trainers_Click;
            // 
            // B_Encount
            // 
            B_Encount.Location = new System.Drawing.Point(156, 80);
            B_Encount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Encount.Name = "B_Encount";
            B_Encount.Size = new System.Drawing.Size(144, 71);
            B_Encount.TabIndex = 19;
            B_Encount.Text = "Encounters";
            B_Encount.UseVisualStyleBackColor = true;
            B_Encount.Click += B_Encount_Click;
            // 
            // B_DumpMovesAbilities
            // 
            B_DumpMovesAbilities.Location = new System.Drawing.Point(308, 80);
            B_DumpMovesAbilities.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpMovesAbilities.Name = "B_DumpMovesAbilities";
            B_DumpMovesAbilities.Size = new System.Drawing.Size(144, 71);
            B_DumpMovesAbilities.TabIndex = 20;
            B_DumpMovesAbilities.Text = "Moves/Abilities";
            B_DumpMovesAbilities.UseVisualStyleBackColor = true;
            B_DumpMovesAbilities.Click += B_DumpMovesAbilities_Click;
            // 
            // B_Cooking
            // 
            B_Cooking.Location = new System.Drawing.Point(4, 157);
            B_Cooking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Cooking.Name = "B_Cooking";
            B_Cooking.Size = new System.Drawing.Size(144, 71);
            B_Cooking.TabIndex = 21;
            B_Cooking.Text = "Cooking";
            B_Cooking.UseVisualStyleBackColor = true;
            B_Cooking.Click += B_Cooking_Click;
            // 
            // Tab_Misc
            // 
            Tab_Misc.Controls.Add(flowLayoutPanel3);
            Tab_Misc.Location = new System.Drawing.Point(4, 26);
            Tab_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Misc.Name = "Tab_Misc";
            Tab_Misc.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Misc.Size = new System.Drawing.Size(476, 334);
            Tab_Misc.TabIndex = 2;
            Tab_Misc.Text = "Misc";
            Tab_Misc.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(B_DumpPath);
            flowLayoutPanel3.Controls.Add(B_DumpHash);
            flowLayoutPanel3.Controls.Add(B_DumpArc);
            flowLayoutPanel3.Controls.Add(B_DumpSpecific);
            flowLayoutPanel3.Controls.Add(B_Misc);
            flowLayoutPanel3.Controls.Add(B_KitakamiPoints);
            flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel3.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(468, 328);
            flowLayoutPanel3.TabIndex = 2;
            // 
            // B_DumpHash
            // 
            B_DumpHash.Location = new System.Drawing.Point(156, 3);
            B_DumpHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpHash.Name = "B_DumpHash";
            B_DumpHash.Size = new System.Drawing.Size(144, 71);
            B_DumpHash.TabIndex = 14;
            B_DumpHash.Text = "Hash Tables";
            B_DumpHash.UseVisualStyleBackColor = true;
            B_DumpHash.Click += B_DumpHash_Click;
            // 
            // B_DumpArc
            // 
            B_DumpArc.Location = new System.Drawing.Point(308, 3);
            B_DumpArc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpArc.Name = "B_DumpArc";
            B_DumpArc.Size = new System.Drawing.Size(144, 71);
            B_DumpArc.TabIndex = 18;
            B_DumpArc.Text = "Archives";
            B_DumpArc.UseVisualStyleBackColor = true;
            B_DumpArc.Click += B_DumpArc_Click;
            // 
            // B_DumpSpecific
            // 
            B_DumpSpecific.Location = new System.Drawing.Point(4, 80);
            B_DumpSpecific.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpSpecific.Name = "B_DumpSpecific";
            B_DumpSpecific.Size = new System.Drawing.Size(144, 71);
            B_DumpSpecific.TabIndex = 19;
            B_DumpSpecific.Text = "Specific Packed";
            B_DumpSpecific.UseVisualStyleBackColor = true;
            B_DumpSpecific.Click += B_DumpSpecific_Click;
            // 
            // B_Misc
            // 
            B_Misc.Location = new System.Drawing.Point(156, 80);
            B_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Misc.Name = "B_Misc";
            B_Misc.Size = new System.Drawing.Size(144, 71);
            B_Misc.TabIndex = 20;
            B_Misc.Text = "Misc";
            B_Misc.UseVisualStyleBackColor = true;
            B_Misc.Click += B_DumpMisc_Click;
            // 
            // B_DumpPath
            // 
            B_DumpPath.Location = new System.Drawing.Point(4, 3);
            B_DumpPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpPath.Name = "B_DumpPath";
            B_DumpPath.Size = new System.Drawing.Size(144, 71);
            B_DumpPath.TabIndex = 21;
            B_DumpPath.Text = "Specific File...";
            B_DumpPath.UseVisualStyleBackColor = true;
            B_DumpPath.Click += B_DumpPath_Click;
            // 
            // Tab_Future
            // 
            Tab_Future.Controls.Add(flowLayoutPanel4);
            Tab_Future.Location = new System.Drawing.Point(4, 26);
            Tab_Future.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Future.Name = "Tab_Future";
            Tab_Future.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Future.Size = new System.Drawing.Size(476, 334);
            Tab_Future.TabIndex = 3;
            Tab_Future.Text = "Future";
            Tab_Future.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(B_DistributionRaids);
            flowLayoutPanel4.Controls.Add(B_DeliveryOutbreaks);
            flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel4.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new System.Drawing.Size(468, 328);
            flowLayoutPanel4.TabIndex = 3;
            // 
            // B_DistributionRaids
            // 
            B_DistributionRaids.Location = new System.Drawing.Point(4, 3);
            B_DistributionRaids.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DistributionRaids.Name = "B_DistributionRaids";
            B_DistributionRaids.Size = new System.Drawing.Size(144, 71);
            B_DistributionRaids.TabIndex = 14;
            B_DistributionRaids.Text = "Distribution Raids";
            B_DistributionRaids.UseVisualStyleBackColor = true;
            B_DistributionRaids.Click += B_DistributionRaids_Click;
            // 
            // B_DeliveryOutbreaks
            // 
            B_DeliveryOutbreaks.Location = new System.Drawing.Point(156, 3);
            B_DeliveryOutbreaks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DeliveryOutbreaks.Name = "B_DeliveryOutbreaks";
            B_DeliveryOutbreaks.Size = new System.Drawing.Size(144, 71);
            B_DeliveryOutbreaks.TabIndex = 15;
            B_DeliveryOutbreaks.Text = "Distribution Outbreaks";
            B_DeliveryOutbreaks.UseVisualStyleBackColor = true;
            B_DeliveryOutbreaks.Click += B_DeliveryOutbreaks_Click;
            // 
            // B_OpenFolder
            // 
            B_OpenFolder.Location = new System.Drawing.Point(376, -1);
            B_OpenFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_OpenFolder.Name = "B_OpenFolder";
            B_OpenFolder.Size = new System.Drawing.Size(88, 31);
            B_OpenFolder.TabIndex = 2;
            B_OpenFolder.Text = "Open Folder";
            B_OpenFolder.UseVisualStyleBackColor = true;
            B_OpenFolder.Click += B_OpenFolder_Click;
            // 
            // B_KitakamiPoints
            // 
            B_KitakamiPoints.Location = new System.Drawing.Point(308, 80);
            B_KitakamiPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_KitakamiPoints.Name = "B_KitakamiPoints";
            B_KitakamiPoints.Size = new System.Drawing.Size(144, 71);
            B_KitakamiPoints.TabIndex = 22;
            B_KitakamiPoints.Text = "Kitakami Spawn Points";
            B_KitakamiPoints.UseVisualStyleBackColor = true;
            B_KitakamiPoints.Click += B_KitakamiPoints_Click;
            // 
            // DumperSV
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(484, 364);
            Controls.Add(B_OpenFolder);
            Controls.Add(TC_Options);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "DumperSV";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SVDump";
            TC_Options.ResumeLayout(false);
            Tab_General.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            Tab_Misc.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            Tab_Future.ResumeLayout(false);
            flowLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.Button B_DumpMovesAbilities;
        private System.Windows.Forms.Button B_Cooking;
        private System.Windows.Forms.Button B_DumpPath;
        private System.Windows.Forms.Button B_DeliveryOutbreaks;
        private System.Windows.Forms.Button B_KitakamiPoints;
    }
}
