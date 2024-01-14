using db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Admin : ContentPage, INotifyPropertyChanged
    {
        private string _ClassId;
        private string _Day;
        private string _ScheduleId;
        private string _NoticeId;
        private string _Email;
        private readonly List<string> daysList = new List<string> { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

        public new string ClassId { get { return _ClassId; } set { _ClassId = value; RaisePropertyChanged(nameof(ClassId)); } }
        public string Day { get { return _Day; } set { _Day = value; RaisePropertyChanged(nameof(Day)); } }
        public string ScheduleId { get { return _ScheduleId; } set { _ScheduleId = value; RaisePropertyChanged(nameof(ScheduleId)); } }
        public string NoticeId { get { return _NoticeId; } set { _NoticeId = value; RaisePropertyChanged(nameof(NoticeId)); } }
        public string Email { get { return _Email; } set { _Email = value; RaisePropertyChanged(nameof(User)); } }

        public Admin()
        {
            InitializeComponent();
            classIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            daySelect.ItemsSource = daysList;
            scheduleIdSelect.ItemsSource = new List<string> { "Wybierz rok i dzień" };
            noticeIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            emailSelect.ItemsSource = new List<string> { "Ładowanie..." };
            BindingContext = this;
        }

        private async void OnEditSchedule(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ClassId) || string.IsNullOrEmpty(Day) || string.IsNullOrEmpty(ScheduleId))
            {
                await DisplayAlert("Błąd", "Wypełnij wszystkie pola.", "OK");
                return;
            }

            await Navigation.PushAsync(new EditSchedule(ClassId, Day, ScheduleId));
        }

        private async void OnEditNotice(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NoticeId))
            {
                await DisplayAlert("Błąd", "Wypełnij wymagane pole.", "OK");
                return;
            }

            await Navigation.PushAsync(new EditNotice(NoticeId));
        }

        private async void OnEditUser(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Email))
            {
                await DisplayAlert("Błąd", "Wypełnij wymagane pole.", "OK");
                return;
            }

            await Navigation.PushAsync(new EditUser(Email));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetSelectItems();
        }

        private async void SetSelectItems()
        {
            try
            {
                DbConnection dbConnection = new DbConnection();
                classIdSelect.ItemsSource = await dbConnection.GetClassIds();
                noticeIdSelect.ItemsSource = await dbConnection.GetNoticeIds();
                emailSelect.ItemsSource = await dbConnection.GetAllUserEmails();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Wystąpił błąd: {ex.Message}");
                await DisplayAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        public void ClassIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (classIdSelect.SelectedIndex > -1)
            {
                classIdSelect.TitleColor = Color.Black;
                ClassId = classIdSelect.Items[classIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                classIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        public void DaySelectChangeHandler(object sender, EventArgs e)
        {
            if (daySelect.SelectedIndex > -1)
            {
                daySelect.TitleColor = Color.Black;
                string dayPolish = daySelect.Items[daySelect.SelectedIndex];
                switch (dayPolish)
                {
                    case "Poniedziałek":
                        Day = "Monday";
                        break;
                    case "Wtorek":
                        Day = "Tuesday";
                        break;
                    case "Środa":
                        Day = "Wednesday";
                        break;
                    case "Czwartek":
                        Day = "Thursday";
                        break;
                    case "Piątek":
                        Day = "Friday";
                        break;
                    case "Sobota":
                        Day = "Saturday";
                        break;
                    case "Niedziela":
                        Day = "Sunday";
                        break;
                }
                selectAngleDown2.Source = "angleDownSolidAlt.png";
            }
            else
            {
                daySelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown2.Source = "angleDownSolid.png";
            }
        }

        public void ScheduleIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (scheduleIdSelect.SelectedIndex > -1)
            {
                scheduleIdSelect.TitleColor = Color.Black;
                ScheduleId = scheduleIdSelect.Items[scheduleIdSelect.SelectedIndex];
                selectAngleDown3.Source = "angleDownSolidAlt.png";
            }
            else
            {
                scheduleIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown3.Source = "angleDownSolid.png";
            }
        }

        public void NoticeIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (noticeIdSelect.SelectedIndex > -1)
            {
                noticeIdSelect.TitleColor = Color.Black;
                NoticeId = noticeIdSelect.Items[noticeIdSelect.SelectedIndex];
                selectAngleDown4.Source = "angleDownSolidAlt.png";
            }
            else
            {
                noticeIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown4.Source = "angleDownSolid.png";
            }
        }
        public void EmailSelectChangeHandler(object sender, EventArgs e)
        {
            if (emailSelect.SelectedIndex > -1)
            {
                emailSelect.TitleColor = Color.Black;
                Email = emailSelect.Items[emailSelect.SelectedIndex];
                selectAngleDown5.Source = "angleDownSolidAlt.png";
            }
            else
            {
                emailSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown5.Source = "angleDownSolid.png";
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == nameof(ClassId) || propertyName == nameof(Day))
            {
                OnClassIdAndDayChanged(this, new EventArgs());
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnClassIdAndDayChanged(object sender, EventArgs e)
        {
            if (ClassId != null && Day != null)
            {
                try
                {
                    DbConnection dbConnection = new DbConnection();
                    var items = await dbConnection.GetScheduleIds(ClassId, Day);
                    if (items.Count > 0)
                    {
                        scheduleIdSelect.ItemsSource = items;
                    }
                    else
                    {
                        scheduleIdSelect.ItemsSource = new List<string> { "Brak zajęć do wyboru" };
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Wystąpił błąd: {ex.Message}");
                    await DisplayAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
                }
            }
        }
    }
}