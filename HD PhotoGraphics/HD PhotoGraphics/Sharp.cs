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
    public partial class Sharp : Form
    {
        public Sharp()
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



        Color clr = new Color(); // I need it to fill values returned from get_Pixel()
        _image fst_image = new _image();
        _image padded_image;


        public void setdata(Bitmap imagetransfer)
        {
            int x;
            int y;
            pictureBox1.Image = imagetransfer;
            localimage = new Bitmap(imagetransfer);
            Buffer2D = new my_color[localimage.Height, localimage.Width];
            width = localimage.Width;
            Height = localimage.Height;
            fst_image.image_bitmap = new Bitmap(imagetransfer);
            fst_image.image_hight = fst_image.image_bitmap.Height;
            fst_image.image_width = fst_image.image_bitmap.Width;

            fst_image.image_buffer = new my_color[fst_image.image_hight, fst_image.image_width];
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
                        fst_image.image_buffer[x, y].Red = (int)r;
                        fst_image.image_buffer[x, y].Green = (int)g;
                        fst_image.image_buffer[x, y].Blue = (int)b;

                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
                }//end for i
            }//end unsafe
            localimage.UnlockBits(bitmapData2);
        }

        my_color[,] Post_processing(my_color[,] work_buffer, string operation)
        {
            if (operation.Equals("normalization"))
            {
                //Normalization
                //initiallize the min, max values();
                int Min_Val_red = 255, Max_Val_red = 0;
                int Min_Val_green = 255, Max_Val_green = 0;
                int Min_Val_blue = 255, Max_Val_blue = 0;

                // get @1st minimum & maximum values
                for (int i = 0; i < work_buffer.GetLength(0); i++)
                {
                    for (int j = 0; j < work_buffer.GetLength(1); j++)
                    {
                        if (Max_Val_red < work_buffer[i, j].Red)
                            Max_Val_red = work_buffer[i, j].Red;
                        if (Min_Val_red > work_buffer[i, j].Red)
                            Min_Val_red = work_buffer[i, j].Red;
                        if (Max_Val_green < work_buffer[i, j].Green)
                            Max_Val_green = work_buffer[i, j].Green;
                        if (Min_Val_green > work_buffer[i, j].Green)
                            Min_Val_green = work_buffer[i, j].Green;
                        if (Max_Val_blue < work_buffer[i, j].Blue)
                            Max_Val_blue = work_buffer[i, j].Blue;
                        if (Min_Val_blue > work_buffer[i, j].Blue)
                            Min_Val_blue = work_buffer[i, j].Blue;
                    }
                }
                for (int i = 0; i < work_buffer.GetLength(0); i++)
                {
                    for (int j = 0; j < work_buffer.GetLength(1); j++)
                    {
                        work_buffer[i, j].Red = (int)(((work_buffer[i, j].Red - Min_Val_red) / (double)(Max_Val_red - Min_Val_red)) * 255);
                        work_buffer[i, j].Green = (int)(((work_buffer[i, j].Green - Min_Val_green) / (double)(Max_Val_green - Min_Val_green)) * 255);
                        work_buffer[i, j].Blue = (int)(((work_buffer[i, j].Blue - Min_Val_blue) / (double)(Max_Val_blue - Min_Val_blue)) * 255);
                    }
                }
            }

            if (operation.Equals("cutoff"))
            {
                //post-processing -> CutOff
                for (int i = 0; i < work_buffer.GetLength(0); i++)
                {
                    for (int j = 0; j < work_buffer.GetLength(1); j++)
                    {
                        if (work_buffer[i, j].Red > 255)
                            work_buffer[i, j].Red = 255;
                        if (work_buffer[i, j].Red < 0)
                            work_buffer[i, j].Red = 0;
                        if (work_buffer[i, j].Green > 255)
                            work_buffer[i, j].Green = 255;
                        if (work_buffer[i, j].Green < 0)
                            work_buffer[i, j].Green = 0;
                        if (work_buffer[i, j].Blue > 255)
                            work_buffer[i, j].Blue = 255;
                        if (work_buffer[i, j].Blue < 0)
                            work_buffer[i, j].Blue = 0;
                    }
                }
            }
            if (operation.Equals("abs"))
            {
                //post-processing -> Absolute
                for (int i = 0; i < work_buffer.GetLength(0); i++)
                {
                    for (int j = 0; j < work_buffer.GetLength(1); j++)
                    {
                        work_buffer[i, j].Red = Math.Abs(work_buffer[i, j].Red);
                        work_buffer[i, j].Green = Math.Abs(work_buffer[i, j].Green);
                        work_buffer[i, j].Blue = Math.Abs(work_buffer[i, j].Blue);
                    }
                }

            }
            return work_buffer;
        }

        public my_color[,] pad_image(ref my_color[,] image1, int mask_width, int mask_height)
        {
            padded_image = new _image();

            //Padding process : add borders to the image , new size then
            //needed variables
            int old_width, old_hight, new_width, new_hight, added_width_size, added_height_size;
            old_width = image1.GetLength(1);
            old_hight = image1.GetLength(0);



            new_width = old_width + mask_width - 1;
            new_hight = old_hight + mask_height - 1;

            padded_image.image_buffer = new my_color[(new_hight), (new_width)];

            added_width_size = (int)((mask_width - 1) / 2);
            added_height_size = (int)((mask_height - 1) / 2);

            for (int i = 0; i < new_hight; i++)
            {
                for (int j = 0; j < new_width; j++)
                {
                    if (j < added_width_size || i < added_height_size)
                    {
                        padded_image.image_buffer[i, j].Red = 0;
                        padded_image.image_buffer[i, j].Green = 0;
                        padded_image.image_buffer[i, j].Blue = 0;
                    }
                    else if (j >= (old_width + added_width_size) || i >= (old_hight + added_height_size))
                    {
                        padded_image.image_buffer[i, j].Red = 0;
                        padded_image.image_buffer[i, j].Green = 0;
                        padded_image.image_buffer[i, j].Blue = 0;
                    }
                    else
                    {
                        padded_image.image_buffer[i, j].Red = image1[(i - added_height_size), (j - added_width_size)].Red;
                        padded_image.image_buffer[i, j].Green = image1[(i - added_height_size), (j - added_width_size)].Green;
                        padded_image.image_buffer[i, j].Blue = image1[(i - added_height_size), (j - added_width_size)].Blue;
                    }
                }
            }

            return padded_image.image_buffer;
        }

        public my_color[,] Linear_filter(ref my_color[,] the_image, ref float[,] the_mask, string _post_processing)
        {
            _image temp_image = new _image();
            int mask_width = the_mask.GetLength(1);
            int mask_height = the_mask.GetLength(0);

            temp_image.image_buffer = pad_image(ref the_image, mask_width, mask_height);
            //MessageBox.Show("Now Image is padded in the Memory , here it is");
            //display_image(temp_image.image_buffer);

            //padded and ready to be masked
            _image resulted_image = new _image();
            resulted_image.image_buffer = new my_color[the_image.GetLength(0), the_image.GetLength(1)];
            //Apply the choosed filter with the given mask

            int added_width = (int)((the_image.GetLength(1) - 1) / 2);
            int added_height = (int)((the_image.GetLength(0) - 1) / 2);

            int W = the_image.GetLength(1);
            int H = the_image.GetLength(0);
            double red_sum, green_sum, blue_sum;
            int Mask_width = the_image.GetLength(1);
            int Mask_hight = the_image.GetLength(0);


            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    red_sum = 0;
                    green_sum = 0;
                    blue_sum = 0;
                    int current_pixel_width = j + added_width;
                    int current_pixel_height = i + added_height;

                    for (int current_height_of_pixel_padded = current_pixel_height - added_height, current_H_pixel_mask = 0; current_H_pixel_mask < mask_height; current_height_of_pixel_padded++, current_H_pixel_mask++)  /////current_height_of_pixel_padded < current_pixel_height + added_height
                    {
                        for (int current_width_of_pixel_padded = current_pixel_width - added_width, current_W_pixel_mask = 0; current_W_pixel_mask < mask_width; current_width_of_pixel_padded++, current_W_pixel_mask++)            ///// current_width_of_pixel_padded < current_pixel_width + added_width
                        {
                            red_sum += (double)temp_image.image_buffer[current_height_of_pixel_padded, current_width_of_pixel_padded].Red * (double)the_mask[current_H_pixel_mask, current_W_pixel_mask];
                            green_sum += (double)temp_image.image_buffer[current_height_of_pixel_padded, current_width_of_pixel_padded].Green * (double)the_mask[current_H_pixel_mask, current_W_pixel_mask];
                            blue_sum += (double)temp_image.image_buffer[current_height_of_pixel_padded, current_width_of_pixel_padded].Blue * (double)the_mask[current_H_pixel_mask, current_W_pixel_mask];
                        }
                    }

                    resulted_image.image_buffer[i, j].Red = (int)red_sum;
                    resulted_image.image_buffer[i, j].Green = (int)green_sum;
                    resulted_image.image_buffer[i, j].Blue = (int)blue_sum;
                }
            }

            //return resulted_image.image_buffer;
            return Post_processing(resulted_image.image_buffer, _post_processing);
        }

        public Bitmap display_image(my_color[,] image_buffer)
        {
            Bitmap temp_bit_map = new Bitmap(image_buffer.GetLength(1), image_buffer.GetLength(0));
            for (int i = 0; i < image_buffer.GetLength(0); i++)
            {
                for (int j = 0; j < image_buffer.GetLength(1); j++)
                {
                    clr = Color.FromArgb(image_buffer[i, j].Red, image_buffer[i, j].Green, image_buffer[i, j].Blue);
                    temp_bit_map.SetPixel(j, i, clr);
                }
            }
            return temp_bit_map;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            TimeSpan dt3 = new TimeSpan();

            dt1 = DateTime.Now;
            float[,] operation_mask1 = { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
            pictureBox2.Image = display_image((Linear_filter(ref fst_image.image_buffer, ref operation_mask1, "cutoff")));
            dt2 = DateTime.Now;
            dt3 = dt2 - dt1;
            //image = image1;
            MessageBox.Show(dt3.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata((Bitmap)pictureBox2.Image);
            fm1.Show();
            this.Hide();
        }
    }
}
