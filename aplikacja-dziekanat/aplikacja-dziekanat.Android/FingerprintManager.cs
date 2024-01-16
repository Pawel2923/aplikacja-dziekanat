using Android;
using Android.App;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using System;
using aplikacja_dziekanat.Droid;
using Xamarin.Forms;
using System.Dynamic;

[assembly: Dependency(typeof(FingerprintManager))]
namespace aplikacja_dziekanat.Droid
{
    public class FingerprintManager : IFingerprintManager
    {
        private readonly FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);
        private static readonly string TAG = "FingerprintManager";

        private bool AreRequirementsFullfilled()
        {
            if (!fingerprintManager.IsHardwareDetected)
            {
                DebugService.WriteLine(TAG, "AreRequirementsFullfilled", "Nie znaleziono linii papilarnych");
                throw new Exception("Nie znaleziono czytnika linii papilarnych");
            }

            KeyguardManager keyguardManager = (KeyguardManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.KeyguardService);

            if (!keyguardManager.IsKeyguardSecure)
            {
                DebugService.WriteLine(TAG, nameof(AreRequirementsFullfilled), "Nie ustawiono blokady ekranu!");
                throw new Exception("Nie ustawiono blokady ekranu");
            }

            return true;
        }

        private bool IsUseFingerprintEnabled()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.UseFingerprint);
#pragma warning restore CS0618 // Type or member is obsolete
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            else
            {
                throw new Exception("Nie ustawiono uprawnień do czytnika linii papilarnych");
            }
        }

        public bool IsFingerprintAvailable() => AreRequirementsFullfilled() && fingerprintManager.HasEnrolledFingerprints && IsUseFingerprintEnabled();

        public void AuthenticateFingerprint(Action onSucceed, Action onCancel = null)
        {
            const int flags = 0;

            CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();

            var cancellationSignal = new Android.Support.V4.OS.CancellationSignal();

            void cancelHandler()
            {
                if (onCancel != null)
                {
                    onCancel();
                    cancellationSignal.Cancel();
                }
                else
                {
                    cancellationSignal.Cancel();
                }
            }

            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(Android.App.Application.Context);

            FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new FingerprintAuthCallback(onSucceed, cancelHandler);

            fingerprintManager.Authenticate(cryptoHelper.BuildCryptoObject(), flags, cancellationSignal, authenticationCallback, null);
        }

    }
}