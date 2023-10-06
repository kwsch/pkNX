namespace pkNX.WinForms
{
    partial class TextInput
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
            Input = new System.Windows.Forms.TextBox();
            B_Export = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // Input
            // 
            Input.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Input.Location = new System.Drawing.Point(12, 12);
            Input.Name = "Input";
            Input.Size = new System.Drawing.Size(320, 23);
            Input.TabIndex = 0;
            Input.TextChanged += Input_TextChanged;
            // 
            // B_Export
            // 
            B_Export.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            B_Export.Location = new System.Drawing.Point(257, 41);
            B_Export.Name = "B_Export";
            B_Export.Size = new System.Drawing.Size(75, 23);
            B_Export.TabIndex = 1;
            B_Export.Text = "Export...";
            B_Export.UseVisualStyleBackColor = true;
            B_Export.Click += B_Export_Click;
            // 
            // TextInput
            // 
            ClientSize = new System.Drawing.Size(344, 72);
            Controls.Add(B_Export);
            Controls.Add(Input);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(360, 111);
            Name = "TextInput";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Input Text";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Button B_Export;
    }
}
