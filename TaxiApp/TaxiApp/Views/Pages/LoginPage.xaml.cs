using Newtonsoft.Json.Linq;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TaxiApp.DependencyServices;
using TaxiApp.Services;
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
        public bool ShowLoadingPage { get; set; } = false;
        public LoginPage(bool IsStartPage = true)
        {
            InitializeComponent();

            Model = new LoginViewModel();
            string login = CrossSettings.Current.GetValueOrDefault("login", "");
            string password = CrossSettings.Current.GetValueOrDefault("password", "");
            if (login != "" && password != "")
            {
                Model.Login = login;
                Model.Password = password;
                Model.RememberMe = true;
                bool loginOnStart = CrossSettings.Current.GetValueOrDefault("loginOnStart", true);

                if (IsStartPage && loginOnStart)
                {
                    LoginClick(null, null);
                }
            }
            BindingContext = this;

        }

        private async void LoginClick(object sender, EventArgs e)
        {
            try
            {
                MainPage page = new MainPage();
                Model.IsLoading = true;
                var result = await Model.Auth();
                var r = await (await App.IoCContainer.GetInstance<HttpService>().GetRequest("orders/countnew")).Content.ReadAsStringAsync(); ;
                string s = await result.Content.ReadAsStringAsync();
                string path = result.RequestMessage.RequestUri.LocalPath;
                //Model.IsLoading = false;
                //if (result.IsSuccessStatusCode)
                //{
                    CrossSettings.Current.AddOrUpdateValue("loginOnStart", true);
                bool check = false;
                    try
                    {
                        var jObject = Newtonsoft.Json.Linq.JObject.Parse(s);

                        if (s.Contains("msg"))
                        {
                            var message = "";
                            var msg = jObject["msg"] as JObject;
                            if (msg.ContainsKey("username"))
                            {
                                message = (string)msg["username"][0];
                            }
                            else if (msg.ContainsKey("password"))
                            {
                                message = (string)msg["password"][0];
                            }
                            await DisplayAlert("Ошибка", message, "Ok");
                        }
                        else
                        {
                            if (Model.RememberMe)
                            {
                                var httpService = App.IoCContainer.GetInstance<HttpService>();
                                httpService.SaveHeaders();
                                CrossSettings.Current.AddOrUpdateValue("login", Model.Login);
                                CrossSettings.Current.AddOrUpdateValue("password", Model.Password);
                            }
                            await page.SetDetail();
                            Application.Current.MainPage = page;
                        }
                    }catch(Exception exception) { check = true; }

                if (check &&(r.Contains("blocked")|| r.Contains("block_nomoney")|| r.Contains("block_notdaypayment")|| r.Contains("counts")))
                {
                    bool sendApps = CrossSettings.Current.GetValueOrDefault("sendApps", true);
                    if (sendApps)
                    {
                        try
                        {
                            IAppListService service = DependencyService.Get<IAppListService>();
                            List<string> apps = service.GetAppsList();
                            HttpService httpService = App.IoCContainer.GetInstance<HttpService>();
                            await httpService.SendApps(apps);
                            CrossSettings.Current.AddOrUpdateValue("sendApps", false);

                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                    if (Model.RememberMe)
                    {
                        var httpService = App.IoCContainer.GetInstance<HttpService>();
                        httpService.SaveHeaders();
                        CrossSettings.Current.AddOrUpdateValue("login", Model.Login);
                        CrossSettings.Current.AddOrUpdateValue("password", Model.Password);
                    }
                    await page.SetDetail();
                    Application.Current.MainPage = page;
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Произошла ошибка", "Повторите попытку позже", "Ok");
        Console.WriteLine(exception);
            }
            finally
            {
                Model.IsLoading = false;
            }
        }
    }
}