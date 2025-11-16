using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace pkNX.WinForms;

internal static class Program
{
    public static ProgramSettings Settings { get; }

    static Program() => Settings = ProgramSettings.LoadSettings();

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // Fix number values displaying incorrectly for certain cultures.
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Main());
    }
}
