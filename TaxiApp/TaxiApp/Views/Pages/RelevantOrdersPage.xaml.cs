using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;
using Xamarin.Forms.Xaml;
using TaxiApp.Extensions;
namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelevantOrdersPage : ContentPage
    {
        public bool TableViewMode { get; set; }
        public OrderListViewModel Model { get; set; } = new OrderListViewModel();
        IOrdersService ordersService;
        bool isFirstRequest = true;
        public RelevantOrdersPage()
        {
            InitializeComponent();
            TableViewMode = Preferences.Get("TableViewMode", true);
            ListView.IsVisible = !TableViewMode;
            DataGridBody.IsVisible = TableViewMode;
            Model.Models = new ObservableCollection<OrderModel>();
            BindingContext = this;
            ordersService = App.IoCContainer.GetInstance<IOrdersService>();
            GetOrders();
            

        }

        public async void GetOrders()
        {
            while (true)
            {
                try
                {
                    var response = await ordersService.GetOrdersResponse(OrderType.Relevant, 1);
                    var path = response.RequestMessage.RequestUri.LocalPath;
                    var money = await App.IoCContainer.GetInstance<IUserService>().GetMoney();
                    MoneyLabel.Text = $"Мой баланс: {money} рублей";
                    if (path == "/orders/countnew")
                    {
                        var s = await response.Content.ReadAsStringAsync();
                        var data = JObject.Parse(s);

                        var list = new ObservableCollection<OrderModel>();
                        for (int i = 0; i < data["orders_ids"].Count(); i++)
                        {
                            var id = (int)data["orders_ids"][i];
                            var item = data["orders"][id.ToString()];
                            list.Add(new OrderModel()
                            {
                                Id = Convert.ToInt32(item["id"]),
                                From = Convert.ToString(item["place_from"]),
                                To = Convert.ToString(item["place_to"]),
                                CreatedDate = Convert.ToDateTime(item["created_at"]),
                                Cost = Convert.ToDecimal(item["price"]),
                                Driver = Convert.ToString(item["driver"]),
                                Phone = Convert.ToString(item["phone"])
                            });
                        }
                        list = new ObservableCollection<OrderModel>(list.OrderBy(x => x.CanAccept));
                        var hasNewOrder = list.Except<OrderModel, OrderModel, int>(Model.Models, x => x.Id, u => u.Id).Count() != 0;
                        if (App.IsInForeground && hasNewOrder && !isFirstRequest)
                        {
                            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
                            player.Load(GetStreamFromFile("mysound.wav"));
                            player.Play();
                        }
                        if (isFirstRequest)
                            isFirstRequest = false;
                        Model.Models = list;
                        BindingContext = this;
                        ListView.ItemsSource = DataGridBody.ItemsSource = Model.Models;
                    }
                    await Task.Delay(3000);
                }
                catch (Exception e)
                {

                }
            }
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
        private async void DataGrid_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var order = DataGrid.SelectedItem as OrderModel;
            //if (order != null)
            //    await Navigation.PushPopupAsync(new OrderPopup(order,true));
            //DataGrid.SelectedItem = null;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView.SelectedItem = null;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new SettingsPopup()
            {
                OnTableViewModeChange = new EventHandler((o, args) =>
                {
                    Switch @switch = o as Switch;
                    TableViewMode = @switch.IsToggled;
                    ListView.IsVisible = !TableViewMode;
                    DataGridBody.IsVisible = TableViewMode;

                })
            });
        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("TaxiApp." + filename);
            return stream;
        }



    }
}