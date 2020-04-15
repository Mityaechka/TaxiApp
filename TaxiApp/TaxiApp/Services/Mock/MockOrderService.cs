using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Mock
{
    public class MockOrderService : IOrdersService
    {
        static Dictionary<OrderType, List<OrderModel>> orders { get; set; } = new Dictionary<OrderType, List<OrderModel>>();
        public MockOrderService()
        {
            orders = new Dictionary<OrderType, List<OrderModel>>(){
            {
                OrderType.Completed, new List<OrderModel>()
                {
                    new OrderModel(){ Id = 1,From="елизавета",To="павловская",CreatedDate=DateTime.Now,Cost=100},
                    new OrderModel(){ Id = 2,From="Кооперации 167 а п 3",To="сыр",CreatedDate=DateTime.Now,Cost=70},
                    new OrderModel(){ Id = 3,From="Громкий переулок 10",To="цепи",CreatedDate=DateTime.Now,Cost=80},
                    new OrderModel(){ Id = 4,From="Тест",To="Тест",CreatedDate=DateTime.Now,Cost=110}
                }
            },
            {
                OrderType.Failed, new List<OrderModel>()
                {
                    new OrderModel(){ Id = 5,From="елизавета",To="павловская",CreatedDate=DateTime.Now,Cost=100},
                    new OrderModel(){ Id = 6,From="Кооперации 167 а п 3",To="сыр",CreatedDate=DateTime.Now,Cost=70},
                    new OrderModel(){ Id = 7,From="Громкий переулок 10",To="цепи",CreatedDate=DateTime.Now,Cost=80},
                    new OrderModel(){ Id = 8,From="Тест",To="Тест",CreatedDate=DateTime.Now,Cost=110}
                }
            },
                {OrderType.Relevant,new List<OrderModel>() }
        };
        }
        public override async Task<OrderModel> GetOrder(int id)
        {
            await Task.Delay(1);
            return orders.SelectMany(x => x.Value).FirstOrDefault(x => x.Id == id);
        }

        public override async Task<List<OrderModel>> GetOrders(OrderType orderType)
        {
            await Task.Delay(1);

            return orders[orderType];
        }

        public override Task<HttpResponseMessage> GetOrdersResponse(OrderType order,int page)
        {
            throw new NotImplementedException();
        }
    }
}
