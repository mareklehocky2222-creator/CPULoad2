using System.Drawing;
using System.Windows.Forms;

namespace CpuTrayMonitor
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            Text = "CPULOAD2 – About";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(260, 100);

            var label = new Label();
            label.Text = "Marek Lehocký\n(2026)";
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            var button = new Button();
            button.Text = "OK";
            button.DialogResult = DialogResult.OK;
            button.Anchor = AnchorStyles.Bottom;
            button.Width = 60;
            button.Height = 25;
            button.Top = ClientSize.Height - button.Height - 10;
            button.Left = (ClientSize.Width - button.Width) / 2;

            Controls.Add(label);
            Controls.Add(button);

            AcceptButton = button;
        }
    }
}
