using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
namespace HD_PhotoGraphics
{
    public partial class Gamma : Form
    {
        public Gamma()
        {
            InitializeComponent();
        }

        Bitmap image;
        my_color[,] Buffer2D;
        my_color[,] mygray;

        double new_min_red1;
        double new_min_blue1;
        double new_min_green1;
        double new_max_red1;
        double new_max_blue1;
        double new_max_green1;

        int new_max_red = 0, new_min_red = 255, new_max_green = 0, new_min_green = 255, new_max_blue = 0, new_min_blue = 255;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap files (*.bmp)|*.bmp|PNG files (*.png)|*.png|TIFF files (*.tif)|*tif|JPEG files (*.jpg)|*.jpg |All files (*.*)|*.*";
            ofd.FilterIndex = 5;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    pictureBox1.Image = Bitmap.FromFile(ofd.FileName);
                    image = new Bitmap(ofd.FileName);
                    int hei = image.Height;
                    int wie = image.Width;
                    Buffer2D = new my_color[hei, wie];
                    mygray   = new my_color[hei, wie];
                    int x, y;
                    BitmapData bitmapData2 = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
             ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        byte* imagePointer1 = (byte*)bitmapData2.Scan0;

                        for (x = 0; x < bitmapData2.Height; x++)
                        {
                            for (y = 0; y < bitmapData2.Width; y++)
                            {
                                double b = (int)imagePointer1[0];
                                double g = (int)imagePointer1[1];
                                double r = (int)imagePointer1[2];
                                Buffer2D[x, y].Blue = (int)b;
                                Buffer2D[x, y].Green = (int)g;
                                Buffer2D[x, y].Red = (int)r;
                                if (new_max_red < r)
                                {
                                    new_max_red = (int)r;
                                }
                                else if (new_min_red > r)
                                {
                                    new_min_red = (int)r;
                                }
                                else if (new_max_blue < b)
                                {
                                    new_max_blue = (int)b;
                                }
                                else if (new_min_blue > b)
                                {
                                    new_min_blue = (int)b;
                                }
                                else if (new_max_green < g)
                                {
                                    new_max_green = (int)g;
                                }
                                else if (new_min_green > g)
                                {
                                    new_min_green = (int)g;
                                }
                                //4 bytes per pixel
                                imagePointer1 += 4;
                            }//end for j
                            //4 bytes per pixel
                            imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                        }//end for i
                    }//end unsafe
                    image.UnlockBits(bitmapData2);
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();

            dt1 = DateTime.Now;
            int i, j;
            double num = Double.Parse(textBox1.Text);
            Bitmap image1 = new Bitmap(image.Width, image.Height);
            BitmapData bitmapData1 = image1.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            new_min_red1 = Math.Pow(new_min_red, num);
            new_min_blue1 = Math.Pow(new_min_blue, num);
            new_min_green1 = Math.Pow(new_min_green, num);
            new_max_red1 = Math.Pow(new_max_red, num);
            new_max_blue1 = Math.Pow(new_max_blue, num);
            new_max_green1 = Math.Pow(new_max_green, num);
            //Bitmap gam = new Bitmap(wie, hei);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        // write the logic implementation here

                        imagePointer1[0] = (byte)(Buffer2D[i, j].Blue);
                        imagePointer1[1] = (byte)(Buffer2D[i, j].Green);
                        imagePointer1[2] = (byte)(Buffer2D[i, j].Red);
                        imagePointer1[3] = (byte)255;
                        double o = Math.Pow(Buffer2D[i, j].Blue, num);
                        double p = Math.Pow(Buffer2D[i, j].Green, num);
                        double q = Math.Pow(Buffer2D[i, j].Red, num);

                        double valred = ((o - new_min_red1) / (new_max_red1 - new_min_red1)) * 255;
                        double valblue = ((q - new_min_blue1) / (new_max_blue1 - new_min_blue1)) * 255;
                        double valgreen = ((p - new_min_green1) / (new_max_green1 - new_min_green1)) * 255;



                        if (valred > 255)
                        {
                            valred = 255;
                        }
                        else if (valred < 0)
                        {
                            valred = 0;
                        }
                        if (valblue > 255)
                        {
                            valblue = 255;
                        }
                        else if (valblue < 0)
                        {
                            valblue = 0;
                        }
                        if (valgreen > 255)
                        {
                            valgreen = 255;
                        }
                        else if (valgreen < 0)
                        {
                            valgreen = 0;
                        }
                        mygray[i, j].Red = (int)valred;
                        mygray[i, j].Blue = (int)valblue;
                        mygray[i, j].Green = (int)valgreen;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j

                    //4 bytes per pixel
                    imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }//end for i
                //image1.UnlockBits(bitmapData1);
            }//end unsafe
            image1.UnlockBits(bitmapData1);
            BitmapData bitmapData2 = image1.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData2.Height; i++)
                {
                    for (j = 0; j < bitmapData2.Width; j++)
                    {
                        // write the logic implementation here

                        imagePointer1[0] = (byte)(mygray[i, j].Blue);
                        imagePointer1[1] = (byte)(mygray[i, j].Green);
                        imagePointer1[2] = (byte)(mygray[i, j].Red);
                        imagePointer1[3] = (byte)255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j

                    //4 bytes per pixel
                    imagePointer1 += (bitmapData2.Stride - (bitmapData2.Width * 4));
                }//end for i
                //image1.UnlockBits(bitmapData1);
            }//end unsafe
            image1.UnlockBits(bitmapData2);
            pictureBox2.Image = image1;
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            image = image1;
            MessageBox.Show(dt3.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata(image);
            fm1.Show();
            this.Hide();
        }
    }
}
