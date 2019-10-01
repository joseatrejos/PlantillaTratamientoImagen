using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageBox
{
    public partial class PopupVideo : Form
    {
        Form1 form;
        public PopupVideo(Form1 f)
        {
            InitializeComponent();
            form = f;
        }

        // Close Popup Window
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
