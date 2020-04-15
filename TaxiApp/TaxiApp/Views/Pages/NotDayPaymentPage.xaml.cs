using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Services;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotDayPaymentPage : ContentPage
    {
        public NotDayPaymentPage()
        {
            InitializeComponent();
        }
        public async void Eexecdaypayment(object sender, EventArgs e)
        {
            var http = App.IoCContainer.GetInstance<HttpService>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            var response =await  http.Execdaypayment();
            var path = response.RequestMessage.RequestUri.LocalPath;
            if (path == "/orders/index")
                MainPage.Instance.SetDetail(Enums.AccountState.Ok);
            await Navigation.PopPopupAsync();
        }
    }
}