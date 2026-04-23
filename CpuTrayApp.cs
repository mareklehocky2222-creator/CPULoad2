using System;
using System.Diagnostics;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CpuTrayMonitor
{
    public class CpuTrayApp : Form
    {
        private readonly NotifyIcon _trayIcon;
        private readonly Timer _timer;
        private readonly PerformanceCounter _cpuCounter;

        public CpuTrayApp()
        {
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
            Visible = false;

            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            _trayIcon = new NotifyIcon
            {
                Visible = true,
                Text = "CPU Load"
            };

            var menu = new ContextMenuStrip();
            var exit = new ToolStripMenuItem("Exit");
            exit.Click += (_, _) => Application.Exit();
            menu.Items.Add(exit);
            _trayIcon.ContextMenuStrip = menu;

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += UpdateCpu;
            _timer.Start();
        }

        private void UpdateCpu(object? sender, EventArgs e)
        {
            int cpu = (int)_cpuCounter.NextValue();
            if (cpu < 0) cpu = 0;
            if (cpu > 100) cpu = 100;

            _trayIcon.Icon = CpuIconRenderer.CreateCpuIcon(cpu);
            _trayIcon.Text = $"CPU: {cpu}%";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            _cpuCounter.Dispose();
            base.OnFormClosing(e);
        }
    }
}