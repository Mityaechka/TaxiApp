using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Http
{
    public class HttpOrdersService : IOrdersService
    {
        public HttpOrdersService(HttpService httpService) {
            HttpService = httpService;
        }

        public HttpService HttpService { get; }

        public override Task<OrderModel> GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<List<OrderModel>> GetOrders(OrderType orderType)
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> GetOrdersResponse(OrderType orderType,int page = 1)
        {
            switch (orderType)
            {
                case OrderType.Relevant:
                    return await HttpService.GetRelevantOrders();
                case OrderType.Failed:
                    return await HttpService.GetFailedOrders(page);
                case OrderType.Completed:
                    return await HttpService.GetCompletedOrders(page);
                default:
                    
                    break;
            }
            return null;
        }

    }
}
