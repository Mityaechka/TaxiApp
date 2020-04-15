using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;

namespace TaxiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private LoginModel model { get; set; }
        IUserService userService;
        public string Login
        {
            get { return model.Login; }
            set
            {
                if (value == model.Login) return;
                model.Login = value;
                RaisePropertyChanged();
            }
        }
        public string Password
        {
            get { return model.Password; }
            set
            {
                if (value == model.Password) return;
                model.Password = value;
                RaisePropertyChanged();
            }
        }
        bool isLoading { get; set; }
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value ;
                RaisePropertyChanged();
            }
        }
        public async Task<HttpResponseMessage> Auth()
        {
            return await userService.Login(model);
        }

        public bool RememberMe
        {
            get { return model.RememberMe; }
            set
            {
                if (value == model.RememberMe) return;
                model.RememberMe = value;
                RaisePropertyChanged();
            }
        }
        public LoginViewModel()
        {
            model = new LoginModel();
            userService = App.IoCContainer.GetInstance<IUserService>();
        }
    }
}
