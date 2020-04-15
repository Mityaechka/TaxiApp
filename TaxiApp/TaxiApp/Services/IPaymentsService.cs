using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services
{
    public abstract class IPaymentsService
    {
        protected IUserService userService;
        public IPaymentsService(IUserService service)
        {
            userService = service;
        }
        public abstract Task<List<PaymentModel>> GetPayments();
        public abstract Task<HttpResponseMessage> GetPaymentsResponse();
    }
}
