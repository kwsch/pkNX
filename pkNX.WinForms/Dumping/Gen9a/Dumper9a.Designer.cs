namespace pkNX.WinForms
{
    partial class Dumper9a
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
            B_PKText = new System.Windows.Forms.Button();
            B_Trainers = new System.Windows.Forms.Button();
            B_Encount = new System.Windows.Forms.Button();
            B_DumpMovesAbilities = new System.Windows.Forms.Button();
            B_Mega = new System.Windows.Forms.Button();
            B_DumpItems = new System.Windows.Forms.Button();
            B_DumpPokeData = new System.Windows.Forms.Button();
            B_DumpEncountData = new System.Windows.Forms.Button();
            Tab_Misc = new System.Windows.Forms.TabPage();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            B_DumpPath = new System.Windows.Forms.Button();
            B_DumpSpecific = new System.Windows.Forms.Button();
            B_DumpFromTextFile = new System.Windows.Forms.Button();
            B_DumpHash = new System.Windows.Forms.Button();
            B_DumpArc = new System.Windows.Forms.Button();
            B_SearchPattern = new System.Windows.Forms.Button();
            B_Misc = new System.Windows.Forms.Button();
            B_Scrubbed = new System.Windows.Forms.Button();
            B_DumpTriggers = new System.Windows.Forms.Button();
            Tab_PKHeX = new System.Windows.Forms.TabPage();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            B_Locations = new System.Windows.Forms.Button();
            Tab_Future = new System.Windows.Forms.TabPage();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            B_OpenFolder = new System.Windows.Forms.Button();
            B_DumpConfig = new System.Windows.Forms.Button();
            TC_Options.SuspendLayout();
            Tab_General.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            Tab_Misc.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            Tab_PKHeX.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            Tab_Future.SuspendLayout();
            SuspendLayout();
            // 
            // TC_Options
            // 
            TC_Options.Controls.Add(Tab_General);
            TC_Options.Controls.Add(Tab_Misc);
            TC_Options.Controls.Add(Tab_PKHeX);
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
            flowLayoutPanel1.Controls.Add(B_PKText);
            flowLayoutPanel1.Controls.Add(B_Trainers);
            flowLayoutPanel1.Controls.Add(B_Encount);
            flowLayoutPanel1.Controls.Add(B_DumpMovesAbilities);
            flowLayoutPanel1.Controls.Add(B_Mega);
            flowLayoutPanel1.Controls.Add(B_DumpItems);
            flowLayoutPanel1.Controls.Add(B_DumpPokeData);
            flowLayoutPanel1.Controls.Add(B_DumpEncountData);
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
            // B_PKText
            // 
            B_PKText.Location = new System.Drawing.Point(156, 3);
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
            B_Trainers.Location = new System.Drawing.Point(308, 3);
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
            B_Encount.Location = new System.Drawing.Point(4, 80);
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
            B_DumpMovesAbilities.Location = new System.Drawing.Point(156, 80);
            B_DumpMovesAbilities.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpMovesAbilities.Name = "B_DumpMovesAbilities";
            B_DumpMovesAbilities.Size = new System.Drawing.Size(144, 71);
            B_DumpMovesAbilities.TabIndex = 20;
            B_DumpMovesAbilities.Text = "Moves/Abilities";
            B_DumpMovesAbilities.UseVisualStyleBackColor = true;
            B_DumpMovesAbilities.Click += B_DumpMovesAbilities_Click;
            // 
            // B_Mega
            // 
            B_Mega.Location = new System.Drawing.Point(308, 80);
            B_Mega.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Mega.Name = "B_Mega";
            B_Mega.Size = new System.Drawing.Size(144, 71);
            B_Mega.TabIndex = 25;
            B_Mega.Text = "Mega";
            B_Mega.UseVisualStyleBackColor = true;
            B_Mega.Click += B_Mega_Click;
            // 
            // B_DumpItems
            // 
            B_DumpItems.Location = new System.Drawing.Point(4, 157);
            B_DumpItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpItems.Name = "B_DumpItems";
            B_DumpItems.Size = new System.Drawing.Size(144, 71);
            B_DumpItems.TabIndex = 26;
            B_DumpItems.Text = "Items";
            B_DumpItems.UseVisualStyleBackColor = true;
            B_DumpItems.Click += B_DumpItems_Click;
            // 
            // B_DumpPokeData
            // 
            B_DumpPokeData.Location = new System.Drawing.Point(156, 157);
            B_DumpPokeData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpPokeData.Name = "B_DumpPokeData";
            B_DumpPokeData.Size = new System.Drawing.Size(144, 71);
            B_DumpPokeData.TabIndex = 27;
            B_DumpPokeData.Text = "PokeData";
            B_DumpPokeData.UseVisualStyleBackColor = true;
            B_DumpPokeData.Click += B_DumpPokeData_Click;
            // 
            // B_DumpEncountData
            // 
            B_DumpEncountData.Location = new System.Drawing.Point(308, 157);
            B_DumpEncountData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpEncountData.Name = "B_DumpEncountData";
            B_DumpEncountData.Size = new System.Drawing.Size(144, 71);
            B_DumpEncountData.TabIndex = 28;
            B_DumpEncountData.Text = "EncountData";
            B_DumpEncountData.UseVisualStyleBackColor = true;
            B_DumpEncountData.Click += B_DumpEncountData_Click;
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
            flowLayoutPanel3.Controls.Add(B_DumpSpecific);
            flowLayoutPanel3.Controls.Add(B_DumpFromTextFile);
            flowLayoutPanel3.Controls.Add(B_DumpHash);
            flowLayoutPanel3.Controls.Add(B_DumpArc);
            flowLayoutPanel3.Controls.Add(B_SearchPattern);
            flowLayoutPanel3.Controls.Add(B_Misc);
            flowLayoutPanel3.Controls.Add(B_Scrubbed);
            flowLayoutPanel3.Controls.Add(B_DumpTriggers);
            flowLayoutPanel3.Controls.Add(B_DumpConfig);
            flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel3.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(468, 328);
            flowLayoutPanel3.TabIndex = 2;
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
            // B_DumpSpecific
            // 
            B_DumpSpecific.Location = new System.Drawing.Point(156, 3);
            B_DumpSpecific.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpSpecific.Name = "B_DumpSpecific";
            B_DumpSpecific.Size = new System.Drawing.Size(144, 71);
            B_DumpSpecific.TabIndex = 19;
            B_DumpSpecific.Text = "Specific Reflection";
            B_DumpSpecific.UseVisualStyleBackColor = true;
            B_DumpSpecific.Click += B_DumpSpecific_Click;
            // 
            // B_DumpFromTextFile
            // 
            B_DumpFromTextFile.Location = new System.Drawing.Point(308, 3);
            B_DumpFromTextFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpFromTextFile.Name = "B_DumpFromTextFile";
            B_DumpFromTextFile.Size = new System.Drawing.Size(144, 71);
            B_DumpFromTextFile.TabIndex = 23;
            B_DumpFromTextFile.Text = "From Text File...";
            B_DumpFromTextFile.UseVisualStyleBackColor = true;
            B_DumpFromTextFile.Click += B_DumpFromTextFile_Click;
            // 
            // B_DumpHash
            // 
            B_DumpHash.Location = new System.Drawing.Point(4, 80);
            B_DumpHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpHash.Name = "B_DumpHash";
            B_DumpHash.Size = new System.Drawing.Size(144, 71);
            B_DumpHash.TabIndex = 14;
            B_DumpHash.Text = "Hash Tables (AHTB)";
            B_DumpHash.UseVisualStyleBackColor = true;
            B_DumpHash.Click += B_DumpHash_Click;
            // 
            // B_DumpArc
            // 
            B_DumpArc.Location = new System.Drawing.Point(156, 80);
            B_DumpArc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpArc.Name = "B_DumpArc";
            B_DumpArc.Size = new System.Drawing.Size(144, 71);
            B_DumpArc.TabIndex = 18;
            B_DumpArc.Text = "Archives (Slow)";
            B_DumpArc.UseVisualStyleBackColor = true;
            B_DumpArc.Click += B_DumpArc_Click;
            // 
            // B_SearchPattern
            // 
            B_SearchPattern.Location = new System.Drawing.Point(308, 80);
            B_SearchPattern.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_SearchPattern.Name = "B_SearchPattern";
            B_SearchPattern.Size = new System.Drawing.Size(144, 71);
            B_SearchPattern.TabIndex = 24;
            B_SearchPattern.Text = "Search Pattern (Slow)";
            B_SearchPattern.UseVisualStyleBackColor = true;
            B_SearchPattern.Click += B_SearchPattern_Click;
            // 
            // B_Misc
            // 
            B_Misc.Location = new System.Drawing.Point(4, 157);
            B_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Misc.Name = "B_Misc";
            B_Misc.Size = new System.Drawing.Size(144, 71);
            B_Misc.TabIndex = 20;
            B_Misc.Text = "Misc";
            B_Misc.UseVisualStyleBackColor = true;
            B_Misc.Click += B_DumpMisc_Click;
            // 
            // B_Scrubbed
            // 
            B_Scrubbed.Location = new System.Drawing.Point(156, 157);
            B_Scrubbed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Scrubbed.Name = "B_Scrubbed";
            B_Scrubbed.Size = new System.Drawing.Size(144, 71);
            B_Scrubbed.TabIndex = 25;
            B_Scrubbed.Text = "Scrubbed";
            B_Scrubbed.UseVisualStyleBackColor = true;
            B_Scrubbed.Click += B_DumpScrubbed_Click;
            // 
            // B_DumpTriggers
            // 
            B_DumpTriggers.Location = new System.Drawing.Point(308, 157);
            B_DumpTriggers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpTriggers.Name = "B_DumpTriggers";
            B_DumpTriggers.Size = new System.Drawing.Size(144, 71);
            B_DumpTriggers.TabIndex = 27;
            B_DumpTriggers.Text = "Triggers";
            B_DumpTriggers.UseVisualStyleBackColor = true;
            B_DumpTriggers.Click += B_Triggers_Click;
            // 
            // Tab_PKHeX
            // 
            Tab_PKHeX.Controls.Add(flowLayoutPanel2);
            Tab_PKHeX.Location = new System.Drawing.Point(4, 26);
            Tab_PKHeX.Name = "Tab_PKHeX";
            Tab_PKHeX.Padding = new System.Windows.Forms.Padding(3);
            Tab_PKHeX.Size = new System.Drawing.Size(476, 334);
            Tab_PKHeX.TabIndex = 4;
            Tab_PKHeX.Text = "PKHeX";
            Tab_PKHeX.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(B_Locations);
            flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(470, 328);
            flowLayoutPanel2.TabIndex = 4;
            // 
            // B_Locations
            // 
            B_Locations.Location = new System.Drawing.Point(4, 3);
            B_Locations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Locations.Name = "B_Locations";
            B_Locations.Size = new System.Drawing.Size(144, 71);
            B_Locations.TabIndex = 21;
            B_Locations.Text = "Locations";
            B_Locations.UseVisualStyleBackColor = true;
            B_Locations.Click += B_Locations_Click;
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
            flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel4.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new System.Drawing.Size(468, 328);
            flowLayoutPanel4.TabIndex = 3;
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
            // B_DumpConfig
            // 
            B_DumpConfig.Location = new System.Drawing.Point(4, 234);
            B_DumpConfig.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpConfig.Name = "B_DumpConfig";
            B_DumpConfig.Size = new System.Drawing.Size(144, 71);
            B_DumpConfig.TabIndex = 28;
            B_DumpConfig.Text = "Config";
            B_DumpConfig.UseVisualStyleBackColor = true;
            B_DumpConfig.Click += B_DumpConfig_Click;
            // 
            // Dumper9a
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(484, 364);
            Controls.Add(B_OpenFolder);
            Controls.Add(TC_Options);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Dumper9a";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "ZADump";
            TC_Options.ResumeLayout(false);
            Tab_General.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            Tab_Misc.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            Tab_PKHeX.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            Tab_Future.ResumeLayout(false);
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
        private System.Windows.Forms.Button B_DumpArc;
        private System.Windows.Forms.Button B_DumpSpecific;
        private System.Windows.Forms.Button B_PKText;
        private System.Windows.Forms.Button B_Trainers;
        private System.Windows.Forms.Button B_Misc;
        private System.Windows.Forms.Button B_Encount;
        private System.Windows.Forms.Button B_DumpMovesAbilities;
        private System.Windows.Forms.Button B_DumpPath;
        private System.Windows.Forms.Button B_DumpFromTextFile;
        private System.Windows.Forms.TabPage Tab_PKHeX;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button B_Locations;
        private System.Windows.Forms.Button B_Mega;
        private System.Windows.Forms.Button B_SearchPattern;
        private System.Windows.Forms.Button B_DumpItems;
        private System.Windows.Forms.Button B_DumpPokeData;
        private System.Windows.Forms.Button B_DumpEncountData;
        private System.Windows.Forms.Button B_Scrubbed;
        private System.Windows.Forms.Button B_DumpTriggers;
        private System.Windows.Forms.Button B_DumpConfig;
    }
}
