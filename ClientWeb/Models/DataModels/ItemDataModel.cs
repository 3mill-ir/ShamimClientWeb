using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ItemDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> CreatedDateOnUTC { get; set; }
        public string SubmitedState { get; set; }
        public Nullable<int> F_MenuID { get; set; }
        public string F_UserID { get; set; }
        public Nullable<int> NumberOfVisitors { get; set; }
    }
}