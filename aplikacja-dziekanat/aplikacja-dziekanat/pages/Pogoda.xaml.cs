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

                            // Update the background image based on weather conditions
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                UpdateBackgroundImage(weatherData.Weather?[0]?.Description);
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _viewModel.ErrorMessage = "Błąd podczas przetwarzania danych pogodowych.";
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _viewModel.ErrorMessage = "Błąd podczas pobierania danych pogodowych.";
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _viewModel.ErrorMessage = $"Wystąpił błąd: {ex.Message}";
                });
            }
        }

        private void UpdateBackgroundImage(string weatherDescription)
        {
            string backgroundImage = "basic.jpg"; // Default background image

            // Update background image based on weather conditions
            if (!string.IsNullOrEmpty(weatherDescription))
            {
                if (weatherDescription.ToLower().Contains("clear"))
                {
                    backgroundImage = "slonce.jpg";
                }
                else if (weatherDescription.ToLower().Contains("cloud"))
                {
                    backgroundImage = "zachmurzeniezima.jpg";
                }
                // Add more conditions for other weather descriptions as needed
            }

            // Set the background image
            Grid grid = new Grid();

            // Add the background image as the first element
            grid.Children.Add(new Image
            {
                Source = backgroundImage,
                Aspect = Aspect.AspectFill,
                InputTransparent = true
            });

            // Add existing UI elements as the second element
            grid.Children.Add(new StackLayout
            {
                Children =
                {
                    // Add your existing UI elements here
                    new Label { Text = "Miejsce:", FontAttributes = FontAttributes.Bold, TextColor = Color.White },
                    new Label { Text = _viewModel.City, TextColor = Color.White },
                    new Label {Text ="Temperatura:",FontAttributes = FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.Temperature, TextColor = Color.White},
                    new Label {Text="Wilgotność:",FontAttributes=FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.Humidity, TextColor = Color.White},
                     new Label { Text = "Ciśnienie:", FontAttributes = FontAttributes.Bold, TextColor = Color.White },
                    new Label { Text = _viewModel.Pressure, TextColor = Color.White },
                    new Label {Text ="Wiatr:",FontAttributes = FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.WindSpeed, TextColor = Color.White},
                    new Label {Text="Kierunek wiatru:",FontAttributes=FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.WindDirection, TextColor = Color.White},
                       new Label { Text = "Zachmurzenie:", FontAttributes = FontAttributes.Bold, TextColor = Color.White },
                    new Label { Text = _viewModel.Cloudiness, TextColor = Color.White },
                    new Label {Text ="Widoczność:",FontAttributes = FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.Visibility, TextColor = Color.White},
                    new Label {Text="Pogoda:",FontAttributes=FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.WeatherCondition, TextColor = Color.White},
                      new Label {Text="Ostatnia aktualizacja:",FontAttributes=FontAttributes.Bold,TextColor = Color.White},
                    new Label {Text=_viewModel.LastUpdate, TextColor = Color.White},


                    // ... Add other labels for existing UI elements
                }
            });

            // Set the Content of the page to the created Grid container
            Device.BeginInvokeOnMainThread(() =>
            {
                Content = grid;
            });
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
