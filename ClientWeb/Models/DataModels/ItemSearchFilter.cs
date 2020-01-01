using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ItemSearchFilter
    {
        public string type { get; set; }
        public int categoryid { get; set; }
        public string keyword { get; set; }
    }
}