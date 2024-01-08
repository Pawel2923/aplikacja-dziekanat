// Pogoda.xaml.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using aplikacja_dziekanat.pages.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using aplikacja_dziekanat.pages.models;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pogoda : ContentPage
    {
        private PogodaViewModel _viewModel;

        public Pogoda()
        {
            InitializeComponent();
            _viewModel = new PogodaViewModel();
            BindingContext = _viewModel;
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

                        if (weatherData != null)
                        {
                            _viewModel.City = $"{city}";

                            float temperaturaWKelwinach = weatherData.Main?.Temp ?? 0;
                            float temperaturaWCelsjuszach = temperaturaWKelwinach - 273.15f;

                            _viewModel.Temperature = $"{temperaturaWCelsjuszach}°C";
                            _viewModel.FeelsLike = $"{weatherData.Main?.FeelsLike}°C";
                            _viewModel.Humidity = $" {weatherData.Main?.Humidity}%";
                            _viewModel.Pressure = $" {weatherData.Main?.Pressure} hPa";

                            _viewModel.WindSpeed = $" {weatherData.Wind?.Speed} m/s";
                            _viewModel.WindDirection = $" {GetWindDirectionName(weatherData.Wind?.Direction)}";

                            _viewModel.Cloudiness = $"{weatherData.Clouds?.Value}%";
                            _viewModel.Visibility = $" {weatherData.Visibility} m";
                            
                            _viewModel.WeatherCondition = $" {weatherData.Weather?[0]?.Description}";

                            _viewModel.LastUpdate = $" {DateTime.Now}";
                        }
                        else
                        {
                            _viewModel.ErrorMessage = "Błąd podczas przetwarzania danych pogodowych.";
                        }
                    }
                    else
                    {
                        _viewModel.ErrorMessage = "Błąd podczas pobierania danych pogodowych.";
                    }
                }
            }
            catch (Exception ex)
            {
                _viewModel.ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }
        }

        private string GetWindDirectionName(int? direction)
        {
            if (direction.HasValue)
            {
                if ((direction >= 0 && direction < 45) || (direction >= 315 && direction <= 360))
                    return "Północny";
                else if (direction >= 45 && direction < 135)
                    return "Wschodni";
                else if (direction >= 135 && direction < 225)
                    return "Południowy";
                else if (direction >= 225 && direction < 315)
                    return "Zachodni";
            }

            return "Nieznany";
        }
    }
}
