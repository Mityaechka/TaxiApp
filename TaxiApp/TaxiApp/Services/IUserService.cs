using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services
{
    public abstract class IUserService
    {
        public static  UserModel AuthUser { get; set; }
        public abstract Task Registration(RegistrationModel model);
        public abstract Task<bool> Login(LoginModel model);
        public abstract Task Logout();
    }
}
