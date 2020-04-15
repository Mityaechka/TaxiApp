using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPopup : PopupPage
    {
        OrderModel model;
        public OrderPopup(OrderModel order,bool showBtn)
        {
            InitializeComponent();
            model = order;
            BindingContext = model;
            BtnLayout.IsVisible = showBtn;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.RemovePopupPageAsync(this);
        }

        private async void TakeOrderClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var id = IdHelper.GetTag(btn);
            
            var service = App.IoCContainer.GetInstance<HttpService>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            var r = await service.TakeOrder(id);
            var content = await r.Content.ReadAsStringAsync();
            content = content.Replace("\"", "");
            await Navigation.PopPopupAsync();
            if (content == "ok")
                await DisplayAlert("Заказа принят", "Вы взяли этот заказ", "Ok");
            else
                await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");

        }
        private async void UntakeOrderClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var id = IdHelper.GetTag(btn);
            var service = App.IoCContainer.GetInstance<HttpService>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            var r = await service.UntakeOrder(id);
            var content = await r.Content.ReadAsStringAsync();
            content = content.Replace("\"", "");
            await Navigation.PopPopupAsync();
            if (content == "ok")
                await DisplayAlert("Заказа отменен", "Вы отменили этот заказ", "Ok");
            else
                await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");

        }
        private async void CallPassengerClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var id = IdHelper.GetTag(btn);
            var service = App.IoCContainer.GetInstance<HttpService>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            var r = await service.CallPassenger(id);
            var content = await r.Content.ReadAsStringAsync();
            content = content.Replace("\"", "");
            await Navigation.PopPopupAsync();
            if (content == "true")
                await DisplayAlert("Звонок пассажирам", "Вы позвонили пассажиру", "Ok");
            else
                await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");

        }
    }
}