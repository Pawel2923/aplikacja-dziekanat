using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomRenderer;
using System.Diagnostics;

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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetForm();
        }

        private void ResetForm()
        {
            firstNameInput.Value = "";
            firstNameLabel.Text = "";
            firstNameLabel.IsVisible = false;

            lastNameInput.Value = "";
            lastNameLabel.Text = "";
            lastNameLabel.IsVisible = false;

            phoneNumberInput.Value = "";
            phoneNumberLabel.Text = "";
            phoneNumberLabel.IsVisible = false;

            addressInput.Value = "";
            addressLabel.Text = "";
            addressLabel.IsVisible = false;

            cityInput.Value = "";
            cityLabel.Text = "";
            cityLabel.IsVisible = false;

            zipCodeInput.Value = "";
            zipCodeLabel.Text = "";
            zipCodeLabel.IsVisible = false;
        }

        private bool CheckForm()
        {
            Input.Result firstNameResult = firstNameInput.CheckValidity(true);
            firstNameLabel.Text = firstNameResult.Message;
            firstNameLabel.IsVisible = firstNameResult.Message.Length > 0;

            Input.Result lastNameResult = lastNameInput.CheckValidity(true);
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
                    throw new NotImplementedException("Zapisywanie do bazy nie zostało zaimplementowane");

                    //ResetForm();
                    //await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}