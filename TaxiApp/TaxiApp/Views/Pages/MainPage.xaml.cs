using System;
using System.Collections.Generic;
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
    public partial class MainPage : Shell
    {
        public UserViewModel User { get;  set; }
        public MainPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("login", typeof(LoginPage));
            User = new UserViewModel();
            BindingContext = this;
        }
        public void LogoutClick(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}