using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using TaxiApp.Models;
using TaxiApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPopup : PopupPage
    {
        private readonly OrderModel model;
        public OrderPopup(OrderModel order, bool showBtn)
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
            try
            {
                Button btn = sender as Button;
                string id = IdHelper.GetTag(btn);

                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(new LoadingPopup());
                ResponseModel<string> r = await service.TakeOrder(id);
                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    await Navigation.PopPopupAsync();
                    if (content == "ok")
                    {
                        await DisplayAlert("Заказа принят", "Вы взяли этот заказ", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                    }
                }
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
        private async void UntakeOrderClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                string id = IdHelper.GetTag(btn);
                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(new LoadingPopup());
                ResponseModel<string> r = await service.UnakeOrder(id);
                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    await Navigation.PopPopupAsync();
                    if (content == "ok")
                    {
                        await DisplayAlert("Заказа отменен", "Вы отменили этот заказ", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                    }
                }
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
        private async void CallPassengerClick(object sender, EventArgs e)
        {
            LoadingPopup loadingPage = new LoadingPopup();
            try
            {
                Button btn = sender as Button;
                string id = IdHelper.GetTag(btn);
                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(loadingPage);
                ResponseModel<string> r = await service.CallPassanger(id);
                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    if (content == "true")
                    {
                        await DisplayAlert("Звонок пассажирам", "Вы позвонили пассажиру", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
            }
            finally
            {
                await Navigation.RemovePopupPageAsync(loadingPage);
            }
        }
    }
}