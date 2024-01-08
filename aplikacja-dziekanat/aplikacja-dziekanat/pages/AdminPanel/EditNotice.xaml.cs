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
    public partial class EditNotice : ContentPage, INotifyPropertyChanged
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

            classIdSelect.ItemsSource = new List<string> { "Ładowanie..." };

            if (_NoticeId != null)
            {
                DbConnection dbConnection = new DbConnection();
                classIdSelect.ItemsSource = await dbConnection.GetClassIds();
                var currentNotice = await dbConnection.GetNoticeById(_NoticeId);

                if (currentNotice != null)
                {
                    NewNotice = currentNotice;

                    classIdSelect.SelectedIndex = classIdSelect.Items.IndexOf(currentNotice.To);
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

        private void ClassIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (classIdSelect.SelectedIndex > -1)
            {
                classIdSelect.TitleColor = Color.Black;
                NewNotice.To = classIdSelect.Items[classIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                classIdSelect.TitleColor = Color.FromHex("#575757");
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