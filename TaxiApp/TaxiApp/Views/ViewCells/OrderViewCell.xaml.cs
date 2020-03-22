using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderViewCell : ViewCell
    {
        public static readonly BindableProperty FromProperty =
        BindableProperty.Create("From", typeof(string), typeof(OrderViewCell), "");
        public string From
        {
            get { return (string)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }
        public static readonly BindableProperty ToProperty =
        BindableProperty.Create("To", typeof(string), typeof(OrderViewCell), "");
        public string To
        {
            get { return (string)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
        public static readonly BindableProperty CostProperty =
        BindableProperty.Create("Cost", typeof(string), typeof(OrderViewCell), "0");
        public string Cost
        {
            get { return (string)GetValue(CostProperty); }
            set { SetValue(CostProperty, value); }
        }
        public OrderViewCell()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                FromLabel.Text =$"Откуда: {From}";
                ToLabel.Text =$"Куда: {To}";
                CostLabel.Text =$"Цена: {Cost}";
            }
        }
    }
}