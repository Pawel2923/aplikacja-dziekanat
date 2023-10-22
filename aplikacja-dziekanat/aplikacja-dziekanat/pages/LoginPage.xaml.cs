using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async public void SignupClickHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}