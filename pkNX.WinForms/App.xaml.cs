using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace pkNX.WinForms;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public sealed partial class App
{
    private App()
    {
        // Fix number values displaying incorrectly for certain cultures.
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
    }
}
