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
        Image<Bgr, byte> _InputColor;
        Image<Gray, byte> _InputGray;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string imagen = "C:\\Users\\josea\\source\\repos\\PlantillaTratamientoImagen\\ImageBox\\img\\lena.jpg";
            _InputColor = new Image<Bgr, byte>(imagen);

            if (_InputColor == null)
            {
                MessageBox.Show("No se cargo la imagen");
                return;
            }

            imageBox1.Image = _InputColor;
            // Desactivar click derecho en el programa corriendo.
            imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // Convertir escala de grises.
            _InputGray = _InputColor.Convert<Gray, byte>();
            imageBox2.Image = _InputGray;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //inicializar variable para determinar rango de color
            DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
            histcolor.Calculate(new Image<Gray, Byte>[] { _InputColor[0] }, false, null);

            Mat m = new Mat();
            histcolor.CopyTo(m);

            histogramBox1.AddHistogram("Histograma del Color azul", Color.Blue, m, 256, new float[] { 0, 256 });
            histogramBox1.Refresh();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //inicializar variable para determinar rango de color
            DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
            histcolor.Calculate(new Image<Gray, Byte>[] { _InputColor[2] }, false, null);

            Mat m = new Mat();
            histcolor.CopyTo(m);

            histogramBox2.AddHistogram("Histograma del Color rojo", Color.Red, m, 256, new float[] { 0, 256 });
            histogramBox2.Refresh();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //inicializar variable para determinar rango de color
            DenseHistogram histcolor = new DenseHistogram(256, new RangeF(0, 255));
            histcolor.Calculate(new Image<Gray, Byte>[] { _InputColor[1] }, false, null);

            Mat m = new Mat();
            histcolor.CopyTo(m);

            histogramBox3.AddHistogram("Histograma del Color verde", Color.Green, m, 256, new float[] { 0, 256 });
            histogramBox3.Refresh();
        }
    }
}
