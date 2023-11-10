using aplikacja_dziekanat.pages;
using System;
using System.Diagnostics;
using System.Net.Mail;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class Input : Entry
    {
        private string value;
        private bool isValid;
        private readonly Input input;

        public class Result
        {
            private bool isValid;
            private string message;

            public Result() { isValid = false; message = ""; }
            public Result(bool isValid, string message) { this.isValid = isValid; this.message = message; }

            public bool IsValid { get { return isValid; } set { isValid = value; } }
            public string Message { get { return message; } set { message = value; } }
        }

        public Input() { value = Text; isValid = false; input = this; }
        public Input(Input input) { value = input.Text; isValid = false; this.input = input; }

        public string Value {  get { return value; } set { this.value = value; } }

        public Result CheckValidity(bool isEmail = false, bool passwordsMatch = true)
        {
            Result result = new Result();

            try
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Pole " + input.Placeholder + " jest puste");
                }

                if (!passwordsMatch)
                {
                    throw new Exception("Hasła nie są takie same");
                }

                if (isEmail)
                {
                    try
                    {
                        MailAddress address = new MailAddress(value);
                        isValid = address.Address == value.Trim();
                    } catch (Exception)
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
