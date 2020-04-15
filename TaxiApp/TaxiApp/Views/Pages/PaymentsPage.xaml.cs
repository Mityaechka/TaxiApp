using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;
using TaxiApp.Services;
using TaxiApp.ViewModels;
using TaxiApp.Views.PopupPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaxiApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentsPage : ContentPage
    {
        public PaymentListViewModel Model { get; set; } = new PaymentListViewModel();
        public PaymentsPage()
        {
            InitializeComponent();
            BindingContext = this;
            Init();
        }
        public async void Init()
        {
            var list = new List<PaymentModel>();
            await Navigation.PushPopupAsync(new LoadingPopup());
            var response = await App.IoCContainer.GetInstance<IPaymentsService>().GetPaymentsResponse();
            var content = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(content);
            foreach(var item in jObject["data"])
            {
                list.Add(new PaymentModel()
                {
                    Id = Convert.ToInt32(item["serial_number"]),
                    Date = Convert.ToDateTime(item["created_at"]),
                    Cost = Convert.ToDecimal(item["price"]),
                    TransactionType = Convert.ToString(item["type"]),
                    Description = Convert.ToString(item["description"]),
                    Driver = Convert.ToString(item["username"])
                });
            }
            Model.Payments = list;
            await Navigation.PopPopupAsync();
        }

        private async void DataGrid_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var payment = DataGrid.SelectedItem as PaymentModel;
            if(payment!=null)
            await Navigation.PushPopupAsync(new PaymentPopup(payment));
            DataGrid.SelectedItem = null;
        }
    }
}