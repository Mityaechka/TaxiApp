using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using TaxiApp.Models;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPopup : PopupPage
    {
        private readonly PaymentModel model;
        public PaymentPopup(PaymentModel payment)
        {
            InitializeComponent();
            model = payment;
            BindingContext = model;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.RemovePopupPageAsync(this);
        }
    }
}