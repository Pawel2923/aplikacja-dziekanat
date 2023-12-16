using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System;
using System.Diagnostics;
using System.ComponentModel;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class profilUz : ContentPage, INotifyPropertyChanged
    {
        private string _FirstName;
        private string _LastName;
        private string _Course;
        private string _AlbumNumber;
        private string _Email;
        private string _PhoneNumber;
        private string _Address;
        private string _City;
        private string _ZipCode;
        private string _StudyStatus;
        private string _Groups;

        public string FirstName { get { return _FirstName; } set { _FirstName = value; RaisePropertyChanged(nameof(FirstName)); } }
        public string LastName { get { return _LastName; } set { _LastName = value; RaisePropertyChanged(nameof(LastName)); } }
        public string Course { get { return _Course; } set { _Course = value; RaisePropertyChanged(nameof(Course)); } }
        public string AlbumNumber { get { return _AlbumNumber; } set { _AlbumNumber = value; RaisePropertyChanged(nameof(AlbumNumber)); } }
        public string Email { get { return _Email; } set { _Email = value; RaisePropertyChanged(nameof(Email)); } }
        public string PhoneNumber { get { return _PhoneNumber; } set { _PhoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); } }
        public string Address { get { return _Address; } set { _Address = value; RaisePropertyChanged(nameof(Address)); } }
        public string City { get { return _City; } set { _City = value; RaisePropertyChanged(nameof(City)); } }
        public string ZipCode { get { return _ZipCode; } set { _ZipCode = value; RaisePropertyChanged(nameof(ZipCode)); } }
        public string StudyStatus { get { return _StudyStatus; } set { _StudyStatus = value; RaisePropertyChanged(nameof(StudyStatus)); } }
        public string Groups { get { return _Groups; } set { _Groups = value; RaisePropertyChanged(nameof(Groups)); } }

        private readonly DbConnection dbConnection = new DbConnection();
        public profilUz()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); 
            UpdateProfile();
        }

        public void OnEditProfileButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Wciśnięto editProfileButton. Dalszego działania nie zaimplementowano");
            //Navigation.PushAsync(new edytujProfil());
        }

        private async void UpdateProfile()
        {
            var auth = DependencyService.Resolve<IFirebaseAuth>();
            var user = await dbConnection.GetUser(auth.CurrentUser.Uid);

            if (user != null)
            {
                Debug.WriteLine("Aktualizowanie profilu użytkownika (uid: {0}), z rolą \"{1}\": ", user.Uid, user.Role);

                // Ustaw wartości pól
                FirstName = user.Profile.FirstName == "" ? "" : user.Profile.FirstName;
                LastName = user.Profile.LastName == "" || user.Profile.LastName.ToLower() == "brak" ? "" : user.Profile.LastName + "!";
                Course = user.ClassId;
                AlbumNumber = user.Profile.AlbumNumber == "" ? "Nie ustawiono" : user.Profile.AlbumNumber;
                Email = user.Email;
                PhoneNumber = user.Profile.PhoneNumber == "" ? "Nie ustawiono" : user.Profile.PhoneNumber;
                Address = user.Profile.Address == "" ? "Nie ustawiono" : user.Profile.Address;
                City = user.Profile.City == "" ? "Nie ustawiono" : user.Profile.City;
                ZipCode = user.Profile.ZipCode == "" ? "Nie ustawiono" : user.Profile.ZipCode;
                StudyStatus = user.Profile.StudyStatus == "" ? "Nie ustawiono" : user.Profile.StudyStatus;
                Groups = "";

                // Ustaw widoczność kontenerów w zależności od roli użytkownika
                courseContainer.IsVisible = user.Role == "student";
                albumNumberContainer.IsVisible = user.Role == "student";
                studyStatusContainer.IsVisible = user.Role == "student";
                groupsContainer.IsVisible = user.Role == "student";

                // Jeśli użytkownik jest studentem to aktualizuj grupy
                if (user.Role == "student")
                {
                    Debug.WriteLine("Długość tablicy grup: {0}", user.Profile.Groups.Length);

                    if (user.Profile.Groups.Length > 0 && user.Profile.Groups[0] != "")
                    {
                        foreach (var group in user.Profile.Groups)
                        {
                            Groups += group.ToUpper() + ", ";
                        }
                        Groups = Groups.Remove(Groups.Length - 2);
                    }
                    else
                    {
                        Groups = "Nie jesteś członkiem żadnej grupy";
                    }
                }

                Debug.WriteLine("Profil został zaktualizowany");
            }

            else
            {
                Debug.WriteLine("Wystąpił błąd podczas aktualizowania profilu użytkownika");
                await DisplayAlert("Błąd", "Nie znaleziono użytkownika", "OK");
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}