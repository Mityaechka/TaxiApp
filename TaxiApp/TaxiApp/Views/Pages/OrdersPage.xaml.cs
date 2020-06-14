using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public OrderListViewModel Model { get; set; } 
        public Command Refresh { get; private set; }
        public bool IsRefreshing { get; set; } = false;
        public double PageCount { get; set; }

        private readonly OrderType OrderType;
        public List<OrderModel> Orders
        {
            get => Model.Models.ToList();
            set  {
                Model.Models = new ObservableCollection<OrderModel>(value);
                if (value.Count == 0)
                {
                    EmptyLayout.IsVisible = true;
                    DataGrid.IsVisible = false;
                }
                else
                {
                    EmptyLayout.IsVisible = false;
                    DataGrid.IsVisible = true;
                }
            }
        }

        public OrdersPage(OrderType order)
        {
            Model = new OrderListViewModel(order);
            
            InitializeComponent();
            BindingContext = this;
            OrderType = order;
            
            
            //DataGrid.RowsBackgroundColorPalette = Model.Palette;
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
            //OrderModel order = DataGrid.SelectedItem as OrderModel;
            //if (order != null)
            //{
            //    await Navigation.PushPopupAsync(new OrderPopup(order, false));
            //}

            //DataGrid.SelectedItem = null;
        }



        public async void LoadPages()
        {

            IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
            for (int i = 2; i <= PageCount; i++)
            {
                ResponseModel<(List<OrderModel>, double)> data = await service.GetOrdersResponse(OrderType, i);
                var m = Model.Models;
                if (data.Status == Status.Ok)
                {
                    (List<OrderModel>, double) list = data.Data;
                    foreach (OrderModel item in list.Item1)
                    {
                        m.Add(item);
                    }
                }
                for (int j = 0; j < Model.Models.Count; j++)
                {
                    Model.Models[j].Index = j + 1;
                }
                Model.Models = m;
            }

        }
    }
}