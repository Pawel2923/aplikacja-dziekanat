using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
       
        private string aktualneOgloszenie = "Ministerstwo Edukacji i Nauki przygotowało broszurę informacyjną skierowaną do studentów, dotyczącą możliwości ubiegania się na studiach o wsparcie finansowe, w ramach bezzwrotnych świadczeń, w formie stypendiów i zapomóg finansowanych z budżetu państwa oraz kredytów studenckich z dopłatami do oprocentowania.";

        public Ogloszenia()
        {
            InitializeComponent();


            Label ogloszenieLabel = new Label
            {
                Text = aktualneOgloszenie,
                FontSize = 18,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

        
            Frame frame = new Frame
            {
                Content = ogloszenieLabel,
                HasShadow = true,
                Padding = new Thickness(15),
                Margin = new Thickness(20),
                CornerRadius = 10
            };

            
            Content = new StackLayout
            {
                Children = { frame }
            };
        }
    }
}
