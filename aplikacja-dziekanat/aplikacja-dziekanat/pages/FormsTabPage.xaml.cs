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
        
       
    }
}