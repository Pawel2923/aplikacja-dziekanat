using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
       
        private string aktualneOgloszenie = "Ministerstwo Edukacji i Nauki przygotowało broszurę informacyjną skierowaną do studentów, dotyczącą możliwości ubiegania się na studiach o wsparcie finansowe, w ramach bezzwrotnych świadczeń, w formie stypendiów i zapomóg finansowanych z budżetu państwa oraz kredytów studenckich z dopłatami do oprocentowania.";

        public Ogloszenia()
        {
            InitializeComponent();

           
            Label pageTitle = new Label
            {
                Text = "Wsparcie finansowe dla studentów",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 10, 0, 10),
                TextDecorations = TextDecorations.Underline,
                TextTransform = TextTransform.Uppercase,
                HorizontalTextAlignment = TextAlignment.Center
               
            };
            
            Label ogloszenieLabel = new Label
            {
                Text = $"{aktualneOgloszenie}",
                FontSize = 18,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand

            };
            Label dataLabel = new Label
            {

                Text = "Data utworzenia: " + DateTime.Now.ToString("dd.MM.yyyy"),
                FontSize = 12, 
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Label authorLabel = new Label
            {
                Text = "Autor: Paweł Kozioł",
                FontSize = 12, 
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

           
            Frame frame = new Frame
            {
                Content = new StackLayout
                {
                    Children = { pageTitle, ogloszenieLabel,dataLabel, authorLabel}
                },
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