using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pkNX.WinForms;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // Make FlatSharp build the serializers for our FlatBuffer objects now and take the startup penalty async
        // Opening a FlatBuffer editor later won't be hit with a >5s delay assuming the user opens the editor no earlier than 10 seconds after program startup.
        _ = Task.Run(() => _ = Structures.FlatBuffers.FlatBufferConverter.SerializeFrom(new Structures.FlatBuffers.Waza8()));

        // Fix number values displaying incorrectly for certain cultures.
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Main());
    }
}
