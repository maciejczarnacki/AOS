using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace Telescope_Tester

{
    public partial class Form1 : Form
    {

        public static Form1 form1Instance;
        public PictureBox picBox;
        public Bitmap cropedImage = new Bitmap(400, 400);
        Bitmap mainImage = new Bitmap(400, 400);     
        Bitmap mainRonchigram = new Bitmap(400, 400);

        Bitmap obraz = new Bitmap(400,400);
        Bitmap buforPanel;
        

        public Form1()
        {
            InitializeComponent();
            form1Instance = this;
            picBox = pictureBox2;
            cropedImage = mainImage;
            buforPanel = new Bitmap(400, 400);
            
        }

        double Diameter = 0;
        double focalLenght = 0;
        double gratingLines = 0;
        double distFromROC = 0;
        double conicConstant = -1;
        double ronchiShift = 0;
        double knifePos = 0;
        double lightSource = 0.15;
        double waveLenght = 550;

        Boolean loadedImageStatus = false;
        bool blink = false;


        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private static double[] DataChecker(string diameter, string focalLenght, string gratingLines, string distFromROC, string b, string ronchiShift, string knifePos, string lightSource, string waveLength)
        {
            string[] InputData = new string[] { diameter,focalLenght,gratingLines,distFromROC,b,ronchiShift,knifePos,lightSource,waveLength };
            double[] outputData = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            for (int i=0; i<=8; i++)
            {
                try
                { 
                    outputData[i] = double.Parse(InputData[i], CultureInfo.InvariantCulture.NumberFormat);
                }
                catch
                {
                    MessageBox.Show("Invalid data format " + "'" + InputData[i] + "'");
                }
            }
            return outputData;
        }


        private static Bitmap RonchigramGenerator(double diameter, double focalLenght, double gratingLines, double distFromROC, double b, double ronchiShift, double knifePos, double lightSource, double waveLength, int Status)
        {
            //deklaracja bitmapy Ronchigramu
            Bitmap ronchigram = new Bitmap(400, 400);
            Bitmap ronchigram_ = new Bitmap(400, 400);
            
            int X0 = 200;
            int Y0 = 200;
            double X, Y, S2;
            int C = 400;
            double Z, L, U;
            //double w = 1/(2*gratingLines); //szerokoœæ linii siatki Ronchi
            double ROC = 2 * focalLenght;
            
            for(int i=0; i<400; i++)
            {
                for(int k=0; k<400; k++)
                {
                    X = (i - X0) * diameter / C;
                    Y = (k - Y0) * diameter / C;
                    S2 = X * X + Y * Y;
                    if(Math.Sqrt(S2)<diameter/2)
                    {
                        if (Status == 0)
                        {
                            Z = 2 * focalLenght + S2 / (2 * focalLenght) * (-1) * b;
                            L = 2 * focalLenght + 2 * distFromROC - Z;
                            U = L * X / Z;
                        }
                        else if(Status == 1)
                        {
                            double SAG = X*X / (ROC * (1 + Math.Sqrt(1 - S2 / Math.Pow(ROC, 2) * (b + 1))));
                            double f = FociiAndRadii(Math.Sqrt(S2), ROC, b);
                            U = X * (distFromROC + (ROC/2 - f)) / (f - SAG);
                        }
                        else if(Status == 2)
                        {
                            double SAG = X * X / (ROC * (1 + Math.Sqrt(1 - S2 / Math.Pow(ROC, 2) * (2*b + 2))));
                            double f = FociiAndRadii(Math.Sqrt(S2), ROC, 2*b+1);
                            U = X * (distFromROC + (ROC / 2 - f)) / (f - SAG);
                        }
                        else if(Status == 3)
                        {
                            Z = 2 * focalLenght + S2 / (2 * focalLenght) * (-1) * b;
                            L = 2 * focalLenght + 2 * distFromROC - Z;
                            U = L * X / Z;
                            if (Math.Sqrt(S2) > diameter)
                            {
                                U = 100 * knifePos;
                            }

                        }
                        else
                        {
                            double SAG = X * X / (ROC * (1 + Math.Sqrt(1 - S2 / Math.Pow(ROC, 2) * (2 * b + 2))));
                            double f = FociiAndRadii(Math.Sqrt(S2), ROC, 2 * b + 1);
                            U = X * (distFromROC + (ROC / 2 - f)) / (f - SAG);
                        }
                        int FL = 0;
                        if (Status == 3 || Status == 4)
                        {
                            if (U >= knifePos)
                            {
                                FL = 1;
                            }
                        }
                        else
                        {
                            double si = Math.Pow(Math.Sin(Math.PI * U * gratingLines - ronchiShift), 2);
                            if (si <= 0.5)
                            {
                                FL = 1;
                            }
                            else
                            {
                                FL = 0;
                            }
                        }
                        if(FL!=0)
                        {
                            if (Status == 3 || Status == 4)
                            {
                                int A = 0;
                                if (Status == 3)
                                {
                                    A = 6000;
                                }
                                else if(Status == 4)
                                {
                                    A = 6000;
                                }
                                double intensity = (A * Math.Abs(U - knifePos))+127;
                                if (intensity < 0)
                                {
                                    intensity = 0;
                                }
                                if (intensity > 255)
                                {
                                    intensity = 255;
                                }
                                ronchigram_.SetPixel(i, k, Color.FromArgb((int)Math.Round(intensity),(int)Math.Round(intensity), (int)Math.Round(intensity)));
                                //ronchigram_.SetPixel(i, k, Color.White);
                            }
                            else
                            {
                                ronchigram_.SetPixel(i, k, Color.White);
                            }
                        }
                        else
                        {
                            if (Status == 3 || Status == 4)
                            {
                                int A = 0;
                                if (Status == 3)
                                {
                                    A = 6000;
                                }
                                else if (Status == 4)
                                {
                                    A = 6000;
                                }
                                double intensity = 127 - (double)(A * Math.Abs(U -knifePos));
                                if(intensity < 0)
                                {
                                    intensity = 0;
                                }
                                if(intensity > 255)
                                {
                                    intensity = 255;
                                }
                                ronchigram_.SetPixel(i, k, Color.FromArgb((int)Math.Round(intensity), (int)Math.Round(intensity), (int)Math.Round(intensity)));
                                //ronchigram_.SetPixel(i, k, Color.White);
                            }
                            else
                            {
                                //ronchigram.SetPixel(i, k, Color.White);
                            }
                        }
                    }
                }
            }
            return ronchigram_;
        }

        //funkcje do obliczen

        private static double Sagitta(double r, double Radii, double conicConstant)
        {
            double Sa = Math.Pow(r, 2) / (Radii * (1 + Math.Sqrt(1 - Math.Pow(r, 2) / Math.Pow(Radii, 2) * (conicConstant + 1))));
            return Sa;
        }

        private static double SagittaAutoCollimation(double r, double Radii, double conicConstant)
        {
            double S = Math.Pow(r, 2) / (Radii * (1 + Math.Sqrt(1 - Math.Pow(r, 2) / Math.Pow(Radii, 2) * (2*conicConstant + 2))));
            return S;
        }

        private static double FociiAndRadii(double r, double Radii, double conicConstant)
        {
            double epsilon = 0.001;
            double aX, a_pX, angle_X, angle_fX, a_fX, b_fX;
            double S = Sagitta(r, Radii, conicConstant);
            double Sp = Sagitta(r+epsilon, Radii, conicConstant);
            aX = (S - Sp) / epsilon;
            a_pX = 1 / aX;
            angle_X = Math.Atan(a_pX) - 2*Math.PI*90/360;
            angle_fX = Math.Atan(a_pX) + angle_X;
            a_fX = Math.Tan(angle_fX);
            b_fX = (S + Sp) / 2 - a_fX * (r + 0.5 * epsilon);
            return b_fX;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            double[] newData = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            newData = DataChecker(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);
            Diameter = newData[0];
            focalLenght = newData[1];
            gratingLines = newData[2];
            distFromROC = newData[3];
            conicConstant = newData[4];
            ronchiShift = newData[5];
            knifePos = newData[6];
            lightSource = newData[7];
            waveLenght = newData[8];
            int Status;
            if(checkBox1.Checked==true)
            {
                Status = 0;
            }
            else if(checkBox2.Checked==true)
            { 
                Status = 1;
            }
            else if(checkBox3.Checked==true)
            {
                Status = 2;
            }
            else if(checkBox4.Checked==true)
            {
                Status = 3;
            }
            else
            {
                Status = 4;
            }
            Bitmap ronchi = RonchigramGenerator(Diameter, focalLenght, gratingLines, distFromROC, conicConstant, ronchiShift, knifePos, lightSource, waveLenght, Status);
            pictureBox1.Image = ronchi;
            mainRonchigram = ronchi;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox8.Checked = false;
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            checkBox1.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox8.Checked = false;
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox8.Checked = false;
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = true;
            checkBox8.Checked = false;
        }

        private void checkBox8_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox8.Checked = true;
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }


        private void loadRonchigramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap loadedImage = new Bitmap(open.FileName);
                int szerokosc = loadedImage.Width;
                int wysokosc = loadedImage.Height;
                Form2 form2 = new Form2(loadedImage, szerokosc, wysokosc);
                form2.Show();
                loadedImageStatus = true;
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            blink = true;
            RonchigramShow();
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            blink = false;
            RonchigramShow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double p_v = Math.Abs(1 + conicConstant) * Math.Pow(Diameter, 4) / 256 / Math.Pow(2*focalLenght, 3) / (waveLenght * Math.Pow(10, (-6)));
            double RMS = p_v / 3.51;
            double Strehl = Math.Pow(1 - 2 * Math.PI * Math.PI * RMS * RMS, 2);
            textBox10.Text = RMS.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat);
            textBox11.Text = p_v.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat);
            textBox12.Text = Strehl.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat);
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            RonchigramShow();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 100;
            trackBar4.Value = 0;
            pictureBox2.Image = cropedImage;
            checkBox6.Checked = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String aboutThisProgramm = "Astromaniac Optical Simulator v0.3\nAuthor - Maciej Czarnacki\nContact - maciej.czarnacki@gmail.com";
            MessageBox.Show(aboutThisProgramm, "About");
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            if(checkBox7.Checked != true)
            {
                double a = 0;
                textBox6.Text = a.ToString("0", CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                double a = 1.5707;
                textBox6.Text = a.ToString("0.0000", CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            /* if (trackBar4.Value == 0)
             { 
                 pictureBox2.Image = cropedImage.ToBitmap();
             }
             else if (trackBar4.Value == 400)
             {
                 pictureBox2.Image = mainRonchigram.ToBitmap();
             }
             else
             {
                 if (trackBar4.Value > 0 && trackBar4.Value != 400)
                 {
                     Rectangle roi_1 = new Rectangle(0, 0, 400, trackBar4.Value);
                     Rectangle roi_2 = new Rectangle(0, trackBar4.Value, 400, 400);

                     simulation = mainRonchigram.Copy();
                     ronchigram = cropedImage.Copy();

                     simulation.ROI = roi_1;
                     ronchigram.ROI = roi_2;
                     var simulationROI = simulation.Copy();
                     var ronchigramROI = ronchigram.Copy();

                     //Image<Bgr, Byte> result2 = new Image<Bgr, Byte>(400, 400);

                     CvInvoke.VConcat(simulationROI, ronchigramROI, result2);
                     pictureBox2.Image = result2.ToBitmap();
                     simulationROI.Dispose();
                     ronchigramROI.Dispose();
                     simulation.Dispose();
                     ronchigram.Dispose();
                 }
             }*/



        }

        private void RonchigramShow()
        {
            Graphics ggBufor = Graphics.FromImage(buforPanel);
            Graphics gg = pictureBox2.CreateGraphics();

            ggBufor.FillRectangle(Brushes.Black, 0, 0, 400, 400);
            ggBufor.DrawImage(mainRonchigram, 0, 0);

            ColorMatrix matrix = new ColorMatrix();
            if (blink == true)
            {
                matrix.Matrix33 = 0;
            }
            else
            {

                matrix.Matrix33 = (float)(1.0 * trackBar3.Value / trackBar3.Maximum);
            }
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            
            ggBufor.DrawImage(cropedImage, new Rectangle(0, 0, 400, 400), 0, 0, 400, 400, GraphicsUnit.Pixel, attributes);
            gg.DrawImage(buforPanel, 0, 0);



        }


    }
}