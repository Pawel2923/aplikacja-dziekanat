using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Admin : ContentPage, INotifyPropertyChanged
    {
        private string _Group;
        private string _Day;
        private string _ScheduleId;
        private string _NoticeId;
        private string _UserId;

        public string Group { get { return _Group; } set { _Group = value; RaisePropertyChanged(nameof(Group)); } }
        public string Day { get { return _Day; } set { _Day = value; RaisePropertyChanged(nameof(Day)); } }
        public string ScheduleId { get { return _ScheduleId; } set { _ScheduleId = value; RaisePropertyChanged(nameof(ScheduleId)); } }
        public string NoticeId { get { return _NoticeId; } set { _NoticeId = value; RaisePropertyChanged(nameof(NoticeId)); } }
        public string UserId { get { return _UserId; } set { _UserId = value; RaisePropertyChanged(nameof(UserId)); } }

        public Admin()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnEditSchedule(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditSchedule(Group, Day, ScheduleId));
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}