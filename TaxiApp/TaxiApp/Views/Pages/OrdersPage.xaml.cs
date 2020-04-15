using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public OrderListViewModel Model { get; set; } = new OrderListViewModel();
        public Command Refresh { get; private set; }
        public bool IsRefreshing { get; set; } = false;
        OrderType OrderType;
        public List<OrderModel> Orders
        {
            get
            {
                return Model.Models.ToList();
            }
            set
            {
                Model.Models = new ObservableCollection<OrderModel>(value);
            }
        }
        string ordersContent { get; set; }
        public string OrdersContent
        {
            set
            {
                ordersContent = value;
                Model.Models = new ObservableCollection<OrderModel>(OrderModel.FromJson(ordersContent));
            }
        }
        public OrdersPage(OrderType order)
        {
            InitializeComponent();
            OrderType = order;
            BindingContext = this;
            switch (order)
            {
                case OrderType.Completed:
                    Title = "Архив";
                    break;
                case OrderType.Failed:
                    Title = "Ложные заказы";
                    break;
                case OrderType.Relevant:
                    Title = "Актуальные заказы";
                    break;
            }
        }
        private async void DataGrid_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var order = DataGrid.SelectedItem as OrderModel;
            if (order != null)
                await Navigation.PushPopupAsync(new OrderPopup(order,false));
            DataGrid.SelectedItem = null;
        }



        public async void LoadPages()
        {
            var pagination = JObject.Parse(ordersContent)["pagination"];
            var pageCount = Math.Round( Convert.ToDouble(pagination["totalCount"] ) / Convert.ToDouble( pagination["defaultPageSize"])+0.1);
            var service = App.IoCContainer.GetInstance<IOrdersService>();
            for (int i = 2; i <= pageCount; i++)
            {
                var data = await service.GetOrdersResponse(OrderType, i);
                var list =  OrderModel.FromJson(await data.Content.ReadAsStringAsync());
                foreach (var item in list)
                {
                    Model.Models.Add(item);
                }
            }
            
        }
    }
}