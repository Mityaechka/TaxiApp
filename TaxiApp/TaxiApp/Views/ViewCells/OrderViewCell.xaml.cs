using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.ViewCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderViewCell : ViewCell
    {


        public event EventHandler TakeOrderEvent;
        public event EventHandler UntakeOrderEvent;
        public event EventHandler CallPassangerEvent;
        public OrderViewCell()
        {
            InitializeComponent();
        }

        private void TakeClick(object sender, EventArgs e)
        {
            TakeOrderEvent?.Invoke(sender, e);
        }
        private void UntakeClick(object sender, EventArgs e)
        {
            UntakeOrderEvent?.Invoke(sender, e);
        }
        private void CallClick(object sender, EventArgs e)
        {
            CallPassangerEvent?.Invoke(sender, e);
        }
    }
}