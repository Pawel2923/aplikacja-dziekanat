using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System.Diagnostics;
using System;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        public Ogloszenia()
        {
            InitializeComponent();
            GetNotices("it-s-2-1");
        }

        private async void GetNotices(string classId)
        {
            DbConnection connection = new DbConnection(AppInfo.DatabaseUrl);

            try
            {
                var notices = await connection.GetNotice(classId);

                foreach (var notice in notices)
                {
                    Debug.WriteLine("Tytuł ogłoszenia: " + notice.Title);
                    Debug.WriteLine("Zawartość ogłoszenia: " + notice.Content);
                    Debug.WriteLine("Autor: " + notice.Author);
                    Debug.WriteLine("Data utworzenia: " + notice.Date);
                    Debug.WriteLine("Adresaci: " + notice.To);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Nie można pobrać listy ogłoszeń: {ex.Message}");
            }
        }
    }
}