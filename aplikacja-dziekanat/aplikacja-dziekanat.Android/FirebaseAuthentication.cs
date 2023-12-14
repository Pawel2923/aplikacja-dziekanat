using aplikacja_dziekanat.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Gms.Extensions;
using Firebase.Auth;
using System.Diagnostics;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private string uid;
        private string email;
        private string token;

        public string Uid()
        {
            return uid;
        }

        public string Email()
        {
            return email;
        }

        public string Token()
        {
            return token;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            uid = user.User.Uid;
            var toktenResult = await (user.User.GetIdToken(false).AsAsync<GetTokenResult>());
            token = toktenResult.Token.ToString();
            this.email = email;

            return uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            uid = user.User.Uid;
            var toktenResult = await (user.User.GetIdToken(false).AsAsync<GetTokenResult>());
            token = toktenResult.Token.ToString();
            this.email = email;

            return uid;
        }

        public void Logout()
        {
            uid = null;
            email = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}