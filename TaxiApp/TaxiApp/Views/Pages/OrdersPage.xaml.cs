using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
         OrderType OrderType { get; set; } = OrderType.Completed;
        public OrderListViewModel Model { get; set; } = new OrderListViewModel();
        public Command Refresh { get; private set; }
        public bool IsRefreshing { get; set; } = false;
        public OrdersPage()
        {
            InitializeComponent();
            Refresh = new Command(async () =>
            {
                //IsRefreshing = true;
                var service = App.IoCContainer.GetInstance<IOrdersService>();
                Model.Models = new ObservableCollection<OrderModel>(await service.GetOrders(OrderType));
                IsRefreshing = false;
            });
            BindingContext = this;
            var location = Shell.Current.CurrentState.Location;
            var orderType = location.ToString().Split('/').Last();
            switch (orderType)
            {
                case ("relevant"):
                    Title = "Актуальные заказы";
                    OrderType = OrderType.Relevant;
                    break;
                case ("completed"):
                    Title = "Мой архив";
                    OrderType = OrderType.Completed;
                    break;
                case ("failed"):
                    Title = "Мой архив";
                    OrderType = OrderType.Failed;
                    break;
                default:
                    throw new ArgumentException();
                    
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var service = App.IoCContainer.GetInstance<IOrdersService>();
            Model.Models = new ObservableCollection<OrderModel>( await service.GetOrders(OrderType));
        }

        protected override void OnTabIndexPropertyChanged(int oldValue, int newValue)
        {
            base.OnTabIndexPropertyChanged(oldValue, newValue);
        }
    }
}