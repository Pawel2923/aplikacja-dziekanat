using Android.App;
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

        private Action onSucceed;

        public FingerprintAuthCallback(Action succeedHandler, Action cancelHandler)
        {
            onSucceed = succeedHandler;
            DebugService.WriteLine(TAG, TAG, "Place your finger on the fingerprint scanner to verify your identity.");
            dialog = new Dialog(Xamarin.Essentials.Platform.CurrentActivity);
            dialog.SetContentView(Resource.Layout.fingerprint_dialog);
            dialog.SetCancelable(true);
            dialog.SetCanceledOnTouchOutside(true);
            dialog.Window.SetDimAmount(0.8f);
            dialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            dialog.CancelEvent += (object sender, EventArgs e) =>
            {
                cancelHandler();
            };

            Button btn = (Button)dialog.FindViewById(Resource.Id.button1);
            btn.Click += (object sender, EventArgs e) =>
            {
                dialog.Dismiss();
                cancelHandler();
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
                    onSucceed();
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
            TextView status = (TextView)dialog.FindViewById(Resource.Id.fingerprint_status);
            status.Text = "Wystąpił błąd";
        }

        public override void OnAuthenticationFailed()
        {
            base.OnAuthenticationFailed();
            DebugService.WriteLine(TAG, nameof(OnAuthenticationFailed), "Authentication failed.");
            TextView status = (TextView)dialog.FindViewById(Resource.Id.fingerprint_status);
            status.Text = "Nie rozpoznano odcisku palca";
        }

        public override void OnAuthenticationHelp(int helpMsgId, Java.Lang.ICharSequence helpString)
        {
            base.OnAuthenticationHelp(helpMsgId, helpString);
            string helpMessage = helpString.ToString();
            DebugService.WriteLine(TAG, nameof(OnAuthenticationHelp), "Authentication help: " + helpMessage);
            TextView status = (TextView)dialog.FindViewById(Resource.Id.fingerprint_status);
            status.Text = helpMessage;
        }
    }
}