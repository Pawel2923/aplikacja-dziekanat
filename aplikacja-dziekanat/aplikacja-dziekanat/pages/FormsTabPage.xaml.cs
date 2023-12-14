using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormsTabPage : FlyoutPage
    {
        public FormsTabPage()
        {
            InitializeComponent();
        }
        private void Handle_Clicked(object sender, System.EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[0];
            IsPresented = false;

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

        async public void NavigateToLoginPage()
        {
            await Navigation.PushAsync(new LoginPage());
        }

        async public void LogoutClickHandler(object sender, EventArgs e)
        {
            IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();

            auth.Logout();
            IsPresented = false;
            await Navigation.PopAsync();
        }

        public void MenuBtnTapHandler(object sender, EventArgs e)
        {
            IsPresented = !IsPresented;
        }

        public void LogoTapHandler(object sender, EventArgs e)
        {
            MainTabbedPage.CurrentPage = MainTabbedPage.Children[0];
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.CurrentUser.Uid == null)
            {
                NavigateToLoginPage();
            }
        }
    }
}
