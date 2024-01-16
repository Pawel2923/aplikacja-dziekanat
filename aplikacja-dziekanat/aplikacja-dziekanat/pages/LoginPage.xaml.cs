using CustomRenderer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        private string _email;
        private string _password;

        public string Email { get { return _email; } set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get { return _password; } set { _password = value; RaisePropertyChanged(nameof(Password)); } }

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void ResetForm()
        {
            Email = "";
            emailLabel.Text = "";
            emailLabel.IsVisible = false;

            Password = "";
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
            if (CheckForm())
            {
                try
                {
                    var auth = DependencyService.Resolve<IFirebaseAuth>();
                    string token = await auth.LoginWithEmailAndPassword(Email, Password);
                    if (token != null)
                    {
                        ResetForm();
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

        private void FingerprintClickHandler(object sender, EventArgs e)
        {
            var fingerprintManager = DependencyService.Resolve<IFingerprintManager>();
            
            try
            {
                if (fingerprintManager.IsFingerprintAvailable())
                {
                    fingerprintManager.AuthenticateFingerprint();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (ex.Message.ToLower().Contains("nie znaleziono czytnika linii papilarnych"))
                {
                    DisplayAlert("Błąd", "Nie znaleziono czytnika linii papilarnych", "OK");
                }
                else if (ex.Message.ToLower().Contains("nie ustawiono blokady ekranu"))
                {
                    DisplayAlert("Błąd", "Nie ustawiono blokady ekranu", "OK");
                }
                else
                {
                    DisplayAlert("Błąd", "Wystąpił błąd podczas logowania", "OK");
                }
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.Token() != null)
            {
                await Navigation.PushAsync(new FormsTabPage());
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}