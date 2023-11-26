using aplikacja_dziekanat.Droid;
using Firebase.Auth;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private string uid;
        private string email;

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
            _ = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            uid = FirebaseAuth.Instance.CurrentUser.Uid;
            this.email = email;

            return uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            uid = FirebaseAuth.Instance.CurrentUser.Uid;
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