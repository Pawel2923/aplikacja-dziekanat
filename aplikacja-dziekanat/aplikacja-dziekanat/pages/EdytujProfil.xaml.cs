using CustomRenderer;
using db;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aplikacja_dziekanat.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdytujProfil : ContentPage, INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _address;
        private string _city;
        private string _zipCode;

        public string FirstName { get { return _firstName; } set { _firstName = value; RaisePropertyChanged(nameof(FirstName)); } }
        public string LastName { get { return _lastName; } set { _lastName = value; RaisePropertyChanged(nameof(LastName)); } }
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); } }
        public string Address { get { return _address; } set { _address = value; RaisePropertyChanged(nameof(Address)); } }
        public string City { get { return _city; } set { _city = value; RaisePropertyChanged(nameof(City)); } }
        public string ZipCode { get { return _zipCode; } set { _zipCode = value; RaisePropertyChanged(nameof(ZipCode)); } }

        public EdytujProfil()
        {
            InitializeComponent();
            BindingContext = this;
            ResetForm();
        }

        private void ResetForm()
        {
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            FirstName = auth.CurrentUser.Profile.FirstName;
            firstNameLabel.Text = "";
            firstNameLabel.IsVisible = false;

            LastName = auth.CurrentUser.Profile.LastName;
            lastNameLabel.Text = "";
            lastNameLabel.IsVisible = false;

            PhoneNumber = auth.CurrentUser.Profile.PhoneNumber;
            phoneNumberLabel.Text = "";
            phoneNumberLabel.IsVisible = false;

            Address = auth.CurrentUser.Profile.Address;
            addressLabel.Text = "";
            addressLabel.IsVisible = false;

            City = auth.CurrentUser.Profile.City;
            cityLabel.Text = "";
            cityLabel.IsVisible = false;

            ZipCode = auth.CurrentUser.Profile.ZipCode;
            zipCodeLabel.Text = "";
            zipCodeLabel.IsVisible = false;
        }

        private bool CheckForm()
        {
            Input.Result firstNameResult = firstName.CheckValidity();
            firstNameLabel.Text = firstNameResult.Message;
            firstNameLabel.IsVisible = firstNameResult.Message.Length > 0;

            Input.Result lastNameResult = lastName.CheckValidity();
            lastNameLabel.Text = lastNameResult.Message;
            lastNameLabel.IsVisible = lastNameResult.Message.Length > 0;

            Input.Result phoneNumberResult = phoneNumber.CheckValidity();
            phoneNumberLabel.Text = phoneNumberResult.Message;
            phoneNumberLabel.IsVisible = phoneNumberResult.Message.Length > 0;

            Input.Result addressResult = address.CheckValidity();
            addressLabel.Text = addressResult.Message;
            addressLabel.IsVisible = addressResult.Message.Length > 0;

            Input.Result cityResult = city.CheckValidity();
            cityLabel.Text = cityResult.Message;
            cityLabel.IsVisible = cityResult.Message.Length > 0;

            Input.Result zipCodeResult = zipCode.CheckValidity();
            zipCodeLabel.Text = zipCodeResult.Message;
            zipCodeLabel.IsVisible = zipCodeResult.Message.Length > 0;

            if (firstNameResult.IsValid || lastNameResult.IsValid || phoneNumberResult.IsValid || addressResult.IsValid || cityResult.IsValid || zipCodeResult.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void SaveClickHandler(object sender, EventArgs e)
        {
            if (CheckForm())
            {
                try
                {
                    var dbConnection = new DbConnection();

                    bool isSuccess = await dbConnection.UpdateProfile(new Profile
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        PhoneNumber = PhoneNumber,
                        Address = Address,
                        City = City,
                        ZipCode = ZipCode
                    });

                    if (isSuccess)
                    {
                        await DisplayAlert("Sukces", "Zapisano zmiany", "OK");
                    }
                    else
                    {
                        throw new Exception("Nie udało się zapisać zmian");
                    }

                    ResetForm();
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await DisplayAlert("Błąd", ex.Message, "OK");
                }
            }
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}