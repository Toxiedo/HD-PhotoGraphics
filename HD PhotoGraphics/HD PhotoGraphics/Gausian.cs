using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fast_Fourier_Transform;
using Homomorphic_Filter;

namespace HD_PhotoGraphics
{
    public partial class Gausian : Form
    {
        public Gausian()
        {
            InitializeComponent();
        }

        Bitmap imagetogausian;
        Bitmap filteredimage;

        public void setdata(Bitmap original_image)
        {
            pictureBox1.Image = original_image;
            imagetogausian = new Bitmap(original_image);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Reading Image
            int[, ,] ImageDataRGB = HomomorphicFilterObject.ReadImageRGB(imagetogausian); // Reading Colour Image Object
            //Output Image
            int[, ,] HMMFilteredImageRGB = new int[ImageDataRGB.GetLength(0), ImageDataRGB.GetLength(1), ImageDataRGB.GetLength(2)]; // Output Image
            int i, j, Width, Height;

            float rL, rH;
            rL = 0.68f;
            rH = 1.11f;

            float Sigma, Slope;
            Sigma = (float)Convert.ToDouble(Mean_Width.Text);
            Slope = 1.0f;

            COMPLEX[,] FFTData;
            int[,] HMMFilteredImage;

            FFT FFTObject;

            //Reading Selected Image Width & Height
            Width = ImageDataRGB.GetLength(1);
            Height = ImageDataRGB.GetLength(2);
            int[,] ImageData = new int[Width, Height];


            //Reading B  Components
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    ImageData[i, j] = ImageDataRGB[0, i, j];
                }

            //Processing B Comonent of the Image
            //Log of Image 
            //LogImage = HomomorphicFilterObject.LogofImage(ImageData);
            //FFT of the LOG Image
            FFTObject = new FFT(ImageData);
            FFTObject.ForwardFFT();
            //Shifting FFT for Filtering
            FFTObject.FFTShift();
            // Applying Filter on the FFT of the Log Image
            FFTData = HomomorphicFilterObject.ApplyFilterHMMFreqDomain(FFTObject.FFTShifted, rH, rL, Sigma, Slope);
            //Inverse FFT of the COMPLEX Data
            FFTObject = new FFT(FFTData);
            //Removing FFT SHIFT
            FFTObject.FFTShifted = FFTData;
            FFTObject.RemoveFFTShift();
            //Inverse FFT
            FFTObject.InverseFFT(FFTObject.FFTNormal);
            // Inverse Log of Image
            HMMFilteredImage = FFTObject.GreyImage;
            //Copying B Component to Processed Image Array
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    HMMFilteredImageRGB[0, i, j] = HMMFilteredImage[i, j];

                }

            //Reading G  Components
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    ImageData[i, j] = ImageDataRGB[1, i, j];
                }

            //Processing B Comonent of the Image
            //Log of Image 
            //LogImage = HomomorphicFilterObject.LogofImage(ImageData);
            //FFT of the LOG Image
            FFTObject = new FFT(ImageData);
            FFTObject.ForwardFFT();
            //Shifting FFT for Filtering
            FFTObject.FFTShift();
            // Applying Filter on the FFT of the Log Image
            FFTData = HomomorphicFilterObject.ApplyFilterHMMFreqDomain(FFTObject.FFTShifted, rH, rL, Sigma, Slope);
            //Inverse FFT of the COMPLEX Data
            FFTObject = new FFT(FFTData);
            //Removing FFT SHIFT
            FFTObject.FFTShifted = FFTData;
            FFTObject.RemoveFFTShift();
            //Inverse FFT
            FFTObject.InverseFFT(FFTObject.FFTNormal);
            // Inverse Log of Image
            HMMFilteredImage = FFTObject.GreyImage;
            //Copying B Component to Processed Image Array
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    HMMFilteredImageRGB[1, i, j] = HMMFilteredImage[i, j];

                }

            //Reading R  Components
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    ImageData[i, j] = ImageDataRGB[2, i, j];
                }

            //Processing R Comonent of the Image
            //Log of Image 
            //LogImage = HomomorphicFilterObject.LogofImage(ImageData);
            //FFT of the LOG Image
            FFTObject = new FFT(ImageData);
            FFTObject.ForwardFFT();
            //Shifting FFT for Filtering
            FFTObject.FFTShift();
            // Applying Filter on the FFT of the Log Image
            FFTData = HomomorphicFilterObject.ApplyFilterHMMFreqDomain(FFTObject.FFTShifted, rH, rL, Sigma, Slope);
            //Inverse FFT of the COMPLEX Data
            FFTObject = new FFT(FFTData);
            //Removing FFT SHIFT
            FFTObject.FFTShifted = FFTData;
            FFTObject.RemoveFFTShift();
            //Inverse FFT
            FFTObject.InverseFFT(FFTObject.FFTNormal);
            // Inverse Log of Image
            HMMFilteredImage = FFTObject.GreyImage;
            //Copying B Component to Processed Image Array
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    HMMFilteredImageRGB[2, i, j] = HMMFilteredImage[i, j];

                }


            //Display Image
            pictureBox2.Image = HomomorphicFilterObject.Displayimage(HMMFilteredImageRGB);
            filteredimage = new Bitmap (pictureBox2.Image);
            //Displaying Gaussian Kernel Used for Filtering
            double WeightHPF;
            double[,] GaussianKernelHPF = HomomorphicFilterObject.GenerateGaussianKernelHPF(256, Sigma, Slope, out WeightHPF);
            Width = GaussianKernelHPF.GetLength(0);
            Height = GaussianKernelHPF.GetLength(1);
            int[,] GaussianImage = new int[Width, Height];
            for (i = 0; i <= Width - 1; i++)
                for (j = 0; j <= Height - 1; j++)
                {
                    GaussianImage[i, j] = (int)(255 * GaussianKernelHPF[i, j]);
                }
            pictureBox1.Image = HomomorphicFilterObject.DisplayGaussianPlot(GaussianImage);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.setdata(filteredimage);
            fm1.Show();
            this.Hide();
        }
    }
}
