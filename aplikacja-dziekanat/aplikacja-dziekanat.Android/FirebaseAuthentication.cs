using Android.App;
using Android.Gms.Extensions;
using Android.Nfc;
using Android.Widget;
using aplikacja_dziekanat.Droid;
using db;
using Firebase.Auth;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private static readonly DbConnection db = new DbConnection();
        private static User currentUser = new User();
        private static string token = null;

        public User CurrentUser { get { return currentUser; } }

        public string Token()
        {
            return token;
        }

        public async Task SetToken()
        {
            if (FirebaseAuth.Instance.CurrentUser == null)
            {
                return;
            }

            var tokenRequest = await FirebaseAuth.Instance.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>();
            token = tokenRequest.Token;

            currentUser = await db.GetUser(FirebaseAuth.Instance.CurrentUser.Uid);
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            var authResult = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            var tokenResult = await (authResult.User.GetIdToken(false).AsAsync<GetTokenResult>());
            token = tokenResult.Token.ToString();
            currentUser = await db.GetUser(authResult.User.Uid);

            return token;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password, User newUser)
        {
            var authResult = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            var tokenResult = await (authResult.User.GetIdToken(false).AsAsync<GetTokenResult>());

            token = tokenResult.Token.ToString();
            currentUser = newUser;
            currentUser.Uid = authResult.User.Uid;
            await db.CreateUser(newUser);

            return token;
        }

        public async Task VerifyBeforeUpdateEmail(string newEmail)
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            await user.VerifyBeforeUpdateEmail(newEmail);
        }

        public async Task ChangeUserEmail(string newEmail)
        {
            try
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                currentUser.Email = newEmail;
                await user.UpdateEmailAsync(newEmail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                if (ex.Message.Contains("Please verify the new email before changing email"))
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async void ChangeUserPassword(string newPassword)
        {
            await FirebaseAuth.Instance.CurrentUser.ReloadAsync();
            var user = FirebaseAuth.Instance.CurrentUser;
            await user.UpdatePasswordAsync(newPassword);
        }

        private async Task<bool> RequestCheckPassword(string password)
        {
            try
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                var credential = EmailAuthProvider.GetCredential(user.Email, password);
                await user.ReauthenticateAndRetrieveDataAsync(credential);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ShowPasswordPrompt(Action onDismiss)
        {
            try
            {
                Dialog dialog = new Dialog(Xamarin.Essentials.Platform.CurrentActivity);
                dialog.SetContentView(Resource.Layout.password_prompt);
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(true);
                dialog.Window.SetDimAmount(0.8f);
                dialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

                dialog.CancelEvent += (object sender, EventArgs e) =>
                {
                    onDismiss();
                };

                Android.Widget.Button btn = (Android.Widget.Button)dialog.FindViewById(Resource.Id.button1);
                btn.Click += (object sender, EventArgs e) =>
                {
                    dialog.Dismiss();
                    onDismiss();
                };

                Android.Widget.Button btn2 = (Android.Widget.Button)dialog.FindViewById(Resource.Id.button2);
                btn2.Click += async (object sender, EventArgs e) =>
                {
                    DebugService.WriteLine("Confirm button clicked");
                    Android.Widget.EditText passwordInput = (Android.Widget.EditText)dialog.FindViewById(Resource.Id.password);
                    Android.Widget.TextView passwordLabel = (Android.Widget.TextView)dialog.FindViewById(Resource.Id.password_caption);
                    string password = passwordInput.Text.ToString();

                    bool isAuthenticated = await RequestCheckPassword(password);

                    if (isAuthenticated)
                    { 
                        passwordLabel.Text = "Wprowadzono prawidłowe hasło";
                        passwordLabel.SetTextColor(Android.Graphics.Color.White);
                        dialog.Dismiss();
                    }
                    else
                    {
                        passwordLabel.Text = "Podano złe hasło";
                        passwordLabel.SetTextColor(Android.Graphics.Color.Red);
                    }
                };

                dialog.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async void DeleteCurrentUser()
        {
            await FirebaseAuth.Instance.CurrentUser.ReloadAsync();
            var user = FirebaseAuth.Instance.CurrentUser;
            await user.DeleteAsync();
        }

        public void Logout()
        {
            Toast.MakeText(Android.App.Application.Context, "Wylogowano", ToastLength.Short).Show();
            currentUser = new User();
            token = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}