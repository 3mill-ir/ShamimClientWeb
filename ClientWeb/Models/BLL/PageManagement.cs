using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class PageManagement
    {
        public async System.Threading.Tasks.Task<PageDataModel> DetailPage(int MenuID, string profile,string lang)
        {
            var Result = await Tools.GetObjectFromRequestAsync(ConfigurationManager.AppSettings["APIAddress"] + "/api/Page/getPageUser?menuid=" + MenuID + "&username="+ profile+"&lang="+lang);
            var Object = JsonConvert.DeserializeObject<PageDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new PageDataModel();
        }
    }
}