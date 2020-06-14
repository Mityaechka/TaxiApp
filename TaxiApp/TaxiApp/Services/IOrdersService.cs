using System.Collections.Generic;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services
{
    public abstract class IOrdersService
    {
        public abstract Task<ResponseModel<List<OrderModel>>> GetRelenantsResponse(OrderType orderType);
        public abstract Task<ResponseModel<(List<OrderModel>, double)>> GetOrdersResponse(OrderType orderType, int page);
        public abstract Task<string> AccountStatePath();
        public abstract Task<ResponseModel<string>> TakeOrder(string id);
        public abstract Task<ResponseModel<string>> UnakeOrder(string id);
        public abstract Task<ResponseModel<string>> CallPassanger(string id);
        public abstract Task<string> GetRelevantTextResponse(OrderType orderType);
    }
}
