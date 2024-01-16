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

            // Add the background image as the first element, spanning the entire grid
            grid.Children.Add(new Image
            {
                Source = backgroundImage,
                Aspect = Aspect.AspectFill,
                InputTransparent = true,
            }, 0, 0); // This sets the image to span the entire grid

            // Create a stack layout for the text and other information in the center
            var centerStackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            // Add the city name inside a black frame
            centerStackLayout.Children.Add(CreateDataLabel("Nowy Sącz", "", 32, 0, 0, 10));

            // Add the rest of the information in two columns
            centerStackLayout.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,

                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Children =
                        {
                            // Wrap each data field in a black frame
                            CreateDataLabel("Temperatura:", _viewModel.Temperature, 16, 18, 0, 10),
                            CreateDataLabel("Wilgotność:", _viewModel.Humidity, 16, 18, 0, 10),
                            CreateDataLabel("Ciśnienie:", _viewModel.Pressure, 16, 18, 0, 10),
                            CreateDataLabel("Wiatr:", _viewModel.WindSpeed, 16, 18, 0, 10),
                        }
                    },
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Children =
                        {
                            // Wrap each data field in a black frame
                            CreateDataLabel("Kierunek wiatru:", _viewModel.WindDirection, 16, 18, 0, 10),
                            CreateDataLabel("Zachmurzenie:", _viewModel.Cloudiness, 16, 18, 0, 10),
                            CreateDataLabel("Widoczność:", _viewModel.Visibility, 16, 18, 0, 10),
                            CreateDataLabel("Pogoda:", _viewModel.WeatherCondition, 16, 18, 0, 10),
                        }
                    }
                }
            });

            // Add the last update information inside a black frame
            centerStackLayout.Children.Add(CreateDataLabel("Ostatnia aktualizacja:", _viewModel.LastUpdate, 16, 18,
                    0, 10));

            // Add the centerStackLayout to the grid
            grid.Children.Add(centerStackLayout, 0, 0);

            // Set the Content of the page to the created Grid container
            Device.BeginInvokeOnMainThread(() =>
            {
                Content = grid;
            });
        }

        private Frame CreateDataLabel(string labelText, string value, int labelFontSize, int valueFontSize, int marginTop, int marginBottom)
        {
            return new Frame
            {
                BackgroundColor = Color.Black,
                Padding = new Thickness(10),
                CornerRadius = 10,
                Margin = new Thickness(0, marginTop, 0, marginBottom),
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children =
                    {
                        new Label { Text = labelText, FontAttributes = FontAttributes.Bold, TextColor = Color.White, FontSize = labelFontSize },
                        new Label { Text = value, TextColor = Color.White, FontSize = valueFontSize },
                    }
                }
            };
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();
        }
    }
}
