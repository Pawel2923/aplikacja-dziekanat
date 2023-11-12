using db;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanZajec : ContentPage
    {
        private readonly IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();
        private DbConnection connection;
        public PlanZajec()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (auth.Uid() != null)
            {
                GetSchedule();
            }
        }

        private async void GetSchedule()
        {
            connection = new DbConnection();
            var schedule = await connection.GetSchedule("it-s-2-1", "13112023");
            if (schedule != null)
            {
                foreach (var item in schedule)
                {
                    Debug.WriteLine("Name: " + item.Name + " TimeStart: " + item.TimeStart + " Duration: " + item.Duration + " ClassType: " + item.ClassType + " Room: " + item.Room + " Teacher: " + item.Teacher);
                }
            }
        }
    }
}