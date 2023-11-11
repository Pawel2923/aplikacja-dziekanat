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
public partial class PlanZajec : ContentPage
{

       
        public PlanZajec()
    {
        InitializeComponent();
            var zajecia = GetZajecia();
            lessonListView.ItemsSource = zajecia;


            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateCurrentDate();
                return true; // Powtarzaj co sekundę
            });
        }
        private void UpdateCurrentDate()
        {
            
            aktualnaData.Text = $"Data: {DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss")}";
        }

        private List<Zajecia> GetZajecia()
        {
            // na szytywno wpisane zajęcia
            return new List<Zajecia>
        {
            new Zajecia { Data = "poniedzialek", Godzina = "9:00 ", Przedmiot = "Matematyka" , Sala="0.7"},
            new Zajecia{ Data = "poniedziałek", Godzina = "10:00 ",Przedmiot = "Matematyka w podstawie informatyki", Sala="2.1" },
            
        };

        }
    }
}