﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        private readonly IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();
        private readonly DbConnection connection = new DbConnection(AppInfo.DatabaseUrl);
        private List<User> users = new List<User>();
        private List<Notice> notices = new List<Notice>();
        private Image plusImage;
        private Entry titleEntry;
        private Entry contentEntry;
        private Entry authorEntry;
        private Button submitButton;
        private StackLayout noticesLayout;

        public Ogloszenia()
        {
            InitializeComponent();
            DodajObrazPlusa();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (auth.Uid() != null)
            {
                GetNotices();
            }
        }

        private void PrintNotices()
        {
            try
            {
                noticesLayout = new StackLayout();
                var frames = new List<Frame>();
                Debug.WriteLine("Liczba notices: " + notices.Count);
                foreach (var notice in notices)
                {
                    Label pageTitle = new Label
                    {
                        Text = notice.Title,
                        FontSize = 24,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(0, 10, 0, 10),
                        TextDecorations = TextDecorations.Underline,
                        TextTransform = TextTransform.Uppercase,
                        HorizontalTextAlignment = TextAlignment.Center
                    };

                    Label ogloszenieLabel = new Label
                    {
                        Text = notice.Content,
                        FontSize = 18,
                        Margin = new Thickness(10),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                    Label dataLabel = new Label
                    {
                        Text = "Data utworzenia: " + notice.Date,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                    Label authorLabel = new Label
                    {
                        Text = "Autor: " + notice.Author,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };

                    Frame frame = new Frame
                    {
                        Content = new StackLayout
                        {
                            Children = { pageTitle, ogloszenieLabel, dataLabel, authorLabel }
                        },
                        HasShadow = true,
                        Padding = new Thickness(15),
                        Margin = new Thickness(20),
                        CornerRadius = 10
                    };

                    frames.Add(frame);
                }

                foreach (var frame in frames)
                {
                    noticesLayout.Children.Add(frame);
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Content = new Grid
                    {
                        Children = {
                            new ScrollView
                            {
                                Content = new StackLayout
                                {
                                    Children = { noticesLayout, titleEntry, contentEntry, authorEntry, submitButton }
                                }
                            },
                            plusImage
                        }
                    };
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in PrintNotices: " + ex.Message);
            }
        }

        private async void GetNotices()
        {
            try
            {
                users = await connection.GetUsers();
                string classId = connection.FindClassId(auth.Email(), users);

                
                notices.Clear();

                notices = await connection.GetNotice(classId);

                PrintNotices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetNotices: " + ex.Message);
                .
            }
        }

        private void DodajObrazPlusa()
        {
            try
            {
                plusImage = new Image
                {
                    Source = "plus.png",
                    WidthRequest = 40,
                    HeightRequest = 40,
                    Margin = new Thickness(0, 0, 20, 20),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.End
                };

                titleEntry = new Entry
                {
                    Placeholder = "Tytuł ogłoszenia...",
                    Margin = new Thickness(20, 0, 20, 0),
                    IsVisible = false 
                };

                contentEntry = new Entry
                {
                    Placeholder = "Treść ogłoszenia...",
                    Margin = new Thickness(20, 0, 20, 0),
                    IsVisible = false 
                };

                authorEntry = new Entry
                {
                    Placeholder = "Twój nick...",
                    Margin = new Thickness(20, 0, 20, 0),
                    IsVisible = false 
                };

                submitButton = new Button
                {
                    Text = "Dodaj ogłoszenie",
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.End,
                    IsVisible = false 
                };

                submitButton.Clicked += OnSubmitButtonClicked;

                

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    
                    titleEntry.IsVisible = !titleEntry.IsVisible;
                    contentEntry.IsVisible = !contentEntry.IsVisible;
                    authorEntry.IsVisible = !authorEntry.IsVisible;
                    submitButton.IsVisible = !submitButton.IsVisible;
                    
                };
                plusImage.GestureRecognizers.Add(tapGestureRecognizer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in DodajObrazPlusa: " + ex.Message);
            }
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            try
            {
                titleEntry.IsVisible = false;
                contentEntry.IsVisible = false;
                authorEntry.IsVisible = false;
                submitButton.IsVisible = false;
                

                string title = titleEntry.Text;
                string content = contentEntry.Text;
                string author = authorEntry.Text;

                Notice newNotice = new Notice
                {
                    Content = content,
                    Title = title,
                    Author = author,
                    Date = DateTime.Now.ToString() 
                };

                
                notices.Add(newNotice);

                
                PrintNotices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in OnSubmitButtonClicked: " + ex.Message);
            }
        }

        private void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Tutaj dodaj logikę obsługi przycisku "Wyślij", jeśli potrzebujesz
            // Na przykład, możesz wysłać treść ogłoszenia do serwera lub innej akcji
            // Po zakończeniu przetwarzania ukryj przycisk "Wyślij" i ewentualnie inne pola
         submitButton.IsVisible = false;
        } 

        // Dodaj dalsze metody, takie jak OnSendButtonClicked itd., jeśli są potrzebne
    }
}

