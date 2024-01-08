using db;
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
    public partial class Teacher : ContentPage, INotifyPropertyChanged
    {
        private List<Notice> userNotices = new List<Notice> { };

        public List<Notice> UserNotices
        {
            get { return userNotices; }
            set { userNotices = value; RaisePropertyChanged(nameof(UserNotices)); }
        }

        public Teacher()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            DbConnection db = new DbConnection();
            UserNotices = await db.GetNotices();
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}