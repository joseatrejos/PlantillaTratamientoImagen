using System;
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

namespace ImageBox
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> _ImgInput;

        public Form1()
        {
            InitializeComponent();
        }

        // Useless
        private void PanAndZoomPictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void FiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // Useless End

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
        public void AplicarOverlay(int min, int max)
        {
            try
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>().InRange(new Gray(min), new Gray(max));
                pictureBox2.Image = _ImgGray.Bitmap;
                pictureBox2.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
        // - PROCESSING FILTERS End -
    }
}
