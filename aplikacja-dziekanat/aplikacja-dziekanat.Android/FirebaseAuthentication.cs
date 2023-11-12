using System.Threading.Tasks;
using Firebase.Auth;
using aplikacja_dziekanat.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        private string uid;
        
        public string Uid()
        {
            return uid;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            uid = FirebaseAuth.Instance.CurrentUser.Uid;
            return uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            uid = FirebaseAuth.Instance.CurrentUser.Uid;
            return uid;
        }

        public void Logout()
        {
            uid = null;
            FirebaseAuth.Instance.SignOut();
        }
    }
}