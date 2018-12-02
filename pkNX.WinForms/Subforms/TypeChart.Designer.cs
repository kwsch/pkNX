namespace pkNX.WinForms
{
    partial class TypeChart
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
            this.B_Save = new System.Windows.Forms.Button();
            this.B_RTM = new System.Windows.Forms.Button();
            this.PB_Chart = new System.Windows.Forms.PictureBox();
            this.L_Hover = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Chart)).BeginInit();
            this.SuspendLayout();
            //
            // B_Save
            //
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(530, 12);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(60, 23);
            this.B_Save.TabIndex = 1;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            //
            // B_RTM
            //
            this.B_RTM.Location = new System.Drawing.Point(12, 12);
            this.B_RTM.Name = "B_RTM";
            this.B_RTM.Size = new System.Drawing.Size(75, 23);
            this.B_RTM.TabIndex = 7;
            this.B_RTM.Text = "Randomize";
            this.B_RTM.UseVisualStyleBackColor = true;
            this.B_RTM.Click += new System.EventHandler(this.B_RTM_Click);
            //
            // PB_Chart
            //
            this.PB_Chart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Chart.Location = new System.Drawing.Point(12, 41);
            this.PB_Chart.Name = "PB_Chart";
            this.PB_Chart.Size = new System.Drawing.Size(579, 579);
            this.PB_Chart.TabIndex = 469;
            this.PB_Chart.TabStop = false;
            this.PB_Chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ClickMouse);
            this.PB_Chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveMouse);
            //
            // L_Hover
            //
            this.L_Hover.AutoSize = true;
            this.L_Hover.Location = new System.Drawing.Point(93, 17);
            this.L_Hover.Name = "L_Hover";
            this.L_Hover.Size = new System.Drawing.Size(117, 13);
            this.L_Hover.TabIndex = 471;
            this.L_Hover.Text = "Effectiveness Summary";
            //
            // TypeChart
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 627);
            this.Controls.Add(this.L_Hover);
            this.Controls.Add(this.PB_Chart);
            this.Controls.Add(this.B_RTM);
            this.Controls.Add(this.B_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "TypeChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Type Chart";
            ((System.ComponentModel.ISupportInitialize)(this.PB_Chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.Button B_RTM;
        private System.Windows.Forms.PictureBox PB_Chart;
        private System.Windows.Forms.Label L_Hover;
    }
}