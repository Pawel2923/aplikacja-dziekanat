using Android.Content;
using CustomRenderer;
using CustomRenderer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Select), typeof(SelectRenderer))]
namespace CustomRenderer.Android
{
    class SelectRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        public SelectRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}