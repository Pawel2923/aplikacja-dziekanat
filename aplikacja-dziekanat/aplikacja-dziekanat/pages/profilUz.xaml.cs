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
        public string AlbumNumber { get { return _AlbumNumber; } set { _AlbumNumber = value; RaisePropertyChanged(nameof(AlbumNumber)); } }
        public string Email { get { return _Email; } set { _Email = value; RaisePropertyChanged(nameof(Email)); } }
        public string PhoneNumber { get { return _PhoneNumber; } set { _PhoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); } }
        public string Address { get { return _Address; } set { _Address = value; RaisePropertyChanged(nameof(Address)); } }
        public string City { get { return _City; } set { _City = value; RaisePropertyChanged(nameof(City)); } }
        public string ZipCode { get { return _ZipCode; } set { _ZipCode = value; RaisePropertyChanged(nameof(ZipCode)); } }
        public string StudyStatus { get { return _StudyStatus; } set { _StudyStatus = value; RaisePropertyChanged(nameof(StudyStatus)); } }
        public string Groups { get { return _Groups; } set { _Groups = value; RaisePropertyChanged(nameof(Groups)); } }
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

        private async void UpdateProfile()
        {
            var auth = DependencyService.Get<IFirebaseAuth>();
            var connection = new DbConnection(AppInfo.DatabaseUrl);
            var users = await connection.GetUsers();
            bool isFound = false;
            foreach (var user in users)
            {
                if (user.Email == auth.Email())
                {
                    FirstName = user.Profile.FirstName;
                    LastName = user.Profile.LastName;
                    AlbumNumber = user.Profile.AlbumNumber;
                    Email = user.Email;
                    PhoneNumber = user.Profile.PhoneNumber;
                    Address = user.Profile.Address;
                    City = user.Profile.City;
                    ZipCode = user.Profile.ZipCode;
                    StudyStatus = user.Profile.StudyStatus;
                    foreach (var group in user.Profile.Groups)
                    {
                        Groups += group + ", ";
                    }
                    isFound = true;
                    Debug.WriteLine("Znaleziono użytkownika");
                    break;
                }
            }

            if (!isFound)
            {
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