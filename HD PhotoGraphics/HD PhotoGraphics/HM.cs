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
    public partial class HM : Form
    {
        public HM()
        {
            InitializeComponent();
        }

        Bitmap choosedimage;
        my_color[,] Buffer2D;
        my_color[,] Buffer;
        my_color[,] mygray;
        my_color[,] bufferequa;
        my_color[,] bufferchoose;
        Bitmap localimage;
        //my_color[,] Buffer2D;// = new my_color[myimage.Height, myimage.Width];
        Bitmap transferedimage;
        //my_color[,] resized_image;// = new my_color[n_hieght, n_width];
        int width;// = width;
        int hieght;// = height;

        int Height;

        //*****************************************************


        public my_color[,] histo_equalization(my_color[,] given)
        {
            my_color[] given_img_histogram = new my_color[256];//1D arrays to calculate histogram
            my_color[] given_img_cumulative = new my_color[256];//1D arrays to calculate cumulative
            my_color[] change_with = new my_color[256];//1D arrays to displacment values
            my_color[,] resulted_img = new my_color[given.GetLength(0), given.GetLength(1)];

            //1- calculate image histogram
            //intiallize the histogram magnitude with 0
            for (int g = 0; g < 256; g++)
            {
                given_img_histogram[g].Red = 0;
                given_img_histogram[g].Green = 0;
                given_img_histogram[g].Blue = 0;
            }
            //calculate the magnitudes
            for (int j = 0; j < given.GetLength(0); j++)
            {
                for (int l = 0; l < given.GetLength(1); l++)
                {
                    given_img_histogram[given[j, l].Red].Red++;
                    given_img_histogram[given[j, l].Green].Green++;
                    given_img_histogram[given[j, l].Blue].Blue++;
                }
            }
            //2- calculate cumulative for it
            for (int i = 0; i < 256; i++)
            {
                if (i == 0)
                {
                    given_img_cumulative[i].Red = given_img_histogram[i].Red;
                    given_img_cumulative[i].Green = given_img_histogram[i].Green;
                    given_img_cumulative[i].Blue = given_img_histogram[i].Blue;
                }
                else
                {
                    given_img_cumulative[i].Red = given_img_histogram[i].Red + given_img_cumulative[i - 1].Red;
                    given_img_cumulative[i].Green = given_img_histogram[i].Green + given_img_cumulative[i - 1].Green;
                    given_img_cumulative[i].Blue = given_img_histogram[i].Blue + given_img_cumulative[i - 1].Blue;
                }
            }
            //**************************************************************************
            //3- divide each value in it by the maximum cumulative
            //4- multibly by the new range : const 255
            for (int j = 0; j < 256; j++)
            {
                change_with[j].Red = (int)Math.Round((decimal)(((double)given_img_cumulative[j].Red / given_img_cumulative[255].Red) * 255));
                change_with[j].Green = (int)Math.Round((decimal)(((double)given_img_cumulative[j].Green / given_img_cumulative[255].Green) * 255));
                change_with[j].Blue = (int)Math.Round((decimal)(((double)given_img_cumulative[j].Blue / given_img_cumulative[255].Blue) * 255));
            }
            //5- from given image and change_with list >> produce the result image
            for (int j = 0; j < given.GetLength(0); j++)
            {
                for (int l = 0; l < given.GetLength(1); l++)
                {
                    resulted_img[j, l].Red = change_with[given[j, l].Red].Red;
                    resulted_img[j, l].Green = change_with[given[j, l].Green].Green;
                    resulted_img[j, l].Blue = change_with[given[j, l].Blue].Blue;
                }
            }
            return resulted_img;
        }

        int[] local_his_red = new int[256];
        int[] local_his_blue = new int[256];
        int[] local_his_green = new int[256];
        int[] choosed_his_red = new int[256];
        int[] choosed_his_blue = new int[256];
        int[] choosed_his_green = new int[256];
        int[] matched_his_green = new int[256];
        int[] matched_his_blue = new int[256];
        int[] matched_his_red = new int[256];

        //*****************************************************

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
                        //local_his_red[(int)r]++;
                        //local_his_green[(int)g]++;
                        //local_his_blue[(int)b]++;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                }//end for i
            }//end unsafe
            localimage.UnlockBits(bitmapData2);
            bufferequa = new my_color[localimage.Height,localimage.Width];
            bufferequa = histo_equalization(Buffer2D);
            for (int i = 0; i < localimage.Height; i++)
            {
                for (int j = 0; j < localimage.Width; j++)
                {
                    local_his_red[bufferequa[i,j].Red]++;
                    local_his_green[bufferequa[i, j].Green]++;
                    local_his_blue[bufferequa[i, j].Blue]++;
                }
            }
            pictureBox1.Image = localimage;

            
            //localimage = new Bitmap(
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata(transferedimage);
            fm1.Show();
            this.Hide();
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
                    mygray   = new my_color[hei, wie];
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
                                //choosed_his_red[(int)r]++;
                                //choosed_his_green[(int)g]++;
                                //choosed_his_blue[(int)b]++;
                                //4 bytes per pixel
                                imagePointer1 += 4;
                            }//end for j
                            //4 bytes per pixel
                            imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                        }//end for i
                    }//end unsafe
                    choosedimage.UnlockBits(bitmapData2);
                }
                catch(Exception)
                {
                    MessageBox.Show("errooooooor");
                }
            }
            bufferchoose = new my_color[choosedimage.Height, choosedimage.Width];
            bufferchoose = histo_equalization(Buffer);
            for (int i = 0; i < choosedimage.Height; i++)
            {
                for (int j = 0; j < choosedimage.Width; j++)
                {
                    choosed_his_red[bufferchoose[i, j].Red]++;
                    choosed_his_green[bufferchoose[i, j].Green]++;
                    choosed_his_blue[bufferchoose[i, j].Blue]++;
                }
            }
        }
        /*
        public my_color[,] histo_equalization()
        {
            // my temp 2D arrays to calculate cumulative
            int[] local_his_red_run_sum = new int[256];
            int[] local_his_green_run_sum = new int[256];
            int[] local_his_blue_run_sum = new int[256];

            int[] choosed_his_red_run_sum = new int[256];
            int[] choosed_his_green_run_sum = new int[256];
            int[] choosed_his_blue_run_sum = new int[256];
            // my temp 2D arrays to the final values
            int[] local_his_red_final = new int[256];
            int[] local_his_green_final = new int[256];
            int[] local_his_blue_final = new int[256];

            int[] choosed_his_red_final = new int[256];
            int[] choosed_his_green_final = new int[256];
            int[] choosed_his_blue_final = new int[256];
            //1- calculate image histogram // Done
            // 1D arrays
            //local_his_red -> first image histogram
            //local_his_green -> first image histogram
            //local_his_bkue -> first image histogram
            //-------------------------------------------------------------------------
            //choosed_his_red -> second image histogram
            //choosed_his_green -> second image histogram
            //choosed_his_blue -> second image histogram
            //**************************************************************************
            //**************************************************************************
            //2- calculate cumulative for it
            for (int i = 0; i < 256; i++)
            {
                if (i == 0)
                {
                    local_his_red_run_sum[i] = local_his_red[i];
                    local_his_green_run_sum[i] = local_his_green[i];
                    local_his_blue_run_sum[i] = local_his_blue[i];

                    choosed_his_red_run_sum[i] = choosed_his_red[i];
                    choosed_his_green_run_sum[i] = choosed_his_green[i];
                    choosed_his_blue_run_sum[i] = choosed_his_blue[i];
                }
                else
                {
                    local_his_red_run_sum[i] = local_his_red[i] + local_his_red_run_sum[i - 1];
                    local_his_green_run_sum[i] = local_his_green[i] + local_his_green_run_sum[i - 1];
                    local_his_blue_run_sum[i] = local_his_blue[i] + local_his_blue_run_sum[i - 1];

                    choosed_his_red_run_sum[i] = choosed_his_red[i] + choosed_his_red_run_sum[i - 1];
                    choosed_his_green_run_sum[i] = choosed_his_green[i] + choosed_his_green_run_sum[i - 1];
                    choosed_his_blue_run_sum[i] = choosed_his_blue[i] + choosed_his_blue_run_sum[i - 1];
                }
            }
            //**************************************************************************
            //3- divide each value in it by the maximum cumulative
            //4- multibly by the new range : const 255
            int load_maximum_cumulative_red = local_his_red_run_sum[255];
            int load_maximum_cumulative_green = local_his_green_run_sum[255];
            int load_maximum_cumulative_blue = local_his_blue_run_sum[255];

            int choosed_maximum_cumulative_red = choosed_his_red_run_sum[255];
            int choosed_maximum_cumulative_green = choosed_his_green_run_sum[255];
            int choosed_maximum_cumulative_blue = choosed_his_blue_run_sum[255];

            for (int j = 0; j < 256; j++)
            {
                local_his_red_final[j] = (int)Math.Round((decimal)((local_his_red_run_sum[j] / load_maximum_cumulative_red) * 255));
                local_his_green_final[j] = (int)Math.Round((decimal)((local_his_green_run_sum[j] / load_maximum_cumulative_green) * 255));
                local_his_blue_final[j] = (int)Math.Round((decimal)((local_his_blue_run_sum[j] / load_maximum_cumulative_blue) * 255));

                choosed_his_red_final[j] = (int)Math.Round((decimal)((choosed_his_red_run_sum[j] / choosed_maximum_cumulative_red) * 255));
                choosed_his_green_final[j] = (int)Math.Round((decimal)((choosed_his_green_run_sum[j] / choosed_maximum_cumulative_green) * 255));
                choosed_his_blue_final[j] = (int)Math.Round((decimal)((choosed_his_blue_run_sum[j] / choosed_maximum_cumulative_blue) * 255));
            }

        }
        */
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();
            dt1 = DateTime.Now;
            int nearstR = 1000;
            int nearstG = 1000;
            int nearstB = 1000;
            int in_red = -1;
            int in_green = -1;
            int in_blue = -1;
            for (int i = 0; i < 256; i++)
            {
                nearstR = 1000;
                nearstG = 1000;
                nearstB = 1000;
                in_red = -1;
                in_green = -1;
                in_blue = -1;
                for (int j = 0; j < 256; j++)
                {
                    if (Math.Abs(local_his_red[i] - choosed_his_red[j]) <= nearstR)
                    {
                        nearstR = Math.Abs(local_his_red[i] - choosed_his_red[j]);
                        in_red = j;
                    }
                    if (Math.Abs(local_his_blue[i] - choosed_his_blue[j]) <= nearstG)
                    {
                        nearstG = Math.Abs(local_his_blue[i] - choosed_his_blue[j]);
                        in_green = j;
                    }
                    if (Math.Abs(local_his_green[i] - choosed_his_green[j]) <= nearstB)
                    {
                        nearstB = Math.Abs(local_his_green[i] - choosed_his_green[j]);
                        in_blue = j;
                    }
                }
                matched_his_red[i] = in_red;
                matched_his_green[i] = in_green;
                matched_his_blue[i] = in_blue;
            }

            Bitmap matchedimage = new Bitmap(localimage.Width,localimage.Height);
            Color newpixel = new Color();
            my_color[,] my_new_pixel = new my_color[matchedimage.Height,matchedimage.Width];
            int x, y;
            BitmapData bitmapData3 = matchedimage.LockBits(new Rectangle(0, 0, matchedimage.Width, matchedimage.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData3.Scan0;

                for (x = 0; x < bitmapData3.Height; x++)
                {
                    for (y = 0; y < bitmapData3.Width; y++)
                    {
                    my_new_pixel[x, y] = Buffer2D[x, y];
                    my_new_pixel[x, y].Red = matched_his_red[(int)my_new_pixel[x, y].Red];
                    my_new_pixel[x, y].Green = matched_his_green[(int)my_new_pixel[x, y].Green];
                    my_new_pixel[x, y].Blue = matched_his_blue[(int)my_new_pixel[x, y].Blue];
                    imagePointer1[0] = (byte)0;
                    imagePointer1[1] = (byte)0;
                    imagePointer1[2] = (byte)0;
                    imagePointer1[3] = (byte)255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData3.Stride - (bitmapData3.Width * 4);
                }//end for i
            }//end unsafe
            matchedimage.UnlockBits(bitmapData3);
            BitmapData bitmapData4 = matchedimage.LockBits(new Rectangle(0, 0, matchedimage.Width, matchedimage.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData4.Scan0;

                for (x = 0; x < bitmapData4.Height; x++)
                {
                    for (y = 0; y < bitmapData4.Width; y++)
                    {
                        //my_new_pixel[x, y] = Buffer2D[x, y];
                        
                        imagePointer1[0] = (byte)my_new_pixel[x, y].Blue;
                        imagePointer1[1] = (byte)my_new_pixel[x, y].Green;
                        imagePointer1[2] = (byte)my_new_pixel[x, y].Red;
                        imagePointer1[3] = (byte)255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData4.Stride - (bitmapData4.Width * 4);
                }//end for i
            }//end unsafe
            matchedimage.UnlockBits(bitmapData4);
            pictureBox3.Image = matchedimage;
            transferedimage = new Bitmap(matchedimage);
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            //image = image1;
            MessageBox.Show(dt3.ToString());
        }
    }
}
