using db;
using System;
using System.Collections.Generic;
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
            var calendarIcon = new Image
            {
                Source = "kanedarz1.jpg", 
                HeightRequest = 30, 
                WidthRequest = 30 
            };
            var datePicker = new DatePicker();

            var showDatePickerButton = new Button { Text = "Pokaż Kalendarz" };
            showDatePickerButton.Clicked += (sender, e) =>
            {
                datePicker.Focus();
            };
            
            var grid = new Grid
            {
                IsVisible = false,
                Children =
    {
        datePicker
    }
            };

            calendarIcon.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    grid.IsVisible = true;
                    datePicker.Focus();
                })
            });

            datePicker.DateSelected += (sender, e) =>
            {
                currentDate = e.NewDate; 
                UpdateCurrentDate();
                GetSchedule("it-s-2-1", currentDate.DayOfWeek.ToString()); 

                grid.IsVisible = false; 
            };

            var previousDayButton = new Button { Text = "Poprzedni dzień" };
            previousDayButton.Clicked += (sender, e) => ScrollToPreviousDay();

            var nextDayButton = new Button { Text = "Następny dzień" };
            nextDayButton.Clicked += (sender, e) => ScrollToNextDay();

            var buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { previousDayButton,calendarIcon, nextDayButton,grid}
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
            GetSchedule("it-s-2-1", currentDate.DayOfWeek.ToString());

        }

        private void ScrollToPreviousDay()
        {
            currentDate = currentDate.AddDays(-1);
            UpdateCurrentDate();
            GetSchedule("it-s-2-1", currentDate.DayOfWeek.ToString());
        }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (auth.Uid() != null)
            {
                GetSchedule("it-s-2-1", DateTime.Now.DayOfWeek.ToString());
            }
        }

        private async void GetSchedule(string classId, string day)
        {
            connection = new DbConnection();
            var schedule = await connection.GetSchedule(classId, day);

            Debug.WriteLine($"Pobrano {schedule?.Count ?? 0} rekordów z bazy danych dla dnia {day}");

            if (schedule != null)
            {
                schedule = schedule.OrderBy(item => item.TimeStart).ToList();
                
                foreach (var item in schedule)
                {
                    item.ClassType = "Rodzaj zajęć: " + item.ClassType;
                    item.Duration = "Czas trwania: " + item.Duration + "h";
                    item.Name = "Nazwa: " + item.Name;
                    item.Room = "Sala: " + item.Room;
                    item.Teacher = "Prowadzący: " + item.Teacher;
                    item.TimeStart = "Godzina rozpoczęcia: " + item.TimeStart;
                }

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


               
                var grid = new Grid

                {
                    Margin = new Thickness(10),
                    RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
                    ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star }
            }
                };

                grid.Children.Add(classTypeLabel1, 0, 0);
                grid.Children.Add(durationLabel, 0, 1);
                grid.Children.Add(nameLabel, 0, 2);
                grid.Children.Add(timeStartLabel, 0, 3);
                grid.Children.Add(roomLabel, 0, 4);
                grid.Children.Add(teacherLabel, 0, 5);

                return new ViewCell { View = grid };
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
