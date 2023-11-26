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
        private readonly FirebaseClient firebase;

        public DbConnection(string databaseUrl)
        {
            firebase = new FirebaseClient(databaseUrl);
        }

        public async Task<List<User>> GetUsers()
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

        public async Task<bool> CreateUser(string email, bool isAdmin, bool isTeacher, string classId)
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

        public async Task<List<Schedule>> GetSchedule(string classId, string day)
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

        public async Task<List<string>> GetClassIds()
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
    }
}
