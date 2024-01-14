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
        private bool isExpanded = false;

        public Teacher()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetNotices();
        }

        private async void GetNotices()
        {
            DbConnection db = new DbConnection();

            var notices = await db.GetNotices();
        }

        private void EditNoticeClickHandler(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}