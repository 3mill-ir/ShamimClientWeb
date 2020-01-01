using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ShekayatInboxOutBox
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> CreatedDateOnUtc { get; set; }
        public Nullable<int> F_ShekayatID { get; set; }
        public string Text { get; set; }

        public virtual ShekayatModel Shekayat { get; set; }
    }
}