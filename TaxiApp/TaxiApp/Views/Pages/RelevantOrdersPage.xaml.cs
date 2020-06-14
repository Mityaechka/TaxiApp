using Newtonsoft.Json.Linq;
using Plugin.SimpleAudioPlayer;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaxiApp.DependencyServices;
using TaxiApp.Extensions;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelevantOrdersPage : ContentPage
    {
        public bool TableViewMode { get; set; }
        public OrderListViewModel Model { get; set; } = new OrderListViewModel(OrderType.Relevant);

        private readonly IOrdersService ordersService;
        private bool isFirstRequest = true;
        public RelevantOrdersPage()
        {
            InitializeComponent();
            TableViewMode = Preferences.Get("TableViewMode", true);
            ListView.IsVisible = !TableViewMode;
            DataGrid.IsVisible = TableViewMode;
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


                    string money = await App.IoCContainer.GetInstance<IUserService>().GetMoney();
                    MoneyLabel.Text = $"Мой баланс: {money} рублей";

                    var s = JObject.Parse(await ordersService.GetRelevantTextResponse(OrderType.Relevant));
                    DayPaymentBtn.IsVisible = false;
                    BlockedLayout.IsVisible = false;
                    if (s.ContainsKey("block_notdaypayment"))
                    {
                        BlockedText.Text = "Выполнить суточное списание";
                        DayPaymentBtn.IsVisible = true;
                        BlockedLayout.IsVisible = true;
                        DataLayout.IsVisible = false;

                    }
                    else if (s.ContainsKey("block_nomoney"))
                    {
                        BlockedText.Text = "Недостаточно денег на счету! Пожалуйста, пополните свой персональный счет через диспетчера.";
                        BlockedLayout.IsVisible = true;
                        DataLayout.IsVisible = false;

                    }
                    else if (s.ContainsKey("blocked"))
                    {
                        BlockedText.Text = "Заблокировано! Пожалуйста, свяжитесь с диспетчером (8-903-44-78-006) или администратором (8-964-93-39-205).";
                        BlockedLayout.IsVisible = true;
                        DataLayout.IsVisible = false;
                    }
                    else
                    {
                        BlockedLayout.IsVisible = false;
                        DataLayout.IsVisible = true;
                        ResponseModel<List<OrderModel>> response = await ordersService.GetRelenantsResponse(OrderType.Relevant);
                        ObservableCollection<OrderModel> list = new ObservableCollection<OrderModel>(response.Data);

                        list = new ObservableCollection<OrderModel>(list.OrderBy(x => x.CanAccept));
                        bool hasNewOrder = list.Except<OrderModel, OrderModel, int>(Model.Models, x => x.Id, u => u.Id).Count() != 0;
                        if (App.IsInForeground && hasNewOrder && !isFirstRequest)
                        {
                            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
                            player.Load(GetStreamFromFile("alert.wav"));
                            player.Play();
                        }
                        if (isFirstRequest)
                        {
                            isFirstRequest = false;
                        }

                        Model.Models = list;

                        ListView.ItemsSource = DataGridBody.ItemsSource = Model.Models;

                        BindingContext = this;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    //if (Model.Models.Count == 0)
                    //{
                    //    EmptyLayout.IsVisible = true;
                    //    DataLayout.IsVisible = false;
                    //}
                    //else
                    //{
                    //    EmptyLayout.IsVisible = false;
                    //    DataLayout.IsVisible = true;
                    //}
                    await Task.Delay(1000);
                }
            }
        }

        private async void TakeOrderClick(object sender, EventArgs e)
        {
            try
            {
                BindableObject btn = sender as BindableObject;
                string id = IdHelper.GetTag(btn);

                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(new LoadingPopup());
                ResponseModel<string> r = await service.TakeOrder(id);

                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    DependencyService.Get<IToast>().AlertShort(content == "ok" ? "Заказа принят" : "Произошла ошибка");
                    //if (content == "ok")
                    //    DependencyService.Get<IToast>().AlertShort("Заказа принят");
                    //    await DisplayAlert("Заказа принят", "Вы взяли этот заказ", "Ok");
                    // else
                    //    await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                DependencyService.Get<IToast>().AlertShort("Произошла ошибка");
                //await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
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
                BindableObject btn = sender as BindableObject;
                string id = IdHelper.GetTag(btn);

                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(new LoadingPopup());

                ResponseModel<string> r = await service.UnakeOrder(id);
                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    DependencyService.Get<IToast>().AlertShort(content == "ok" ? "Заказ отменён" : "Произошла ошибка");
                    //if (content == "ok")
                    //    await DisplayAlert("Заказа отменен", "Вы отменили этот заказ", "Ok");
                    //else
                    //    await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                DependencyService.Get<IToast>().AlertShort("Произошла ошибка");
                //await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
            }
            finally
            {
                await Navigation.PopPopupAsync();
            }
        }
        private async void CallPassengerClick(object sender, EventArgs e)
        {
            try
            {
                BindableObject btn = sender as BindableObject;
                string id = IdHelper.GetTag(btn);
                OrderModel order = Model.Models.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
                IOrdersService service = App.IoCContainer.GetInstance<IOrdersService>();
                await Navigation.PushPopupAsync(new LoadingPopup());
                ResponseModel<string> r = await service.CallPassanger(id);
                if (r.Status == Status.Ok)
                {
                    string content = r.Data;
                    content = content.Replace("\"", "");
                    string message = "";
                    if (content == "true")
                    {
                        // message = "Пассажир вызван" ;
                    }
                    else if (content == "false")
                    {
                        //message =  "Вызов отменён";

                    }
                    else
                    {
                        message = "Произошла ошибка";
                    }
                    //DependencyService.Get<IToast>().AlertShort(message);
                    //if (content == "true")
                    //    await DisplayAlert("Звонок пассажирам", "Вы позвонили пассажиру", "Ok");
                    //else
                    //    await DisplayAlert("Произошла ошибка", "Попробуйте позже", "Ok");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                DependencyService.Get<IToast>().AlertShort("Произошла ошибка");
                //await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
            }
            finally
            {
                await Navigation.PopPopupAsync();
            }
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
                    DataGrid.IsVisible = TableViewMode;

                })
            });
        }

        private Stream GetStreamFromFile(string filename)
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("TaxiApp." + filename);
            return stream;
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