//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using TaxiApp.Models;

//namespace TaxiApp.Services.Mock
//{
//    public class MockUserService : IUserService
//    {
//        public static List<UserModel> users = new List<UserModel>()
//        {
//            new UserModel(){Id=1,Name="Dima"},
//            new UserModel(){Id=2,Name="Юра"},
//            new UserModel(){Id=3,Name="Дмитрий"},
//            new UserModel(){Id=4,Name="Федор"}
//        };
//        public const string password = "123456";

//        public override Task<string> GetMoney()
//        {
//            throw new NotImplementedException();
//        }

//        public override async Task<HttpResponseMessage> Login(LoginModel model)
//        {
//            var user = users.FirstOrDefault(x => x.Name == model.Login && model.Password == password);
//            await Task.Delay(1);
//            if (user == null)
//                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
//            else
//            {
//                AuthUser = user;
//                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest };
//            }
//        }

//        public override Task<HttpResponseMessage> Logout()
//        {
//            throw new NotImplementedException();
//        }

//        public override Task Registration(RegistrationModel model)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
