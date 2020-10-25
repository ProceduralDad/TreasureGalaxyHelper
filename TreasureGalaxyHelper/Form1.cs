using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreasureGalaxyHelper
{
    public partial class Form1 : Form
    {
        private BoxHandler _boxHandler;
        private OcrHandler _ocr;
        private bool _done = false;

        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.Image = Image.FromFile("Images/Code.png");
            TextBox[] boxes = {this.textBox1,this.textBox2,this.textBox3,this.textBox4,
                    this.textBox5,this.textBox6,this.textBox7,this.textBox8,
                    this.textBox9,this.textBox10,this.textBox11,this.textBox12,
                    this.textBox13,this.textBox14,this.textBox15,this.textBox16,
                    this.textBox17,this.textBox18,this.textBox19,this.textBox20,
                    this.textBox21,this.textBox22,this.textBox23,this.textBox24,
                    this.textBox25,this.textBox26 };

            txtError.ReadOnly = true;
            txtError.Font = new Font("Arial", 12, FontStyle.Bold);
            txtError.ForeColor = Color.Red;
            txtError.Visible = false;

            _boxHandler = new BoxHandler(boxes, txtError);
            _boxHandler.Init();

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Preview Capture
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CapturePreview preview = new CapturePreview(GetChosenBitmap());
                preview.Show();
            }
            catch(Exception ex)
            {
                txtError.Visible = true;
                txtError.Text = ex.Message;
            }

        }

        private Bitmap GetChosenBitmap()
        {
            int x = int.Parse(txtX.Text);
            int y = int.Parse(txtY.Text);
            int width = int.Parse(txtWidth.Text);
            int height = int.Parse(txtHeight.Text);
            int screenNumber = int.Parse(txtScreenNum.Text);

            Rectangle captureRectangle = Screen.AllScreens[screenNumber].Bounds;
            Bitmap captureBitmap = new Bitmap(captureRectangle.Width, captureRectangle.Height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            return captureBitmap.Clone(new Rectangle(x, y, width, height), PixelFormat.Format32bppArgb);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _ocr = null;
            try
            {
                _ocr = new OcrHandler();
                DoOCR();
            }
            catch(Exception ex)
            {
                txtError.Visible = true;
                txtError.Text = "OCR ENGINE INIT FAILED: " + ex.Message;
            }
        }

        private void DoOCR()
        {
            int i = 1;
            while (true && _ocr != null && !_done) //Hmmm... should spin off a thread or something like a competent programmer.
            {
                try
                {
                   
                    Bitmap b = GetChosenBitmap();
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    List<OcrResult> results = _ocr.DoOCR(b);
                    s.Stop();
                    if (results.Count >= 2)
                    {
                        txtError.Visible = true;
                        txtError.Text = $"RUN {i}: OCR Found Results in {s.ElapsedMilliseconds} milliseconds";
                        _boxHandler.AcceptOcrResults(results);
                    }
                    else
                    {
                        txtError.Visible = true;
                        txtError.Text = $"RUN {i}:OCR Ran but did not find results in {s.ElapsedMilliseconds} milliseconds";
                    }
                    Task.Delay(5000).Wait();
                }
                catch (Exception ex)
                {
                    txtError.Visible = true;
                    txtError.Text = "OCR FAILED: " + ex.Message;
                }
                i++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _done = true;
        }
    }
}
