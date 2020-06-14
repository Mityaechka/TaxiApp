using TaxiApp.Models;
using TaxiApp.Services;

namespace TaxiApp.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private UserModel model { get; set; }
        public UserViewModel()
        {
            model = IUserService.AuthUser;
        }
        public int Id
        {
            get => model.Id;
            set
            {
                if (value == model.Id)
                {
                    return;
                }

                model.Id = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => model.Name;
            set
            {
                if (value == model.Name)
                {
                    return;
                }

                model.Name = value;
                RaisePropertyChanged();
            }
        }
    }
}
