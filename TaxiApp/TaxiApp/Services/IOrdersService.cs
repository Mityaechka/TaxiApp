using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services
{
    public abstract class IOrdersService
    {
        public abstract Task<HttpResponseMessage> GetOrdersResponse(OrderType orderType,int page);
        public abstract Task<List<OrderModel>> GetOrders(OrderType orderType);
        public abstract Task<OrderModel> GetOrder(int id );

    }
}
