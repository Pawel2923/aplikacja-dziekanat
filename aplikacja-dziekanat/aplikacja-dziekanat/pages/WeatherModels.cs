// WeatherData.cs
using System;

namespace aplikacja_dziekanat.pages.models
{
    public class WeatherData
    {
        public MainData Main { get; set; }
        public WindData Wind { get; set; }  // Dodano obiekt WindData
        public CloudsData Clouds { get; set; }  // Dodano obiekt CloudsData
        public int Visibility { get; set; }
        public PrecipitationData Precipitation { get; set; }  // Dodano obiekt PrecipitationData
        public WeatherDescription[] Weather { get; set; }
        public DateTime LastUpdate { get; set; }  // Dodano pole LastUpdate

        public class MainData
        {
            public float Temp { get; set; }
            public float FeelsLike { get; set; }
            public int Humidity { get; set; }
            public int Pressure { get; set; }
        }

        public class WindData
        {
            public float Speed { get; set; }
            public int Direction { get; set; }
        }

        public class CloudsData
        {
            public int Value { get; set; }
        }

        public class PrecipitationData
        {
            public float Value { get; set; }
            public string Mode { get; set; }
        }

        public class WeatherDescription
        {
            public string Description { get; set; }
        }
    }
}
