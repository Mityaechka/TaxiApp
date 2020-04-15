using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        async void Init()
        {
            await Navigation.PushPopupAsync(new LoadingPopup());
            var service = App.IoCContainer.GetInstance<IOrdersService>();
            var page1 = new OrdersPage(OrderType.Completed);
            var page2 = new OrdersPage(OrderType.Failed);
            Children.Add(page1);
            Children.Add(page2);

            var t1 = service.GetOrdersResponse(OrderType.Completed,1);
            var t2 = service.GetOrdersResponse(OrderType.Failed,1);
            
            await Task.WhenAll(t1, t2);
            page1.OrdersContent = await t1.Result.Content.ReadAsStringAsync();
            page2.OrdersContent = await t2.Result.Content.ReadAsStringAsync();

            await Navigation.PopPopupAsync();

            page1.LoadPages();
            page2.LoadPages();
        }
        
    }
}