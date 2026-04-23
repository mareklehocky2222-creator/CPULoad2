using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CpuTrayMonitor
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private System.Windows.Forms.Timer timer;
        private PerformanceCounter cpuCounter;
        private ToolStripMenuItem autostartMenuItem;

        public TrayAppContext()
        {
            // CPU counter
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // Tray icon
            trayIcon = new NotifyIcon
            {
                Visible = true,
                Text = "CPULoad2",
                ContextMenuStrip = new ContextMenuStrip()
            };

            // Autostart položka
            autostartMenuItem = new ToolStripMenuItem();
            autostartMenuItem.Click += ToggleAutostart;

            // Synchronizácia s installerom
            SyncAutostartWithInstaller();

            // Nastaví text podľa registry
            UpdateAutostartMenuText();

            trayIcon.ContextMenuStrip.Items.Add(autostartMenuItem);
            trayIcon.ContextMenuStrip.Items.Add("About", null, (s, e) => ShowAbout());
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Exit());

            // Timer
            timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void SyncAutostartWithInstaller()
        {
            string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcut = Path.Combine(startup, "CPULoad2.lnk");

            if (File.Exists(shortcut))
            {
                Autostart.Enable();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int cpu = (int)cpuCounter.NextValue();

            Icon icon = CpuIconRenderer.CreateCpuIcon(cpu);
            trayIcon.Icon = icon;

            trayIcon.Text = $"CPU: {cpu}%";
        }

        private void ToggleAutostart(object sender, EventArgs e)
        {
            if (Autostart.IsEnabled())
                Autostart.Disable();
            else
                Autostart.Enable();

            UpdateAutostartMenuText();
        }

        private void UpdateAutostartMenuText()
        {
            autostartMenuItem.Text = Autostart.IsEnabled()
                ? "Disable autostart"
                : "Enable autostart";
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "Marek Lehocký\n(2026)",
                "CPULoad2 – About",
                MessageBoxButtons.OK,
                MessageBoxIcon.None
            );
        }

        private void Exit()
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
