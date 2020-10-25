using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreasureGalaxyHelper
{
    public class BoxHandler
    {
        private Dictionary<int,TextBox> _boxes;
        private TextBox _errorText;

        public BoxHandler(TextBox[] boxes, TextBox error)
        {
            _boxes = new Dictionary<int, TextBox>();
            for(int i = 0; i<26; i++)
            {
                _boxes.Add(i, boxes[i]);
            }
            _errorText = error;
        }

        public void Init()
        {
            foreach(TextBox b in _boxes.Values)
            {
                b.Font = new System.Drawing.Font("Arial Bold", 16);
                b.LostFocus += LostFocusHandler;
            }
        }

        private void LostFocusHandler(object sender, EventArgs e)
        {
            DoMathAndFillBoxes();
        }

        private void DoMathAndFillBoxes()
        {
            try
            {
                _errorText.Visible = false;
                LostFocusLogic();
            }
            catch (System.Exception ex)
            {
                _errorText.Visible = true;
                _errorText.Text = ex.Message;
                ClearAll();
            }
        }

        private void LostFocusLogic()
        {
            Dictionary<int, TextBox> filled = FindFilled();
            if (filled.Count == 2)
            {
                int firstIndex = filled.FirstOrDefault().Key;
                int secondIndex = filled.Last().Key;
                int firstValue;
                int secondValue;
                try
                {
                    firstValue = int.Parse(filled.FirstOrDefault().Value.Text);
                    secondValue = int.Parse(filled.Last().Value.Text);
                }
                catch(System.Exception ex)
                {
                    throw new ApplicationException("Input is not numerical, try again.");
                }

                decimal spacingSizeD = (decimal)(secondValue - firstValue) / (secondIndex - firstIndex);
                if(spacingSizeD != Math.Round(spacingSizeD))
                {
                    throw new ApplicationException("Non-integer spacing detected, try again.");
                }
                if(spacingSizeD == 0)
                {
                    throw new ApplicationException("That's not a valid code, try again.");
                }
                int theValueOfLetterA = firstValue - (firstIndex * (int)spacingSizeD);
                SetAllBoxes(theValueOfLetterA, (int)spacingSizeD);
            }
            else if (filled.Count > 2)
            {
                ClearAll();
            }
        }

        private void SetAllBoxes(int theValueOfLetterA, int spacingSize)
        {
            for (int i = 0; i < 26; i++)
            {
                _boxes[i].Text = (theValueOfLetterA + (i * spacingSize)).ToString();
            }
        }

        public void ClearAll()
        {
            foreach(TextBox b in _boxes.Values)
            {
                b.Clear();
            }
        }

        internal void AcceptOcrResults(List<OcrResult> results)
        {
            ClearAll();
            for(int i = 0; i < 2; i++)
            {
                _boxes[results[i].index].Text = results[i].value.ToString();
            }
            LostFocusLogic();
        }

        public Dictionary<int, TextBox> FindFilled()
        {
            Dictionary<int, TextBox> dict = new Dictionary<int, TextBox>();
            foreach(KeyValuePair<int, TextBox> b in _boxes)
            {
                if(!String.IsNullOrWhiteSpace(b.Value.Text))
                {
                    dict.Add(b.Key, b.Value);
                }
            }
            return dict;
        }
    }
}
