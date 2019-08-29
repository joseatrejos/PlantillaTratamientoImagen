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

        private void LoadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _ImgInput = new Image<Bgr, byte>(ofd.FileName);
                panAndZoomPictureBox1.Image = _ImgInput.Bitmap;
            }
        }

        private void CannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> _ImgCanny = new Image<Gray, byte>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
            _ImgCanny = _ImgInput.Canny(10, 25);
            panAndZoomPictureBox1.Image = _ImgCanny.Bitmap;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Estás seguro de cerrar?", "Cerrar Ventana", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void SovelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_ImgInput != null)
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                Image<Gray, float> _ImgSobel = new Image<Gray, float>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
                _ImgSobel = _ImgGray.Sobel(1, 1, 1);
                panAndZoomPictureBox1.Image = _ImgSobel.Bitmap;
            }
        }

        private void LaplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_ImgInput != null)
            {
                Image<Gray, byte> _ImgGray = _ImgInput.Convert<Gray, byte>();
                Image<Gray, float> _ImgLaplace = new Image<Gray, float>(_ImgInput.Width, _ImgInput.Height, new Gray(0));
                _ImgLaplace = _ImgGray.Laplace(1);
                panAndZoomPictureBox1.Image = _ImgLaplace.Bitmap;
            }
        }
    }
}
