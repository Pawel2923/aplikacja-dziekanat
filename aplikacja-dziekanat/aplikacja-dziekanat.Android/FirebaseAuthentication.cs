using aplikacja_dziekanat.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Gms.Extensions;
using Firebase.Auth;
using db;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private User currentUser;
        private string token;

        public User CurrentUser { get { return currentUser; } }

        public string Token()
        {
            return token;
        }

        public FirebaseAuthentication()
        {
            token = null;
            currentUser = new User();
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            var tokenResult = await (user.User.GetIdToken(false).AsAsync<GetTokenResult>());
            
            SetToken(tokenResult.Token.ToString());
            SetCurrentUser(user.User);

            return currentUser.Uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            var tokenResult = await (user.User.GetIdToken(false).AsAsync<GetTokenResult>());

            SetToken(tokenResult.Token.ToString());
            SetCurrentUser(user.User);

            return currentUser.Uid;
        }

        private async void SetCurrentUser(FirebaseUser user)
        {
            DbConnection db = new DbConnection();
            currentUser = await db.GetUser(user.Uid);
            currentUser.Uid = user.Uid;
            db.Dispose();
        }

        private void SetToken(string token)
        {
            this.token = token;
        }

        private void DestroyUser()
        { 
            currentUser = new User();
            currentUser = null;
        }

        public void Logout()
        {
            DestroyUser();
            token = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}