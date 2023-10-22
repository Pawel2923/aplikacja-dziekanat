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
    }
}
