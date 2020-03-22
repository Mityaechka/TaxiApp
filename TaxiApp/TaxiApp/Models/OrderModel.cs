using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiApp.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Cost { get; set; }
    }
}
