using System.Threading.Tasks;
using db;

namespace aplikacja_dziekanat
{
    public interface IFirebaseAuth
    {
        string Uid();
        string Email();

        Task<string> LoginWithEmailAndPassword(string email, string password);
        Task<string> RegisterWithEmailAndPassword(string email, string password);

        void Logout();
    }
}
