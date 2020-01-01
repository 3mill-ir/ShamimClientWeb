using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Models.DataModels
{
    public class PageDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string ActionContent { get; set; }
        public string Language { get; set; }

    }
}