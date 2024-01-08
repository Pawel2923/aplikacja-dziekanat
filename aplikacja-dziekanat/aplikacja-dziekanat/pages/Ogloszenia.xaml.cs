using db;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ogloszenia : ContentPage
    {
        private readonly DbConnection dbConnection = new DbConnection();
        private List<User> users = new List<User>();
        private List<Notice> notices = new List<Notice>();
        private Image plusImage;
        private Entry titleEntry;
        private Entry contentEntry;
        private Button submitButton;
        private StackLayout noticesLayout;
        private readonly TapGestureRecognizer plusImageTapGestureRecognizer;

        public Ogloszenia()
        {
            InitializeComponent();
            plusImageTapGestureRecognizer = new TapGestureRecognizer();
            plusImageTapGestureRecognizer.Tapped += OnPlusImageTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            if (auth.CurrentUser.Uid != null)
            {
                if (auth.CurrentUser.Role == "admin" || auth.CurrentUser.Role == "teacher")
                {
                    DodajObrazPlusa();
                }
                _ = GetNotices();
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

                submitButton = new Button
                {
                    Text = "Dodaj ogłoszenie",
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.End,
                    IsVisible = false
                };

                submitButton.Clicked += OnSubmitButtonClicked;

                plusImage.GestureRecognizers.Add(plusImageTapGestureRecognizer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd w DodajObrazPlusa: " + ex.Message);
            }
        }

        private async void OnPlusImageTapped(object sender, EventArgs e)
        {
            try
            {
                string title = await DisplayPromptAsync("Nowe ogłoszenie", "Wprowadź tytuł ogłoszenia");
                string content = await DisplayPromptAsync("Nowe ogłoszenie", "Wprowadź treść ogłoszenia");
                string author = await DisplayPromptAsync("Nowe ogłoszenie", "Wprowadź autora ogłoszenia");

                if (title != null && content != null && author != null)
                {

                    string[] classOptions = { "it-s-2-1", "mt-s-1-1", "it-n-2-2", "til-s-1-1" };
                    string selectedClass = await DisplayActionSheet("Wybierz Rok", "Anuluj", null, classOptions);

                    if (selectedClass != "Anuluj")
                    {
                        Notice newNotice = new Notice
                        {
                            Content = content,
                            Title = title,
                            Author = author,
                            Date = DateTime.Now.Date.ToString("dd.MM.yyyy"),
                            To = selectedClass
                        };


                        await dbConnection.SendNotice(newNotice);


                        await GetNotices();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd w OnPlusImageTapped: " + ex.Message);
            }
        }



        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            try
            {
                titleEntry.IsVisible = false;
                contentEntry.IsVisible = false;
                submitButton.IsVisible = false;

                string title = titleEntry.Text;
                string content = contentEntry.Text;

                var auth = DependencyService.Resolve<IFirebaseAuth>();

                string author = $"{auth.CurrentUser.Profile.FirstName} {auth.CurrentUser.Profile.LastName}";

                Notice newNotice = new Notice
                {
                    Content = content,
                    Title = title,
                    Author = author,
                    Date = DateTime.Now.ToString("dd.MM.yyyy")
                };

                notices.Add(newNotice);

                PrintNotices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd w OnSubmitButtonClicked: " + ex.Message);
            }
        }

        public async Task GetNotices()
        {
            try
            {
                var auth = DependencyService.Resolve<IFirebaseAuth>();
                notices = await dbConnection.GetNotices(auth.CurrentUser.ClassId);

                PrintNotices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                notices.Add(new Notice());
                notices[0].Content = "Brak nowych ogłoszeń";

                Device.BeginInvokeOnMainThread(() =>
                {
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (plusImage != null)
                    {
                        Content = new Grid
                        {
                            Children = {
                            new ScrollView
                            {
                                Content = new StackLayout
                                {
                                    Children = { noticesLayout, titleEntry, contentEntry, submitButton }
                                }
                            },
                            plusImage
                        }
                        };
                    }
                    else
                    {
                        Content = new Grid
                        {
                            Children = {
                            new ScrollView
                            {
                                Content = new StackLayout
                                {
                                    Children = { noticesLayout }
                                }
                            }
                        }
                        };
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd w PrintNotices: " + ex.Message);
            }
        }
    }
}