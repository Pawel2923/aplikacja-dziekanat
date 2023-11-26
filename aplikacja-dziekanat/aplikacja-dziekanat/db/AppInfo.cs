namespace db
{
    class AppInfo 
    {
        private static readonly string databaseUrl = "https://aplikacja-dziekanat-default-rtdb.europe-west1.firebasedatabase.app/";

        public static string DatabaseUrl { get { return databaseUrl; } }
    };
}