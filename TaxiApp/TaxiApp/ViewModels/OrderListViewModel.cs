using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TaxiApp.Models;
using TaxiApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace TaxiApp.ViewModels
{
    public class OrderListViewModel:BaseViewModel
    {
        ObservableCollection<OrderModel> models { get; set; } = new ObservableCollection<OrderModel>();
        public PaletteCollection Palette
        {
            get
            {
                var p = new PaletteCollection();
                foreach(var model in models)
                {
                    if (model.CanAccept)
                        p.Add(Color.FromHex("00FF00"));
                    else
                        p.Add(Color.FromHex("fff6aa"));

                }
                return p;
            }
        }
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
                RaisePropertyChanged("HasPhone");
                RaisePropertyChanged("PhoneLineLength");
                RaisePropertyChanged("PhoneColumnLength");
            }
        }
        public bool HasPhone => models.Any(x => x.Phone != "");
        public GridLength PhoneLineLength => HasPhone ? new GridLength(1) : new GridLength(0);
        public GridLength PhoneColumnLength => HasPhone ? new GridLength(2, GridUnitType.Star) : new GridLength(0);
    }
}
