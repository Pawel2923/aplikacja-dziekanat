using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using db;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        private readonly IFirebaseAuth auth = DependencyService.Get<IFirebaseAuth>();
        private readonly DbConnection connection = new DbConnection(AppInfo.DatabaseUrl);
        private List<User> users = new List<User>();
        private Notice notice = new Notice();

        public Ogloszenia()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (auth.Uid() != null)
            {
                GetNotices();
            }
        }

        private async void GetNotices()
        {
            bool noticesFound = false;
            try
            {
                users = await connection.GetUsers();
                string classId = connection.FindClassId(auth.Email(), users);
                var notices = await connection.GetNotice(classId);

                foreach (var notice in notices)
                {
                    this.notice = notice;
                }

                noticesFound = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                notice.Content = "Brak nowych ogłoszeń";
            }

            if (!noticesFound)
            {
                Device.BeginInvokeOnMainThread(() => {
                    Label ogloszenieLabel = new Label
                    {
                        Text = notice.Content,
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
                return;
            }

            Device.BeginInvokeOnMainThread(() => {
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


                Content = new StackLayout
                {
                    Children = { frame }
                };
            });
        }
    }
}