using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamarinFirebase.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;

namespace aplikacja_dziekanat
{
    public class DbConnection
    {
        private FirebaseClient firebase;
        public FirebaseClient Firebase { get { return firebase; } set { firebase = value; } }
        public async Task<List<User>> GetUsers()
        {
            firebase = new FirebaseClient("https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/");
            return (await firebase
              .Child("users")
              .OnceAsync<User>()).Select(item => new User
              {
                  Email = item.Object.Email,
                  Uid = item.Object.Uid
              }).ToList();
        }
    }
}
