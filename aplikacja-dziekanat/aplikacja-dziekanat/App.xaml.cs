using aplikacja_dziekanat.pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new FormsTabPage());
        }

        protected override void OnStart()
        {
            if (Application.Current.MainPage is TabbedPage tabbedPage)
            {
                int indeksPlanZajec = 1;
                int indeksOgloszenia = 2;
                int indeksPogoda = 3;

                if (indeksPlanZajec >= 0 && indeksPlanZajec < tabbedPage.Children.Count &&
                    indeksOgloszenia >= 0 && indeksOgloszenia < tabbedPage.Children.Count &&
                    indeksPogoda >= 0 && indeksPogoda < tabbedPage.Children.Count)
                {
                    // Stworzenie nowej listy zakładek z odpowiednią kolejnością
                    var noweZakladki = new List<Page>
        {
            tabbedPage.Children[indeksPlanZajec],
            tabbedPage.Children[indeksOgloszenia],
            tabbedPage.Children[indeksPogoda]
        };

                    // Ustawienie nowej listy zakładek jako dzieci TabbedPage
                    tabbedPage.Children.Clear();
                    foreach (var zakladka in noweZakladki)
                    {
                        tabbedPage.Children.Add(zakladka);
                    }
                }
            }


        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
