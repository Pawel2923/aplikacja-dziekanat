using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Javax.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aplikacja_dziekanat.Droid
{
    class FingerprintAuthCallback : FingerprintManagerCompat.AuthenticationCallback
    {
        static readonly byte[] SECRET_BYTES = { 14, 4, 40, 212, 180, 25, 3, 169, 175 };
        static readonly string TAG = "X:" + typeof(FingerprintAuthCallback).Name;

        public FingerprintAuthCallback()
        {
        }

        public FingerprintAuthCallback(Context context)
        {
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);

                    Log.Info(TAG, "Fingerprint authentication succeeded.");
                }
                catch (BadPaddingException bpe)
                {
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);
                }
                catch (IllegalBlockSizeException ibse)
                {
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);
                }
            }
            else
            {
                Log.Info(TAG, "Fingerprint authentication succeeded.");
            }
        }

        public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        {
            base.OnAuthenticationError(errMsgId, errString);

            string errorMessage = errString.ToString();
            Log.Error(TAG, "Authentication error: " + errorMessage);
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            Log.Info(TAG, "Authentication failed.");
        }

        public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpMsgId, helpString);
            string helpMessage = helpString.ToString();
            Log.Info(TAG, "Authentication help: " + helpMessage);
        }
    }
}