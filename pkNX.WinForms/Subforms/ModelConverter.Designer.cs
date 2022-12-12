using pkNX.WinForms.Subforms;

namespace pkNX.WinForms
{
    partial class ModelConverter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.TT_ButtonTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.PG_Test = new System.Windows.Forms.PropertyGrid();
            this.PG_Test_SWSH = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.B_ShowConverter = new System.Windows.Forms.Button();
            this.B_Convert = new System.Windows.Forms.Button();
            this.PG_Converted = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CB_SWSHSpecies = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(168, 60);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(75, 25);
            label1.TabIndex = 3;
            label1.Text = "Species:";
            // 
            // PG_Test
            // 
            this.PG_Test.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_Test.Location = new System.Drawing.Point(6, 106);
            this.PG_Test.Margin = new System.Windows.Forms.Padding(6);
            this.PG_Test.Name = "PG_Test";
            this.PG_Test.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PG_Test.Size = new System.Drawing.Size(453, 873);
            this.PG_Test.TabIndex = 1;
            // 
            // PG_Test_SWSH
            // 
            this.PG_Test_SWSH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_Test_SWSH.Location = new System.Drawing.Point(471, 106);
            this.PG_Test_SWSH.Margin = new System.Windows.Forms.Padding(6);
            this.PG_Test_SWSH.Name = "PG_Test_SWSH";
            this.PG_Test_SWSH.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PG_Test_SWSH.Size = new System.Drawing.Size(453, 873);
            this.PG_Test_SWSH.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.B_Convert, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.PG_Converted, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.PG_Test, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PG_Test_SWSH, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1395, 985);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.CB_Species);
            this.panel1.Controls.Add(this.B_ShowConverter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(459, 94);
            this.panel1.TabIndex = 5;
            // 
            // CB_Species
            // 
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(249, 57);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(203, 33);
            this.CB_Species.TabIndex = 2;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.CB_Species_SelectedIndexChanged);
            // 
            // B_ShowConverter
            // 
            this.B_ShowConverter.Location = new System.Drawing.Point(231, 9);
            this.B_ShowConverter.Name = "B_ShowConverter";
            this.B_ShowConverter.Size = new System.Drawing.Size(209, 34);
            this.B_ShowConverter.TabIndex = 1;
            this.B_ShowConverter.Text = "Open Matrix Converter";
            this.B_ShowConverter.UseVisualStyleBackColor = true;
            this.B_ShowConverter.Click += new System.EventHandler(this.B_ShowConverter_Click);
            // 
            // B_Convert
            // 
            this.B_Convert.Location = new System.Drawing.Point(940, 40);
            this.B_Convert.Margin = new System.Windows.Forms.Padding(10, 40, 3, 3);
            this.B_Convert.Name = "B_Convert";
            this.B_Convert.Size = new System.Drawing.Size(125, 34);
            this.B_Convert.TabIndex = 6;
            this.B_Convert.Text = "Convert";
            this.B_Convert.UseVisualStyleBackColor = true;
            this.B_Convert.Click += new System.EventHandler(this.B_Convert_Click);
            // 
            // PG_Converted
            // 
            this.PG_Converted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_Converted.Location = new System.Drawing.Point(936, 106);
            this.PG_Converted.Margin = new System.Windows.Forms.Padding(6);
            this.PG_Converted.Name = "PG_Converted";
            this.PG_Converted.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PG_Converted.Size = new System.Drawing.Size(453, 873);
            this.PG_Converted.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(label2);
            this.panel2.Controls.Add(this.CB_SWSHSpecies);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(468, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(459, 94);
            this.panel2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(172, 60);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(75, 25);
            label2.TabIndex = 5;
            label2.Text = "Species:";
            // 
            // CB_SWSHSpecies
            // 
            this.CB_SWSHSpecies.FormattingEnabled = true;
            this.CB_SWSHSpecies.Location = new System.Drawing.Point(253, 57);
            this.CB_SWSHSpecies.Name = "CB_SWSHSpecies";
            this.CB_SWSHSpecies.Size = new System.Drawing.Size(203, 33);
            this.CB_SWSHSpecies.TabIndex = 4;
            this.CB_SWSHSpecies.SelectedIndexChanged += new System.EventHandler(this.CB_SWSHSpecies_SelectedIndexChanged);
            // 
            // ModelConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1395, 985);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(925, 1023);
            this.Name = "ModelConverter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TRModel Converter";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip TT_ButtonTooltip;
        private System.Windows.Forms.PropertyGrid PG_Test;
        private System.Windows.Forms.PropertyGrid PG_Test_SWSH;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button B_Convert;
        private System.Windows.Forms.PropertyGrid PG_Converted;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button B_ShowConverter;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CB_SWSHSpecies;
    }
}
