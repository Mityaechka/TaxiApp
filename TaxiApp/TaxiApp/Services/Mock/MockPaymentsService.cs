using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models;

namespace TaxiApp.Services.Mock
{
    public class MockPaymentsService : IPaymentsService
    {
        static List<PaymentModel> payments = new List<PaymentModel>()
        {
            new PaymentModel(){Id=1,Date = DateTime.Now,Cost=100,Spent = "Алина Анчагова",TransactionType="Зачисление",Description="путёвка Бикмухаметов",Driver="Dima"},
            new PaymentModel(){Id=2,Date = DateTime.Now,Cost=130,Spent = null,TransactionType="Зачисление",Description="Возврат за снятие водителя с заказа",Driver="Dima"},
            new PaymentModel(){Id=3,Date = DateTime.Now,Cost=-70,Spent = null,TransactionType="Списание",Description="Списание за взятие заказа",Driver="01 Дмитрий Прищепа"},
            new PaymentModel(){Id=4,Date = DateTime.Now,Cost=90,Spent = "Алина Анчагова",TransactionType="Зачисление",Description="путёвка Бикмухаметов",Driver="01 Дмитрий Прищепа"},
            new PaymentModel(){Id=5,Date = DateTime.Now,Cost=110,Spent = "Нина Спирчагова",TransactionType="Зачисление",Description="путёвка Гачанова",Driver="01 Дмитрий Прищепа"}

        };

        public MockPaymentsService(IUserService service) : base(service)
        {
        }

        public override async Task<List<PaymentModel>> GetPayments()
        {
            await Task.Delay(1);
            var user = IUserService.AuthUser;
            return payments.Where(x => x.Driver == user.Name).ToList();
        }

        public override Task<HttpResponseMessage> GetPaymentsResponse()
        {
            throw new NotImplementedException();
        }
    }
}
