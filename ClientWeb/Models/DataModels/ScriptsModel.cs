using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Models.DataModels
{
    public class ScriptsModel
    {
        public string ScriptName { get; set; }
        public string DisplayPlace { get; set; }
        [AllowHtml]
        public string Script { get; set; }
    }
}