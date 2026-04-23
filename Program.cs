using System;
using System.Windows.Forms;

namespace CpuTrayMonitor
{
    internal static class Program
    {
        [STAThread]   // Kritické pre WinForms + tray + autostart v .NET 8
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new TrayAppContext());
        }
    }
}

