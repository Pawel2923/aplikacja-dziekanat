using System.Threading.Tasks;

namespace aplikacja_dziekanat
{
    public interface IFirebaseAuth
    {
        string Uid();

        Task<string> LoginWithEmailAndPassword(string email, string password);
        Task<string> RegisterWithEmailAndPassword(string email, string password);

        void Logout();
    }
}
