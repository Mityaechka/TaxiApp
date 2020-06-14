using System.Net.Http;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;

namespace TaxiApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private LoginModel model { get; set; }

        private readonly IUserService userService;
        public string Login
        {
            get => model.Login;
            set
            {
                if (value == model.Login)
                {
                    return;
                }

                model.Login = value;
                RaisePropertyChanged();
            }
        }
        public string Password
        {
            get => model.Password;
            set
            {
                if (value == model.Password)
                {
                    return;
                }

                model.Password = value;
                RaisePropertyChanged();
            }
        }

        private bool isLoading { get; set; }
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                RaisePropertyChanged();
            }
        }
        public async Task<HttpResponseMessage> Auth()
        {
            return await userService.Login(model);
        }

        public bool RememberMe
        {
            get => model.RememberMe;
            set
            {
                if (value == model.RememberMe)
                {
                    return;
                }

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
