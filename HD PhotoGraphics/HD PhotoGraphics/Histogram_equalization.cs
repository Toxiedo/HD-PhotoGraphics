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
	public partial class Histogram_equalization : Form
	{
		#region variables
		Bitmap image;
		my_color[,] image_Buffer2D, mygray,result;
		Color clr;

		int Height, width;
		Bitmap localimage;

		public void setdata(Bitmap imagetransfer)
		{
			int x;
			int y;
			pictureBox1.Image = imagetransfer;
			localimage = new Bitmap(imagetransfer);
			image_Buffer2D = new my_color[localimage.Height, localimage.Width];
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
						image_Buffer2D[x, y].Blue = (int)b;
						image_Buffer2D[x, y].Green = (int)g;
						image_Buffer2D[x, y].Red = (int)r;
						//4 bytes per pixel
						imagePointer1 += 4;
					}//end for j
					//4 bytes per pixel
					imagePointer1 += bitmapData2.Stride - (bitmapData2.Width * 4);
				}//end for i
			}//end unsafe
			localimage.UnlockBits(bitmapData2);
			pictureBox1.Image = localimage;;
		}

		double new_min_red1, new_min_blue1, new_min_green1;
		double new_max_red1, new_max_blue1, new_max_green1;

		int new_max_red = 0, new_min_red = 255,
			new_max_green = 0, new_min_green = 255,
			new_max_blue = 0, new_min_blue = 255;
		#endregion

		public void draw_histogram_1(my_color[,] current)
		{
			double[] red_frequencies = new double[256];
			float min_val,max_val,range,point;

			for (int w = 0; w < 256; w++)
				red_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			
			for (int u = 0; u < current.GetLength(0); u++)
			{
				for (int k = 0; k < current.GetLength(1); k++)
				{
					if (current[u, k].Red < min_val)
						min_val = current[u, k].Red;

					if (current[u, k].Red > max_val)
						max_val = current[u, k].Red;
				}
			}
			range = 1;
			min_val -= range;
			chart1.ChartAreas[0].AxisX.Minimum = min_val;
			chart1.ChartAreas[0].AxisX.Maximum = max_val;
			chart1.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < current.GetLength(0); j++)
			{
				for (int l = 0; l < current.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (current[j, l].Red <= (min_val + (i * range)))
						{
							red_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
			chart1.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < red_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart1.Series[0].Points.AddXY(point, red_frequencies[q]);
			}
		}
		public void draw_histogram_2(my_color[,] current)
		{
			double[] red_frequencies = new double[256];
			float min_val, max_val, range, point;

			for (int w = 0; w < 256; w++)
				red_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;

			for (int u = 0; u < current.GetLength(0); u++)
			{
				for (int k = 0; k < current.GetLength(1); k++)
				{
					if (current[u, k].Red < min_val)
						min_val = current[u, k].Red;

					if (current[u, k].Red > max_val)
						max_val = current[u, k].Red;
				}
			}
			range = 1;
			min_val -= range;
			chart2.ChartAreas[0].AxisX.Minimum = min_val;
			chart2.ChartAreas[0].AxisX.Maximum = max_val;
			chart2.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < current.GetLength(0); j++)
			{
				for (int l = 0; l < current.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (current[j, l].Red <= (min_val + (i * range)))
						{
							red_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
			chart2.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < red_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart2.Series[0].Points.AddXY(point, red_frequencies[q]);
			}
		}
		public Histogram_equalization()
		{
			InitializeComponent();
		}
		public my_color[,] histo_equalization(my_color[,] given)
		{
			my_color[] given_img_histogram = new my_color[256] ;//1D arrays to calculate histogram
			my_color[] given_img_cumulative = new my_color[256];//1D arrays to calculate cumulative
			my_color[] change_with = new my_color[256];//1D arrays to displacment values
			my_color[,] resulted_img = new my_color[given.GetLength(0),given.GetLength(1)];
			
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
					given_img_cumulative[i].Red = given_img_histogram[i].Red + given_img_cumulative[i-1].Red;
					given_img_cumulative[i].Green = given_img_histogram[i].Green + given_img_cumulative[i-1].Green;
					given_img_cumulative[i].Blue = given_img_histogram[i].Blue + given_img_cumulative[i-1].Blue ;
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
					resulted_img[j,l].Red = change_with[given[j,l].Red].Red;
					resulted_img[j,l].Green = change_with[given[j,l].Green].Green;
					resulted_img[j,l].Blue = change_with[given[j,l].Blue].Blue;
				}
			}
			return resulted_img;
		}
		
		private void button2_Click(object sender, EventArgs e)
		{
			result = histo_equalization(image_Buffer2D);
			draw_histogram_1(image_Buffer2D);
			draw_histogram_2(result);
			Bitmap result_image_bitmap = new Bitmap(result.GetLength(1),result.GetLength(0));

			//display resulted image
			for (int i = 0; i < result.GetLength(0); i++)
			{
				for (int j = 0; j < result.GetLength(1); j++)
				{
					clr = Color.FromArgb(result[i, j].Red, result[i, j].Green, result[i, j].Blue);
					result_image_bitmap.SetPixel(j, i, clr);
				}
			}
			pictureBox2.Image = result_image_bitmap;
			
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Form1 fm1 = new Form1();
			fm1.setdata((Bitmap)pictureBox2.Image);
			fm1.Show();
			this.Hide();
		}
	}
}
