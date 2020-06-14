using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Http
{
    public class HttpOrdersService : IOrdersService
    {
        public HttpOrdersService(HttpService httpService)
        {
            HttpService = httpService;
        }

        public HttpService HttpService { get; }

        public override async Task<string> AccountStatePath()
        {
            HttpResponseMessage r = await HttpService.GetRequest("orders");
            return r.RequestMessage.RequestUri.LocalPath;
        }
        public override async Task<string> GetRelevantTextResponse(OrderType orderType)
        {
            var task = await  HttpService.GetRequest("orders/countnew");
            return await task.Content.ReadAsStringAsync();
        }
        public override async Task<ResponseModel<List<OrderModel>>> GetRelenantsResponse(OrderType orderType)
        {
            Task<HttpResponseMessage> task = null;
            string path = "";

            task = HttpService.GetRequest("orders/countnew");
            path = "/orders/countnew";

            HttpResponseMessage result = await task;
            var s = await result.Content.ReadAsStringAsync();
            bool correctPath = result.RequestMessage.RequestUri.LocalPath == path;
            return correctPath
                ? ResponseModel<List<OrderModel>>.OkResponse(getOrders(s))
                : ResponseModel<List<OrderModel>>.ErrorResponse();
        }
        public override async Task<ResponseModel<(List<OrderModel>, double)>> GetOrdersResponse(OrderType orderType, int page)
        {
            Task<HttpResponseMessage> task = null;
            string path = "";
            switch (orderType)
            {
                case OrderType.Failed:
                    task = HttpService.GetRequest("orders/failed?format=json&page=" + page);
                    path = "/orders/failed";
                    break;
                case OrderType.Completed:
                    task = HttpService.GetRequest("orders/archive?format=json&page=" + page);
                    path = "/orders/archive";
                    break;
                default:

                    break;
            }
            HttpResponseMessage result = await task;
            string content = await result.Content.ReadAsStringAsync();

            bool correctPath = result.RequestMessage.RequestUri.LocalPath == path;
            return correctPath
                ? ResponseModel<(List<OrderModel>, double)>.OkResponse((OrderModel.FromJson(content), getPageCount(content)))
                : ResponseModel<(List<OrderModel>, double)>.ErrorResponse();

        }



        public override async Task<ResponseModel<string>> TakeOrder(string id)
        {
            HttpResponseMessage r = await HttpService.GetRequest("orders/take?id=" + id);
            return ResponseModel<string>.OkResponse(await r.Content.ReadAsStringAsync());
        }

        public override async Task<ResponseModel<string>> UnakeOrder(string id)
        {
            HttpResponseMessage r = await HttpService.GetRequest("orders/untake?id=" + id);
            return ResponseModel<string>.OkResponse(await r.Content.ReadAsStringAsync());
        }

        public override async Task<ResponseModel<string>> CallPassanger(string id)
        {
            HttpResponseMessage r = await HttpService.GetRequest("orders/call_passenger?id=" + id);
            return ResponseModel<string>.OkResponse(await r.Content.ReadAsStringAsync());
        }

        private double getPageCount(string content)
        {

            JToken pagination = JObject.Parse(content)["pagination"];
            return Math.Ceiling(Convert.ToDouble(pagination["totalCount"]) / Convert.ToDouble(pagination["defaultPageSize"]));
            // Math.Round(Convert.ToDouble(pagination["totalCount"]) / Convert.ToDouble(pagination["defaultPageSize"]) + 0.1);
        }

        private List<OrderModel> getOrders(string s)
        {
            JObject data = JObject.Parse(s);

            List<OrderModel> list = new List<OrderModel>();
            for (int i = 0; i < data["orders_ids"].Count(); i++)
            {
                int id = (int)data["orders_ids"][i];
                JToken item = data["orders"][id.ToString()];
                list.Add(new OrderModel()
                {
                    Id = Convert.ToInt32(item["id"]),
                    From = Convert.ToString(item["place_from"]),
                    To = Convert.ToString(item["place_to"]),
                    CreatedDate = Convert.ToDateTime(item["created_at"]),
                    Cost = Convert.ToDecimal(item["price"]),
                    Driver = Convert.ToString(item["driver"]),
                    Phone = Convert.ToString(item["phone"]),
                    CallPassenger = Convert.ToBoolean(item["call_passenger"]),
                    IsPreliminary = Convert.ToBoolean(item["is_preliminary"]) 

                });
            }
            return list;
        }

    }
}
