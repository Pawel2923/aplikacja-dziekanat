using Xamarin.Forms;

namespace aplikacja_dziekanat
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            // flayout1.listview1.ItemSelected += OnSelectedItem;
        }
    }


    //private void OnSelectedItem(
    //    object sender, SelectedItemChangedEventArgs e)
    //{
    //    var item = e.SelectedItem as flayout1;
    //    if (item != null)
    //    {
    //        NavigationPage Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
    //        flayout1.listview1.SelectedItem = null;
    //        bool IsPresented = false;
    //    }
    //}
}
