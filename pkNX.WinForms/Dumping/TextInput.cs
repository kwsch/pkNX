using System.Windows.Forms;

namespace pkNX.WinForms;

public partial class TextInput : Form
{
    public string Result { get; set; } = string.Empty;
    public TextInput() => InitializeComponent();
    private void Input_TextChanged(object sender, System.EventArgs e) => Result = Input.Text;

    private void B_Export_Click(object sender, System.EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
