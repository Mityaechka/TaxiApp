using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TaxiApp.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Driver { get; set; }
        public bool CanAccept => Driver == "";
        public DateTime CreatedDate { get; set; }
        public string DateFormat => CreatedDate.ToString("d.MM.yyyy HH:mm");
        public decimal Cost { get; set; }
        public bool CallPassenger { get; set; }
        public bool IsPreliminary { get; set; }

        public string Phone { get; set; }
        public bool HasPhone => Phone != "";
        public GridLength PhoneLineLength => HasPhone ? new GridLength(1) : new GridLength(0);
        public GridLength PhoneColumnLength => HasPhone ? new GridLength(2, GridUnitType.Star) : new GridLength(0);
        public string CallImage => CallPassenger ? "phone_green.png" : "phone_red.png";
        public string CallImageBig => CallPassenger ? "phone_green_big.png" : "phone_red_big.png";
        public Color CallColor => CallPassenger ? Color.Green : Color.Red;
        public string CallText => CallPassenger ? "Вызвать пасажира" : "Отменить вызов";

        public bool IsFaked { get; set; }

        public Color Color
        {
            get
            {
                if (IsPreliminary)
                    return Color.FromHex("FFD700");
                else if(IsFaked)
                    return Color.FromHex("FF3333");
                else
                {
                    if (CanAccept)
                    {
                        return Color.FromHex("00FF00");

                    }
                    else
                    {
                        return Color.FromHex("fff6aa");
                    }
                }
            }
        }
        public static List<OrderModel> FromJson(string s)
        {
            List<OrderModel> list = new List<OrderModel>();
            int index = 1;
            foreach (JToken item in JObject.Parse(s)["data"])
            {

                list.Add(new OrderModel()
                {
                    Index=index++,
                    From = Convert.ToString(item["place_from"]),
                    To = Convert.ToString(item["place_to"]),
                    CreatedDate = Convert.ToDateTime(item["created_at"]),
                    Cost = Convert.ToDecimal(item["price"]),
                    Phone = Convert.ToString(item["phone"]),
                    IsPreliminary = Convert.ToBoolean(item["is_preliminary"]),
                    IsFaked = Convert.ToBoolean(item["is_faked"])

                });
            }
            return list;

        }
    }
}
