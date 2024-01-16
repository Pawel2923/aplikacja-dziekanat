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

        private async void Handle_Clicked4(object sender, System.EventArgs e)
        {
            IsPresented = false;
            await Navigation.PushAsync(new Admin());
        }

        private async void Handle_Clicked5(object sender, System.EventArgs e)
        {
            IsPresented = false;
            await Navigation.PushAsync(new Teacher());
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();
            // Set token if user is logged in
            await auth.SetToken();

            if (auth.Token() == null)
            {
                await Navigation.PushAsync(new LoginPage());
            }

            if (auth.CurrentUser.Role != "admin")
            {
                adminPanelBtn.IsVisible = false;
            }

            if (auth.CurrentUser.Role != "teacher")
            {
                teacherPanelBtn.IsVisible = false;
            }
        }
    }
}
