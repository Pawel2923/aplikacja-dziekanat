using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class FormsTabPage : MasterDetailPage
{
    public FormsTabPage()
    {
        InitializeComponent();
        OnInit();
    }
        private void Handle_Clicked(object sender, System.EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[0];
            IsPresented =false;

        }
        private void Handle_Clicked1(object sender, System.EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[1];
            IsPresented = false;
        }
        private void Handle_Clicked2(object sender, System.EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[2];
            IsPresented = false;
        }

        private void Handle_Clicked3(object sender, System.EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[3];
            IsPresented = false;
        }

        async public void OnInit()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        async public void LogoutClickHandler(object sender, EventArgs e)
        {
            IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

            auth.Logout();
            IsPresented = false;
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
