using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Telescope_Tester
{
    public partial class Form2 : Form
    {

        static int x;
        static int y;
        static int width, heigth;

        bool MouseDown = false;
        bool selected = false;
        bool rotated = false;

        Rectangle rect;
        Point StartROI, EndROI;

        Image<Bgr, Byte> sImage = new Image<Bgr, Byte>(x, y);
        Image<Bgr, Byte> lImage = new Image<Bgr, Byte>(x, y);
        Image<Bgr, Byte> rotatedImage = new Image<Bgr, byte>(x, y);


        public Form2(Image<Bgr, Byte> showImage, int w, int h)
        {
            InitializeComponent();
            numericUpDown1.DecimalPlaces = 1;
            numericUpDown1.Increment = 0.1M;
            rect = Rectangle.Empty;
            Image<Bgr, Byte> scaledImage = showImage.Resize(800, (int)(float)(h * 800 / w), Emgu.CV.CvEnum.Inter.Linear);

            lImage = scaledImage;
            pictureBox1.Image = scaledImage.ToBitmap();
            x = 800;
            y = (int)(h * 800 / w);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(x, y);
            Image<Gray, Byte> resultGray = new Image<Gray, Byte>(x, y);
            if (rotated)
            {
                result = rotatedImage.Copy();
                result.ROI = rect;
                Form1.form1Instance.cropedImage = result.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).Copy();
                resultGray = result.Copy().Convert<Gray, Byte>();
                Form1.form1Instance.cropedImageGray = resultGray.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).Copy();
                Form1.form1Instance.picBox.Image = result.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).ToBitmap();
                result.Dispose();
                resultGray.Dispose();
            }
            else
            {
                result = lImage.Copy();
                result.ROI = rect;
                Form1.form1Instance.cropedImage = result.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).Copy();
                resultGray = result.Copy().Convert<Gray, Byte>();
                Form1.form1Instance.cropedImageGray = resultGray.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).Copy();
                Form1.form1Instance.picBox.Image = result.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear).ToBitmap();
                result.Dispose();
                resultGray.Dispose();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            sImage = lImage.Copy();
            sImage = sImage.Rotate((double)numericUpDown1.Value, new Bgr(0, 0, 0));
            pictureBox1.Image = sImage.ToBitmap();
            rotatedImage = sImage.Copy();
            rotated = true;
            rect = Rectangle.Empty;
            sImage.Dispose();
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
            if (MouseDown)
            {
                width = Math.Max(StartROI.X, e.X) - Math.Min(StartROI.X, e.X);
                //int heigth = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
                rect = new Rectangle(Math.Min(StartROI.X, e.X), Math.Min(StartROI.Y, e.X), width, width);
                if (rotated == false)
                {
                    sImage = lImage.Clone();
                }
                else
                {
                    sImage = rotatedImage.Clone();
                }
                CvInvoke.Line(sImage, new Point(0, e.Y), new Point(800, e.Y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                CvInvoke.Line(sImage, new Point(e.X, 0), new Point(e.X, y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                CvInvoke.Rectangle(sImage, rect, new MCvScalar(0, 234, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                pictureBox1.Image = sImage.ToBitmap();
                sImage.Dispose();
            }
            else
            {
                if (rect.IsEmpty)
                {
                    if (rotated == false)
                    {
                        sImage = lImage.Clone();
                    }
                    else
                    {
                        sImage = rotatedImage.Clone();
                    }
                    CvInvoke.Line(sImage, new Point(0, e.Y), new Point(800, e.Y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    CvInvoke.Line(sImage, new Point(e.X, 0), new Point(e.X, y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    pictureBox1.Image = sImage.ToBitmap();
                    sImage.Dispose();
                }
                else
                {
                    if (rotated == false)
                    {
                        sImage = lImage.Clone();
                    }
                    else
                    {
                        sImage = rotatedImage.Clone();
                    }
                    CvInvoke.Line(sImage, new Point(0, e.Y), new Point(800, e.Y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    CvInvoke.Line(sImage, new Point(e.X, 0), new Point(e.X, y), new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    CvInvoke.Rectangle(sImage, rect, new MCvScalar(0, 234, 255), 1, Emgu.CV.CvEnum.LineType.EightConnected);
                    pictureBox1.Image = sImage.ToBitmap();
                    sImage.Dispose();
                }

            }
        }

    }
}
