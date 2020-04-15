using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using Xamarin.Essentials;

namespace TaxiApp.Services
{
    public class HttpService
    {
        readonly string baseUrl = "http://testing.taxivek.ru/";
        const string csrf_cook = "145df92375049aa8dd40bb89135e7609d63538b80083f8895f2919aff43ee70ca%3A2%3A%7Bi%3A0%3Bs%3A5%3A%22_csrf%22%3Bi%3A1%3Bs%3A32%3A%22LsvWF9VsAmmLdPrX3eS2kFRj_LXxdCQM%22%3B%7D";
        const string csrf_post = "WmRlV0RHUWYWFxMAAn4HFRsJCBsgFyM.aQE2ZS8BAwwFKD0vIAQAKw==";
        static HttpClient client;
        public HttpService()
        {
            
        }
        static bool flag = true;

        public async Task<HttpResponseMessage> Login(LoginModel loginModel)
        {
            if (flag)
            {
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true
                };
                var uri = new Uri(baseUrl);
                client = new HttpClient(handler) { BaseAddress = uri };
                cookieContainer.Add(uri, new Cookie("_csrf", csrf_cook));
                flag = false;
            }
            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", csrf_post),
                    new KeyValuePair<string, string>("LoginForm[username]", loginModel.Login),
                    new KeyValuePair<string, string>("LoginForm[password]", loginModel.Password),
                    new KeyValuePair<string, string>("LoginForm[rememberMe]", loginModel.RememberMe?"1":"0")
                   });
            
            var result = await client.PostAsync("site/login", content);
            return result;
        }
        public async Task<HttpResponseMessage> Execdaypayment()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", csrf_post)
                });
            var result = await client.PostAsync("orders/execdaypayment", content);
            return result;
        }
        public async Task<HttpResponseMessage> GetRelevantOrders()
        {
            var uri = new Uri(baseUrl);

            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", csrf_post)
                });
            var result = await client.PostAsync("orders/countnew", content);
            return result;
        }
        public async Task<HttpResponseMessage> Logout()
        {
            var csrfResponse = await client.PostAsync("site/logout", null);
            var doc = new HtmlDocument();
            doc.LoadHtml(await csrfResponse.Content.ReadAsStringAsync());
            var token =doc.DocumentNode.SelectSingleNode("//meta[@name='csrf-token']").Attributes["content"].Value;
            var uri = new Uri(baseUrl);

            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", token)
                });
            var result = await client.PostAsync("site/logout",content);
            var s = await result.Content.ReadAsStringAsync();
            //var cookieContainer = new CookieContainer();
            //var handler = new HttpClientHandler
            //{
            //    CookieContainer = cookieContainer,
            //    UseCookies = true
            //};
            //client = new HttpClient(handler) { BaseAddress = uri };
            //cookieContainer.Add(uri, new Cookie("_csrf", csrf_cook));

            return result;
        }
        public async Task<HttpResponseMessage> TakeOrder(string id)
        {
            var result = await client.GetAsync("orders/take?id="+id);
            return result;
        }
        public async Task<HttpResponseMessage> UntakeOrder(string id)
        {
            var result = await client.GetAsync("orders/untake?id=" + id);
            return result;
        }
        public async Task<HttpResponseMessage> CallPassenger(string id)
        {
            var result = await client.GetAsync("orders/call_passenger?id=" + id);
            return result;
        }
        public async Task<HttpResponseMessage> GetPayments(int page)
        {
            var result = await client.GetAsync("payments?format=json");
            return result;
        }
        public async Task<HttpResponseMessage> GetFailedOrders(int page = 1)
        {
            var result = await client.GetAsync("orders/failed?format=json&page="+page);
            return result;
        }
        public async Task<HttpResponseMessage> GetCompletedOrders(int page = 1)
        {
            var result = await client.GetAsync("orders/archive?format=json&page="+page);
            return result;
        }
        public async Task<HttpResponseMessage> ProfileResponse()
        {
            var result = await client.GetAsync("site/profile?format=json");
            return result;
        }
    }
}
