namespace pkNX.WinForms
{
    partial class DumperPLA
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
            B_Wild = new System.Windows.Forms.Button();
            B_Static = new System.Windows.Forms.Button();
            B_Gift = new System.Windows.Forms.Button();
            B_ItemInfo = new System.Windows.Forms.Button();
            B_Moves = new System.Windows.Forms.Button();
            B_Details = new System.Windows.Forms.Button();
            B_Placement = new System.Windows.Forms.Button();
            B_Resident = new System.Windows.Forms.Button();
            B_DumpDrops = new System.Windows.Forms.Button();
            Tab_PKHeX = new System.Windows.Forms.TabPage();
            flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            B_PKText = new System.Windows.Forms.Button();
            B_PKLearn = new System.Windows.Forms.Button();
            B_PKEvo = new System.Windows.Forms.Button();
            Tab_Misc = new System.Windows.Forms.TabPage();
            flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            B_DumpHash = new System.Windows.Forms.Button();
            B_FlavorText = new System.Windows.Forms.Button();
            B_Dex = new System.Windows.Forms.Button();
            B_Outbreak = new System.Windows.Forms.Button();
            B_MoveShop = new System.Windows.Forms.Button();
            B_GingkoRareShop = new System.Windows.Forms.Button();
            B_ScriptCommandNames = new System.Windows.Forms.Button();
            B_EventTriggers = new System.Windows.Forms.Button();
            Tab_Future = new System.Windows.Forms.TabPage();
            flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            B_OpenFolder = new System.Windows.Forms.Button();
            TC_Options.SuspendLayout();
            Tab_General.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            Tab_PKHeX.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            Tab_Misc.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            Tab_Future.SuspendLayout();
            SuspendLayout();
            // 
            // TC_Options
            // 
            TC_Options.Controls.Add(Tab_General);
            TC_Options.Controls.Add(Tab_PKHeX);
            TC_Options.Controls.Add(Tab_Misc);
            TC_Options.Controls.Add(Tab_Future);
            TC_Options.Dock = System.Windows.Forms.DockStyle.Fill;
            TC_Options.Location = new System.Drawing.Point(0, 0);
            TC_Options.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TC_Options.Name = "TC_Options";
            TC_Options.SelectedIndex = 0;
            TC_Options.Size = new System.Drawing.Size(473, 310);
            TC_Options.TabIndex = 0;
            // 
            // Tab_General
            // 
            Tab_General.Controls.Add(flowLayoutPanel1);
            Tab_General.Location = new System.Drawing.Point(4, 24);
            Tab_General.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_General.Name = "Tab_General";
            Tab_General.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_General.Size = new System.Drawing.Size(465, 282);
            Tab_General.TabIndex = 0;
            Tab_General.Text = "General";
            Tab_General.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(B_ParsePKMDetails);
            flowLayoutPanel1.Controls.Add(B_Wild);
            flowLayoutPanel1.Controls.Add(B_Static);
            flowLayoutPanel1.Controls.Add(B_Gift);
            flowLayoutPanel1.Controls.Add(B_ItemInfo);
            flowLayoutPanel1.Controls.Add(B_Moves);
            flowLayoutPanel1.Controls.Add(B_Details);
            flowLayoutPanel1.Controls.Add(B_Placement);
            flowLayoutPanel1.Controls.Add(B_Resident);
            flowLayoutPanel1.Controls.Add(B_DumpDrops);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(457, 276);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // B_ParsePKMDetails
            // 
            B_ParsePKMDetails.Location = new System.Drawing.Point(4, 3);
            B_ParsePKMDetails.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_ParsePKMDetails.Name = "B_ParsePKMDetails";
            B_ParsePKMDetails.Size = new System.Drawing.Size(144, 63);
            B_ParsePKMDetails.TabIndex = 1;
            B_ParsePKMDetails.Text = "Personal Stats";
            B_ParsePKMDetails.UseVisualStyleBackColor = true;
            B_ParsePKMDetails.Click += B_ParsePersonal_Click;
            // 
            // B_Wild
            // 
            B_Wild.Location = new System.Drawing.Point(156, 3);
            B_Wild.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Wild.Name = "B_Wild";
            B_Wild.Size = new System.Drawing.Size(144, 63);
            B_Wild.TabIndex = 11;
            B_Wild.Text = "Wild Encounters";
            B_Wild.UseVisualStyleBackColor = true;
            B_Wild.Click += B_Wild_Click;
            // 
            // B_Static
            // 
            B_Static.Location = new System.Drawing.Point(308, 3);
            B_Static.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Static.Name = "B_Static";
            B_Static.Size = new System.Drawing.Size(144, 63);
            B_Static.TabIndex = 10;
            B_Static.Text = "Static Encounters";
            B_Static.UseVisualStyleBackColor = true;
            B_Static.Click += B_Static_Click;
            // 
            // B_Gift
            // 
            B_Gift.Location = new System.Drawing.Point(4, 72);
            B_Gift.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Gift.Name = "B_Gift";
            B_Gift.Size = new System.Drawing.Size(144, 63);
            B_Gift.TabIndex = 9;
            B_Gift.Text = "Gift Encounters";
            B_Gift.UseVisualStyleBackColor = true;
            B_Gift.Click += B_Gift_Click;
            // 
            // B_ItemInfo
            // 
            B_ItemInfo.Location = new System.Drawing.Point(156, 72);
            B_ItemInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_ItemInfo.Name = "B_ItemInfo";
            B_ItemInfo.Size = new System.Drawing.Size(144, 63);
            B_ItemInfo.TabIndex = 6;
            B_ItemInfo.Text = "Item Info";
            B_ItemInfo.UseVisualStyleBackColor = true;
            B_ItemInfo.Click += B_ItemInfo_Click;
            // 
            // B_Moves
            // 
            B_Moves.Location = new System.Drawing.Point(308, 72);
            B_Moves.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Moves.Name = "B_Moves";
            B_Moves.Size = new System.Drawing.Size(144, 63);
            B_Moves.TabIndex = 7;
            B_Moves.Text = "Move Info";
            B_Moves.UseVisualStyleBackColor = true;
            B_Moves.Click += B_Moves_Click;
            // 
            // B_Details
            // 
            B_Details.Location = new System.Drawing.Point(4, 141);
            B_Details.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Details.Name = "B_Details";
            B_Details.Size = new System.Drawing.Size(144, 63);
            B_Details.TabIndex = 15;
            B_Details.Text = "Poke Details";
            B_Details.UseVisualStyleBackColor = true;
            B_Details.Click += B_ParsePKMDetails_Click;
            // 
            // B_Placement
            // 
            B_Placement.Location = new System.Drawing.Point(156, 141);
            B_Placement.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Placement.Name = "B_Placement";
            B_Placement.Size = new System.Drawing.Size(144, 63);
            B_Placement.TabIndex = 18;
            B_Placement.Text = "Placement";
            B_Placement.UseVisualStyleBackColor = true;
            B_Placement.Click += B_Placement_Click;
            // 
            // B_Resident
            // 
            B_Resident.Location = new System.Drawing.Point(308, 141);
            B_Resident.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Resident.Name = "B_Resident";
            B_Resident.Size = new System.Drawing.Size(144, 63);
            B_Resident.TabIndex = 19;
            B_Resident.Text = "Resident";
            B_Resident.UseVisualStyleBackColor = true;
            B_Resident.Click += B_Resident_Click;
            // 
            // B_DumpDrops
            // 
            B_DumpDrops.Location = new System.Drawing.Point(4, 210);
            B_DumpDrops.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpDrops.Name = "B_DumpDrops";
            B_DumpDrops.Size = new System.Drawing.Size(144, 63);
            B_DumpDrops.TabIndex = 20;
            B_DumpDrops.Text = "PokeDrops";
            B_DumpDrops.UseVisualStyleBackColor = true;
            B_DumpDrops.Click += B_PokeDrops_Click;
            // 
            // Tab_PKHeX
            // 
            Tab_PKHeX.Controls.Add(flowLayoutPanel2);
            Tab_PKHeX.Location = new System.Drawing.Point(4, 24);
            Tab_PKHeX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_PKHeX.Name = "Tab_PKHeX";
            Tab_PKHeX.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_PKHeX.Size = new System.Drawing.Size(465, 282);
            Tab_PKHeX.TabIndex = 1;
            Tab_PKHeX.Text = "PKHeX";
            Tab_PKHeX.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(B_PKText);
            flowLayoutPanel2.Controls.Add(B_PKLearn);
            flowLayoutPanel2.Controls.Add(B_PKEvo);
            flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel2.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new System.Drawing.Size(457, 276);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // B_PKText
            // 
            B_PKText.Location = new System.Drawing.Point(4, 3);
            B_PKText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_PKText.Name = "B_PKText";
            B_PKText.Size = new System.Drawing.Size(144, 63);
            B_PKText.TabIndex = 5;
            B_PKText.Text = "Text Files";
            B_PKText.UseVisualStyleBackColor = true;
            B_PKText.Click += B_PKText_Click;
            // 
            // B_PKLearn
            // 
            B_PKLearn.Location = new System.Drawing.Point(156, 3);
            B_PKLearn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_PKLearn.Name = "B_PKLearn";
            B_PKLearn.Size = new System.Drawing.Size(144, 63);
            B_PKLearn.TabIndex = 6;
            B_PKLearn.Text = "Learnset Binary";
            B_PKLearn.UseVisualStyleBackColor = true;
            B_PKLearn.Click += B_PKLearn_Click;
            // 
            // B_PKEvo
            // 
            B_PKEvo.Location = new System.Drawing.Point(308, 3);
            B_PKEvo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_PKEvo.Name = "B_PKEvo";
            B_PKEvo.Size = new System.Drawing.Size(144, 63);
            B_PKEvo.TabIndex = 8;
            B_PKEvo.Text = "Evolution Binary";
            B_PKEvo.UseVisualStyleBackColor = true;
            B_PKEvo.Click += B_PKEvo_Click;
            // 
            // Tab_Misc
            // 
            Tab_Misc.Controls.Add(flowLayoutPanel3);
            Tab_Misc.Location = new System.Drawing.Point(4, 24);
            Tab_Misc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Misc.Name = "Tab_Misc";
            Tab_Misc.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Misc.Size = new System.Drawing.Size(465, 282);
            Tab_Misc.TabIndex = 2;
            Tab_Misc.Text = "Misc";
            Tab_Misc.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(B_DumpHash);
            flowLayoutPanel3.Controls.Add(B_FlavorText);
            flowLayoutPanel3.Controls.Add(B_Dex);
            flowLayoutPanel3.Controls.Add(B_Outbreak);
            flowLayoutPanel3.Controls.Add(B_MoveShop);
            flowLayoutPanel3.Controls.Add(B_GingkoRareShop);
            flowLayoutPanel3.Controls.Add(B_ScriptCommandNames);
            flowLayoutPanel3.Controls.Add(B_EventTriggers);
            flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel3.Location = new System.Drawing.Point(4, 3);
            flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new System.Drawing.Size(457, 276);
            flowLayoutPanel3.TabIndex = 2;
            // 
            // B_DumpHash
            // 
            B_DumpHash.Location = new System.Drawing.Point(4, 3);
            B_DumpHash.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_DumpHash.Name = "B_DumpHash";
            B_DumpHash.Size = new System.Drawing.Size(144, 63);
            B_DumpHash.TabIndex = 14;
            B_DumpHash.Text = "Hash Tables";
            B_DumpHash.UseVisualStyleBackColor = true;
            B_DumpHash.Click += B_DumpHash_Click;
            // 
            // B_FlavorText
            // 
            B_FlavorText.Location = new System.Drawing.Point(156, 3);
            B_FlavorText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_FlavorText.Name = "B_FlavorText";
            B_FlavorText.Size = new System.Drawing.Size(144, 63);
            B_FlavorText.TabIndex = 16;
            B_FlavorText.Text = "Flavor Text";
            B_FlavorText.UseVisualStyleBackColor = true;
            B_FlavorText.Click += B_FlavorText_Click;
            // 
            // B_Dex
            // 
            B_Dex.Location = new System.Drawing.Point(308, 3);
            B_Dex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Dex.Name = "B_Dex";
            B_Dex.Size = new System.Drawing.Size(144, 63);
            B_Dex.TabIndex = 18;
            B_Dex.Text = "Dex Indexes";
            B_Dex.UseVisualStyleBackColor = true;
            B_Dex.Click += B_GetDex_Click;
            // 
            // B_Outbreak
            // 
            B_Outbreak.Location = new System.Drawing.Point(4, 72);
            B_Outbreak.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_Outbreak.Name = "B_Outbreak";
            B_Outbreak.Size = new System.Drawing.Size(144, 63);
            B_Outbreak.TabIndex = 19;
            B_Outbreak.Text = "Outbreaks";
            B_Outbreak.UseVisualStyleBackColor = true;
            B_Outbreak.Click += B_GetOutbreak_Click;
            // 
            // B_MoveShop
            // 
            B_MoveShop.Location = new System.Drawing.Point(156, 72);
            B_MoveShop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_MoveShop.Name = "B_MoveShop";
            B_MoveShop.Size = new System.Drawing.Size(144, 63);
            B_MoveShop.TabIndex = 20;
            B_MoveShop.Text = "Move Shop";
            B_MoveShop.UseVisualStyleBackColor = true;
            B_MoveShop.Click += B_MoveShop_Click;
            // 
            // B_GingkoRareShop
            // 
            B_GingkoRareShop.Location = new System.Drawing.Point(308, 72);
            B_GingkoRareShop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_GingkoRareShop.Name = "B_GingkoRareShop";
            B_GingkoRareShop.Size = new System.Drawing.Size(144, 63);
            B_GingkoRareShop.TabIndex = 23;
            B_GingkoRareShop.Text = "Gingko Rare Shop";
            B_GingkoRareShop.UseVisualStyleBackColor = true;
            B_GingkoRareShop.Click += B_GingkoRareShop_Click;
            // 
            // B_ScriptCommandNames
            // 
            B_ScriptCommandNames.Location = new System.Drawing.Point(4, 141);
            B_ScriptCommandNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_ScriptCommandNames.Name = "B_ScriptCommandNames";
            B_ScriptCommandNames.Size = new System.Drawing.Size(144, 63);
            B_ScriptCommandNames.TabIndex = 21;
            B_ScriptCommandNames.Text = "Script Commands";
            B_ScriptCommandNames.UseVisualStyleBackColor = true;
            B_ScriptCommandNames.Click += B_DumpScriptCommands;
            // 
            // B_EventTriggers
            // 
            B_EventTriggers.Location = new System.Drawing.Point(156, 141);
            B_EventTriggers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_EventTriggers.Name = "B_EventTriggers";
            B_EventTriggers.Size = new System.Drawing.Size(144, 63);
            B_EventTriggers.TabIndex = 22;
            B_EventTriggers.Text = "Event Triggers";
            B_EventTriggers.UseVisualStyleBackColor = true;
            B_EventTriggers.Click += B_EventTriggers_Click;
            // 
            // Tab_Future
            // 
            Tab_Future.Controls.Add(flowLayoutPanel4);
            Tab_Future.Location = new System.Drawing.Point(4, 24);
            Tab_Future.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Future.Name = "Tab_Future";
            Tab_Future.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Future.Size = new System.Drawing.Size(465, 282);
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
            flowLayoutPanel4.Size = new System.Drawing.Size(457, 276);
            flowLayoutPanel4.TabIndex = 3;
            // 
            // B_OpenFolder
            // 
            B_OpenFolder.Location = new System.Drawing.Point(376, -1);
            B_OpenFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            B_OpenFolder.Name = "B_OpenFolder";
            B_OpenFolder.Size = new System.Drawing.Size(88, 27);
            B_OpenFolder.TabIndex = 2;
            B_OpenFolder.Text = "Open Folder";
            B_OpenFolder.UseVisualStyleBackColor = true;
            B_OpenFolder.Click += B_OpenFolder_Click;
            // 
            // DumperPLA
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(473, 310);
            Controls.Add(B_OpenFolder);
            Controls.Add(TC_Options);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "DumperPLA";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "PLADump";
            TC_Options.ResumeLayout(false);
            Tab_General.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            Tab_PKHeX.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            Tab_Misc.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
            Tab_Future.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl TC_Options;
        private System.Windows.Forms.TabPage Tab_General;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button B_ParsePKMDetails;
        private System.Windows.Forms.Button B_ItemInfo;
        private System.Windows.Forms.Button B_Moves;
        private System.Windows.Forms.TabPage Tab_PKHeX;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button B_PKText;
        private System.Windows.Forms.Button B_PKLearn;
        private System.Windows.Forms.Button B_PKEvo;
        private System.Windows.Forms.Button B_Gift;
        private System.Windows.Forms.Button B_Static;
        private System.Windows.Forms.Button B_Wild;
        private System.Windows.Forms.TabPage Tab_Misc;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button B_DumpHash;
        private System.Windows.Forms.Button B_FlavorText;
        private System.Windows.Forms.Button B_OpenFolder;
        private System.Windows.Forms.TabPage Tab_Future;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button B_Details;
        private System.Windows.Forms.Button B_Dex;
        private System.Windows.Forms.Button B_Outbreak;
        private System.Windows.Forms.Button B_MoveShop;
        private System.Windows.Forms.Button B_GingkoRareShop;
        private System.Windows.Forms.Button B_ScriptCommandNames;
        private System.Windows.Forms.Button B_Placement;
        private System.Windows.Forms.Button B_Resident;
        private System.Windows.Forms.Button B_DumpDrops;
        private System.Windows.Forms.Button B_EventTriggers;
    }
}
