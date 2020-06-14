using Plugin.Settings;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Blocked : ContentPage
    {
        public Blocked()
        {
            InitializeComponent();
            //CrossSettings.Current.AddOrUpdateValue("loginOnStart", false);
        }
    }
}