using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxiApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
         public LoginViewModel Model { get; set; }
        public ICommand Login { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            Model = new LoginViewModel();
            var login = CrossSettings.Current.GetValueOrDefault("login", "");
            var password = CrossSettings.Current.GetValueOrDefault("password", "");
            if(login!=""&&password!="")
            {
                Model.Login = login;
                Model.Password = password;
                Model.RememberMe = true;
            }
            BindingContext = this;
        }

        private async void LoginClick(object sender, EventArgs e)
        {
            var result = await Model.Auth();
            if (result)
            {
                if (Model.RememberMe)
                {
                    CrossSettings.Current.AddOrUpdateValue("login", Model.Login);
                    CrossSettings.Current.AddOrUpdateValue("password", Model.Password);
                }
                App.Current.MainPage = new MainPage();
            }
        }
    }
}