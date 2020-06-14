using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TaxiApp.Enums;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public UserViewModel User { get; set; }
        public static MainPage Instance { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            Routing.RegisterRoute("login", typeof(LoginPage));
            BindingContext = this;
        }
        public async Task SetDetail()
        {
            try
            {
                //var httpService = App.IoCContainer.GetInstance<HttpService>();
                //var response = await httpService.GetRequest("orders/countnew");
                //var s = JObject.Parse(await response.Content.ReadAsStringAsync());
                //if (s.ContainsKey("block_notdaypayment"))
                //    Detail = new NavigationPage(new NotDayPaymentPage());
                //else if (s.ContainsKey("block_nomoney"))
                //    Detail = new NavigationPage(new NoMoneyPage());
                //else if (s.ContainsKey("blocked"))
                //    Detail = new NavigationPage(new Blocked());
                //else
                    Detail = new NavigationPage(new RelevantOrdersPage());


            }
            catch (Exception e)
            {

            }
            //switch (state)
            //{
            //    case AccountState.NotDayPayment:
            //        Detail = new NavigationPage(new NotDayPaymentPage());
            //        break;
            //    case AccountState.NoMoney:
            //        Detail = new NavigationPage(new NoMoneyPage());
            //        break;
            //    case AccountState.Blocked:
            //        Detail = new NavigationPage(new Blocked());
            //        break;
            //    default:
            //        Detail = new NavigationPage(new RelevantOrdersPage());
            //        break;
            //}
        }
        public void LogoutClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}