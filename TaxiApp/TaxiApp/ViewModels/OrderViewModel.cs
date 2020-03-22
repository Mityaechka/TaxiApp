using System;
using System.Collections.Generic;
using System.Text;
using TaxiApp.Models;

namespace TaxiApp.ViewModels
{
    public class OrderViewModel:BaseViewModel
    {
        private OrderModel model { get; set; }
        public OrderViewModel()
        {

        }
        public int Id
        {
            get { return model.Id; }
            set
            {
                if (value == model.Id) return;
                model.Id = value;
                RaisePropertyChanged();
            }
        }
        public string From
        {
            get { return model.From; }
            set
            {
                if (value == model.From) return;
                model.From = value;
                RaisePropertyChanged();
            }
        }
        public string To
        {
            get { return model.To; }
            set
            {
                if (value == model.To) return;
                model.To = value;
                RaisePropertyChanged();
            }
        }
        public DateTime CreatedDate
        {
            get { return model.CreatedDate; }
            set
            {
                if (value == model.CreatedDate) return;
                model.CreatedDate = value;
                RaisePropertyChanged();
            }
        }
        public decimal Cost
        {
            get { return model.Cost; }
            set
            {
                if (value == model.Cost) return;
                model.Cost = value;
                RaisePropertyChanged();
            }
        }
    }
}
