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
        private static readonly DbConnection db = new DbConnection();
        private static User currentUser;
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
            currentUser.Uid = user.User.Uid;
            token = tokenResult.Token.ToString();
            currentUser = await db.GetUser(user.User.Uid);

            return token;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password, User newUser)
        {
            var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            var tokenResult = await (user.User.GetIdToken(false).AsAsync<GetTokenResult>());

            token = tokenResult.Token.ToString();
            currentUser = newUser;
            currentUser.Uid = user.User.Uid;
            await db.CreateUser(newUser);

            return token;
        }

        private void DestroyUser()
        { 
            currentUser = new User();
        }

        public void Logout()
        {
            DestroyUser();
            token = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}