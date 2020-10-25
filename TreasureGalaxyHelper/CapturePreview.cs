using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreasureGalaxyHelper
{
    public partial class CapturePreview : Form
    {
        private Bitmap _b;
        public CapturePreview(Bitmap b)
        {
            InitializeComponent();
            this.pictureBox1.Image = b;
            _b = b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _b.Save("C:\\TGXHelper\\Capture.bmp", ImageFormat.Bmp);
        }
    }
}
