using CustomRenderer;
using db;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        private Input email;
        private Input password;
        private Input confirmPassword;
        private readonly Select select;
        private readonly DbConnection dbConnection = new DbConnection();

        public SignupPage()
        {
            InitializeComponent();
            classIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            SetSelectItems();
            select = new Select(classIdSelect);
        }

        private async void SetSelectItems()
        {
            classIdSelect.ItemsSource = await dbConnection.GetClassIds();
        }

        private bool CheckForm()
        {
            Input.Result emailResult = email.CheckValidity(true);
            emailLabel.Text = emailResult.Message;
            emailLabel.IsVisible = emailResult.Message.Length > 0;
            Input.Result passwordResult = password.CheckValidity(false, passwordInput.Text == confirmPasswordInput.Text);
            passwordLabel.Text = passwordResult.Message;
            passwordLabel.IsVisible = passwordResult.Message.Length > 0;
            Input.Result confirmPasswordResult = confirmPassword.CheckValidity(false, passwordInput.Text == confirmPasswordInput.Text);
            confirmPasswordLabel.Text = confirmPasswordResult.Message;
            confirmPasswordLabel.IsVisible = confirmPasswordResult.Message.Length > 0;
            Select.Result classIdResult = select.CheckValidity();
            classIdLabel.Text = classIdResult.Message;
            classIdLabel.IsVisible = classIdResult.Message.Length > 0;

            if (emailResult.IsValid && passwordResult.IsValid && confirmPasswordResult.IsValid && classIdResult.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void SignupClickHandler(object sender, EventArgs e)
        {
            email = new Input(emailInput);
            password = new Input(passwordInput);
            confirmPassword = new Input(confirmPasswordInput);

            if (CheckForm())
            {
                IFirebaseAuth auth = DependencyService.Resolve<IFirebaseAuth>();

                try
                {
                    string uid = await auth.RegisterWithEmailAndPassword(email.Value, password.Value);
                    if (uid != null)
                    {
                        var profil = new Profile
                        {
                            Address = "Test"
                        };

                        bool result = await dbConnection.CreateUser(email.Value, false, false, select.Value, profil);
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                    {
                        email.SetMessageLabel(emailLabel, "Wprowadzono niepoprawny email lub hasło");
                    }
                    else if (ex.Message.Contains("email address is already in use"))
                    {
                        email.SetMessageLabel(emailLabel, "Ten email jest już zajęty");
                    }
                    else if (ex.Message.Contains("email address is badly formatted"))
                    {
                        email.SetMessageLabel(emailLabel, "Wprowadzono niepoprawny email");
                    }
                    else if (ex.Message.Contains("Password should be at least"))
                    {
                        Label[] passwordLabels = { passwordLabel, confirmPasswordLabel };
                        email.SetMessageLabel(passwordLabels, "Hasło powinno mieć co najmniej 6 znaków");
                    }
                    else
                    {
                        email.SetMessageLabel(classIdLabel, "Wystąpił problem z logowaniem");
                    }
                }
            }
        }

        async public void LoginClickHandler(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void SelectChangeHandler(object sender, EventArgs e)
        {
            if (classIdSelect.SelectedIndex > -1)
            {
                select.TitleColor = Color.Black;
                select.Value = classIdSelect.Items[classIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                select.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }
    }
}