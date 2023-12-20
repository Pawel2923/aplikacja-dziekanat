using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace CustomRenderer
{
    public class Select : Picker
    {
        private bool isValid;

        public class Result
        {
            private bool isValid;
            private string message;

            public Result() { isValid = false; message = ""; }
            public Result(bool isValid, string message) { this.isValid = isValid; this.message = message; }

            public bool IsValid { get { return isValid; } set { isValid = value; } }
            public string Message { get { return message; } set { message = value; } }
        }

        public Select() { isValid = false; }

        public Result CheckValidity(string value)
        {
            Result result = new Result();

            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Pole " + Title + " jest puste");
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
