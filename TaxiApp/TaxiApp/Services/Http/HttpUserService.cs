using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Http
{
    public class HttpUserService : IUserService
    {
        public HttpUserService(HttpService httpService) : base()
        {
            HttpService = httpService;
        }

        public HttpService HttpService { get; }
        public override async Task<string> GetMoney()
        {

            HttpService httpService = App.IoCContainer.GetInstance<HttpService>();
            HttpResponseMessage response = await httpService.ProfileResponse();
            if (response.RequestMessage.RequestUri.LocalPath == "/site/login")
            {
                return "0";
            }

            string content = await response.Content.ReadAsStringAsync();
            JObject jObject = JObject.Parse(content);
            string money = jObject["profile"]["money"].ToString();
            return money;
        }

        public override async Task<HttpResponseMessage> Login(LoginModel model)
        {
            HttpResponseMessage r = await HttpService.Login(model);
            string s = await r.Content.ReadAsStringAsync();
            //switch (r.RequestMessage.RequestUri.LocalPath)
            //{
            //    case "У данного пользователя есть другая активная сессия. Закройте ее, чтобы продолжить работу!":
            //        break;
            //}

            return r;
        }

        public override async Task<HttpResponseMessage> Logout()
        {
            return await HttpService.Logout();

        }

        public override Task Registration(RegistrationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
