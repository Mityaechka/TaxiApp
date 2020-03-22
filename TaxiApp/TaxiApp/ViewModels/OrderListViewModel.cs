using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaxiApp.Models;
using TaxiApp.Services;
using Xamarin.Forms;

namespace TaxiApp.ViewModels
{
    public class OrderListViewModel:BaseViewModel
    {
        ObservableCollection<OrderModel> models { get; set; } = new ObservableCollection<OrderModel>();
        public bool IsEmpty
        {
            get
            {
                return models.Count == 0;
            }
        }
        public OrderListViewModel()
        {
        }
        public ObservableCollection<OrderModel> Models
        {
            get { return models; }
            set
            {
                if (value == models)
                    return;
                models = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEmpty");
            }
        }
    }
}
