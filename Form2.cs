using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telescope_Tester
{
    public partial class Form2 : Form
    {
        private Bitmap obraz;
        private Bitmap buforPanel;
        private int xx, yy, x, y, width;

        bool MouseDown = false;
        bool selected = false;

        Rectangle rect;
        Point StartROI, EndROI;

        public Form2(Bitmap loadedImage, int w, int h)
        {
            InitializeComponent();
            numericUpDown1.DecimalPlaces = 1;
            numericUpDown1.Increment = 0.1M;
            rect = Rectangle.Empty;  
            x = 600;
            y = (int)(h * 600 / w);
            obraz = new Bitmap(loadedImage, new Size(x, y));
            buforPanel = new Bitmap(x, y);
            Graphics gg = Graphics.FromImage(obraz);
            pictureBox1.Image = obraz;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            xx = -1;
            yy = -1;
            rysujBitmape();
        }

        private void rysujBitmape()
        {
            Graphics ggBufor = Graphics.FromImage(buforPanel);
            Graphics gg = pictureBox1.CreateGraphics();

            ggBufor.FillRectangle(Brushes.Black, 0, 0, x, y);
            ggBufor.TranslateTransform(x / 2, y / 2);
            ggBufor.RotateTransform((float)numericUpDown1.Value);
            //ggBufor.DrawImage(obraz, -50, -50);

            ColorMatrix matrix = new ColorMatrix();
            //matrix.Matrix33 = (float)(1.0 * trackBar2.Value/trackBar2.Maximum);
            matrix.Matrix33 = (float)1.0;

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //now draw the image  
            ggBufor.DrawImage(obraz, new Rectangle(-obraz.Width / 2, -obraz.Height / 2, obraz.Width, obraz.Height), 0, 0, obraz.Width, obraz.Height, GraphicsUnit.Pixel, attributes);

            ggBufor.RotateTransform(-(float)numericUpDown1.Value);
            ggBufor.TranslateTransform(-x / 2, -y / 2);
            
            if (yy > 0)
            {
                int lastII = -1;
                int lastYY = 0;
                int lastDYY = 0;
                for (int ii = 50; ii < buforPanel.Width; ii++)
                {
                    Color cc = buforPanel.GetPixel(ii, yy);
                    if (ii == 50)
                    {
                        lastII = 50;
                        lastYY = (int)(cc.GetBrightness() * 50);
                    }
                    ggBufor.DrawLine(Pens.Red, ii, (int)(cc.GetBrightness() * 50), lastII, lastYY);
                    ggBufor.DrawLine(Pens.Navy, ii, 70 + (int)((cc.GetBrightness() * 50) - lastYY), lastII, lastDYY);

                    lastII = ii;
                    lastYY = (int)(cc.GetBrightness() * 50);
                    lastDYY = 70 + (int)((cc.GetBrightness() * 50) - lastYY);
                }


            }

            if (xx > 0)
            {
                int lastII = -1;
                int lastXX = 0;
                for (int ii = 51; ii < buforPanel.Height; ii++)
                {
                    Color cc = buforPanel.GetPixel(xx, ii);
                    if (ii == 51)
                    {
                        lastII = 51;
                        lastXX = (int)(cc.GetBrightness() * 50);
                    }
                    ggBufor.DrawLine(Pens.Red, (int)(cc.GetBrightness() * 50), ii, lastXX, lastII);

                    lastII = ii;
                    lastXX = (int)(cc.GetBrightness() * 50);
                }


            }
            

            ggBufor.DrawLine(Pens.Gray, 0, yy, buforPanel.Width, yy);
            ggBufor.DrawLine(Pens.Gray, xx, 0, xx, buforPanel.Height);

            if (MouseDown)
            {
                width = Math.Max(StartROI.X, xx) - Math.Min(StartROI.X, xx);
                //int heigth = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
                rect = new Rectangle(Math.Min(StartROI.X, xx), Math.Min(StartROI.Y, xx), width, width);
                //rect = new Rectangle(StartROI.X, StartROI.Y, EndROI.X, EndROI.Y);
                ggBufor.DrawRectangle(Pens.Yellow, rect);
                gg.DrawImage(buforPanel, 0, 0);
            }
            else
            {
                if(selected)
                {
                    ggBufor.DrawRectangle(Pens.Yellow, rect);
                    gg.DrawImage(buforPanel, 0, 0);
                }
                gg.DrawImage(buforPanel, 0, 0);
            }

            //gg.DrawImage(buforPanel, 0, 0);

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            StartROI = e.Location;
            selected = false;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
            EndROI = e.Location;
            selected = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            xx = e.X;
            yy = e.Y;
            rysujBitmape();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(rect.IsEmpty)
            {
                rect = new Rectangle(0, 0, x, y);
            }
            System.Drawing.Imaging.PixelFormat format = buforPanel.PixelFormat;
            Form1.form1Instance.cropedImage = new Bitmap(buforPanel.Clone(rect, format), new Size(400, 400));
            Form1.form1Instance.picBox.Image = new Bitmap(buforPanel.Clone(rect, format), new Size(400, 400));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            rysujBitmape();
        }

    }
}
