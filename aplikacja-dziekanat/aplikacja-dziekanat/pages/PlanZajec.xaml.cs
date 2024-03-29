﻿using db;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanZajec : ContentPage
    {
        private DateTime currentDate;
        private readonly StackLayout mainStackLayout;
        private readonly StackLayout buttonsStackLayout;
        private readonly DbConnection dbConnection = new DbConnection();
        private IFirebaseAuth auth = DependencyService.Resolve<IFirebaseAuth>();

        public PlanZajec()
        {
            InitializeComponent();
            InitializeListView();
            currentDate = DateTime.Now;
            UpdateCurrentDate();
            Device.StartTimer(TimeSpan.FromMinutes(30), () =>
            {
                UpdateCurrentDate();
                return true;
            });
            var calendarIcon = new Image
            {
                Source = "calendar.png",
                HeightRequest = 30,
                WidthRequest = 30,
                Margin = new Thickness(0, 0, 0, 10)
            };
            var datePicker = new DatePicker();

            var showDatePickerButton = new Button
            {
                Text = "Pokaż Kalendarz"
            };
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
                GetSchedule();

                grid.IsVisible = false;
            };

            datePicker.IsVisible = false;

            var previousDayButton = new Button
            {
                Text = "Poprzedni dzień",
                Style = (Style)Application.Current.Resources["btnPrimaryStyle"],
                Margin = new Thickness(0, 0, 5, 10)
            };
            previousDayButton.Clicked += (sender, e) => ScrollToPreviousDay();

            var nextDayButton = new Button
            {
                Text = "Następny dzień",
                Style = (Style)Application.Current.Resources["btnPrimaryStyle"],
                Margin = new Thickness(5, 0, 0, 10)
            };
            nextDayButton.Clicked += (sender, e) => ScrollToNextDay();

            buttonsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { previousDayButton, calendarIcon, nextDayButton, grid }
            };

            mainStackLayout = new StackLayout
            {
                Children = { aktualnaData, lessonListView, buttonsStackLayout }
            };

            Content = mainStackLayout;
            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                if (string.IsNullOrEmpty(auth.CurrentUser?.ClassId))
                {
                    return true;
                }
                GetSchedule();
                return false;
            });
        }
        private void UpdateCurrentDate()
        {
            aktualnaData.Text = currentDate.ToString("dddd, dd.MM.yyyy");
        }

        private void ScrollToNextDay()
        {
            currentDate = currentDate.AddDays(1);
            UpdateCurrentDate();
            GetSchedule();
        }

        private void ScrollToPreviousDay()
        {
            currentDate = currentDate.AddDays(-1);
            UpdateCurrentDate();
            GetSchedule();
        }

        private async void GetSchedule()
        {
            if (auth.Token() == null)
            {
                return;
            }

            try
            {
                string day = currentDate.DayOfWeek.ToString();
                var schedule = await dbConnection.GetSchedule(auth.CurrentUser.ClassId, day);

                Debug.WriteLine($"Pobrano {schedule?.Count ?? 0} rekordów z bazy danych dla dnia {day}");

                if (schedule != null && schedule.Count > 0)
                {
                    if (!mainStackLayout.Children.Contains(lessonListView))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mainStackLayout.Children.Clear();
                            mainStackLayout.Children.Add(aktualnaData);
                            mainStackLayout.Children.Add(lessonListView);
                            mainStackLayout.Children.Add(buttonsStackLayout);
                        });
                    }

                    schedule = schedule.OrderBy(item =>
                    {
                        var provider = new NumberFormatInfo
                        {
                            NumberDecimalSeparator = "."
                        };
                        return Convert.ToDouble(item.TimeStart.Replace(':', '.'), provider);
                    }).ToList();

                    foreach (var item in schedule)
                    {
                        item.ClassType = "Rodzaj zajęć: " + item.ClassType;
                        item.Duration = "Czas trwania: " + item.Duration + "h";
                        item.Name = "Nazwa: " + item.Name;
                        item.Room = "Sala: " + item.Room;
                        item.Teacher = "Prowadzący: " + item.Teacher;
                        item.TimeStart = "Godzina rozpoczęcia: " + item.TimeStart;
                    }

                    Device.BeginInvokeOnMainThread(() => lessonListView.ItemsSource = schedule);
                }
                else if (schedule.Count == 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lessonListView.ItemsSource = null;
                        mainStackLayout.Children.Clear();
                        mainStackLayout.Children.Add(aktualnaData);
                        mainStackLayout.Children.Add(new Label
                        {
                            Text = "Brak zajęć",
                            TextColor = Color.FromHex("#aaaaaa"),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                        });
                        mainStackLayout.Children.Add(buttonsStackLayout);
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", ex.Message, "OK");
                Debug.WriteLine("Exception: " + ex);
            }
        }

        private bool isExpanded = false;
        private void InitializeListView()
        {

            lessonListView.ItemTemplate = new DataTemplate(() =>
            {
                var timeStartLabel = new Label
                {
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                timeStartLabel.SetBinding(Label.TextProperty, "TimeStart");
                var nameLabel = new Label
                {
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontAttributes = FontAttributes.Bold

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
                var durationLabel = new Label
                {
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };

                durationLabel.SetBinding(Label.TextProperty, "Duration");
                var classTypeLabel1 = new Label
                {
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                classTypeLabel1.SetBinding(Label.TextProperty, "ClassType");

                var grid = new Grid
                {
                    Margin = new Thickness(10),
                    RowDefinitions = new RowDefinitionCollection

            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = 0 },
                new RowDefinition { Height = 0 },
                new RowDefinition { Height = 0 }
            },
                    ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star }
            }

                };
                grid.Children.Add(timeStartLabel, 0, 0);
                grid.Children.Add(nameLabel, 0, 1);
                grid.Children.Add(roomLabel, 0, 2);
                grid.Children.Add(teacherLabel, 0, 3);
                grid.Children.Add(durationLabel, 0, 4);
                grid.Children.Add(classTypeLabel1, 0, 5);
                var viewCell = new ViewCell { View = grid };
                viewCell.Tapped += (sender, e) =>
                {
                    Console.WriteLine("Tapped event triggered");
                    grid.RowDefinitions[3].Height = isExpanded ? 0 : GridLength.Auto;
                    grid.RowDefinitions[4].Height = isExpanded ? 0 : GridLength.Auto;
                    grid.RowDefinitions[5].Height = isExpanded ? 0 : GridLength.Auto;

                    isExpanded = !isExpanded;
                };

                return viewCell;
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

