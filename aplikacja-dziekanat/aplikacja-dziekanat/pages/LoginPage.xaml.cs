using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomRenderer;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void ResetForm()
        {
            emailInput.Text = "";
            emailLabel.Text = "";
            emailLabel.IsVisible = false;

            passwordInput.Text = "";
            passwordLabel.Text = "";
            passwordLabel.IsVisible = false;
        }

        private bool CheckForm()
        {
            Input email = new Input(emailInput);
            Input password = new Input(passwordInput);

            Input.Result emailResult = email.CheckValidity(true);
            emailLabel.Text = emailResult.Message;
            emailLabel.IsVisible = emailResult.Message.Length > 0;
            Input.Result passwordResult = password.CheckValidity();
            passwordLabel.Text = passwordResult.Message;
            passwordLabel.IsVisible = passwordResult.Message.Length > 0;

            if (emailResult.IsValid && passwordResult.IsValid)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void LoginClickHandler(object sender, EventArgs e)
        {
            Debug.WriteLine(CheckForm());
        }

        async public void SignupClickHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
            ResetForm();
        }
    }
}