using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaxiApp.Models;
using TaxiApp.Services;

namespace TaxiApp.ViewModels
{
    public class PaymentListViewModel : BaseViewModel
    {
         ObservableCollection<PaymentModel> models { get;  set; }
        public List<PaymentModel> Payments
        {
            set
            {
                Models = new ObservableCollection<PaymentModel>(value);
            }
        }
        public PaymentListViewModel()
        {
        }
        public ObservableCollection<PaymentModel> Models
        {
            get { return models; }
            set
            {
                if (value == models)
                    return;
                models = value;
                RaisePropertyChanged();
            }
        }
    }
}
