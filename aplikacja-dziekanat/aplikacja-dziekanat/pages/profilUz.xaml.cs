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

        public string FirstName { get { return _FirstName; } set { _FirstName = value; RaisePropertyChanged(nameof(FirstName)); } }
        public string LastName { get { return _LastName; } set { _LastName = value; RaisePropertyChanged(nameof(LastName)); } }
        public string AlbumNumber { get { return _AlbumNumber; } set { _AlbumNumber = value; RaisePropertyChanged(nameof(AlbumNumber)); } }
        public string Email { get { return _Email; } set { _Email = value; RaisePropertyChanged(nameof(Email)); } }
        public string PhoneNumber { get { return _PhoneNumber; } set { _PhoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); } }

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