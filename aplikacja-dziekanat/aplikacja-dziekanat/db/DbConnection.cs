using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace db
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
                  IsAdmin = item.Object.IsAdmin,
                  IsTeacher = item.Object.IsTeacher
              }).ToList();
        }

        public async Task<bool> CreateUser(string email, bool isAdmin, bool isTeacher)
        {
            firebase = new FirebaseClient("https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/");
            try
            {
                await firebase.Child("users").PostAsync(new User() { Email = email, IsAdmin = isAdmin, IsTeacher = isTeacher });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Schedule>> GetSchedule(string className, string date)
        {
            firebase = new FirebaseClient("https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/");

            try
            {
                return (await firebase.Child("schedule").Child(className).Child(date).OnceAsync<Schedule>()).Select(item => new Schedule
                {
                    ClassType = item.Object.ClassType,
                    Duration = item.Object.Duration,
                    Name = item.Object.Name,
                    Room = item.Object.Room,
                    Teacher = item.Object.Teacher,
                    TimeStart = item.Object.TimeStart
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

    }
}
