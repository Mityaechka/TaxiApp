using SimpleInjector;
using System;
using System.Threading;
using TaxiApp.Services;
using TaxiApp.Services.Http;
//using TaxiApp.Services.Mock;
using TaxiApp.Views.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TaxiApp
{
    public partial class App : Application
    {
        private static Container ioCContainer = new SimpleInjector.Container();
        private static readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        public static CancellationToken Token => cancelTokenSource.Token;
        public static Container IoCContainer
        {
            get => ioCContainer;
            set => ioCContainer = value;
        }
        public App()
        {
            InitializeComponent();

            try
            {
                IoCContainer.Register<HttpService, HttpService>(Lifestyle.Transient);

                IoCContainer.Register<IOrdersService, HttpOrdersService>(Lifestyle.Transient);
                IoCContainer.Register<IUserService, HttpUserService>(Lifestyle.Transient);
                IoCContainer.Register<IPaymentsService, HttpPaymentsService>(Lifestyle.Transient);
            }
            catch (Exception exceprion)
            {
                Console.WriteLine(exceprion.Message);
            }
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            MainPage = new LoginPage();
        }

        public static bool IsInForeground { get; set; } = false;

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.None)
            {
                cancelTokenSource.Cancel();
            }
        }
        protected override void OnStart()
        {
            IsInForeground = true;
        }

        protected override void OnSleep()
        {
            IsInForeground = false;
        }

        protected override void OnResume()
        {
            IsInForeground = true;
        }
    }
}
