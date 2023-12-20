using CustomRenderer;
using db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage, INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _selectedClassId;

        public string Email { get { return _email; } set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get { return _password; } set { _password = value; RaisePropertyChanged(nameof(Password)); } }
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; RaisePropertyChanged(nameof(ConfirmPassword)); } }
        public string SelectedClassId { get { return _selectedClassId; } set { _selectedClassId = value; RaisePropertyChanged(nameof(ClassId)); } }

        public SignupPage()
        {
            InitializeComponent();
            BindingContext = this;
            classIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            SetSelectItems();
        }

        private async void SetSelectItems()
        {
            DbConnection dbConnection = new DbConnection();
            classIdSelect.ItemsSource = await dbConnection.GetClassIds();
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

            Select.Result classIdResult = classIdSelect.CheckValidity(SelectedClassId);
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
            if (CheckForm())
            {
                try
                {
                    IFirebaseAuth auth = DependencyService.Resolve<IFirebaseAuth>();
                    string token = await auth.RegisterWithEmailAndPassword(Email, Password, new User
                    {
                        Email = Email,
                        Role = "student",
                        ClassId = SelectedClassId,
                        Profile = new Profile()
                    });
                    if (token != null)
                    {
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
                        email.SetMessageLabel(classIdLabel, "Wystąpił problem z rejestracją");
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
                classIdSelect.TitleColor = Color.Black;
                SelectedClassId = classIdSelect.Items[classIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                classIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}