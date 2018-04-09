using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HD_PhotoGraphics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Variables

        int[, ,] image_buffer;
        Bitmap original_bm;
        imagedata myimage;
        Resize bilinear;
        #endregion
		public void _fill_charts(my_color[,] buffer)
		{
			double[] red_frequencies = new double[256];
			double[] green_frequencies = new double[256];
			double[] blue_frequencies = new double[256];
			float min_val, max_val, range, point;

			#region Draw red Histogram
			for (int w = 0; w < 256; w++)
				red_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			for (int u = 0; u < buffer.GetLength(0); u++)
			{
				for (int k = 0; k < buffer.GetLength(1); k++)
				{
					if (buffer[u, k].Red < min_val)
						min_val = buffer[u, k].Red;

					if (buffer[u, k].Red > max_val)
						max_val = buffer[u, k].Red;
				}
			}
			range = 1;
			min_val -= range;
			chart1.ChartAreas[0].AxisX.Minimum = min_val;
			chart1.ChartAreas[0].AxisX.Maximum = max_val;
			chart1.ChartAreas[0].AxisX.Interval = range;
			for (int j = 0; j < buffer.GetLength(0); j++)
			{
				for (int l = 0; l < buffer.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (buffer[j, l].Red <= (min_val + (i * range)))
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
			#endregion
			#region Draw green Histogram
			for (int w = 0; w < 256; w++)
				green_frequencies[w] = 0;
			min_val = float.MaxValue;
			max_val = float.MinValue;
			for (int u = 0; u < buffer.GetLength(0); u++)
			{
				for (int k = 0; k < buffer.GetLength(1); k++)
				{
					if (buffer[u, k].Green < min_val)
						min_val = buffer[u, k].Green;

					if (buffer[u, k].Green > max_val)
						max_val = buffer[u, k].Green;
				}
			}
			range = 1;
			min_val -= range;
			chart2.ChartAreas[0].AxisX.Minimum = min_val;
			chart2.ChartAreas[0].AxisX.Maximum = max_val;
			chart2.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < buffer.GetLength(0); j++)
			{
				for (int l = 0; l < buffer.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (buffer[j, l].Green <= (min_val + (i * range)))
						{
							green_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
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
			for (int u = 0; u < buffer.GetLength(0); u++)
			{
				for (int k = 0; k < buffer.GetLength(1); k++)
				{
					if (buffer[u, k].Blue < min_val)
						min_val = buffer[u, k].Blue;

					if (buffer[u, k].Blue > max_val)
						max_val = buffer[u, k].Blue;
				}
			}
			range = 1;
			min_val -= range;
			chart3.ChartAreas[0].AxisX.Minimum = min_val;
			chart3.ChartAreas[0].AxisX.Maximum = max_val;
			chart3.ChartAreas[0].AxisX.Interval = range;

			for (int j = 0; j < buffer.GetLength(0); j++)
			{
				for (int l = 0; l < buffer.GetLength(1); l++)
				{
					for (int i = 1; i <= 255; i++)
					{
						if (buffer[j, l].Blue <= (min_val + (i * range)))
						{
							blue_frequencies[i - 1]++;
							break;
						}
					}
				}
			}
			chart3.Series[0].Points.Clear();
			point = 0;
			for (int q = 0; q < blue_frequencies.Length; q++)
			{
				point = (min_val + (q * range)) + (range / 2.0f);
				chart3.Series[0].Points.AddXY(point, blue_frequencies[q]);
			}
			#endregion
		}
        public void setdata(Bitmap imagetransfer)
        {
            original_bm = imagetransfer;
            inputimage.Image = original_bm;
			#region chart Draw
			//convert image to buffer
			Bitmap im = new Bitmap(original_bm);
			my_color[,] image_buffer = new my_color[original_bm.Height, original_bm.Width];
			for (int i = 0; i < original_bm.Height; i++)
			{
				for (int j = 0; j < original_bm.Width; j++)
				{
					Color _clr;
					_clr = im.GetPixel(j, i);
					image_buffer[i, j].Red = _clr.R;
					image_buffer[i, j].Green = _clr.G;
					image_buffer[i, j].Blue = _clr.B;
				}
			}
			_fill_charts(image_buffer);
			#endregion
		}

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           OpenFileDialog openFileDialog1 = new OpenFileDialog();
           if (openFileDialog1.ShowDialog() == DialogResult.OK)
           {
               //Open the browsed image and display it
               myimage = new imagedata();
               string OpenedFilePath = openFileDialog1.FileName;
               original_bm = new Bitmap(OpenedFilePath);
               inputimage.Image = original_bm;
               //image_buffer = new int[3, original_bm.Width, original_bm.Height];
               //image_buffer = myimage.ReadImageRGB(original_bm);
           }
        }

        private void openP3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                opentype p3 = new opentype();
                string OpenedFilePath = openFileDialog1.FileName;
                original_bm = new Bitmap(p3.openp3(OpenedFilePath));
                inputimage.Image = original_bm;
                //image_buffer = new int[3, original_bm.Width, original_bm.Height];
                //image_buffer = myimage.ReadImageRGB(original_bm);
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizedform rf = new resizedform();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void gausianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gausian gf = new Gausian();
            gf.setdata(original_bm);
            gf.Show();
            this.Hide();
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gamma gm = new Gamma();
            gm.Show();
            this.Hide();
        }

        private void quantizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quantization gf = new quantization();
            gf.setdata(original_bm);
            gf.Show();
            this.Hide();
        }

        private void histogramMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HM gf = new HM();
            gf.setdata(original_bm);
            gf.Show();
            this.Hide();
        }

        private void meanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mean gf = new Mean();
            gf.setdata(original_bm);
            gf.Show();
            this.Hide();
        }

        private void gausianToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Gauss gf = new Gauss();
            gf.setdata(original_bm);
            gf.Show();
            this.Hide();
        }

        private void resizeThreadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resizedform rf = new resizedform();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void sharpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sharp rf = new Sharp();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void unsharpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Unsharp rf = new Unsharp();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void edgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EdgeDetection rf = new EdgeDetection();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contrast rf = new Contrast();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void notToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Not rf = new Not();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gray rf = new Gray();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addoperation rf = new Addoperation();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void subToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Suboperation rf = new Suboperation();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brightness rf = new Brightness();
            rf.setdata(original_bm);
            rf.Show();
            this.Hide();
        }

		private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Histogram_equalization He = new Histogram_equalization();
			He.setdata(original_bm);
			He.Show();
			this.Hide();
		}

		private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Histogram Hr = new Histogram();
			Hr.setdata(original_bm);
			Hr.Show();
			this.Hide();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sv = new SaveFileDialog();
			if (sv.ShowDialog() == DialogResult.OK)
			{
				string openfile = sv.FileName;
				Bitmap saved_img = new Bitmap(inputimage.Image);
				saved_img.Save(openfile);
			}
		}

		

    }
}
