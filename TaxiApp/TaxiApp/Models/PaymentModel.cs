using System;

namespace TaxiApp.Models
{
    public class PaymentModel
    {

        public int Id { get; set; }
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        private string spent;

        public string Spent
        {
            get => string.IsNullOrEmpty(spent) ? "не задано" : spent;
            set => spent = value;
        }

        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string Driver { get; set; }
        public string DateFormat => Date.ToString("d.MM.yyyy HH:mm");
    }
}
