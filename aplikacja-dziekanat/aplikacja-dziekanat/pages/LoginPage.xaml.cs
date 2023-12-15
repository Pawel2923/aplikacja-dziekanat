using CustomRenderer;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        private Input email;
        private Input password;

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
            Input.Result emailResult = email.CheckValidity(true);
            emailLabel.Text = emailResult.Message;
            emailLabel.IsVisible = emailResult.Message.Length > 0;
            Input.Result passwordResult = password.CheckValidity();
            passwordLabel.Text = passwordResult.Message;
            passwordLabel.IsVisible = passwordResult.Message.Length > 0;

            if (emailResult.IsValid && passwordResult.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void LoginClickHandler(object sender, EventArgs e)
        {
            email = new Input(emailInput);
            password = new Input(passwordInput);

            if (CheckForm())
            {
                try
                {
                    var auth = DependencyService.Resolve<IFirebaseAuth>();
                    string token = await auth.LoginWithEmailAndPassword(email.Value, password.Value);
                    if (token != null)
                    {
                        await Navigation.PushAsync(new FormsTabPage());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                    {
                        password.SetMessageLabel(passwordLabel, "Wprowadzono niepoprawny email lub hasło");
                    }
                    else
                    {
                        password.SetMessageLabel(passwordLabel, "Wystąpił błąd podczas logowania");
                    }
                }
            }
        }

        async public void SignupClickHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
            ResetForm();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.CurrentUser.Uid != null)
            {
                await Navigation.PushAsync(new FormsTabPage());
            }
        }
    }
}