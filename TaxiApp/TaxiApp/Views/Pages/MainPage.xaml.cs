using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Enums;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public UserViewModel User { get;  set; }
        public static MainPage Instance { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            Instance = this;
            Routing.RegisterRoute("login", typeof(LoginPage));
            BindingContext = this;
        }
        public void SetDetail(AccountState state)
        {
            switch (state)
            {
                case AccountState.NotDayPayment:
                    Detail = new NavigationPage(new NotDayPaymentPage());
                    break;
                case AccountState.NoMoney:
                    Detail = new NavigationPage(new NoMoneyPage());
                    break;
                case AccountState.Blocked:
                    Detail = new NavigationPage(new Blocked());
                    break;
                default:
                    Detail = new NavigationPage(new RelevantOrdersPage());
                    break;
            }
        }
        public void LogoutClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}