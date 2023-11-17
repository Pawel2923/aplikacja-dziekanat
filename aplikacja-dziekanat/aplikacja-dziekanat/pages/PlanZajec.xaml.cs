using db;
using System;
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
            InitializeListView();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateCurrentDate();
                return true; 
            });
        }
        private void UpdateCurrentDate()
        {
            aktualnaData.Text = $"Data: {DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss")}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (auth.Uid() != null)
            {
                GetSchedule("it-s-2-1", DateTime.Now.DayOfWeek.ToString());
            }
        }

        private async void GetSchedule(string classId, string date)
        {
            connection = new DbConnection();
            var schedule = await connection.GetSchedule(classId, date);
            if (schedule != null)
            {
                lessonListView.ItemsSource = schedule; 
            }
        }

        private void InitializeListView()
        {
            lessonListView.ItemTemplate = new DataTemplate(() =>
            {
                var classTypeLabel = new Label();
                classTypeLabel.SetBinding(Label.TextProperty, "ClassType");

                var durationLabel = new Label();
                durationLabel.SetBinding(Label.TextProperty, "Duration");

                var nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var roomLabel = new Label();
                roomLabel.SetBinding(Label.TextProperty, "Room");

                var teacherLabel = new Label();
                teacherLabel.SetBinding(Label.TextProperty, "Teacher");

                var timeStartLabel = new Label();
                timeStartLabel.SetBinding(Label.TextProperty, "TimeStart");

                

                var stackLayout = new StackLayout
                {



                    Children = { classTypeLabel, durationLabel, nameLabel, timeStartLabel, roomLabel, teacherLabel }
                };

                var scrollView = new ScrollView { Content = stackLayout };

                return new ViewCell { View = stackLayout };
            });

            lessonListView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Debug.WriteLine("Selected Item: " + e.SelectedItem);
                    lessonListView.SelectedItem = null; 
                }
            };
        }
    }
}
