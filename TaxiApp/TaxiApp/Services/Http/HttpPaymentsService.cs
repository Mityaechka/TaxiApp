using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Http
{
    class HttpPaymentsService : IPaymentsService
    {
        public HttpPaymentsService(IUserService service) : base(service)
        {
        }

        public override Task<List<PaymentModel>> GetPayments()
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> GetPaymentsResponse()
        {
            var service = App.IoCContainer.GetInstance<HttpService>();
            return await service.GetPayments(1);
        }
    }
}
