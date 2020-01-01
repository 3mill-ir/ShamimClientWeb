using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ShopItem
    {
        public int ID { get; set; }
        public Nullable<int> F_ItemID { get; set; }
        public Nullable<int> F_ShopID { get; set; }
        public Nullable<DateTime> CreatedDateOnUtc { get; set; }
        public Nullable<int> Count { get; set; }
        public virtual ItemNew Item { get; set; }
        public virtual Shop Shop { get; set; }
    }
}