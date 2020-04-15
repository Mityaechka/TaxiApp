using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

            var httpService = App.IoCContainer.GetInstance<HttpService>();
            var response = await httpService.ProfileResponse();
            if (response.RequestMessage.RequestUri.LocalPath == "/site/login")
                return "0";
            var content = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(content);
            var money = jObject["profile"]["money"].ToString();
            return money;
        }

        public override async Task<HttpResponseMessage> Login(LoginModel model)
        {
            var r = await HttpService.Login(model);
            var s = await r.Content.ReadAsStringAsync();
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
