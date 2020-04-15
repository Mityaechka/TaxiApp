using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TaxiApp.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Driver { get; set; }
        public bool CanAccept => Driver == "";
        public DateTime CreatedDate { get; set; }
        public string DateFormat => CreatedDate.ToString("d.MM.yyyy HH:mm");
        public decimal Cost { get; set; }
        public bool CallPassenger => !CanAccept;
        public string Phone { get; set; }
        public bool HasPhone => Phone != "";
        public GridLength PhoneLineLength => HasPhone? new GridLength(1):new GridLength(0);
        public GridLength PhoneColumnLength =>HasPhone? new GridLength(2,GridUnitType.Star): new GridLength(0);

        public Color Color
        {
            get
            {
                if (CanAccept)
                    return Color.FromHex("00FF00");
                else
                    return Color.FromHex("fff6aa");
            }
        }
        public static List<OrderModel> FromJson(string s)
        {
            var list = new List<OrderModel>();
            foreach (var item in JObject.Parse(s)["data"])
            {
                list.Add(new OrderModel()
                {
                    From = Convert.ToString(item["place_from"]),
                    To = Convert.ToString(item["place_to"]),
                    CreatedDate = Convert.ToDateTime(item["created_at"]),
                    Cost = Convert.ToDecimal(item["price"]),
                    Phone = Convert.ToString(item["phone"])
                });
            }
            return list;

        }
    }
}
