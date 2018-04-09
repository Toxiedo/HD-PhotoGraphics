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
	public partial class Histogram : Form
	{
		Bitmap image;
		my_color[,] image_Buffer2D, mygray;
		Color clr;

		double new_min_red1, new_min_blue1, new_min_green1;
		double new_max_red1, new_max_blue1, new_max_green1;

		int new_max_red = 0, new_min_red = 255,
			new_max_green = 0, new_min_green = 255,
			new_max_blue = 0, new_min_blue = 255;

		public Histogram()
		{
			InitializeComponent();
		}

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
			pictureBox1.Image = localimage;
			Draw();
		}
		public void Draw()
		{
			double[] red_frequencies = new double[256];
			double[] green_frequencies = new double[256];
			double[] blue_frequencies = new double[256];
			float min_val,max_val,range,point;
 
			#region Draw red Histogram
			for (int w = 0; w < 256; w++)
 				red_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			for (int u = 0; u < image_Buffer2D.GetLength(0); u++)
			{
				for (int k = 0; k < image_Buffer2D.GetLength(1); k++)
				{
					if (image_Buffer2D[u,k].Red < min_val)
						min_val = image_Buffer2D[u,k].Red;

					if (image_Buffer2D[u,k].Red > max_val)
						max_val = image_Buffer2D[u,k].Red;
				}
			}
			range = 1;
			min_val -= range;
			chart1.ChartAreas[0].AxisX.Minimum = min_val;
			chart1.ChartAreas[0].AxisX.Maximum = max_val;
			chart1.ChartAreas[0].AxisX.Interval = range;
		
			for (int j = 0; j < image_Buffer2D.GetLength(0) ; j++)
			{
				for (int l = 0; l < image_Buffer2D.GetLength(1) ; l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (image_Buffer2D[j,l].Red <= (min_val + (i * range)))
						{
							red_frequencies[i-1]++;
							break;
						}
					}
				}
			}
			int max_red = 0;
			for (int h = 0; h < red_frequencies.Length; h++)
			{
				if (red_frequencies[h] > max_red)
					max_red = (int)red_frequencies[h];
			}
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = max_red;
			chart1.ChartAreas[0].AxisY.Interval = range;
			chart1.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < red_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart1.Series[0].Points.AddXY(point, red_frequencies[q]);
			}
			#endregion
			#region Draw green Histogram
			for (int w = 0; w < 256; w++)
				green_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			for (int u = 0; u < image_Buffer2D.GetLength(0); u++)
			{
				for (int k = 0; k < image_Buffer2D.GetLength(1); k++)
				{
					if (image_Buffer2D[u, k].Green < min_val)
						min_val = image_Buffer2D[u, k].Green;

					if (image_Buffer2D[u, k].Green > max_val)
						max_val = image_Buffer2D[u, k].Green;
				}
			}
			range = 1;
			min_val -= range;
			chart2.ChartAreas[0].AxisX.Minimum = min_val;
			chart2.ChartAreas[0].AxisX.Maximum = max_val;
			chart2.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < image_Buffer2D.GetLength(0); j++)
			{
				for (int l = 0; l < image_Buffer2D.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (image_Buffer2D[j, l].Green <= (min_val + (i * range)))
						{
							green_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
			int max_green = 0;
			for (int h = 0; h < green_frequencies.Length; h++)
			{
				if (green_frequencies[h] > max_green)
					max_green = (int)green_frequencies[h];
			}
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = max_green;
			chart1.ChartAreas[0].AxisY.Interval = range;
			chart2.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < green_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart2.Series[0].Points.AddXY(point, green_frequencies[q]);
			}
			#endregion
			#region Draw blue Histogram
			for (int w = 0; w < 256; w++)
				blue_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			for (int u = 0; u < image_Buffer2D.GetLength(0); u++)
			{
				for (int k = 0; k < image_Buffer2D.GetLength(1); k++)
				{
					if (image_Buffer2D[u, k].Blue < min_val)
						min_val = image_Buffer2D[u, k].Blue;

					if (image_Buffer2D[u, k].Blue > max_val)
						max_val = image_Buffer2D[u, k].Blue;
				}
			}
			range = 1;
			min_val -= range;
			chart3.ChartAreas[0].AxisX.Minimum = min_val;
			chart3.ChartAreas[0].AxisX.Maximum = max_val;
			chart3.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < image_Buffer2D.GetLength(0); j++)
			{
				for (int l = 0; l < image_Buffer2D.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (image_Buffer2D[j, l].Blue <= (min_val + (i * range)))
						{
							blue_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
			int max_blue = 0;
			for (int h = 0; h < blue_frequencies.Length; h++)
			{
				if (blue_frequencies[h] > max_blue)
					max_blue = (int)blue_frequencies[h];
			}
			chart1.ChartAreas[0].AxisY.Minimum = 0;
			chart1.ChartAreas[0].AxisY.Maximum = max_blue;
			chart1.ChartAreas[0].AxisY.Interval = range;

			chart3.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < blue_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart3.Series[0].Points.AddXY(point, blue_frequencies[q]);
			}
			#endregion
		}

		private void label5_Click(object sender, EventArgs e)
		{
			Form1 fm1 = new Form1();
			fm1.setdata((Bitmap)pictureBox1.Image);
			fm1.Show();
			this.Hide();
		}

	}
}
