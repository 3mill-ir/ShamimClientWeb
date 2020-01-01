using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class Shop
    {
        public Shop()
        {
            this.ShopItem = new List<ShopItem>();
        }

        public int ID { get; set; }
        public string F_UserID { get; set; }
        public string TotalPrice { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> CreatedDateOnUtc { get; set; }
        public Nullable<System.DateTime> OrderedDate { get; set; }
        public Nullable<bool> Payed { get; set; }
        public string PayType { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryCost { get; set; }
        public Nullable<System.DateTime> DeliveryTime { get; set; }

        public List<ShopItem> ShopItem { get; set; }
    }
}