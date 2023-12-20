using System;
using System.Diagnostics;
using System.Net.Mail;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class Input : Entry
    {
        private bool isValid = false;

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

        public Result CheckValidity(bool isEmail = false, bool passwordsMatch = true)
        {
            Result result = new Result();

            try
            {
                if (string.IsNullOrEmpty(Text))
                {
                    throw new Exception("Pole " + Placeholder + " jest puste");
                }

                if (!passwordsMatch)
                {
                    throw new Exception("Hasła nie są takie same");
                }

                if (isEmail)
                {
                    try
                    {
                        MailAddress address = new MailAddress(Text);
                        isValid = address.Address == Text.Trim();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Podany adres email jest niepoprawny");
                    }
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Message = ex.Message;
                Debug.WriteLine(ex);
            }

            result.IsValid = isValid;

            return result;
        }
    }
}
