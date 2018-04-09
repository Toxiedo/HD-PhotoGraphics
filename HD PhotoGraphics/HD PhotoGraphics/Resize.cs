using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.IO;

namespace HD_PhotoGraphics
{
   public struct my_color
    {
        public int Red;
        public int Green;
        public int Blue;
    }
    class Resize
    {
        public Bitmap bil_resize(Bitmap image , int width , int height)
        {
            int n_width = width;
            int n_hieght = height;

            float w_ratio = (float)(image.Width  / n_width);
            float h_ratio = (float)(image.Height / n_hieght);

            Bitmap myimage = image;
            int X1, X2, Y1, Y2;
            
            float OldX, OldY, NewX, NewY;

            float XFraction, YFraction;
            float Z1, Z2;

            Bitmap b1 = new Bitmap(n_width, n_hieght);
            int x;
            int y;
            my_color[,] Buffer2D = new my_color[myimage.Height, myimage.Width];
            my_color[,] resized_image = new my_color[n_hieght, n_width];
            BitmapData bitmapData2 = myimage.LockBits(new Rectangle(0, 0, myimage.Width, myimage.Height),
                         ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData2.Scan0;

                for ( x = 0; x < bitmapData2.Height; x++)
                {
                    for (y = 0; y < bitmapData2.Width; y++)
                    {
                        double b= (int)imagePointer1[0];
                        double g= (int)imagePointer1[1];
                        double r= (int)imagePointer1[2];
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
            myimage.UnlockBits(bitmapData2);
            my_color P1, P2, P3, P4;
            my_color newpixel = new my_color();
            for (int i = 0; i < n_hieght; i++)
            {
                NewY = i;
                OldY = NewY * h_ratio;
                for (int j = 0; j < n_width; j++)
                {

                    NewX = j;
                    OldX = NewX * w_ratio;


                    X1 = (int)Math.Floor(OldX); X2 = X1 + 1;
                    Y1 = (int)Math.Floor(OldY); Y2 = Y1 + 1;

                    if (X2 == image.Width) X2 -= 1;
                    if (Y2 == image.Height) Y2 -= 1;
                    if (X1 == image.Width) X1 -= 1;
                    if (Y1 == image.Height) Y1 -= 1;

                    P1 = Buffer2D[ Y1  ,X1 ]; P2 = Buffer2D[  Y1 , X2 ];
                    P3 = Buffer2D[  Y2 ,X1 ]; P4 = Buffer2D[  Y2 ,X2 ];

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

                    
                    resized_image[i, j].Blue = newpixel.Blue;
                    resized_image[i, j].Red = newpixel.Red;
                    resized_image[i, j].Green = newpixel.Green;
                    Color clr;
                    clr = Color.FromArgb(255, newpixel.Red, newpixel.Green, newpixel.Blue);
                    b1.SetPixel(j, i, clr);
                }
            }
            Bitmap output = new Bitmap(n_width, n_hieght);
            BitmapData bitmapData3 = output.LockBits(new Rectangle(0, 0, n_width, n_hieght),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData3.Scan0;
                for (int i = 0; i < bitmapData3.Height; i++)
                {
                    for (int j = 0; j < bitmapData3.Width; j++)
                    {
                        imagePointer1[0] = (byte)resized_image[i,j].Blue;
                        imagePointer1[1] = (byte)resized_image[i,j].Green;
                        imagePointer1[2] = (byte)resized_image[i,j].Red;
                        imagePointer1[3] = 255;
                        //4 bytes per pixel
                        imagePointer1 += 4;
                    }//end for j
                    //4 bytes per pixel
                    imagePointer1 += (bitmapData3.Stride - (bitmapData3.Width * 4));
                }//end for i
            }//end unsafe
            output.UnlockBits(bitmapData3);
            return b1;// col;
        }
    }
}
