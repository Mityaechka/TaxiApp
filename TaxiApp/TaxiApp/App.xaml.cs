using SimpleInjector;
using System;
using TaxiApp.Services;
using TaxiApp.Services.Http;
using TaxiApp.Services.Mock;
using TaxiApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp
{
    public partial class App : Application
    {
        private static Container ioCContainer = new SimpleInjector.Container();
        public static Container IoCContainer
        {
            get => ioCContainer;
            set => ioCContainer = value;
        }
        public App()
        {
            InitializeComponent();
            IoCContainer.Register<HttpService, HttpService>(Lifestyle.Transient);

            IoCContainer.Register<IOrdersService, HttpOrdersService>(Lifestyle.Transient);
            IoCContainer.Register<IUserService, HttpUserService>(Lifestyle.Transient);
            IoCContainer.Register<IPaymentsService, HttpPaymentsService>(Lifestyle.Transient);

            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            MainPage = new LoginPage();
        }

        public static bool IsInForeground { get; set; } = false;

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
