﻿// Pogoda.xaml.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using aplikacja_dziekanat.pages.models;  // Popraw ścieżkę do przestrzeni nazw Models
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using aplikacja_dziekanat.pages.models;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pogoda : ContentPage
    {
        public Pogoda()
        {
            InitializeComponent();

            // Wywołaj metodę do pobrania danych z API
            GetWeatherData();
        }

        private async void GetWeatherData()
        {
            try
            {
                string apiKey = "05ac3731afc62ef0200bb7a844fefb39";
                string city = "Nowy Sącz";

                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                        // Przekształć temperaturę z kelwinów na stopnie Celsiusza
                        float temperaturaWKelwinach = weatherData.Main.Temp;
                        float temperaturaWCelsjuszach = temperaturaWKelwinach - 273.15f;

                        // Aktualizuj etykietę z danymi pogodowymi
                        weatherLabel.Text = $"Temperatura: {temperaturaWCelsjuszach}°C\nOpis: {weatherData.Weather[0].Description}";
                    }
                    else
                    {
                        // Obsłuż błędy odpowiedzi
                        weatherLabel.Text = "Błąd podczas pobierania danych pogodowych.";
                    }
                }
            }
            catch (Exception ex)
            {
                // Obsłuż wyjątki
                weatherLabel.Text = $"Wystąpił błąd: {ex.Message}";
            }
        }
    }
}
