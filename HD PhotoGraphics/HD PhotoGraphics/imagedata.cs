using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace HD_PhotoGraphics
{
    class imagedata
    {
        public int[,] ReadImage(Bitmap ImageData)
        {
            int i, j, Width, Height;

            Width = ImageData.Width;
            Height = ImageData.Height;

            int[,] GreyImage = new int[Width, Height];  //[Row,Column]

            BitmapData bitmapData1 = ImageData.LockBits(new Rectangle(0, 0, ImageData.Width, ImageData.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        GreyImage[j, i] = (int)((imagePointer1[0] + imagePointer1[1] + imagePointer1[2]) / 3.0);
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);
                }//end for i
            }//end unsafe
            ImageData.UnlockBits(bitmapData1);
            return GreyImage;
        }

        public  int[, ,] ReadImageRGB(Bitmap ImageData)
        {
            int i, j, Width, Height;

            Width = ImageData.Width;
            Height = ImageData.Height;

            int[, ,] GreyImage = new int[3, Width, Height];  //[Colour, Row,Column]

            BitmapData bitmapData1 = ImageData.LockBits(new Rectangle(0, 0, ImageData.Width, ImageData.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        GreyImage[0, j, i] = (int)imagePointer1[0];
                        GreyImage[1, j, i] = (int)imagePointer1[1];
                        GreyImage[2, j, i] = (int)imagePointer1[2];

                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += bitmapData1.Stride - (bitmapData1.Width * 4);
                }//end for i
            }//end unsafe
            ImageData.UnlockBits(bitmapData1);
            return GreyImage;
        }

        public Bitmap Displayimage(int[,] image)
        {
            int i, j;
            Bitmap output = new Bitmap(image.GetLength(0), image.GetLength(1));
            BitmapData bitmapData1 = output.LockBits(new Rectangle(0, 0, image.GetLength(0), image.GetLength(1)),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        imagePointer1[0] = (byte)image[j, i];
                        imagePointer1[1] = (byte)image[j, i];
                        imagePointer1[2] = (byte)image[j, i];
                        imagePointer1[3] = 255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }//end for i
            }//end unsafe
            output.UnlockBits(bitmapData1);
            return output;// col;

        }

        public Bitmap Displayimage(int[, ,] image)
        {
            int i, j;
            Bitmap output = new Bitmap(image.GetLength(1), image.GetLength(2));
            BitmapData bitmapData1 = output.LockBits(new Rectangle(0, 0, image.GetLength(1), image.GetLength(2)),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        imagePointer1[0] = (byte)image[0, j, i];
                        imagePointer1[1] = (byte)image[1, j, i];
                        imagePointer1[2] = (byte)image[2, j, i];
                        imagePointer1[3] = 255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += (bitmapData1.Stride - (bitmapData1.Width * 4));
                }//end for i
            }//end unsafe
            output.UnlockBits(bitmapData1);
            return output;// col;

        }
    }
}
