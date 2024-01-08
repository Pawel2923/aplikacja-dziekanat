using db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUser : ContentPage, INotifyPropertyChanged
    {
        private User newUser;

        public User NewUser { get { return newUser; } set { newUser = value; RaisePropertyChanged(nameof(NewUser)); } }

        public EditUser()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public EditUser(string email)
        {
            InitializeComponent();
            NewUser = new User { Email = email } ;
            SetSelects();
            BindingContext = this;
        }

        private async void SetSelects()
        {
            classIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            roleSelect.ItemsSource = new List<string> { "Ładowanie..." };

            if (NewUser != null)
            {
                DbConnection dbConnection = new DbConnection();
                NewUser = await dbConnection.GetUserByEmail(NewUser.Email);
                degreeLabel.IsVisible = NewUser.Role == "teacher";
                degreeFrame.IsVisible = NewUser.Role == "teacher";

                var classIds = await dbConnection.GetClassIds();
                var roles = new List<string> { "admin", "teacher", "student" };

                classIdSelect.ItemsSource = classIds;
                roleSelect.ItemsSource = roles;

                classIdSelect.SelectedIndex = classIdSelect.Items.IndexOf(NewUser.ClassId);
                roleSelect.SelectedIndex = roleSelect.Items.IndexOf(NewUser.Role);
            }
        }

        private async void UpdateUser(object sender, EventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            var result = await dbConnection.UpdateUser(NewUser);

            if (result)
            {
                await DisplayAlert("Sukces", "Zaktualizowano użytkownika", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się zaktualizować użytkownika", "OK");
            }
        }

        private async void DeleteUser(object sender, EventArgs e)
        {
            if (NewUser.Uid == null)
            {
                await DisplayAlert("Błąd", "Nie udało się usunąć użytkownika", "OK");
                return;
            }

            var confirmEmail = await DisplayPromptAsync("Potwierdź", $"Wpisz {NewUser.Email} aby potwierdzić", "OK", "Anuluj", "Wpisz email", 50, Keyboard.Email, "");
            
            if (confirmEmail == NewUser.Email)
            {
                DbConnection dbConnection = new DbConnection();
                var result = await dbConnection.DeleteUser(NewUser.Uid);

                if (result)
                {
                    await DisplayAlert("Sukces", "Usunięto użytkownika", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Błąd", "Nie udało się usunąć użytkownika", "OK");
                }
            }
            else
            {
                await DisplayAlert("Błąd", "Nie wpisano poprawnego adresu", "OK");
            }
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ClassIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (classIdSelect.SelectedIndex > -1)
            {
                classIdSelect.TitleColor = Color.Black;
                NewUser.ClassId = classIdSelect.Items[classIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                classIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        private void RoleSelectChangeHandler(object sender, EventArgs e)
        {
            if (roleSelect.SelectedIndex > -1)
            {
                roleSelect.TitleColor = Color.Black;
                NewUser.Role = roleSelect.Items[roleSelect.SelectedIndex];
                selectAngleDown2.Source = "angleDownSolidAlt.png";
            }
            else
            {
                roleSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown2.Source = "angleDownSolid.png";
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}