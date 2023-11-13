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
        private DateTime currentDate;

        public PlanZajec()
        {
            InitializeComponent();
            InitializeListView();
            currentDate = DateTime.Now;
            UpdateCurrentDate();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateCurrentDate();
                return true;
            });

            // Dodaj przyciski do przewijania dni
            var previousDayButton = new Button { Text = "Poprzedni dzień" };
            previousDayButton.Clicked += (sender, e) => ScrollToPreviousDay();

            var nextDayButton = new Button { Text = "Następny dzień" };
            nextDayButton.Clicked += (sender, e) => ScrollToNextDay();

            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { previousDayButton, nextDayButton }
            };

            var mainStackLayout = new StackLayout
            {
                Children = { aktualnaData, lessonListView, buttonsStackLayout }
            };

            Content = mainStackLayout;
        }

        private void UpdateCurrentDate()
        {
            aktualnaData.Text = currentDate.ToString("dddd, dd.MM.yyyy");
        }

        private void ScrollToNextDay()
        {
            currentDate = currentDate.AddDays(1);
            UpdateCurrentDate();
            // Tutaj możesz dodać kod do odświeżenia listy zajęć na nowy dzień
        }

        private void ScrollToPreviousDay()
        {
            currentDate = currentDate.AddDays(-1);
            UpdateCurrentDate();
            // Tutaj możesz dodać kod do odświeżenia listy zajęć na nowy dzień
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
            var schedule = await connection.GetSchedule("it-s-2-1", currentDate.ToString("ddMMyyyy"));
            if (schedule != null)
            {
                lessonListView.ItemsSource = schedule;
            }
        }
    


private void InitializeListView()
        {
            lessonListView.ItemTemplate = new DataTemplate(() =>
            {
                
                
                    var classTypeLabel1 = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                    classTypeLabel1.SetBinding(Label.TextProperty, "ClassType");

                    var durationLabel = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                durationLabel.SetBinding(Label.TextProperty, "Duration");

                    var nameLabel = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                    var roomLabel = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };

                roomLabel.SetBinding(Label.TextProperty, "Room");

                    var teacherLabel = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                teacherLabel.SetBinding(Label.TextProperty, "Teacher");

                    var timeStartLabel = new Label
                    {

                        TextColor = Color.White,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                    };
                timeStartLabel.SetBinding(Label.TextProperty, "TimeStart");

               


                    var stackLayout = new StackLayout
                    {



                        Children = { classTypeLabel1, durationLabel, nameLabel, timeStartLabel, roomLabel, teacherLabel }
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
