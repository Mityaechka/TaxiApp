using Newtonsoft.Json.Linq;
using Plugin.Settings;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
         public LoginViewModel Model { get; set; }
        public ICommand Login { get; set; }
        public bool ShowLoadingPage { get; set; } = false;
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
            Model.IsLoading= true;
            //await Navigation.PushPopupAsync(new LoadingPopup());
            var result = await Model.Auth();
            var s = await result.Content.ReadAsStringAsync();
            var path = result.RequestMessage.RequestUri.LocalPath;
            if (result.IsSuccessStatusCode)
            {
                if (Model.RememberMe&&path!= "/site/login")
                {
                    CrossSettings.Current.AddOrUpdateValue("login", Model.Login);
                    CrossSettings.Current.AddOrUpdateValue("password", Model.Password);
                }
                var page = new MainPage();
                switch (path)
                {
                    case "/orders":
                        page.SetDetail(Enums.AccountState.Ok);
                        Application.Current.MainPage = page;
                        break;
                    case "/orders/block_notdaypayment":
                        page.SetDetail(Enums.AccountState.NotDayPayment);
                        Application.Current.MainPage = page;
                        break;
                    case "/site/login":
                        await DisplayAlert("Ошибка", "Неверное имя пользователя или пароль.", "Ok");
                        break;
                    case "/orders/nomoney":
                        page.SetDetail(Enums.AccountState.NoMoney);
                        Application.Current.MainPage = page;
                        break;
                    case "/orders/blocked":
                        Application.Current.MainPage = new Blocked();
                        break;
                    default:
                        //throw new Exception("Незарегистрированный путь");
                        page.SetDetail(Enums.AccountState.Ok);
                        Application.Current.MainPage = page;
                        break;
                }
                
            }
            Model.IsLoading = false;
            //await Navigation.PopPopupAsync();
        }
    }
}