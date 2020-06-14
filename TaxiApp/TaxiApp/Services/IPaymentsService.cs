using System.Collections.Generic;
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
        public abstract Task<ResponseModel<(List<PaymentModel>, double)>> GetPaymentsResponse(int page);
    }
}
