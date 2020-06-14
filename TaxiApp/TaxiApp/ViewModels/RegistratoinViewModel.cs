using TaxiApp.Models;

namespace TaxiApp.ViewModels
{
    public class RegistratoinViewModel : BaseViewModel
    {
        private RegistrationModel model { get; set; }
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
        public string ConfirmPassword
        {
            get => model.ConfirmPassword;
            set
            {
                if (value == model.ConfirmPassword)
                {
                    return;
                }

                model.ConfirmPassword = value;
                RaisePropertyChanged();
            }
        }
        public RegistratoinViewModel()
        {
            model = new RegistrationModel();
        }
    }
}
