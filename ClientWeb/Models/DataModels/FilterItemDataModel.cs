using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class FilterItemDataModel
    {
        public int MenuId { get; set; }
        public string type { get; set; }
        public Nullable<DateTime> FromTime { get; set; }
        public Nullable<DateTime> ToTime { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string search { get; set; }
        public int Searchtype { get; set; }
        public string sortby { get; set; }
        public string profile { get; set; }
    }
}