using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaxiApp.DependencyServices;
using TaxiApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidToast))]
namespace TaxiApp.Droid
{
    public class AndroidToast : IToast
    {
        public void AlertShort(string message)
        {
            var t = Toast.MakeText(Application.Context, message, ToastLength.Short);
            Color c = Color.Black;
            ColorMatrixColorFilter CM = new ColorMatrixColorFilter(new float[]
            {
                0,0,0,0,c.R,
                0,0,0,0,c.G,
                0,0,0,0,c.B,
                0,0,0,1,0
            });
            t.View.Background.SetColorFilter(CM);
            t.View.FindViewById<TextView>(Android.Resource.Id.Message).SetTextColor(Color.White);
            t.Show();
        }
    }
}