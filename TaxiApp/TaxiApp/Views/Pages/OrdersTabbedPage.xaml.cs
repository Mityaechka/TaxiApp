using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersTabbedPage : TabbedPage
    {
        public OrdersTabbedPage()
        {
            InitializeComponent();
            Init();

        }

        private async void Init()
        {
            await Navigation.PushPopupAsync(new LoadingPopup());
            IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
            OrdersPage page1 = new OrdersPage(OrderType.Completed);
            OrdersPage page2 = new OrdersPage(OrderType.Failed);
            Children.Add(page1);
            Children.Add(page2);
            try
            {
                Task<ResponseModel<(List<OrderModel>, double)>> t1 = service.GetOrdersResponse(OrderType.Completed, 1);
                Task<ResponseModel<(List<OrderModel>, double)>> t2 = service.GetOrdersResponse(OrderType.Failed, 1);

                await Task.WhenAll(t1, t2);
                if (t1.Result.Status == Status.Ok)
                {
                    page1.Orders = t1.Result.Data.Item1;
                    page1.PageCount = t1.Result.Data.Item2;
                }
                if (t2.Result.Status == Status.Ok)
                {
                    page2.PageCount = t2.Result.Data.Item2;
                    page2.Orders = t2.Result.Data.Item1;
                }

                page1.LoadPages();
                page2.LoadPages();
            }
            catch (Exception exception)
            {
                await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
                Console.WriteLine(exception.Message);
            }
            finally
            {

                await Navigation.PopPopupAsync();
            }
        }

    }
}