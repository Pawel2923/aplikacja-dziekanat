using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomRenderer;
using Xamarin.Essentials;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage
    {
        private DbConnection connection;
        private Input email;
        private Input password;
        private Input confirmPassword;
        public SignupPage ()
		{
			InitializeComponent ();
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

            if (emailResult.IsValid && passwordResult.IsValid && confirmPasswordResult.IsValid) 
            {
                return true;
            } else
            {
                return false;
            }
        }

        public async void SignupClickHandler(object sender, EventArgs e)
        {
            email = new Input(emailInput);
            password = new Input(passwordInput);
            confirmPassword = new Input(confirmPasswordInput);

            Debug.WriteLine("Form is ok? " + CheckForm());
            if (CheckForm())
            {
                IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

                try
                {
                    string uid = await auth.RegisterWithEmailAndPassword(email.Value, password.Value);
                    if (uid != null)
                    {
                        Debug.WriteLine("Uid: " + uid);
                        connection = new DbConnection();
                        bool result = await connection.CreateUser(email.Value, false, false);
                        Debug.WriteLine("Create user result: " + result);
                        await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                    {
                        confirmPasswordLabel.Text = "Wprowadzono niepoprawny email lub hasło";
                        confirmPasswordLabel.IsVisible = true;
                    }
                    else if (ex.Message.Contains("email address is already in use"))
                    {
                        confirmPasswordLabel.Text = "Ten email jest już zajęty";
                        confirmPasswordLabel.IsVisible = true;
                    }
                    else
                    {
                        confirmPasswordLabel.Text = "Wystąpił problem z logowaniem";
                        confirmPasswordLabel.IsVisible = true;
                    }
                }
            }
        }

        async public void LoginClickHandler (object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}