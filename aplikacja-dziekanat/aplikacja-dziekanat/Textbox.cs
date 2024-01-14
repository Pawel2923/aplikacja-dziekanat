using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class Textbox : Editor
    {
        public class Result
        {
            private bool isValid;
            private string message;

            public Result() { isValid = false; message = ""; }
            public Result(bool isValid, string message) { this.isValid = isValid; this.message = message; }

            public bool IsValid { get { return isValid; } set { isValid = value; } }
            public string Message { get { return message; } set { message = value; } }
        }

        public void SetMessageLabel(Label label, string message)
        {
            label.Text = message;
            label.IsVisible = true;
        }

        public void SetMessageLabel(Label[] label, string message)
        {
            foreach (Label l in label)
            {
                l.Text = message;
                l.IsVisible = true;
            }
        }

        public Result CheckValidity()
        {
            Result result = new Result();

            try
            {
                if (string.IsNullOrEmpty(Text))
                {
                    throw new Exception("Pole " + Placeholder + " jest puste");
                }

                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Message = ex.Message;
                Debug.WriteLine(ex);
            }

            return result;
        }
    }
}
