using db;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        private string _Degree;
        private string _panelTitle;
        private string _panelDescription;

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
        public string Degree { get { return _Degree; } set { _Degree = value; RaisePropertyChanged(nameof(Degree)); } }
        public string PanelTitle { get { return _panelTitle; } set { _panelTitle = value; RaisePropertyChanged(nameof(PanelTitle)); } }
        public string PanelDescription { get { return _panelDescription; } set { _panelDescription = value; RaisePropertyChanged(nameof(PanelDescription)); } }

        public profilUz()
        {
            InitializeComponent();
            BindingContext = this;
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateProfile();
        }

        private async void OnEditProfileButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Przechodzenie do edycji profilu");
            await Navigation.PushAsync(new EdytujProfil());
        }

        private async void OnEditLoginBtnClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Przechodzenie do edycji profilu");
            await Navigation.PushAsync(new EdytujLogin());
        }

        private async void OnPanelBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var auth = DependencyService.Resolve<IFirebaseAuth>();
                if (auth.CurrentUser.Role == "admin")
                {
                    Debug.WriteLine("Przechodzenie do panelu administracyjnego");
                    await Navigation.PushAsync(new Admin());
                }
                else if (auth.CurrentUser.Role == "teacher")
                {
                    Debug.WriteLine("Przechodzenie do panelu nauczyciela");
                    await Navigation.PushAsync(new Teacher());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Wystąpił błąd podczas przechodzenia do panelu: {0}", ex.Message);
                await DisplayAlert("Błąd", "Wystąpił błąd podczas przechodzenia do panelu", "OK");
            }
        }

        private async void OnDeleteAccountBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var auth = DependencyService.Resolve<IFirebaseAuth>();

                if (auth.CurrentUser.Uid == null)
                {
                    throw new Exception("Nie udało się usunąć użytkownika");
                }

                string confirmEmail = await DisplayPromptAsync("Potwierdź", $"Wpisz {auth.CurrentUser.Email} aby potwierdzić", "OK", "Anuluj", "Wpisz swój email", 50, Keyboard.Email, "");

                if (confirmEmail == null)
                    return;

                if (confirmEmail == auth.CurrentUser.Email)
                {
                    DbConnection dbConnection = new DbConnection();
                    var result = await dbConnection.DeleteUser(auth.CurrentUser.Uid);

                    if (result)
                    {
                        auth.DeleteCurrentUser();
                        await DisplayAlert("Sukces", "Usunięto użytkownika", "OK");
                        auth.Logout();
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        throw new Exception("Nie udało się usunąć użytkownika");
                    }
                }
                else
                {
                    throw new Exception("Nie wpisano poprawnego adresu");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Wystąpił błąd podczas usuwania konta użytkownika: {ex.Message}");
                await DisplayAlert("Błąd", ex.Message, "OK");
            }
        }

        private async void UpdateProfile()
        {
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.CurrentUser != null)
            {
                Debug.WriteLine("Aktualizowanie profilu użytkownika (uid: {0}), z rolą \"{1}\": ", auth.CurrentUser.Uid, auth.CurrentUser.Role);

                // Ustaw wartości pól
                FirstName = auth.CurrentUser.Profile.FirstName == "" ? "" : auth.CurrentUser.Profile.FirstName;
                LastName = auth.CurrentUser.Profile.LastName == "" ? "" : auth.CurrentUser.Profile.LastName;
                Course = auth.CurrentUser.ClassId;
                AlbumNumber = auth.CurrentUser.Profile.AlbumNumber == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.AlbumNumber;
                Email = auth.CurrentUser.Email;
                PhoneNumber = auth.CurrentUser.Profile.PhoneNumber == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.PhoneNumber;
                Address = auth.CurrentUser.Profile.Address == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.Address;
                City = auth.CurrentUser.Profile.City == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.City;
                ZipCode = auth.CurrentUser.Profile.ZipCode == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.ZipCode;
                StudyStatus = auth.CurrentUser.Profile.StudyStatus == "" ? "Nie ustawiono" : auth.CurrentUser.Profile.StudyStatus;
                Groups = "";

                // Ustaw widoczność kontenerów w zależności od roli użytkownika
                courseContainer.IsVisible = auth.CurrentUser.Role == "student";
                albumNumberContainer.IsVisible = auth.CurrentUser.Role == "student";
                studyStatusContainer.IsVisible = auth.CurrentUser.Role == "student";
                groupsContainer.IsVisible = auth.CurrentUser.Role == "student";

                // Jeśli użytkownik jest studentem to aktualizuj grupy
                if (auth.CurrentUser.Role == "student")
                {
                    Debug.WriteLine("Długość tablicy grup: {0}", auth.CurrentUser.Profile.Groups.Length);

                    if (auth.CurrentUser.Profile.Groups.Length > 0 && auth.CurrentUser.Profile.Groups[0] != "")
                    {
                        foreach (var group in auth.CurrentUser.Profile.Groups)
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

                if (auth.CurrentUser.Role == "teacher")
                {
                    Degree = auth.CurrentUser.Profile.Degree;
                    PanelTitle = "Panel zarządzania dla nauczycieli";
                    PanelDescription = "W tym panelu możesz zarządzać swoimi grupami, dodawać ogłoszenia i je usuwać";
                }
                else if (auth.CurrentUser.Role == "admin")
                {
                    degreeContainer.IsVisible = false;
                    PanelTitle = "Panel administracyjny";
                    PanelDescription = "W tym panelu możesz zarządzać użytkownikami, grupami, ogłoszeniami oraz harmonogramem";
                }
                else
                {
                    degreeContainer.IsVisible = false;
                    adminTeacherPanel.IsVisible = false;
                }

                Debug.WriteLine("Profil został zaktualizowany");
            }

            else
            {
                Debug.WriteLine("Wystąpił błąd podczas aktualizowania profilu użytkownika");
                await DisplayAlert("Błąd", "Nie znaleziono użytkownika", "OK");
            }
        }
    }
}