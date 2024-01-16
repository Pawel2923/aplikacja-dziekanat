using Android;
using Android.App;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using System;
using aplikacja_dziekanat.Droid;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Dynamic;
using System.Threading.Tasks;

[assembly: Dependency(typeof(SecureStorageService))]
namespace aplikacja_dziekanat.Droid
{
    public class SecureStorageService : ISecureStorageService
    {
        public void Delete(string key)
        {
            SecureStorage.Remove(key);
        }

        public async Task<string> Load(string key)
        {
            try
            {
                string value = await SecureStorage.GetAsync(key);

                return value;
            }
            catch (Exception ex)
            {
                DebugService.WriteLine("SecureStorageService", "Load", ex.Message);
                return null;
            }
        }

        public async void Save(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                DebugService.WriteLine("SecureStorageService", "Save", ex.Message);
            }
        }
    }
}