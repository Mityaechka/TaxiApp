using HtmlAgilityPack;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TaxiApp.Models;
using Xamarin.Essentials;
using AnySerializer;
using AnySerializer.Extensions;
namespace TaxiApp.Services
{
    public class HttpService
    {
        private readonly string baseUrl = "http://taxivek.ru/";
        private  string csrf_cook = "145df92375049aa8dd40bb89135e7609d63538b80083f8895f2919aff43ee70ca%3A2%3A%7Bi%3A0%3Bs%3A5%3A%22_csrf%22%3Bi%3A1%3Bs%3A32%3A%22LsvWF9VsAmmLdPrX3eS2kFRj_LXxdCQM%22%3B%7D";
        private static  string csrf_post = "";
        private static string csrf_token = "";

        private static HttpClient client;
        static CookieContainer cookieContainer;
        public HttpClient Client => client;

        public void LoadHTTPClient()
        {
            cookieContainer = new CookieContainer();
            Uri uri = new Uri(baseUrl);
            var s = CrossSettings.Current.GetValueOrDefault("session", null);
            if (s != null)
            {
                byte[] bytes = System.Convert.FromBase64String(s);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    var stream = new MemoryStream(bytes);
                    cookieContainer = bf.Deserialize(stream) as CookieContainer;
                    //cookieContainer = Serializer.Deserialize<CookieContainer>(bytes, SerializerOptions.Compact);
                }
                catch (Exception e)
                {
                    var m = e.Message;
                }
            }
            else
            {
                cookieContainer.Add(uri, new Cookie("_csrf", csrf_cook));
            }
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true
            };
            client = new HttpClient(handler) { BaseAddress = uri };
        }

        public void SaveHeaders()
        {
            // SAVE client Cookies

            var stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, cookieContainer);
            var bytes = stream.ToArray();
            var s = System.Convert.ToBase64String(bytes);

            CrossSettings.Current.AddOrUpdateValue("session", s);
        }
        public void RemoveHeaders()
        {
            CrossSettings.Current.Remove("session");
        }
        public Task<HttpResponseMessage> GetRequest(string path)
        {
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            return client.GetAsync(path, App.Token);
        }
        public HttpService()
        {
        }

        private static bool flag = true;

        public async Task<HttpResponseMessage> Login(LoginModel loginModel)
        {
            //RemoveHeaders();
            bool isHeadersSaved = CrossSettings.Current.GetValueOrDefault("session", null) != null;
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            LoadHTTPClient();
            csrf_post = CrossSettings.Current.GetValueOrDefault("csrf_post", "");
            //if (!isHeadersSaved)
            //{
                var tokenRequest =await  client.GetStringAsync("site/login");
                HtmlAgilityPack.HtmlDocument document = new HtmlDocument();
                document.LoadHtml(tokenRequest);
                var csrf = document.DocumentNode.SelectSingleNode("//meta[@name='csrf-token']");
                csrf_post = csrf.Attributes["content"].Value;
                CrossSettings.Current.AddOrUpdateValue("csrf_post", csrf_post);
            //}


            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", csrf_post),
                    new KeyValuePair<string, string>("LoginForm[username]", loginModel.Login),
                    new KeyValuePair<string, string>("LoginForm[password]", loginModel.Password),
                    new KeyValuePair<string, string>("LoginForm[rememberMe]", loginModel.RememberMe?"1":"0")
                   });

            HttpResponseMessage result = /*!isHeadersSaved ?*/
                await client.PostAsync("site/login?format=json", content, App.Token)/* : await client.PostAsync("orders", content, App.Token)*/;

            return result;
        }

        internal async Task SendApps(List<string> apps)
        {
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("_csrf", csrf_post),
                    new KeyValuePair<string, string>("list",Newtonsoft.Json.JsonConvert.SerializeObject( apps))
            });
            HttpResponseMessage response = await client.PostAsync("site/setprofileapplist?format=json", content, App.Token);
            string s = await response.Content.ReadAsStringAsync();

        }
        public async Task<HttpResponseMessage> Execdaypayment()
        {
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", csrf_post)
                });
            HttpResponseMessage result = await client.PostAsync("orders/execdaypayment", content, App.Token);
            return result;
        }
        public async Task<HttpResponseMessage> Logout()
        {
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            HttpResponseMessage csrfResponse = await client.PostAsync("site/logout", null, App.Token);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await csrfResponse.Content.ReadAsStringAsync());
            string token = doc.DocumentNode.SelectSingleNode("//meta[@name='csrf-token']").Attributes["content"].Value;
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("_csrf", token)
                });
            HttpResponseMessage result = await client.PostAsync("site/logout", content, App.Token);
            CrossSettings.Current.AddOrUpdateValue("sendApps", true);
            RemoveHeaders();
            return result;
        }

        public async Task<HttpResponseMessage> ProfileResponse()
        {
            if (!CheckInternetConnection())
            {
                throw new TaskCanceledException("No internet connection");
            }
            HttpResponseMessage result = await client.GetAsync("site/profile?format=json");
            return result;
        }

        private bool CheckInternetConnection()
        {
            NetworkAccess current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
