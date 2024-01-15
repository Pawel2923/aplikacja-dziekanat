using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace aplikacja_dziekanat.Droid
{
    public class FingerprintManager : IFingerprintManager
    {
        private readonly FingerprintManagerCompat fingerprintManager;

        public FingerprintManager()
        {
            fingerprintManager = FingerprintManagerCompat.From(Application.Context);
            if (!fingerprintManager.IsHardwareDetected)
            {
                throw new Exception("Nie znaleziono czytnika odcisku palca!");
            }

            KeyguardManager keyguardManager = (KeyguardManager)Application.Context.GetSystemService(Android.Content.Context.KeyguardService);

            if (!keyguardManager.IsKeyguardSecure)
            {
                throw new Exception("Nie ustawiono blokady ekranu!");
            }
        }

        public bool IsFingerprintAvailable()
        {
            return fingerprintManager.HasEnrolledFingerprints;
        }

        public bool IsUseFingerprintEnabled()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.UseFingerprint);
#pragma warning restore CS0618 // Type or member is obsolete
            if (permissionResult == Android.Content.PM.Permission.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}