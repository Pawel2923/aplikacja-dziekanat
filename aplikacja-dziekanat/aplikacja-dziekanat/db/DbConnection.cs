using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace db
{
    public static class DbConnection
    {
        private static string databaseUrl = "https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/";
        private static FirebaseClient firebase = new FirebaseClient(databaseUrl);

        public static async Task<List<User>> GetUsers()
        {
            try
            {
                var userItems = await firebase
                    .Child("users")
                    .OnceAsync<User>();

                return userItems.Select(item => new User
                {
                    Email = item.Object.Email,
                    IsAdmin = item.Object.IsAdmin,
                    IsTeacher = item.Object.IsTeacher,
                    ClassId = item.Object.ClassId
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

        public static async Task<bool> CreateUser(string email, bool isAdmin, bool isTeacher, string classId)
        {
            try
            {
                await firebase.Child("users").PostAsync(new User() { Email = email, IsAdmin = isAdmin, IsTeacher = isTeacher, ClassId = classId });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public static async Task<List<Schedule>> GetSchedule(string classId, string day)
        {
            try
            {

                var scheduleItems = await firebase
                    .Child("schedule")
                    .Child(classId)
                    .Child(day)
                    .OnceAsync<Schedule>();

                return scheduleItems.Select(item => new Schedule

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

        public static async Task<List<Notice>> GetNotice(string classId)
        {
            try
            {
                var scheduleItems = await firebase
                    .Child("notice")
                    .OnceAsync<Notice>();

                return scheduleItems.Select(item => item.Object.To == classId ? (new Notice
                {
                    Author = item.Object.Author,
                    Date = item.Object.Date,
                    Content = item.Object.Content,
                    Title = item.Object.Title,
                    To = item.Object.To,
                }) : throw new Exception("Brak nowych ogłoszeń")).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<string>> GetClassIds()
        {
            try
            {
                var classIdItems = await firebase
                    .Child("classIds")
                    .OnceAsListAsync<string>();

                return classIdItems.Select(item => item.Object).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

        public static string FindClassId(string email, List<User> users)
        {
            try
            {
                string userClassId = null;
                foreach (var user in users)
                {
                    if (user.Email == email)
                    {
                        userClassId = user.ClassId;
                        break;
                    }
                }
                if (userClassId != null)
                {
                    return userClassId;
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return "Nie znalezino roku i kierunku";
            }
        }
    }
}
