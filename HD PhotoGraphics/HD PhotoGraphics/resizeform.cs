using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
namespace HD_PhotoGraphics
{
    public partial class resizeform : Form
    {
        public resizeform()
        {
            InitializeComponent();
        }

        Bitmap localimage;
        my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
        Bitmap transferedimage;
        //my_color[,] resized_image;// = new my_color[n_hieght, n_width];
        int width;// = width;
        int hieght;// = height;

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

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();

            dt1 = DateTime.Now;
            int n_width = int.Parse(textBox2.Text);
            int n_hieght = int.Parse(textBox3.Text);
            
            my_color [,] resizeee = new my_color[n_hieght,n_width];
            float w_ratio = (float)width / n_width;
            float h_ratio = (float)Height / n_hieght;

            int X1, X2, Y1, Y2;
            my_color P1, P2, P3, P4;
            float OldX, OldY, NewX, NewY;

            float XFraction, YFraction;
            float Z1, Z2;

            Bitmap b1 = new Bitmap(n_width, n_hieght);
            my_color newpixel = new my_color();
            //int i, j;


            Parallel.For (0, n_hieght, i =>
            {
                NewY = i;
                Parallel.For(0, n_width, j =>
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

                    P1 = Buffer2D[Y1, X1]; P2 = Buffer2D[Y1, X2];
                    P3 = Buffer2D[Y2, X1]; P4 = Buffer2D[Y2, X2];

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

                    //Thread.CurrentThread.ManagedThreadId);
                    //Thread.Sleep(1000);
                    resizeee[i, j].Red = newpixel.Red;
                    resizeee[i, j].Blue = newpixel.Blue;
                    resizeee[i, j].Green = newpixel.Green;


                });
                //Thread.CurrentThread.ManagedThreadId);
            
            });

            int x, y;
            BitmapData bitmapData3 = b1.LockBits(new Rectangle(0, 0, b1.Width, b1.Height),
                         ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData3.Scan0;

                for (x = 0; x < bitmapData3.Height; x++)
                {
                    for (y = 0; y < bitmapData3.Width; y++)
                    {
                        imagePointer1[0] = (byte)resizeee[x,y].Blue;
                        imagePointer1[1] = (byte)resizeee[x,y].Green;
                        imagePointer1[2] = (byte)resizeee[x,y].Red;
                        imagePointer1[3] = (byte)255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData3.Stride - (bitmapData3.Width * 4);
                }//end for i
            }//end unsafe
            b1.UnlockBits(bitmapData3);
            transferedimage = new Bitmap(b1);
            pictureBox2.Image = b1;
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            //image = image1;
            MessageBox.Show(dt3.ToString());
        }
    }
}
