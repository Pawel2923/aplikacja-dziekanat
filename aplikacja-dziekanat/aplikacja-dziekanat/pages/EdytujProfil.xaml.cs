using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomRenderer;
using System.Diagnostics;
using db;

namespace aplikacja_dziekanat.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdytujProfil : ContentPage
    {
        private Input firstNameInput;
        private Input lastNameInput;
        private Input phoneNumberInput;
        private Input addressInput;
        private Input cityInput;
        private Input zipCodeInput;

        public EdytujProfil()
        {
            InitializeComponent();
            ResetForm();
        }

        private void ResetForm()
        {
            var auth = DependencyService.Resolve<IFirebaseAuth>();

            firstName.Text = auth.CurrentUser.Profile.FirstName;
            firstNameLabel.Text = "";
            firstNameLabel.IsVisible = false;

            lastName.Text = auth.CurrentUser.Profile.LastName;
            lastNameLabel.Text = "";
            lastNameLabel.IsVisible = false;

            phoneNumber.Text = auth.CurrentUser.Profile.PhoneNumber;
            phoneNumberLabel.Text = "";
            phoneNumberLabel.IsVisible = false;

            address.Text = auth.CurrentUser.Profile.Address;
            addressLabel.Text = "";
            addressLabel.IsVisible = false;

            city.Text = auth.CurrentUser.Profile.City;
            cityLabel.Text = "";
            cityLabel.IsVisible = false;

            zipCode.Text = auth.CurrentUser.Profile.ZipCode;
            zipCodeLabel.Text = "";
            zipCodeLabel.IsVisible = false;
        }

        private bool CheckForm()
        {
            Input.Result firstNameResult = firstNameInput.CheckValidity();
            firstNameLabel.Text = firstNameResult.Message;
            firstNameLabel.IsVisible = firstNameResult.Message.Length > 0;

            Input.Result lastNameResult = lastNameInput.CheckValidity();
            lastNameLabel.Text = lastNameResult.Message;
            lastNameLabel.IsVisible = lastNameResult.Message.Length > 0;

            Input.Result phoneNumberResult = phoneNumberInput.CheckValidity();
            phoneNumberLabel.Text = phoneNumberResult.Message;
            phoneNumberLabel.IsVisible = phoneNumberResult.Message.Length > 0;

            Input.Result addressResult = addressInput.CheckValidity();
            addressLabel.Text = addressResult.Message;
            addressLabel.IsVisible = addressResult.Message.Length > 0;

            Input.Result cityResult = cityInput.CheckValidity();
            cityLabel.Text = cityResult.Message;
            cityLabel.IsVisible = cityResult.Message.Length > 0;

            Input.Result zipCodeResult = zipCodeInput.CheckValidity();
            zipCodeLabel.Text = zipCodeResult.Message;
            zipCodeLabel.IsVisible = zipCodeResult.Message.Length > 0;

            if (firstNameResult.IsValid && lastNameResult.IsValid && phoneNumberResult.IsValid && addressResult.IsValid && cityResult.IsValid && zipCodeResult.IsValid)
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
            firstNameInput = new Input(firstName);
            lastNameInput = new Input(lastName);
            phoneNumberInput = new Input(phoneNumber);
            addressInput = new Input(address);
            cityInput = new Input(city);
            zipCodeInput = new Input(zipCode);

            if (CheckForm())
            {
                try
                {
                    var dbConnection = new DbConnection();

                    bool isSuccess = await dbConnection.UpdateProfile(new Profile
                    {
                        FirstName = firstNameInput.Value,
                        LastName = lastNameInput.Value,
                        PhoneNumber = phoneNumberInput.Value,
                        Address = addressInput.Value,
                        City = cityInput.Value,
                        ZipCode = zipCodeInput.Value
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
    }
}