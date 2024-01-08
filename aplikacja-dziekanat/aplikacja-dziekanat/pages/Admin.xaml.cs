using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Admin : ContentPage, INotifyPropertyChanged
    {
        private string _Group;
        private string _Day;
        private string _ScheduleId;
        private string _NoticeId;
        private User _User = new User();
        private readonly List<string> daysList = new List<string> { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };

        public string Group { get { return _Group; } set { _Group = value; RaisePropertyChanged(nameof(Group)); } }
        public string Day { get { return _Day; } set { _Day = value; RaisePropertyChanged(nameof(Day)); } }
        public string ScheduleId { get { return _ScheduleId; } set { _ScheduleId = value; RaisePropertyChanged(nameof(ScheduleId)); } }
        public string NoticeId { get { return _NoticeId; } set { _NoticeId = value; RaisePropertyChanged(nameof(NoticeId)); } }
        public User User { get { return _User; } set { _User = value; RaisePropertyChanged(nameof(User)); } }

        public Admin()
        {
            InitializeComponent();
            groupSelect.ItemsSource = new List<string> { "Ładowanie..." };
            daySelect.ItemsSource = daysList;
            scheduleIdSelect.ItemsSource = new List<string> { "Wybierz rok i dzień" };
            noticeIdSelect.ItemsSource = new List<string> { "Ładowanie..." };
            userSelect.ItemsSource = new List<string> { "Ładowanie..." };
            SetSelectItems();
            BindingContext = this;
        }

        private async void OnEditSchedule(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditSchedule(Group, Day, ScheduleId));
        }
        private async void SetSelectItems()
        {
            try
            {
                DbConnection dbConnection = new DbConnection();
                groupSelect.ItemsSource = await dbConnection.GetClassIds();
                noticeIdSelect.ItemsSource = await dbConnection.GetNoticeIds();
                userSelect.ItemsSource = await dbConnection.GetAllUserEmails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                await DisplayAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
            }
        }

        public void GroupSelectChangeHandler(object sender, EventArgs e)
        {
            if (groupSelect.SelectedIndex > -1)
            {
                groupSelect.TitleColor = Color.Black;
                Group = groupSelect.Items[groupSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                groupSelect.TitleColor = Color.FromHex("#575757");
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
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                daySelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        public void ScheduleIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (scheduleIdSelect.SelectedIndex > -1)
            {
                scheduleIdSelect.TitleColor = Color.Black;
                ScheduleId = scheduleIdSelect.Items[scheduleIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                scheduleIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        public void NoticeIdSelectChangeHandler(object sender, EventArgs e)
        {
            if (noticeIdSelect.SelectedIndex > -1)
            {
                noticeIdSelect.TitleColor = Color.Black;
                NoticeId = noticeIdSelect.Items[noticeIdSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                noticeIdSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }
        public void UserSelectChangeHandler(object sender, EventArgs e)
        {
            if (userSelect.SelectedIndex > -1)
            {
                userSelect.TitleColor = Color.Black;
                User.Email = userSelect.Items[userSelect.SelectedIndex];
                selectAngleDown.Source = "angleDownSolidAlt.png";
            }
            else
            {
                userSelect.TitleColor = Color.FromHex("#575757");
                selectAngleDown.Source = "angleDownSolid.png";
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == nameof(Group) || propertyName == nameof(Day))
            {
                OnGroupAndDayChanged(this, new EventArgs());
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnGroupAndDayChanged(object sender, EventArgs e)
        {
            if (Group != null && Day != null)
            {
                try
                {
                    DbConnection dbConnection = new DbConnection();
                    var items = await dbConnection.GetScheduleIds(Group, Day);
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
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                    await DisplayAlert("Błąd", "Wystąpił błąd. Spróbuj ponownie.", "OK");
                }
            }
        }
    }
}