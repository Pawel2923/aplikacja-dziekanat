using aplikacja_dziekanat;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace db
{
    public class DbConnection
    {
        private static readonly IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();
        private static readonly string databaseUrl = "https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/";
        private static readonly FirebaseClient firebase = new FirebaseClient(databaseUrl, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(auth?.Token())
        });

        public async Task<User> GetUser(string uid)
        {
            try
            {
                var user = await firebase.Child("users").Child(uid).OnceSingleAsync<User>();
                user.Uid = uid;
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var userItems = await firebase.Child("users").OnceAsync<User>();

                return userItems.Select(item => new User
                {
                    Uid = item.Key,
                    Email = item.Object.Email,
                    Role = item.Object.Role,
                    ClassId = item.Object.ClassId,
                    Profile = item.Object.Profile
                }).FirstOrDefault(item => item.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

        public async Task<List<string>> GetAllUserEmails()
        {
            try
            {
                var userItems = await firebase.Child("users").OnceAsync<User>();

                return userItems.Select(item => item.Object.Email).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return null;
            }
        }

        public async Task<bool> CreateUser(User newUser)
        {
            try
            {
                await firebase.Child("users").Child(auth.CurrentUser.Uid).PutAsync(new User
                {
                    Email = newUser.Email,
                    Role = newUser.Role,
                    ClassId = newUser.ClassId,
                    Profile = newUser.Profile
                });
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> DeleteUser(string uid)
        {
            try
            {
                await firebase.Child("users").Child(uid).DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> ChangeEmail(string email)
        {
            try
            {
                await auth.ChangeUserEmail(email);

                auth.CurrentUser.Email = email;
                await firebase.Child("users").Child(auth.CurrentUser.Uid).PutAsync(auth.CurrentUser);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Please verify the new email before changing email"))
                {
                    throw new Exception(ex.Message);
                }
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> UpdateProfile(Profile newProfile)
        {
            if (newProfile == null)
                throw new Exception("Brak danych");

            newProfile.AlbumNumber = auth.CurrentUser.Profile.AlbumNumber;
            newProfile.StudyStatus = auth.CurrentUser.Profile.StudyStatus;
            newProfile.Groups = auth.CurrentUser.Profile.Groups;
            newProfile.Degree = auth.CurrentUser.Profile.Degree;

            try
            {
                await firebase.Child("users").Child(auth.CurrentUser.Uid).Child("Profile").PutAsync(new Profile
                {
                    FirstName = newProfile.FirstName,
                    LastName = newProfile.LastName,
                    AlbumNumber = newProfile.AlbumNumber,
                    PhoneNumber = newProfile.PhoneNumber,
                    Address = newProfile.Address,
                    City = newProfile.City,
                    ZipCode = newProfile.ZipCode,
                    StudyStatus = newProfile.StudyStatus,
                    Groups = newProfile.Groups,
                    Degree = newProfile.Degree
                });

                auth.CurrentUser.Profile = newProfile;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> UpdateUser(User newUser)
        {
            try
            {
                if (newUser == null)
                    throw new Exception("Brak danych");

                await firebase.Child("users").Child(newUser.Uid).PutAsync(new User
                {
                    Email = newUser.Email,
                    Role = newUser.Role,
                    ClassId = newUser.ClassId,
                    Profile = newUser.Profile
                });

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
                if (classId == null || day == null)
                    throw new Exception("Brak danych");

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
                throw new Exception(ex.Message);
            }
        }

        public async Task<Schedule> GetScheduleById(string classId, string day, string scheduleId)
        {
            try
            {
                if (classId == null || day == null || scheduleId == null)
                    throw new Exception("Brak danych");

                var schedule = await firebase
                    .Child("schedule")
                    .Child(classId)
                    .Child(day)
                    .Child(scheduleId)
                    .OnceSingleAsync<Schedule>();

                return schedule;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateSchedule(string classId, string day, string scheduleId, Schedule newSchedule)
        {
            try
            {
                if (classId == null || day == null || scheduleId == null || newSchedule == null)
                    throw new Exception("Brak danych");

                await firebase.Child("schedule").Child(classId).Child(day).Child(scheduleId).PutAsync(new Schedule
                {
                    ClassType = newSchedule.ClassType,
                    Duration = newSchedule.Duration,
                    Name = newSchedule.Name,
                    Room = newSchedule.Room,
                    Teacher = newSchedule.Teacher,
                    TimeStart = newSchedule.TimeStart
                });

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<List<string>> GetScheduleIds(string classId, string day)
        {
            try
            {
                if (classId == null || day == null)
                    throw new Exception("Brak danych");

                var scheduleItems = await firebase
                    .Child("schedule")
                    .Child(classId)
                    .Child(day)
                    .OnceAsync<Schedule>();

                return scheduleItems.Select(item => item.Key).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Notice>> GetNotices(string classId)
        {
            try
            {
                var notices = await firebase
                    .Child("notice")
                    .OnceAsync<Notice>();

                return notices.Select(item => item.Object.To == classId ? (new Notice
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

        public async Task<Notice> GetNoticeById(string noticeId)
        {
            try
            {
                var notice = await firebase
                    .Child("notice")
                    .Child(noticeId)
                    .OnceSingleAsync<Notice>();

                return notice;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetNoticeIds()
        {
            try
            {
                var scheduleItems = await firebase
                    .Child("notice")
                    .OnceAsync<Notice>();

                return scheduleItems.Select(item => item.Key).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetNoticeIds(string email)
        {
            try
            {
                var scheduleItems = await firebase
                    .Child("notice")
                    .OnceAsync<Notice>();

                var result = new List<string>();

                foreach (var item in scheduleItems)
                {
                    if (item.Object.Author.Contains(email.Split('@')[0]))
                    {
                        result.Add(item.Key);
                    }
                }

                if (result.Count == 0)
                {
                    throw new Exception("Brak ogłoszeń");
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetNoticeIds(string firstName, string lastName)
        {
            try
            {
                var scheduleItems = await firebase
                    .Child("notice")
                    .OnceAsync<Notice>();

                var result = new List<string>();

                foreach (var item in scheduleItems)
                {
                    if (item.Object.Author.Contains($"{firstName} {lastName}"))
                    {
                        result.Add(item.Key);
                    }
                }

                if (result.Count == 0)
                {
                    throw new Exception("Brak ogłoszeń");
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendNotice(Notice newNotice)
        {
            try
            {
                await firebase.Child("notice").PostAsync(new Notice
                {
                    Author = newNotice.Author,
                    Date = newNotice.Date,
                    Content = newNotice.Content,
                    Title = newNotice.Title,
                    To = newNotice.To
                });

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> UpdateNotice(string noticeId, Notice newNotice)
        {
            try
            {
                if (noticeId == null || newNotice == null)
                    throw new Exception("Brak danych");

                await firebase.Child("notice").Child(noticeId).PutAsync(new Notice
                {
                    Author = newNotice.Author,
                    Date = newNotice.Date,
                    Content = newNotice.Content,
                    Title = newNotice.Title,
                    To = newNotice.To
                });

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
            }
        }

        public async Task<bool> DeleteNotice(string noticeId)
        {
            try
            {
                if (noticeId == null)
                    throw new Exception("Brak danych");

                await firebase.Child("notice").Child(noticeId).DeleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                return false;
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
