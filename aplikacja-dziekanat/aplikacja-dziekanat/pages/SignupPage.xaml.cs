using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomRenderer;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage
	{
		public SignupPage ()
		{
			InitializeComponent ();
		}

		private bool CheckForm()
		{
            Input email = new Input(emailInput);
            Input password = new Input(passwordInput);
            Input confirmPassword = new Input(confirmPasswordInput);

            Input.Result emailResult = email.CheckValidity(true);
            emailLabel.Text = emailResult.Message;
            emailLabel.IsVisible = emailResult.Message.Length > 0;
            Input.Result passwordResult = password.CheckValidity(false, passwordInput.Text == confirmPasswordInput.Text);
            passwordLabel.Text = passwordResult.Message;
            passwordLabel.IsVisible = passwordResult.Message.Length > 0;
            Input.Result confirmPasswordResult = confirmPassword.CheckValidity(false, passwordInput.Text == confirmPasswordInput.Text);
            confirmPasswordLabel.Text = confirmPasswordResult.Message;
            confirmPasswordLabel.IsVisible = confirmPasswordResult.Message.Length > 0;

            if (emailResult.IsValid && passwordResult.IsValid && confirmPasswordResult.IsValid) 
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void SignupClickHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(CheckForm());
        }

        async public void LoginClickHandler (object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}