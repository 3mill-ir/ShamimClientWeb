using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class LinkModel
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}