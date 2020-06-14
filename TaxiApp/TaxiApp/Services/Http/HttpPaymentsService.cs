using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Http
{
    internal class HttpPaymentsService : IPaymentsService
    {
        private readonly HttpService httpService;

        public HttpPaymentsService(IUserService service, HttpService httpService) : base(service)
        {
            this.httpService = httpService;
        }


        public override async Task<ResponseModel<(List<PaymentModel>, double)>> GetPaymentsResponse(int page)
        {
            HttpResponseMessage result = await httpService.GetRequest("payments?format=json&page=" + page);
            double count = 0.0;
            try
            {
                JToken pagination = JObject.Parse(await result.Content.ReadAsStringAsync())["pagination"];
                count = Math.Ceiling(Convert.ToDouble(pagination["totalCount"]) / Convert.ToDouble(pagination["defaultPageSize"]));
            }
            catch (Exception)
            {

            }
            return result.RequestMessage.RequestUri.LocalPath == "/payments"
                ? ResponseModel<(List<PaymentModel>, double)>.OkResponse((getPayments(await result.Content.ReadAsStringAsync()), count))
                : (ResponseModel<(List<PaymentModel>, double)>.ErrorResponse());
        }

        private List<PaymentModel> getPayments(string content)
        {
            List<PaymentModel> list = new List<PaymentModel>();
            JObject jObject = JObject.Parse(content);
            int index = 0;
            foreach (JToken item in jObject["data"])
            {
                try
                {
                    list.Add(new PaymentModel()
                    {
                        Index = index++,
                        Id = Convert.ToInt32(item["serial_number"]),
                        Date = Convert.ToDateTime(item["created_at"]),
                        Cost = Convert.ToDecimal(item["price"]),
                        TransactionType = Convert.ToString(item["type"]),
                        Description = Convert.ToString(item["description"]),
                        Driver = Convert.ToString(item["username"])
                    });
                }catch(Exception e) { }
            }
            return list;
        }

    }
}
