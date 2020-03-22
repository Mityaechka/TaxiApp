using SimpleInjector;
using System;
using TaxiApp.Services;
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

            IoCContainer.Register<IOrdersService, MockOrderService>(Lifestyle.Transient);
            IoCContainer.Register<IUserService, MockUserService>(Lifestyle.Transient);
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            // You have to create a Master ContentPage()
            MainPage = new LoginPage();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
