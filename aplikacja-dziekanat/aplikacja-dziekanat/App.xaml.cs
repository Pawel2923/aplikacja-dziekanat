using aplikacja_dziekanat;
using aplikacja_dziekanat.pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new FormsTabPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
