using Rg.Plugins.Popup.Extensions;
using System;
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
            HttpService http = App.IoCContainer.GetInstance<HttpService>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            try
            {
                System.Net.Http.HttpResponseMessage response = await http.Execdaypayment();
                string path = response.RequestMessage.RequestUri.LocalPath;
                await MainPage.Instance.SetDetail();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
            }
            finally
            {
                await Navigation.PopPopupAsync();
            }
        }
    }
}