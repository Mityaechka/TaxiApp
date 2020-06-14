using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
        public double PageCount { get; set; }
        public PaymentsPage()
        {
            InitializeComponent();
            BindingContext = this;
            Init();
        }
        public async void Init()
        {
            try
            {
                List<PaymentModel> list = new List<PaymentModel>();
                await Navigation.PushPopupAsync(new LoadingPopup());
                ResponseModel<(List<PaymentModel>, double)> response = await App.IoCContainer.GetInstance<IPaymentsService>().GetPaymentsResponse(1);
                if (response.Status == Status.Ok)
                {
                    int i = 1;
                    foreach (var item in response.Data.Item1)
                    {
                        item.Index = i;
                        i++;
                    }
                    Model.Payments = response.Data.Item1;
                    PageCount = response.Data.Item2;
                    LoadPages();
                }
            }
            catch (Exception exception)
            {
                await DisplayAlert("Произошла ошибка", "Пвторите попытку позже", "Ok");
                Console.WriteLine(exception.Message);
            }
            finally
            {
                await Navigation.PopPopupAsync();
            }
        }

        private async void DataGrid_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PaymentModel payment = DataGrid.SelectedItem as PaymentModel;
            if (payment != null)
            {
                await Navigation.PushPopupAsync(new PaymentPopup(payment));
            }

            DataGrid.SelectedItem = null;
        }
        public async void LoadPages()
        {

            IPaymentsService service = App.IoCContainer.GetInstance<IPaymentsService>();
            for (int i = 2; i <= PageCount; i++)
            {
                ResponseModel<(List<PaymentModel>, double)> data = await service.GetPaymentsResponse(i);
                if (data.Status == Status.Ok)
                {
                    (List<PaymentModel>, double) list = data.Data;
                    foreach (PaymentModel item in list.Item1)
                    {
                        Model.Models.Add(item);
                    }
                }
                for (int j = 0; j < Model.Models.Count; j++)
                {
                    Model.Models[j].Index = j + 1;
                }
            }

        }
    }
}