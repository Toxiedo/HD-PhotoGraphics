using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using Fast_Fourier_Transform;


namespace Homomorphic_Filter
{
    public static class HomomorphicFilterObject
    {
        public static int[,] ReadImage(Bitmap ImageData)
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
            return GreyImage ;
        }

        public static int[,,] ReadImageRGB(Bitmap ImageData)
        {
            int i, j, Width, Height;

            Width = ImageData.Width;
            Height = ImageData.Height;

            int[,,] GreyImage = new int[3,Width, Height];  //[Colour, Row,Column]

            BitmapData bitmapData1 = ImageData.LockBits(new Rectangle(0, 0, ImageData.Width, ImageData.Height),
                                     ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;

                for (i = 0; i < bitmapData1.Height; i++)
                {
                    for (j = 0; j < bitmapData1.Width; j++)
                    {
                        GreyImage[0, j, i] = (int) imagePointer1[0];
                        GreyImage[1, j, i] = (int) imagePointer1[1];
                        GreyImage[2, j, i] = (int) imagePointer1[2];

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

        public static Bitmap Displayimage(int[,] image)
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
        
        public static Bitmap Displayimage(int[,,] image)
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
                        imagePointer1[0] = (byte)image[0,j, i];
                        imagePointer1[1] = (byte)image[1,j, i];
                        imagePointer1[2] = (byte)image[2,j, i];
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
        
        public static Bitmap DisplayGaussianPlot(int[,] image)
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
                                           

                       
                            imagePointer1[0] = 0;
                            imagePointer1[1] = (byte)image[j, i];
                            imagePointer1[2] = 0;
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
               
        public static double[,] GenerateGaussianKernel(int N, float Sigma, float Slope, out double Weight)
        {
            float pi;
            pi = (float)Math.PI;
            int i, j;
            int SizeofKernel = N;
            double[,] GaussianKernel = new double[N, N]; ;
            float[,] Kernel = new float[N, N];

            float[,] OP = new float[N, N];
            float D1, D2;

            D1 = 1 / (2 * pi * Sigma * Sigma);
            D2 = 2 * Sigma * Sigma;

            float min = 1000, max = 0;

            for (i = -SizeofKernel / 2; i <= SizeofKernel / 2 - 1; i++)
            {
                for (j = -SizeofKernel / 2; j <= SizeofKernel / 2 - 1; j++)
                {
                    Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] = ((1 / D1) * (float)Math.Exp(-Slope*(i * i + j * j) / D2));

                    if (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] < min)
                        min = Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];
                    if (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] > max)
                        max = Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];

                }
            }
            //Converting to the scale of 0-1
            double sum = 0;
            for (i = -SizeofKernel / 2; i <= SizeofKernel / 2 - 1; i++)
            {
                for (j = -SizeofKernel / 2; j <= SizeofKernel / 2 - 1; j++)
                {
                    GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] = (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] - min) / (max - min);
                    sum = sum + GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];
                }

            }
            //Normalizing kernel Weight
            Weight = sum;

            return GaussianKernel;
        }

         /// <summary>
        /// Generates Gaussian Filter Kernel
        /// </summary>
        /// <param name="N">Size of the Filter</param>
        /// <param name="Sigma">Spread of the Gaussian</param>
        /// <param name="Slope">Harpness of the Slope of the Gaussian</param>
        /// <param name="Weight">Weight of the Filter Kernel (Out Variable)</param>
        /// <returns>GAussian Kernel</returns>
        public static double[,] GenerateGaussianKernelHPF(int N, float Sigma, float Slope, out double Weight)
        {
            float pi;
            pi = (float)Math.PI;
            int i, j;
            int SizeofKernel = N;
            double[,] GaussianKernel = new double[N, N]; ;
            float[,] Kernel = new float[N, N];

            float[,] OP = new float[N, N];
            float D1, D2;
                      

            D1 = 1 / (2 * pi * Sigma * Sigma);
            D2 = 2 * Sigma * Sigma;

            float min = 1000, max = 0;

            for (i = -SizeofKernel / 2; i <= SizeofKernel / 2 - 1; i++)
            {
                for (j = -SizeofKernel / 2; j <= SizeofKernel / 2 - 1; j++)
                {
                    Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] = ((1 / D1) * (float)Math.Exp(-Slope*(i * i + j * j) / D2));

                    if (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] < min)
                        min = Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];
                    if (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] > max)
                        max = Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];

                }
            }
            //Converting to the scale of 0-1
            double sum = 0;
            for (i = -SizeofKernel / 2; i <= SizeofKernel / 2 - 1; i++)
            {
                for (j = -SizeofKernel / 2; j <= SizeofKernel / 2 - 1; j++)
                {
                    GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] = (Kernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] - min) / (max - min);

                    GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j] = 1 - GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];

                    sum = sum + GaussianKernel[SizeofKernel / 2 + i, SizeofKernel / 2 + j];
                }

            }
            //Normalizing kernel Weight
            Weight = sum;

            return GaussianKernel;
        }
              
        /// <summary>
        /// Applies Gaussian Filter on the Image Data
        /// </summary>
        /// <param name="FFTData">FFT of the Image</param>
        /// <param name="rL">Lower Homomrphic Threshold</param>
        /// <param name="rH">Upper Homomrphic Threshold</param>
        /// <param name="Sigma"> Spread of the Gaussian</param>
        /// <param name="Slope">Slope of the Sharpness of the Gaussian Filter</param>
        /// <returns></returns>
        
        public static COMPLEX[,] ApplyFilterHMMFreqDomain(COMPLEX[,] FFTData,float rH, float rL, float Sigma, float Slope)
        {
            COMPLEX[,] Output = new COMPLEX[FFTData.GetLength(0), FFTData.GetLength(1)];
            int i, j, W, H;
  
            W = FFTData.GetLength(0);
            H = FFTData.GetLength(1);

            double Weight;
            //Taking FFT of Gaussian HPF
            double[,] GaussianHPF = GenerateGaussianKernelHPF(FFTData.GetLength(0), Sigma, Slope, out Weight);
            
            //Variables for FFT of Gaussian Filter
            COMPLEX[,] GaussianHPFFFT;
           // FFT GaussianFFTObject;

            for (i = 0; i <= GaussianHPF.GetLength(0) - 1; i++)
                for (j = 0; j <= GaussianHPF.GetLength(1) - 1; j++)
                {
                    GaussianHPF[i, j] = GaussianHPF[i, j];// / Weight;
                }

            FFT GaussianFFTObject = new FFT(GaussianHPF);
            GaussianFFTObject.ForwardFFT(GaussianHPF);
            //Shifting FFT for Filtering
            GaussianFFTObject.FFTShift();
        

            GaussianHPFFFT = GaussianFFTObject.FFTShifted;
            for (i = 0; i <= GaussianHPF.GetLength(0) - 1; i++)
                for (j = 0; j <= GaussianHPF.GetLength(1) - 1; j++)
                {
                    GaussianHPFFFT[i, j].real = (rH - rL) * GaussianHPFFFT[i, j].real + rL;
                    GaussianHPFFFT[i, j].imag = (rH - rL) * GaussianHPFFFT[i, j].imag + rL;
                    

                }

            // Applying Filter on the FFT of the Log Image by Multiplying in Frequency Domain
            Output = MultiplyFFTMatrices(GaussianHPFFFT, FFTData);
                               
           
            return Output;
        }

        public static double[,] LogofImage(int[,] ImageData)
        {
            int i,j,W,H;

            W=ImageData.GetLength(0);
            H=ImageData.GetLength(1);
            double[,] ImageLogOutput = new double[W, H];
            for (i = 0; i <= W - 1; i++)
               for (j = 0; j <= H - 1; j++)
                {
                    ImageLogOutput[i, j] = Math.Log(ImageData[i, j] +1);
                }


            return (ImageLogOutput);

        }

        public static int[,] InverseLogofImage(double[,] Data)
        {
            int i, j, W, H;

            W = Data.GetLength(0);
            H = Data.GetLength(1);

            int[,] ImageData = new int[W, H];
             
            for (i = 0; i <= W - 1; i++)
                for (j = 0; j <= H - 1; j++)
                {
                    
                    ImageData[i, j] = (int) Math.Exp (Data[i, j]);
                }
            

               return ImageData;


        }

        public static int[,] InverseLogofImage(COMPLEX[,] ImageData)
        {

            int i, j, W, H;

            W = ImageData.GetLength(0);
            H = ImageData.GetLength(1);

            int[,] Output = new int[W, H];

            for (i = 0; i <= W - 1; i++)
                for (j = 0; j <= H - 1; j++)
                {
                    Output[i, j] = (int) Math.Exp(ImageData[i,j].Magnitude());
                }


            return Output;
            
            
            
        }

        public static COMPLEX[,] MultiplyFFTMatrices(COMPLEX [,] A, COMPLEX [,] B)
        {
            
            int i, j;
            int Width, Height;

            double a, b, c, d;

            Width = A.GetLength(0);
            Height = A.GetLength(1);
            COMPLEX[,] Output = new COMPLEX[Width, Height];
            for (i = 0; i <= Width - 1; i++)
             for (j = 0; j <= Height - 1; j++)
                  {
                      a = A[i, j].real;
                      b = A[i, j].imag;
                      c = B[i, j].real;
                      d = B[i, j].imag;

                      Output[i, j].real = (a * c - b * d);
                      Output[i, j].imag = (a * d + b * c);

                   }


            return Output;


        }
    }
}
