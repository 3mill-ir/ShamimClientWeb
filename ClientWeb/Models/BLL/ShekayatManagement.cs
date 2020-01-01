using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class ShekayatManagement
    {
        public async Task<string> AddShekayat(ShekayatModel model)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shekayat/PostShekayat", HttpMethod.Post);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public List<ShekayatInboxOutBox> ShekayatTracking(string profile, string ShekayatID)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Shekayat/GetShekayatTracking?profile=" + profile + "&ShekayatId=" + ShekayatID);
            var Object = JsonConvert.DeserializeObject<List<ShekayatInboxOutBox>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<ShekayatInboxOutBox>();
        }
    }
}