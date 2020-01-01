using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ShopPagedList
    {
        public ShopPagedList()
        {
            ShopList = new List<Shop>();
        }
        public List<Shop> ShopList { get; set; }
        public int Total { get; set; }
    }
}