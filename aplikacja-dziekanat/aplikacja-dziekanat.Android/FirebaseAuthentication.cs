using System.Threading.Tasks;
using Android.Gms.Extensions;
using Firebase.Auth;
using aplikacja_dziekanat.Droid;
using Xamarin.Forms;
using XamarinFirebase.Model;

[assembly: Dependency(typeof(FirebaseAuthentication))]
namespace aplikacja_dziekanat.Droid
{
    public class FirebaseAuthentication : IFirebaseAuth
    {
        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            return (string)await FirebaseAuth.Instance.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>();
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            _ = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
            return (string)await FirebaseAuth.Instance.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>();
        }
    }
}