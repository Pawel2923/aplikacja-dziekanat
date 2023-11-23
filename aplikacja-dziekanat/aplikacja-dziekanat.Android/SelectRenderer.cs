using Android.App;
using Android.Content;
using Android.Widget;
using CustomRenderer;
using CustomRenderer.Android;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Views;
using aplikacja_dziekanat.Droid;
using static Android.Widget.AdapterView;
using ListView = Android.Widget.ListView;
using View = Android.Views.View;
using AndrColor = Android.Graphics.Color;
using AndrButton = Android.Widget.Button;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(Select), typeof(SelectRenderer))]
namespace CustomRenderer.Android
{
    class SelectRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        private Dialog dialog;

        public SelectRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
                Control.Click += Control_Click;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Control.Click -= Control_Click;
            base.Dispose(disposing);
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Picker model = Element;
            dialog = new Dialog(Xamarin.Essentials.Platform.CurrentActivity);
            dialog.SetContentView(Resource.Layout.dialog);

            ListView listView = (ListView)dialog.FindViewById(Resource.Id.lv);
            listView.Adapter = new MyAdaptr((List<string>)model.ItemsSource);
            listView.ItemClick += (object sender1, ItemClickEventArgs e1) =>
            {
                Element.SelectedIndex = e1.Position;
                dialog.Dismiss();
            };

            dialog.Window.SetBackgroundDrawable(new ColorDrawable(AndrColor.Transparent));

            var mainDisplayInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            var height = mainDisplayInfo.Height;
            var width = mainDisplayInfo.Width - 160;
            dialog.Window.SetLayout(Convert.ToInt32(width), LayoutParams.WrapContent);

            AndrButton btn = (AndrButton)dialog.FindViewById(Resource.Id.button1);
            btn.Click += (object sender2, EventArgs e2) =>
            {
                dialog.Dismiss();
            };

            dialog.Show();
        }

        class MyAdaptr : BaseAdapter
        {
            private IList<string> mList;
            public MyAdaptr(IList<string> itemsSource)
            {
                mList = itemsSource;
            }



            public override int Count => mList.Count;



            public override Java.Lang.Object GetItem(int position)
            {
                return mList[position];
            }



            public override long GetItemId(int position)
            {
                return position;
            }



            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View view = convertView;
                convertView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.celllayout, null);
                TextView text = convertView.FindViewById<TextView>(Resource.Id.textview1);
                text.Text = mList[position];

                return convertView;
            }
        }
    }
}