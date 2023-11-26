using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        // Przykładowe ogłoszenie (możesz zastąpić własnymi danymi)
        private string aktualneOgloszenie = "Ministerstwo Edukacji i Nauki przygotowało broszurę informacyjną skierowaną do studentów, dotyczącą możliwości ubiegania się na studiach o wsparcie finansowe, w ramach bezzwrotnych świadczeń, w formie stypendiów i zapomóg finansowanych z budżetu państwa oraz kredytów studenckich z dopłatami do oprocentowania.";

        public Ogloszenia()
        {
            InitializeComponent();

            // Dodaj Label do tytułu strony
            Label pageTitle = new Label
            {
                Text = "Wsparcie finansowe dla studentów",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 10, 0, 10)
            };

            // Dodaj Label do wyświetlania ogłoszenia z tytułem i datą utworzenia
            Label ogloszenieLabel = new Label
            {
                Text = $"Tytuł: {pageTitle.Text}\n\n{aktualneOgloszenie}\n\nUtworzono: {DateTime.Now.ToString("dd.MM.yyyy")}",
                FontSize = 18,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // Umieść Label w kontrolce Frame
            Frame frame = new Frame
            {
                Content = ogloszenieLabel,
                HasShadow = true, // Opcjonalne: Dodaj cień do ramki
                Padding = new Thickness(15),
                Margin = new Thickness(20),
                CornerRadius = 10 // Opcjonalne: Zaokrąglij narożniki ramki
            };

            // Dodaj wszystkie elementy do głównego układu strony
            Content = new StackLayout
            {
                Children = { frame }
            };
        }
    }
}
