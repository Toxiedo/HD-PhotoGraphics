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
    public partial class Addoperation : Form
    {
        public Addoperation()
        {
            InitializeComponent();
        }


        Bitmap choosedimage;
        my_color[,] Buffer2D;
        my_color[,] Buffer;
        my_color[,] mygray;
        Bitmap localimage;
        Bitmap b1;
        //my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
        Bitmap transferedimage;
        //my_color[,] resized_image;// = new my_color[n_hieght, n_width];
        int width;// = width;
        //int hieght;// = height;

        int Height;

        //*****************************************************

        int[] local_his_red = new int[256];
        int[] local_his_blue = new int[256];
        int[] local_his_green = new int[256];
        int[] choosed_his_red = new int[256];
        int[] choosed_his_blue = new int[256];
        int[] choosed_his_green = new int[256];
        int[] matched_his_green = new int[256];
        int[] matched_his_blue = new int[256];
        int[] matched_his_red = new int[256];

        double old_max_red = 0, old_min_red = 255, old_max_green = 0, old_min_green = 255, old_max_blue = 0, old_min_blue = 255,
    new_max_red, new_min_red, new_max_green, new_min_green, new_max_blue, new_min_blue;
        //int Height;
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
                    pictureBox2.Image = Bitmap.FromFile(ofd.FileName);
                    choosedimage = new Bitmap(ofd.FileName);
                    int hei = choosedimage.Height;
                    int wie = choosedimage.Width;
                    Buffer = new my_color[hei, wie];
                    mygray = new my_color[hei, wie];
                    int x, y;
                    BitmapData bitmapData2 = choosedimage.LockBits(new Rectangle(0, 0, choosedimage.Width, choosedimage.Height),
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
                                Buffer[x, y].Blue = (int)b;
                                Buffer[x, y].Green = (int)g;
                                Buffer[x, y].Red = (int)r;
                                choosed_his_red[(int)r]++;
                                choosed_his_green[(int)g]++;
                                choosed_his_blue[(int)b]++;
                                //4 bytes per pixel
                                imagePointer1 += 4;
                            }//end for j
                            //4 bytes per pixel
                            imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                        }//end for i
                    }//end unsafe
                    choosedimage.UnlockBits(bitmapData2);
                }
                catch (Exception)
                {
                    MessageBox.Show("errooooooor");
                }
            }

            int n_width = width;
            int n_hieght = Height;

            float w_ratio = (float)width / n_width;
            float h_ratio = (float)Height / n_hieght;

            int X1, X2, Y1, Y2;
            my_color P1, P2, P3, P4;
            float OldX, OldY, NewX, NewY;

            float XFraction, YFraction;
            float Z1, Z2;

            b1 = new Bitmap(n_width, n_hieght);
            my_color newpixel = new my_color();



            for (int i = 0; i < n_hieght; i++)
            {
                NewY = i;
                for (int j = 0; j < n_width; j++)
                {

                    NewX = j;
                    OldX = NewX * w_ratio;
                    OldY = NewY * h_ratio;

                    X1 = (int)Math.Floor(OldX); X2 = X1 + 1;
                    Y1 = (int)Math.Floor(OldY); Y2 = Y1 + 1;

                    if (X2 == width) X2 -= 1;
                    if (Y2 == Height) Y2 -= 1;
                    if (X1 == width) X1 -= 1;
                    if (Y1 == Height) Y1 -= 1;

                    P1 = Buffer[Y1, X1]; P2 = Buffer[Y1, X2];
                    P3 = Buffer[Y2, X1]; P4 = Buffer[Y2, X2];

                    XFraction = OldX - X1;
                    YFraction = OldY - Y1;

                    Z1 = (float)(P1.Red * (1 - XFraction) + P2.Red * XFraction);
                    Z2 = (float)(P3.Red * (1 - XFraction) + P4.Red * XFraction);
                    float Z12 = (Z1 * (1 - YFraction) + Z2 * YFraction);
                    int temp1 = (int)(Z12 * 10) % 10;
                    if (temp1 > 5)
                        newpixel.Red = (int)Math.Ceiling(Z12);
                    else
                        newpixel.Red = (int)Math.Floor(Z12);

                    //newpixel.Red = (int)(Z1 * (1 - YFraction) + Z2 * YFraction);

                    Z1 = (float)(P1.Green * (1 - XFraction) + P2.Green * XFraction);
                    Z2 = (float)(P3.Green * (1 - XFraction) + P4.Green * XFraction);
                    float Z11 = (Z1 * (1 - YFraction) + Z2 * YFraction);
                    int temp2 = (int)(Z11 * 10) % 10;
                    if (temp2 > 5)
                        newpixel.Green = (int)Math.Ceiling(Z11);
                    else
                        newpixel.Green = (int)Math.Floor(Z11);


                    Z1 = (float)(P1.Blue * (1 - XFraction) + P2.Blue * XFraction);
                    Z2 = (float)(P3.Blue * (1 - XFraction) + P4.Blue * XFraction);
                    float Z13 = (Z1 * (1 - YFraction) + Z2 * YFraction);
                    int temp3 = (int)(Z13 * 10) % 10;
                    if (temp3 > 5)
                        newpixel.Blue = (int)Math.Ceiling(Z13);
                    else
                        newpixel.Blue = (int)Math.Floor(Z13);

                    //newpixel.Blue = (int)(Z1 * (1 - YFraction) + Z2 * YFraction);


                    Color clr;
                    clr = Color.FromArgb(255, newpixel.Red, newpixel.Green, newpixel.Blue);
                    b1.SetPixel(j, i, clr);

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            transferedimage = new Bitmap(width,Height);
            my_color newpixel = new my_color();
            for (int i = 0; i < Height;i++ )
            {
                for (int j = 0; j < width; j++)
                {
                    Color clr1;
                    Color clr2;
                    clr1 = localimage.GetPixel(j,i);
                    clr2 = b1.GetPixel(j,i);
                    newpixel.Red = clr1.R + clr2.R;
                    newpixel.Blue = clr1.B + clr2.B;
                    newpixel.Green = clr1.G + clr2.G;
                    if (newpixel.Red > 255)
                    {
                        newpixel.Red = 255;
                    }
                    if (newpixel.Blue > 255)
                    {
                        newpixel.Blue = 255;
                    }
                    if (newpixel.Green > 255)
                    {
                        newpixel.Green = 255;
                    }
                    Color clr3;
                    clr3 = Color.FromArgb(newpixel.Red, newpixel.Green, newpixel.Blue);
                    transferedimage.SetPixel(j, i, clr3);
                }
            }
            pictureBox3.Image = transferedimage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata((Bitmap)pictureBox3.Image);
            fm1.Show();
            this.Hide();
        }
    }
}
