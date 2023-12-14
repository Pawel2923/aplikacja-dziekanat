using db;
using System.Threading.Tasks;
namespace aplikacja_dziekanat
{
    public interface IFirebaseAuth
    {
        User CurrentUser { get; }
        string Token();

        Task<string> LoginWithEmailAndPassword(string email, string password);
        Task<string> RegisterWithEmailAndPassword(string email, string password);

        void Logout();
    }
}
