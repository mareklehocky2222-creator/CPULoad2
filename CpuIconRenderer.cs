using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CpuTrayMonitor
{
    public static class CpuIconRenderer
    {
        public static Icon CreateCpuIcon(int cpu)
        {
            int size = 32;
            Bitmap bmp = new Bitmap(size, size);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.Transparent);

                Color color =
                    cpu <= 1 ? Color.FromArgb(120, 255, 120) :
                    cpu <= 4 ? Color.FromArgb(0, 180, 0) :
                               Color.Orange;

                string text = cpu.ToString();
                int digits = text.Length;

                if (cpu < 10)
                {
                    // 0–9 % → kruh + veľké číslo
                    int circleSize = 13;
                    int circleX = 2;
                    int circleY = (size - circleSize) / 2;

                    using (Brush ring = new SolidBrush(color))
                        g.FillEllipse(ring, circleX, circleY, circleSize, circleSize);

                    float fontSize = 24f; // skoro ako pôvodných 24
                    using (Font font = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        SizeF ts = g.MeasureString(text, font);

                        // trochu bližšie ku guličke, aby to pôsobilo kompaktne
                        float textX = circleX + circleSize - 1;
                        float textY = (size - ts.Height) / 2f;

                        using (Brush brush = new SolidBrush(color))
                        {
                            g.DrawString(text, font, brush, textX, textY);
                        }
                    }
                }
                else
                {
                    // 10 % a viac → iba číslo, čo najväčšie
                    float fontSize = digits switch
                    {
                        2 => 24f,  // 10–99, fakt veľké
                        _ => 20f   // 100, stále výrazné
                    };

                    using (Font font = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                    {
                        SizeF ts = g.MeasureString(text, font);

                        float textX = (size - ts.Width) / 2f;
                        float textY = (size - ts.Height) / 2f;

                        using (Brush brush = new SolidBrush(color))
                        {
                            g.DrawString(text, font, brush, textX, textY);
                        }
                    }
                }
            }

            return Icon.FromHandle(bmp.GetHicon());
        }
    }
}
