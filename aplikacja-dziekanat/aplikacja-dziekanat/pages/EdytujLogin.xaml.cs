using CustomRenderer;
using db;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdytujLogin : ContentPage
    {
        private string _email;
        private string _password;
        private string _confirmPassword;

        public string Email { get { return _email; } set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get { return _password; } set { _password = value; RaisePropertyChanged(nameof(Password)); } }
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; RaisePropertyChanged(nameof(ConfirmPassword)); } }

        public EdytujLogin()
        {
            InitializeComponent();
            ReauthenticateUser();
            ResetForm();
            BindingContext = this;
        }

        private async void ReauthenticateUser()
        {
            DebugService.WriteLine("Przeprowadzanie reautentykacji");
            var storageService = DependencyService.Get<ISecureStorageService>();
            try
            {
                var fingerprintManager = DependencyService.Get<IFingerprintManager>();
                var auth = DependencyService.Get<IFirebaseAuth>();
                var fingerprint = await storageService.Load($"fingerprint_{auth.CurrentUser.Uid}");

                if (fingerprintManager.IsFingerprintAvailable() && fingerprint == "true")
                {
                    fingerprintManager.AuthenticateFingerprint(() =>
                    {
                        DebugService.WriteLine("Reautentykacja zakończona powodzeniem");
                    }, async () => {
                        await Navigation.PopAsync();
                    });
                }
                else
                {
                    auth.ShowPasswordPrompt(async () => {
                        await Navigation.PopAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                DebugService.WriteLine(ex.Message, "Wystąpił błąd");
            }
        }
        private void ResetForm()
        {
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            Email = auth.CurrentUser.Email;
            emailLabel.Text = "";
            emailLabel.IsVisible = false;

            Password = "";
            passwordLabel.Text = "";
            passwordLabel.IsVisible = false;

            ConfirmPassword = "";
            confirmPasswordLabel.Text = "";
            confirmPasswordLabel.IsVisible = false;
        }

        private bool CheckForm()
        {
            Input.Result emailResult = email.CheckValidity(true);
            emailLabel.Text = emailResult.Message;
            emailLabel.IsVisible = emailResult.Message.Length > 0;
            Input.Result passwordResult = password.CheckValidity(false, Password == ConfirmPassword);
            passwordLabel.Text = passwordResult.Message;
            passwordLabel.IsVisible = passwordResult.Message.Length > 0;

            Input.Result confirmPasswordResult = confirmPassword.CheckValidity(false, Password == ConfirmPassword);
            confirmPasswordLabel.Text = confirmPasswordResult.Message;
            confirmPasswordLabel.IsVisible = confirmPasswordResult.Message.Length > 0;

            if (emailResult.IsValid || (passwordResult.IsValid && confirmPasswordResult.IsValid))
            {
                emailLabel.IsVisible = false;
                passwordLabel.IsVisible = false;
                confirmPasswordLabel.IsVisible = false;

                return true;
            }
            else
            {
                return false;
            }
        }

        private async void SaveHandler(string actionType)
        {
            try
            {
                if (CheckForm())
                {
                    // Zapytaj użytkownika czy jest pewien zapisania zmian
                    bool isUserSure = await DisplayAlert("Zapisz zmiany", "Czy jesteś pewien, że chcesz zapisać zmiany?", "Tak", "Nie");

                    if (!isUserSure)
                    {
                        return;
                    }

                    var auth = DependencyService.Resolve<IFirebaseAuth>();
                    var dbConnection = new DbConnection();

                    if (actionType == "email")
                    {
                        try
                        {
                            // Verify the new email
                            await auth.VerifyBeforeUpdateEmail(Email);

                            // Add alert to inform user that he needs to verify new email
                            await DisplayAlert("Potwierdź nowy email", "Na nowy adres email został wysłany link weryfikacyjny. Po jego kliknięciu zmiana adresu email zostanie zapisana.", "OK");

                            // change email
                            bool isSuccess = await dbConnection.ChangeEmail(Email);

                            if (!isSuccess)
                            {
                                throw new Exception("Nie udało się zmienić adresu email");
                            }

                            // reset form and logout
                            ResetForm();
                            await new Task(() => auth.Logout());
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            if (ex.Message.Contains("Please verify the new email before changing email"))
                            {
                                await DisplayAlert("Błąd", "Nie zweryfikowano nowego adresu email. Spróbuj ponownie", "OK");
                            }
                            else if (ex.Message.Contains("INVALID_NEW_EMAIL"))
                            {
                                await DisplayAlert("Błąd", "Wprowadzono niepoprawny adres email", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Błąd", ex.Message, "OK");
                            }
                        }
                    }
                    else if (actionType == "password")
                    {
                        auth.ChangeUserPassword(Password);

                        await DisplayAlert("Sukces", "Zapisano zmiany", "OK");

                        ResetForm();
                        auth.Logout();
                    }
                    else
                    {
                        throw new Exception("Nieznany typ akcji");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        private void OnSaveEmailBtnClicked(object sender, EventArgs e)
        {
            SaveHandler("email");
        }

        private void OnSavePasswordBtnClicked(object sender, EventArgs e)
        {
            SaveHandler("password");
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}