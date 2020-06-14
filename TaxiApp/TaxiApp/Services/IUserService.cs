using System.Net.Http;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services
{
    public abstract class IUserService
    {
        public static UserModel AuthUser { get; set; }
        public abstract Task Registration(RegistrationModel model);
        public abstract Task<HttpResponseMessage> Login(LoginModel model);
        public abstract Task<HttpResponseMessage> Logout();
        public abstract Task<string> GetMoney();
    }
}
