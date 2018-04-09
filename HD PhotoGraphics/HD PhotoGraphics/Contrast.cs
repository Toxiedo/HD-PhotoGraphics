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
    public partial class Contrast : Form
    {
        public Contrast()
        {
            InitializeComponent();
        }

        Bitmap localimage;
        my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
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
            new_max_red = double.Parse(textBox1.Text);
            new_max_green = double.Parse(textBox1.Text);
            new_max_blue = double.Parse(textBox1.Text);
            new_min_red = double.Parse(textBox2.Text);
            new_min_green = double.Parse(textBox2.Text);
            new_min_blue = double.Parse(textBox2.Text);
            Bitmap c_contrast = new Bitmap(localimage);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color clr = new Color();
                    clr = c_contrast.GetPixel(j, i);
                    int new_val_red = (int)((((clr.R - old_min_red) / (double)(old_max_red - old_min_red)) * (double)(new_max_red - new_min_red)) + new_min_red);

                    int new_val_blue = (int)((((clr.B - old_min_blue) / (double)(old_max_blue - old_min_blue)) * (double)(new_max_blue - new_min_blue)) + new_min_blue);

                    int new_val_green = (int)((((clr.G - old_min_green) / (double)(old_max_green - old_min_green)) * (double)(new_max_green - new_min_green)) + new_min_green);

                    if (new_val_red > 255)
                        new_val_red = 255;
                    if (new_val_red < 0)
                        new_val_red = 0;
                    if (new_val_green > 255)
                        new_val_green = 255;
                    if (new_val_blue > 255)
                        new_val_blue = 255;
                    if (new_val_green < 0)
                        new_val_green = 0;
                    if (new_val_blue < 0)
                        new_val_blue = 0;


                    //arr_clr_red[(int)new_val_red] += 1;
                    //arr_clr_green[(int)new_val_green] += 1;
                    //arr_clr_blue[(int)new_val_blue] += 1;


                    Color clr2;
                    clr2 = Color.FromArgb(new_val_red, new_val_green, new_val_blue);
                    c_contrast.SetPixel(j, i, clr2);
                }
            }

            pictureBox2.Image = c_contrast;
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
