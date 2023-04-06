using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyImgaesViewer
{
    public partial class FrmImageViewer : Form
    {
        public FrmImageViewer(string imgPath)
        {
            InitializeComponent();
            this.pictureBox1.Image = new Bitmap(imgPath);
        }
    }
}
