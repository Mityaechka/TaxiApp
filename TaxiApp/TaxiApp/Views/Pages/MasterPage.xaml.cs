using Plugin.Settings;
using Rg.Plugins.Popup.Extensions;
using System;
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
            set => MoneyLabel.Text = $"Финансы: {value}";
        }
        public MasterPage()
        {
            InitializeComponent();
            BindingContext = userViewModel;
            GetMoney();
        }

        private async void GetMoney()
        {
            while (true)
            {
                try
                {
                    IUserService httpService = App.IoCContainer.GetInstance<IUserService>();
                    Money = await httpService.GetMoney();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    await Task.Delay(1500);
                }
            }
        }
        private async void RelevantBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                MainPage.Instance.IsPresented = false;
                await Task.Delay(250);
                await MainPage.Instance.SetDetail();
                //IOrdersService service = App.IoCContainer.GetService<IOrdersService>();
                //string path = await service.AccountStatePath();

                //switch (path)
                //{
                //    case "/orders/block_notdaypayment":
                //        MainPage.Instance.Detail = new NavigationPage(new NotDayPaymentPage());
                //        break;
                //    case "/orders/nomoney":
                //        MainPage.Instance.Detail = new NavigationPage(new NoMoneyPage());
                //        break;
                //    case "/orders/blocked":
                //        MainPage.Instance.Detail = (new Blocked());
                //        break;
                //    default:
                //        MainPage.Instance.Detail = new NavigationPage(new RelevantOrdersPage());
                //        break;
                //}
            }
            catch (Exception exception)
            {
                await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
                Console.WriteLine(exception.Message);
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

            PaymentsPage page = new PaymentsPage();
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
            IUserService service = App.IoCContainer.GetService<IUserService>();
            var httpService = App.IoCContainer.GetService<HttpService>();

            try
            {
                await Navigation.PushPopupAsync(new LoadingPopup());
                System.Net.Http.HttpResponseMessage response =                await service.Logout();
                var str = await response.Content.ReadAsStringAsync();

                if (response.RequestMessage.RequestUri.LocalPath == "/site/login")
                {
                    Application.Current.MainPage = new LoginPage(false);
                }
                else
                {
                    await DisplayAlert("Произошла ошибка.", "Повторите операцию или перезагрузите приложение", "Ok");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Произошла ошибка.", "Повторите операцию или перезагрузите приложение", "Ok");
            }
            finally
            {
                await Navigation.PopPopupAsync();
                CrossSettings.Current.AddOrUpdateValue("loginOnStart", false);
            }
        }
    }
}