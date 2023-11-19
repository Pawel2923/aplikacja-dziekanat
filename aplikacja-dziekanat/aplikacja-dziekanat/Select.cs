using System;
using System.Diagnostics;
using System.Net.Mail;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class Select : Picker
    {
        private string value;
        private bool isValid;
        private readonly Select select;

        public class Result
        {
            private bool isValid;
            private string message;

            public Result() { isValid = false; message = ""; }
            public Result(bool isValid, string message) { this.isValid = isValid; this.message = message; }

            public bool IsValid { get { return isValid; } set { isValid = value; } }
            public string Message { get { return message; } set { message = value; } }
        }

        public Select() { value = SelectedIndex >= 0 ? Items[SelectedIndex] : null; isValid = false; select = this; }
        public Select(Select select) { value = SelectedIndex >= 0 ? select.Items[select.SelectedIndex] : null; isValid = false; this.select = select; }

        public string Value { get { return value; } set { this.value = value; } }

        public Result CheckValidity()
        {
            Result result = new Result();

            try
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Pole " + select.Title + " jest puste");
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
