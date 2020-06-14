using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaxiApp.Models;

namespace TaxiApp.ViewModels
{
    public class PaymentListViewModel : BaseViewModel
    {
        private ObservableCollection<PaymentModel> models { get; set; }
        public List<PaymentModel> Payments
        {
            set => Models = new ObservableCollection<PaymentModel>(value);
        }
        public PaymentListViewModel()
        {
        }
        public ObservableCollection<PaymentModel> Models
        {
            get => models;
            set
            {
                if (value == models)
                {
                    return;
                }

                models = value;
                RaisePropertyChanged();
            }
        }
    }
}
