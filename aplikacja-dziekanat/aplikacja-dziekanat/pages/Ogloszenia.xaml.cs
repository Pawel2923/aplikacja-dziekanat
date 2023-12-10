﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        private List<User> users = new List<User>();
        private List<Notice> notices = new List<Notice>();

        public Ogloszenia()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.Uid() != null)
            {
                GetNotices();
            }
        }

        private void PrintNotices()
        {
            try
            {
                var noticesLayout = new StackLayout();
                var frames = new List<Frame>();
                Debug.WriteLine("Liczba notices: " + notices.Count);
                foreach (var notice in notices)
                {
                    Label pageTitle = new Label
                    {
                        Text = notice.Title,
                        TextColor = Color.Black,
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
                        TextColor = Color.Black,
                        FontSize = 18,
                        Margin = new Thickness(10),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand

                    };
                    Label dataLabel = new Label
                    {
                        Text = "Data utworzenia: " + notice.Date,
                        TextColor = Color.Black,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
        Label authorLabel = new Label
        {
            Text = "Autor: " + notice.Author,
            TextColor = Color.Black,
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

                Device.BeginInvokeOnMainThread(() => {
                    Content = new ScrollView
                    {
                        Content = noticesLayout
                    };
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void GetNotices()
        {
            try
            {
                var auth = DependencyService.Resolve<IFirebaseAuth>();
                users = await DbConnection.GetUsers();
                string classId = DbConnection.FindClassId(auth.Email(), users);
                notices = await DbConnection.GetNotice(classId);

                PrintNotices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                notices.Add(new Notice());
                notices[0].Content = "Brak nowych ogłoszeń";

                Device.BeginInvokeOnMainThread(() => {
                    Label ogloszenieLabel = new Label
                    {
                        Text = notices[0].Content,
                        FontSize = 18,
                        Margin = new Thickness(10),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };

                    Frame frame = new Frame
                    {
                        Content = new StackLayout
                        {
                            Children = { ogloszenieLabel }
                        },
                        HasShadow = true,
                        Padding = new Thickness(15),
                        Margin = new Thickness(20),
                        CornerRadius = 10
                    };

                    Content = new StackLayout
                    {
                        Children = { frame }
                    };
                });
            }
        }
    }
}