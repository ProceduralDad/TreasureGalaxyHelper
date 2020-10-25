using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace TreasureGalaxyHelper
{
    public class OcrHandler
    {
        private TesseractEngine _engine;

        public OcrHandler()
        {
            _engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        }

        public List<OcrResult> DoOCR(Bitmap bitmap)
        {
            int squareWidth = bitmap.Width / 26;
            List<OcrResult> results = new List<OcrResult>();
            for (int i = 0; i < 26; i++)
            {
                Bitmap onesquare = ((Bitmap)bitmap).Clone(new Rectangle(i * squareWidth, 0, squareWidth, bitmap.Height), PixelFormat.DontCare);
                using(Bitmap largerbmp = new Bitmap(squareWidth* 10, bitmap.Height * 10))
                {
                    Graphics g = Graphics.FromImage(largerbmp);
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(onesquare, squareWidth, squareWidth);
                    string temp = Path.GetTempPath() + "temp.jpg";
                    largerbmp.Save(temp, System.Drawing.Imaging.ImageFormat.Jpeg);
                    using (var page = _engine.Process(Pix.LoadFromFile(temp)))
                    {
                        string text = page.GetText().Replace("\n", "");
                        int myInteger;
                        if (int.TryParse(text, out myInteger))
                        {
                            results.Add(new OcrResult() { index = i, value = myInteger });
                        }
                    }
                }


            }
            return results;
        }
    }
}
