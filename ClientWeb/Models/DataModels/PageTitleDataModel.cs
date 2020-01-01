using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class PageTitleDataModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public PageTitleDataModel Menu2 { get; set; }
    }
}