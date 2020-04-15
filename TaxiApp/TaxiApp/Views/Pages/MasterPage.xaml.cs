using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public UserViewModel userViewModel = new UserViewModel();
        public string Money
        {
            set
            {
                MoneyLabel.Text = $"Финансы: {value}";
            }
        }
        public MasterPage()
        {
            InitializeComponent();
            BindingContext = userViewModel;
            GetMoney();
        }
        async void GetMoney()
        {
            while (true)
            {
                var httpService = App.IoCContainer.GetInstance<IUserService>();
                Money = await httpService.GetMoney();
                await Task.Delay(1500);
            }
        }
        private async void RelevantBtn_Clicked(object sender, EventArgs e)
        {
            MainPage.Instance.IsPresented = false;
            await Task.Delay(250);
            var httpService = App.IoCContainer.GetService<IOrdersService>();
            var response = await httpService.GetOrdersResponse(OrderType.Relevant,1);
            var path = response.RequestMessage.RequestUri.LocalPath;

            switch (path)
            {
                case "/orders/block_notdaypayment":
                    MainPage.Instance.Detail = new NavigationPage(new NotDayPaymentPage());
                    break;
                case "/orders/nomoney":
                    MainPage.Instance.Detail = new NavigationPage(new NoMoneyPage());
                    break;
                case "/orders/blocked":
                    MainPage.Instance.Detail = (new Blocked());
                    break;
                default:
                    MainPage.Instance.Detail = new NavigationPage(new RelevantOrdersPage());
                    break;
            }

        }

        private async void Archive_Clicked(object sender, EventArgs e)
        {
            MainPage.Instance.IsPresented = false;
            await Task.Delay(250);
            MainPage.Instance.Detail = new NavigationPage(new OrdersTabbedPage());
        }

        private async void PaymentsBtn_Clicked(object sender, EventArgs e)
        {
            MainPage.Instance.IsPresented = false;
            await Task.Delay(250);

            var page = new PaymentsPage();
            MainPage.Instance.Detail = new NavigationPage(page);
        }

        private async void GeomapBtn_Clicked(object sender, EventArgs e)
        {
            MainPage.Instance.IsPresented = false;
            await Task.Delay(250);
            MainPage.Instance.Detail = new NavigationPage(new GeomapPage());

        }

        private async void LogoutBtn_Clicked(object sender, EventArgs e)
        {

            MainPage.Instance.IsPresented = false;
            var service = App.IoCContainer.GetService<IUserService>();
            
            try
            {
                await Navigation.PushPopupAsync(new LoadingPopup());
                var response = await service.Logout();
                await Navigation.PopPopupAsync();
                var s = await response.Content.ReadAsStringAsync();
                if(response.RequestMessage.RequestUri.LocalPath=="/site/login")
                    Application.Current.MainPage = new LoginPage();
                else
                    await DisplayAlert("Произошла ошибка.", "Повторите операцию или перезагрузите приложение", "Ok");
            }
            catch (Exception ex) {
                await DisplayAlert("Произошла ошибка.", "Повторите операцию или перезагрузите приложение", "Ok");
            }
        }
    }
}