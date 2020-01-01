using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class LanguageManagement
    {
        public List<LanguageDataModel> Loadlanguage()
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Utility/GetLanguages");
            var Object = JsonConvert.DeserializeObject<List<LanguageDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<LanguageDataModel>();
        }
    }
}