// PogodaViewModel.cs
using System;
using System.ComponentModel;

namespace aplikacja_dziekanat.pages.ViewModels
{
    public class PogodaViewModel : INotifyPropertyChanged
    {
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        private string _temperature;
        public string Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        private string _feelsLike;
        public string FeelsLike
        {
            get { return _feelsLike; }
            set
            {
                if (_feelsLike != value)
                {
                    _feelsLike = value;
                    OnPropertyChanged(nameof(FeelsLike));
                }
            }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    OnPropertyChanged(nameof(Humidity));
                }
            }
        }

        private string _pressure;
        public string Pressure
        {
            get { return _pressure; }
            set
            {
                if (_pressure != value)
                {
                    _pressure = value;
                    OnPropertyChanged(nameof(Pressure));
                }
            }
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get { return _windSpeed; }
            set
            {
                if (_windSpeed != value)
                {
                    _windSpeed = value;
                    OnPropertyChanged(nameof(WindSpeed));
                }
            }
        }

        private string _windDirection;
        public string WindDirection
        {
            get { return _windDirection; }
            set
            {
                if (_windDirection != value)
                {
                    _windDirection = value;
                    OnPropertyChanged(nameof(WindDirection));
                }
            }
        }

        private string _cloudiness;
        public string Cloudiness
        {
            get { return _cloudiness; }
            set
            {
                if (_cloudiness != value)
                {
                    _cloudiness = value;
                    OnPropertyChanged(nameof(Cloudiness));
                }
            }
        }

        private string _visibility;
        public string Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged(nameof(Visibility));
                }
            }
        }

        private string _precipitation;
        public string Precipitation
        {
            get { return _precipitation; }
            set
            {
                if (_precipitation != value)
                {
                    _precipitation = value;
                    OnPropertyChanged(nameof(Precipitation));
                }
            }
        }

        private string _weatherCondition;
        public string WeatherCondition
        {
            get { return _weatherCondition; }
            set
            {
                if (_weatherCondition != value)
                {
                    _weatherCondition = value;
                    OnPropertyChanged(nameof(WeatherCondition));
                }
            }
        }

        private string _lastUpdate;
        public string LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                if (_lastUpdate != value)
                {
                    _lastUpdate = value;
                    OnPropertyChanged(nameof(LastUpdate));
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
