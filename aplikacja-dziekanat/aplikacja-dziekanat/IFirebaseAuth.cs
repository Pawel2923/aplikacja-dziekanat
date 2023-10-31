using System.Threading.Tasks;

namespace aplikacja_dziekanat
{
    public interface IFirebaseAuth
    {
        Task<string> LoginWithEmailAndPassword(string email, string password);
        Task<string> RegisterWithEmailAndPassword(string email, string password);
    }
}
