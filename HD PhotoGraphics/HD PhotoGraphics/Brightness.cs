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
    public partial class Brightness : Form
    {
        public Brightness()
        {
            InitializeComponent();
        }

        Bitmap localimage;
        my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
        my_color[,] mygray;
        Bitmap transferedimage;
        //my_color[,] resized_image;// = new my_color[n_hieght, n_width];
        int width;// = width;
        int hieght;// = height;
        double old_max_red = 0, old_min_red = 255, old_max_green = 0, old_min_green = 255, old_max_blue = 0, old_min_blue = 255,
    new_max_red, new_min_red, new_max_green, new_min_green, new_max_blue, new_min_blue;
        int Height;
        // = new Bitmap(n_width, n_hieght);
        public void setdata(Bitmap imagetransfer)
        {
            int x;
            int y;
            pictureBox1.Image = imagetransfer;
            localimage = new Bitmap(imagetransfer);
            Buffer2D = new my_color[localimage.Height, localimage.Width];
            mygray = new my_color[localimage.Height, localimage.Width];
            width = localimage.Width;
            Height = localimage.Height;
            BitmapData bitmapData2 = localimage.LockBits(new Rectangle(0, 0, localimage.Width, localimage.Height),
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
                        if (old_max_red < r)
                        {
                            old_max_red = r;
                        }
                        if (old_min_red > r)
                        {
                            old_min_red = r;
                        }
                        if (old_max_blue < b)
                        {
                            old_max_blue = b;
                        }
                        if (old_min_blue > b)
                        {
                            old_min_blue = b;
                        }
                        if (old_max_green < g)
                        {
                            old_max_green = g;
                        }
                        if (old_min_green > g)
                        {
                            old_min_green = g;
                        }
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                }//end for i
            }//end unsafe
            localimage.UnlockBits(bitmapData2);
            //localimage = new Bitmap(
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int i, j;
            double bright = Double.Parse(textBox1.Text);
            Bitmap image1 = new Bitmap(localimage.Width, localimage.Height);
            BitmapData bitmapData1 = image1.LockBits(new Rectangle(0, 0, localimage.Width, localimage.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
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
                        mygray[i, j].Red = Buffer2D[i, j].Red + (int)bright;
                        mygray[i, j].Blue = Buffer2D[i, j].Blue + (int)bright;
                        mygray[i, j].Green = Buffer2D[i, j].Green + (int)bright;
                        if (mygray[i, j].Red > 255)
                        {
                            mygray[i, j].Red = 255;
                        }
                        if (mygray[i, j].Blue > 255)
                        {
                            mygray[i, j].Blue = 255;
                        }
                        if (mygray[i, j].Green > 255)
                        {
                            mygray[i, j].Green = 255;
                        }
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j

                    //4 bytes per pixel
                    imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }//end for i
                //image1.UnlockBits(bitmapData1);
            }//end unsafe
            image1.UnlockBits(bitmapData1);
            BitmapData bitmapData2 = image1.LockBits(new Rectangle(0, 0, localimage.Width, localimage.Height),
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
            transferedimage = new Bitmap(image1);
            pictureBox2.Image = image1;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata((Bitmap)pictureBox2.Image);
            fm1.Show();
            this.Hide();
        }
    }
}
