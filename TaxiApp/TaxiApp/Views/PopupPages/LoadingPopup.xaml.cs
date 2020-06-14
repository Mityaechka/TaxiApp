using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : PopupPage
    {
        public LoadingPopup()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        { return true; }
    }
}