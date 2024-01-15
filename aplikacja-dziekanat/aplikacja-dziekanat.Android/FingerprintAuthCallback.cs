using Android.App;
using Android.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Widget;
using Javax.Crypto;
using System;

namespace aplikacja_dziekanat.Droid
{
    class FingerprintAuthCallback : FingerprintManagerCompat.AuthenticationCallback
    {
        private readonly Dialog dialog;
        static readonly byte[] SECRET_BYTES = { 14, 4, 40, 212, 180, 25, 3, 169, 175 };
        private static readonly string TAG = "FingerprintAuthCallback";

        public FingerprintAuthCallback()
        {
        }

        public FingerprintAuthCallback(Context context)
        {
            DebugService.WriteLine(TAG, TAG, "Place your finger on the fingerprint scanner to verify your identity.");
            dialog = new Dialog(Xamarin.Essentials.Platform.CurrentActivity);
            dialog.SetContentView(Resource.Layout.fingerprint_dialog);
            dialog.SetCancelable(true);
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Window.SetDimAmount(0.8f);
            dialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            Button btn = (Button)dialog.FindViewById(Resource.Id.button1);
            btn.Click += (object sender, EventArgs e) =>
            {
                dialog.Dismiss();
            };

            dialog.Show();
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);

                    DebugService.WriteLine("Fingerprint authentication succeeded.");
                    dialog.Dismiss();
                }
                catch (BadPaddingException bpe)
                {
                    DebugService.WriteLine(TAG, nameof(OnAuthenticationSucceeded), "Failed to encrypt the data with the generated key." + bpe);
                    throw new Exception("Failed to encrypt the data with the generated key." + bpe);
                }
                catch (IllegalBlockSizeException ibse)
                {
                    DebugService.WriteLine(TAG, nameof(OnAuthenticationSucceeded), "Failed to encrypt the data with the generated key." + ibse);
                    throw new Exception("Failed to encrypt the data with the generated key." + ibse);
                }
            }
            else
            {
                DebugService.WriteLine("Fingerprint authentication succeeded.");
            }
        }

        public override void OnAuthenticationError(int errMsgId, Java.Lang.ICharSequence errString)
        {
            base.OnAuthenticationError(errMsgId, errString);

            string errorMessage = errString.ToString();
            DebugService.WriteLine(TAG, nameof(OnAuthenticationError), "Authentication error: " + errorMessage);
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            DebugService.WriteLine(TAG, nameof(OnAuthenticationFailed), "Authentication failed.");
        }

        public override void OnAuthenticationHelp(int helpMsgId, Java.Lang.ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpMsgId, helpString);
            string helpMessage = helpString.ToString();
            DebugService.WriteLine(TAG, nameof(OnAuthenticationHelp), "Authentication help: " + helpMessage);
        }
    }
}