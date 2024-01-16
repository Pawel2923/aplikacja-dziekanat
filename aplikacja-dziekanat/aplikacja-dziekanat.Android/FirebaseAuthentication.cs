using Android.Gms.Extensions;
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

        public async void DeleteCurrentUser()
        {
            await FirebaseAuth.Instance.CurrentUser.ReloadAsync();
            var user = FirebaseAuth.Instance.CurrentUser;
            await user.DeleteAsync();
        }

        public void Logout()
        {
            currentUser = new User();
            token = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}