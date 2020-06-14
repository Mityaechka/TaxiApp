using System.Collections.ObjectModel;
using System.Linq;
using TaxiApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace TaxiApp.ViewModels
{
    public class OrderListViewModel : BaseViewModel
    {
        private ObservableCollection<OrderModel> models { get; set; } = new ObservableCollection<OrderModel>();
        public PaletteCollection Palette
        {
            get
            {
                PaletteCollection p = new PaletteCollection();
                switch (OrderType)
                {
                    case OrderType.Relevant:
                        p.Add(Color.FromHex("00FF00"));
                        break;
                    case OrderType.Failed:
                        p.Add(Color.FromHex("FF7070"));

                        break;
                    case OrderType.Completed:
                        p.Add(Color.FromRgb(255,246,170));

                        break;
                    default:
                        break;
                }
                return p;
            }
        }
        public bool IsEmpty => models.Count == 0;
        public OrderListViewModel(OrderType orderType)
        {
            OrderType = orderType;
        }
        public ObservableCollection<OrderModel> Models
        {
            get => models;
            set

            {
                //if (value == models)
                //{
                //    return;
                //}

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

        public OrderType OrderType { get; }
    }
}
