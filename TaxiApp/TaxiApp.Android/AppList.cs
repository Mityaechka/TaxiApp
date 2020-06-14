using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaxiApp.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(TaxiApp.Droid.AppList))]
namespace TaxiApp.Droid
{
    
    public class AppList : IAppListService
    {

        public List<string> GetAppsList()
        {
            var flags = PackageInfoFlags.MetaData |
            PackageInfoFlags.SharedLibraryFiles |
            PackageInfoFlags.UninstalledPackages;

            var apps = Android.App.Application.Context.PackageManager.GetInstalledApplications(flags).ToList();
            var list = new List<string>();
            var info = new ApplicationInfo();
            var f = new List<ApplicationInfoFlags>();
            foreach (var app in apps)
            {
                f.Add(app.Flags & ApplicationInfoFlags.System);
                if((app.Flags & ApplicationInfoFlags.System)==ApplicationInfoFlags.None)
                    list.Add(app.LoadLabel(Android.App.Application.Context.PackageManager));
            }
            return list;
        }
    }
}