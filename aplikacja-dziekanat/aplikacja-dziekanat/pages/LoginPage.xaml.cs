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
        private readonly IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

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
                    string uid = await auth.LoginWithEmailAndPassword(email.Value, password.Value);
                    if (uid != null)
                    {
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                    {
                        passwordLabel.Text = "Wprowadzono niepoprawny email lub hasło";
                        passwordLabel.IsVisible = true;
                    }
                    else
                    {
                        passwordLabel.Text = "Wystąpił problem z logowaniem";
                        passwordLabel.IsVisible = true;
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

            if (auth.Uid() != null)
            {
                await Navigation.PopAsync();
            }
        }
    }
}