using aplikacja_dziekanat.Droid;
using Firebase.Auth;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Firebase;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private FirebaseAuth auth;
        private string uid;
        private string email;

        public FirebaseAuthentication()
        {
            var app = FirebaseApp.InitializeApp(Android.App.Application.Context);
            auth = Firebase.Auth.FirebaseAuth.GetInstance(app);
        }

        public string Uid()
        {
            return uid;
        }

        public string Email()
        {
            return email;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            _ = await auth.SignInWithEmailAndPasswordAsync(email, password);
            uid = auth.CurrentUser.Uid;
            this.email = email;

            return uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            _ = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            uid = auth.CurrentUser.Uid;
            this.email = email;

            return uid;
        }

        public void Logout()
        {
            uid = null;
            email = null;
            auth.SignOut();
        }
    }
}