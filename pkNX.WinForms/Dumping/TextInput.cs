using System;
using System.Drawing;
using System.Windows.Forms;

namespace pkNX.WinForms;

public partial class TextInput : Form
{
    public string Result { get; set; } = string.Empty;
    private readonly Func<string, bool>? Validator;

    public TextInput(Func<string, bool>? validator = null)
    {
        Validator = validator;
        InitializeComponent();
    }

    private void Input_TextChanged(object sender, System.EventArgs e)
    {
        Result = Input.Text;
        if (Validator != null)
            B_Export.ForeColor = Validator(Result) ? Color.Green : Color.Red;
    }

    private void B_Export_Click(object sender, System.EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
