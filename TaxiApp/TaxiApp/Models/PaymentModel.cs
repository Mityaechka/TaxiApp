using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Spent { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string Driver { get; set; }
    }
}
