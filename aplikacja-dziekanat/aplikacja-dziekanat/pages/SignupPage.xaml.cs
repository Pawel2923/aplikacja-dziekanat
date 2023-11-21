using CustomRenderer;
using db;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        private DbConnection connection;
        private Input email;
        private Input password;
        private Input confirmPassword;
        private Select select;
        public SignupPage()
        {
            InitializeComponent();
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
            select = new Select(classIdSelect);

            if (CheckForm())
            {
                IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

                try
                {
                    string uid = await auth.RegisterWithEmailAndPassword(email.Value, password.Value);
                    if (uid != null)
                    {
                        connection = new DbConnection();
                        bool result = await connection.CreateUser(email.Value, false, false);
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                    {
                        classIdLabel.Text = "Wprowadzono niepoprawny email lub hasło";
                        classIdLabel.IsVisible = true;
                    }
                    else if (ex.Message.Contains("email address is already in use"))
                    {
                        classIdLabel.Text = "Ten email jest już zajęty";
                        classIdLabel.IsVisible = true;
                    }
                    else
                    {
                        classIdLabel.Text = "Wystąpił problem z logowaniem";
                        classIdLabel.IsVisible = true;
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
                classIdSelect.Value = classIdSelect.Items[classIdSelect.SelectedIndex];
                Debug.WriteLine(classIdSelect.Value);
            }
            else
            {
                classIdSelect.TitleColor = Color.FromHex("#575757");
            }
        }
    }
}