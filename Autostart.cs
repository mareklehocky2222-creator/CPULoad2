using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace CpuTrayMonitor
{
    public static class Autostart
    {
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "CPULoad2";

        private static RegistryKey OpenRunKey(bool writable)
        {
            return Registry.CurrentUser.OpenSubKey(RunKeyPath, writable);
        }

        public static bool IsEnabled()
        {
            try
            {
                using (RegistryKey key = OpenRunKey(false))
                {
                    if (key == null)
                        return false;

                    return key.GetValue(AppName) != null;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void Enable()
        {
            try
            {
                using (RegistryKey key = OpenRunKey(true))
                {
                    if (key == null)
                        return;

                    string exePath = Application.ExecutablePath;
                    key.SetValue(AppName, exePath);
                }
            }
            catch
            {
                // nechceme otravovať usera chybou
            }
        }

        public static void Disable()
        {
            try
            {
                using (RegistryKey key = OpenRunKey(true))
                {
                    if (key == null)
                        return;

                    key.DeleteValue(AppName, false);
                }
            }
            catch
            {
                // ignorujeme chyby
            }
        }
    }
}
