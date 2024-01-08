using db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNotice : ContentPage
    {
        private string _NoticeId;
        private Notice newNotice;

        public Notice NewNotice { get { return newNotice; } set { newNotice = value; RaisePropertyChanged(nameof(NewNotice)); } }

        public EditNotice()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public EditNotice(string noticeId)
        {
            InitializeComponent();
            _NoticeId = noticeId;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            groupSelect.ItemsSource = new List<string> { "Ładowanie..." };

            if (_NoticeId != null)
            {
                DbConnection dbConnection = new DbConnection();
                groupSelect.ItemsSource = await dbConnection.GetClassIds();
                var currentNotice = await dbConnection.GetNoticeById(_NoticeId);

                if (currentNotice != null)
                {
                    NewNotice = currentNotice;

                    groupSelect.SelectedIndex = groupSelect.Items.IndexOf(currentNotice.To);
                }
            }
        }

        private async void UpdateNotice(object sender, EventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            NewNotice.Date = DateTime.Now.ToString("dd.MM.yyyy");
            string currentUserFullName = $"{auth.CurrentUser.Profile.FirstName} {auth.CurrentUser.Profile.LastName}";
            if (!NewNotice.Author.Contains(currentUserFullName))
            {
                NewNotice.Author += $", {currentUserFullName}";
            }

            var result = await dbConnection.UpdateNotice(_NoticeId, NewNotice);

            if (result)
            {
                await DisplayAlert("Sukces", "Zaktualizowano plan zajęć", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się zaktualizować planu zajęć", "OK");
            }
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void GroupSelectChangeHandler(object sender, EventArgs e)
        {
            if (groupSelect.SelectedIndex > -1)
            {
                groupSelect.TitleColor = Color.Black;
                NewNotice.To = groupSelect.Items[groupSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                groupSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}