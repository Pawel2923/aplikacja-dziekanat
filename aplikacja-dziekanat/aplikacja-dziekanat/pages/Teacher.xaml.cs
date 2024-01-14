using db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Teacher : ContentPage
    {
        private string noticeId;

        public Teacher()
        {
            InitializeComponent();
            noticeIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetNoticeIdSelectItems();
        }

        private async void SetNoticeIdSelectItems()
        {
            try
            {
                editNoticeButton.IsEnabled = true;
                var auth = DependencyService.Get<IFirebaseAuth>();
                DbConnection dbConnection = new DbConnection();
                if (!string.IsNullOrEmpty(auth.CurrentUser.Profile.FirstName) && !string.IsNullOrEmpty(auth.CurrentUser.Profile.LastName))
                {
                    Debug.WriteLine("Pobieranie id ogłoszeń przy użyciu imienia i nazwiska");
                    noticeIdSelect.ItemsSource = await dbConnection.GetNoticeIds(auth.CurrentUser.Profile.FirstName, auth.CurrentUser.Profile.LastName);
                }
                else
                {
                    Debug.WriteLine("Pobieranie id ogłoszeń przy użyciu adresu email");
                    noticeIdSelect.ItemsSource = await dbConnection.GetNoticeIds(auth.CurrentUser.Email);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Wystąpił błąd: {ex.Message}");

                if (ex.Message == "Brak ogłoszeń")
                {
                    noticeIdSelect.ItemsSource = new List<string> { "Brak ogłoszeń" };
                    editNoticeButton.IsEnabled = false;
                }
            }
        }

        private async void OnEditNotice(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(noticeId))
            {
                await DisplayAlert("Błąd", "Wypełnij wymagane pole.", "OK");
                return;
            }

            await Navigation.PushAsync(new EditNotice(noticeId));
        }

        public void NoticeIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (noticeIdSelect.SelectedIndex > -1)
            {
                noticeIdSelect.TitleColor = Color.Black;
                noticeId = noticeIdSelect.Items[noticeIdSelect.SelectedIndex];
                selectAngleDown4.Source = "angleDownSolidAlt.png";
            }
            else
            {
                noticeIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown4.Source = "angleDownSolid.png";
            }
        }
    }
}