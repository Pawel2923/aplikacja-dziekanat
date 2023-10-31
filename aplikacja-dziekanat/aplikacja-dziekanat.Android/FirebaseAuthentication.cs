using System.Threading.Tasks;
using Firebase.Auth;
using aplikacja_dziekanat.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }
    }
}