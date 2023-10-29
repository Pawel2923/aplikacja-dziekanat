

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
    }
        private void Handle_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new pages.PlanZajec());

        }
        private void Handle_Clicked1(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new pages.Ogloszenia());

        }
        private void Handle_Clicked2(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new pages.Pogoda());

        }
        private void Handle_Clicked3(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new pages.FormsTabPage());

        }
    }
}