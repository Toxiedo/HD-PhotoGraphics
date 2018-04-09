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
     
    class opentype
    {

        public  Bitmap openp3(string path)
        {
            Bitmap b1;
            FileStream FS = new FileStream(path, FileMode.Open);
            StreamReader SR = new StreamReader(FS);
            string line1 = SR.ReadLine();
            if (line1.Equals("P3"))
            {
                string line2 = SR.ReadLine();
                string[] words = line2.Split(' ');
                if (words[0].Equals("#"))
                {
                    line2 = SR.ReadLine();
                }
                string[] widthheight = line2.Split(' ');
                int width = int.Parse(widthheight[0]);
                int height = int.Parse(widthheight[1]);
                //textBox1.Text = widthheight[0];
                string num_of_bits = SR.ReadLine();
                string Remaining_of_file = SR.ReadToEnd();
                char[] delimiters = new char[] { ' ', '\n' };
                string[] Data_of_Image = Remaining_of_file.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                my_color[,] Buffer2D = new my_color[height, width];
                int current = 0;
                b1 = new Bitmap(width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Buffer2D[i, j] = new my_color();
                        Buffer2D[i, j].Red = int.Parse(Data_of_Image[current]);
                        Buffer2D[i, j].Green = int.Parse(Data_of_Image[current + 1]);
                        Buffer2D[i, j].Blue = int.Parse(Data_of_Image[current + 2]);
                        current += 3;
                        Color clr;
                        clr = Color.FromArgb(Buffer2D[i, j].Red, Buffer2D[i, j].Green, Buffer2D[i, j].Blue);
                        b1.SetPixel(j, i, clr);
                    }
                }
            }
            else
            {
                b1 = new Bitmap(100,100);
                return b1;
            }
                SR.Close();
                return b1;
        }
    }
}
