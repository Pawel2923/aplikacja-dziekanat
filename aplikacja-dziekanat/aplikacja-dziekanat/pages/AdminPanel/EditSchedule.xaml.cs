using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System.Diagnostics;
using System.ComponentModel;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSchedule : ContentPage, INotifyPropertyChanged
    {
        private string _ScheduleId;
        private string _Day;
        private string _Group;
        private Schedule newSchedule;

        public Schedule NewSchedule { get { return newSchedule; } set { newSchedule = value; RaisePropertyChanged(nameof(NewSchedule)); } }

        public EditSchedule()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public EditSchedule(string group, string day, string scheduleId)
        {
            InitializeComponent();
            _ScheduleId = scheduleId;
            _Day = day;
            _Group = group;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_ScheduleId != null)
            {
                DbConnection dbConnection = new DbConnection();
                var currentSchedule = await dbConnection.GetScheduleById(_Group, _Day, _ScheduleId);

                if (currentSchedule != null)
                {
                    NewSchedule = currentSchedule;
                }
            }
        }

        private async void UpdateSchedule(object sender, EventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            var result = await dbConnection.UpdateSchedule(_Group, _Day, _ScheduleId, NewSchedule);

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

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}