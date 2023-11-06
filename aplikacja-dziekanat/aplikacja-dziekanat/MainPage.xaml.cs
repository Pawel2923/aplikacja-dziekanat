using aplikacja_dziekanat.pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace aplikacja_dziekanat
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            OnInit();
        }

        async public void OnInit ()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        async public void LogoutClickHandler(object sender, EventArgs e)
        {
            IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

            auth.Logout();
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
