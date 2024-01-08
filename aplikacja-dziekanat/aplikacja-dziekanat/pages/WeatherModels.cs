namespace aplikacja_dziekanat.pages.models
{
    public class WeatherData
    {
        public MainData Main { get; set; }
        public WeatherDescription[] Weather { get; set; }
    }

    public class MainData
    {
        public float Temp { get; set; }
    }

    public class WeatherDescription
    {
        public string Description { get; set; }
    }
}
