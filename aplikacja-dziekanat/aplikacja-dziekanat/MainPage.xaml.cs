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
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            OnInit();
           // flayout1.listview1.ItemSelected += OnSelectedItem;
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


    //private void OnSelectedItem(
    //    object sender, SelectedItemChangedEventArgs e)
    //{
    //    var item = e.SelectedItem as flayout1;
    //    if (item != null)
    //    {
    //        NavigationPage Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
    //        flayout1.listview1.SelectedItem = null;
    //        bool IsPresented = false;
    //    }
    //}
}
