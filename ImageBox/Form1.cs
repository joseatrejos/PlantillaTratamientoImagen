﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace ImageBox
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> _ImgInput;
        Bitmap image;
        bool show = false;

        public Form1()
        {
            InitializeComponent();
        }

        // -  FILE -
        // Image Load
        private void LoadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Show the Picture in the 1st PanAndZoomPictureBox
                _ImgInput = new Image<Bgr, byte>(ofd.FileName);
                panAndZoomPictureBox1.Image = _ImgInput.Bitmap;
                
                // Clear the rest of the displays
                histogramBox1.ClearHistogram();
                histogramBox1.Refresh();
            }
        }

        // Exit Button
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea cerrar?", "Cerrar Ventana", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        // -  FILE End -

        // - HISTOGRAMS -
        // Green Histogram
        private void GreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                // Inicializar variable para determinar rango de color
                DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
                histcolor.Calculate(new Image<Gray, Byte>[] { _ImgInput[1] }, false, null);

                Mat m = new Mat();
                histcolor.CopyTo(m);

                histogramBox1.ClearHistogram();
                histogramBox1.AddHistogram("Histograma del Color verde", Color.Green, m, 256, new float[] { 0, 256 });
                histogramBox1.Refresh();
            }
        }

        // Red Histogram
        private void RedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                // Inicializar variable para determinar rango de color
                DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
                histcolor.Calculate(new Image<Gray, Byte>[] { _ImgInput[2] }, false, null);

                Mat m = new Mat();
                histcolor.CopyTo(m);

                histogramBox1.ClearHistogram();
                histogramBox1.AddHistogram("Histograma del Color rojo", Color.Red, m, 256, new float[] { 0, 256 });
                histogramBox1.Refresh();
            }
        }

        // Blue Histogram
        private void BlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                // Inicializar variable para determinar rango de color
                DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
                histcolor.Calculate(new Image<Gray, Byte>[] { _ImgInput[0] }, false, null);

                Mat m = new Mat();
                histcolor.CopyTo(m);

                histogramBox1.ClearHistogram();
                histogramBox1.AddHistogram("Histograma del Color azul", Color.Blue, m, 256, new float[] { 0, 256 });
                histogramBox1.Refresh();
            }
        }
        // - HISTOGRAMS End -

        // - BORDER FILTERS -
        // Canny Filter
        private void CannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                Image<Gray, byte> _ImgCanny = new Image<Gray, byte>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
                _ImgCanny = _ImgInput.Canny(10, 25);
                panAndZoomPictureBox2.Image = _ImgCanny.Bitmap;
            }
        }

        // Sobel Filter
        private void SovelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                Image<Gray, float> _ImgSobel = new Image<Gray, float>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
                _ImgSobel = _ImgGray.Sobel(1, 1, 1);
                panAndZoomPictureBox2.Image = _ImgSobel.Bitmap;
            }
        }

        // Laplacian Filter
        private void LaplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                Image<Gray, float> _ImgLaplace = new Image<Gray, float>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
                _ImgLaplace = _ImgGray.Laplace(1);
                panAndZoomPictureBox2.Image = _ImgLaplace.Bitmap;
            }
        }
        // - BORDER FILTERS End -

        // - PROCESSING FILTERS -
        // Range Filter
        private void FiltroPorRangoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                // Make another ImageBox appear to compare the original state with the applied effect
                pictureBox1.Image = _ImgInput.Bitmap;

                // Popup Window
                ParametrosRange fp = new ParametrosRange(this);
                fp.Show();
            }
        }

        // Función propia para aplicar el filtro con los datos de la otra ventana (mandados habiendo heredado este Form1 en el form Parametros)
        public void AplicarRangeFilter(int min, int max)
        {
            try
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>().InRange(new Gray(min), new Gray(max));
                pictureBox2.Image = _ImgGray.Bitmap;
                pictureBox2.Invalidate();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        // Overlay Filter
        private void overlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                pictureBox1.Image = _ImgInput.Bitmap;

                // Make another ImageBox appear to overlay a clone of it with an effect applied
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                Image<Bgr, byte> temp = _ImgInput.Clone();
                temp.SetValue(new Bgr(0, 0, 255), _ImgGray);
                pictureBox2.Image = _ImgGray.Bitmap;

                // Popup Window
                ParametrosOverlay fp = new ParametrosOverlay(this);
                fp.Show();
            }
        }

        // Función propia para aplicar el filtro con los datos de la otra ventana (mandados habiendo heredado este Form1 en el form Parametros)
        public void AplicarOverlay(int rojo, int verde, int azul)
        {
            if(pictureBox1.Image != null)
            {
                try
                {
                    Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                    Image<Bgr, byte> temp = _ImgInput.Clone();
                    temp.SetValue(new Bgr(azul, verde, rojo), _ImgGray);
                    pictureBox2.Image = temp.Bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
            
        }
        // - PROCESSING FILTERS End -

        // - CONTORNOS -
        // Detect
        private void DetectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                // Make another ImageBox appear to compare the original state with the applied effect
                pictureBox1.Image = _ImgInput.Bitmap;

                //Binarizacion
                Image<Gray, byte> _imgOutput = _ImgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(200), new Gray(255));
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                Mat hier = new Mat();

                //Encontrat los contornos en la imagen
                CvInvoke.FindContours(_imgOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                CvInvoke.DrawContours(_imgOutput, contours, -1, new MCvScalar(255, 0, 0));

                pictureBox2.Image = _imgOutput.Bitmap;
            }
        }

        private void DetectarFormasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                try
                {
                    Image<Bgr, byte> _ImgOutput = _ImgInput;

                    // Make another ImageBox appear to compare the original state with the applied effect
                    pictureBox1.Image = _ImgInput.Bitmap;

                    // Suaviza la imagen y aplica binarización, almacenándola en una variable temporal
                    var temp = _ImgInput.SmoothGaussian(5).Convert<Gray, byte>().ThresholdBinaryInv(new Gray(230), new Gray(255));

                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    Mat m = new Mat();

                    // Aplicar función FindContours para detectar los contornos de las figuras
                    CvInvoke.FindContours(temp, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);

                    for (int i = 0; i < contours.Size; i++)
                    {
                        double perimetro = CvInvoke.ArcLength(contours[i], true);
                        VectorOfPoint approx = new VectorOfPoint();

                        // Detección de curvas en función a una precisión indicada eje (0.04)
                        CvInvoke.ApproxPolyDP(contours[1], approx, 0.04 * perimetro, true);
                        CvInvoke.DrawContours(_ImgOutput, contours, i, new MCvScalar(0, 0, 255), 2);

                        // Encontrar centro de la figura
                        var momentos = CvInvoke.Moments(contours[i]);
                        int x = (int)(momentos.M10 / momentos.M00);
                        int y = (int)(momentos.M01 / momentos.M00);

                        // Detección de Triángulos
                        if (approx.Size == 3)
                        {
                            CvInvoke.PutText(_ImgOutput, "Triangulo", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }

                        // Detección de Cuadrados y Rectángulos
                        if (approx.Size == 4)
                        {
                            Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                            double ar = (double)rect.Width / rect.Height;

                            if (ar >= 0.95 && ar <= 1.05)
                            {
                                CvInvoke.PutText(_ImgOutput, "Cuadrado", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                            }
                            else
                            {
                                CvInvoke.PutText(_ImgOutput, "Rectángulo", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                            }
                        }

                        // Detección de Pentágonos
                        if (approx.Size == 5)
                        {
                            CvInvoke.PutText(_ImgOutput, "Hexágono", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }

                        // Detección de Hexágonos
                        if (approx.Size == 6)
                        {
                            CvInvoke.PutText(_ImgOutput, "Hexágono", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }

                        // Detección de Círculos
                        if (approx.Size > 6)
                        {
                            CvInvoke.PutText(_ImgOutput, "Círculo", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }
                    }
                    pictureBox2.Image = _ImgOutput.Bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
            }
        }

        private async void detectarTextoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> imgOutput = _ImgInput.Convert<Gray, byte>().Not().ThresholdBinary(new Gray(50), new Gray(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hier = new Mat();

            CvInvoke.FindContours(imgOutput, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            show = true;
            if (contours.Size > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                    _ImgInput.ROI = rect;

                    image = _ImgInput.Copy().Bitmap;
                    _ImgInput.ROI = Rectangle.Empty;
                    this.Invalidate();

                    await Task.Delay(500);
                }
                show = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (show == true)
            {
                pictureBox2.Image = image;
            }
        }
    }
}
