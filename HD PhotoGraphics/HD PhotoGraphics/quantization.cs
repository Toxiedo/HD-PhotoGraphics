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
    public partial class quantization : Form
    {
        public quantization()
        {
            InitializeComponent();
        }

        Bitmap image;
        my_color[,] Buffer2D;
        my_color[,] Buffer;
        my_color[,] mygray;
        Bitmap localimage;
        //my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
        Bitmap transferedimage;
        //my_color[,] resized_image;// = new my_color[n_hieght, n_width];
        int width;// = width;
        int hieght;// = height;

        int Height;

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

        public int reduce(int number)
        {
            int temp = 0;
            switch (number)
            {
                case 1:
                    temp = 254;
                    break;
                case 2:
                    temp = 252;
                    break;
                case 3:
                    temp = 248;
                    break;
                case 4:
                    temp = 240;
                    break;
                case 5:
                    temp = 224;
                    break;
                case 6:
                    temp = 192;
                    break;
                case 7:
                    temp = 128;
                    break;
            }
            return temp;
        }
        //Quantization function

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();

            dt1 = DateTime.Now;
            int reduce_num = int.Parse(textBox1.Text);
            int mask = reduce(reduce_num);
            int x, y;
            Buffer = new my_color[localimage.Height, localimage.Width];
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
                        Buffer[x, y].Blue = ((int)b) & mask;
                        Buffer[x, y].Green = ((int)g) & mask;
                        Buffer[x, y].Red = ((int)r) & mask;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                }//end for i
            }//end unsafe
            localimage.UnlockBits(bitmapData2);
            int i, j;
            Bitmap image1 = new Bitmap(Buffer.GetLength(1), Buffer.GetLength(0));
            BitmapData bitmapData1 = image1.LockBits(new Rectangle(0, 0, Buffer.GetLength(1), Buffer.GetLength(0)),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        // write the logic implementation here

                        imagePointer1[0] = (byte)(Buffer[i, j].Blue);
                        imagePointer1[1] = (byte)(Buffer[i, j].Green);
                        imagePointer1[2] = (byte)(Buffer[i, j].Red);
                        imagePointer1[3] = (byte)255;
                        
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j

                    //4 bytes per pixel
                    imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }//end for i
                //image1.UnlockBits(bitmapData1);
            }//end unsafe
            image1.UnlockBits(bitmapData1);
            transferedimage = image1;
            pictureBox2.Image = image1;
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            //image = image1;
            MessageBox.Show(dt3.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata(transferedimage);
            fm1.Show();
            this.Hide();
        }
    }
}
