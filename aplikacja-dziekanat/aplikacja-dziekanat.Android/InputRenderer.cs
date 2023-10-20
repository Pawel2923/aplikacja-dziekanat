using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CustomRenderer;
using CustomRenderer.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(Input), typeof(InputRenderer))]
namespace CustomRenderer.Android
{
    class InputRenderer : EntryRenderer
    {
        public InputRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Rgb(217, 217, 217));
            }
        }
    }
}