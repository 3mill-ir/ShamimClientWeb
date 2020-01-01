using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class RollManagement
    {
        public List<AspNetRoles> AllRollsOfUser(string Token)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Roll/GetAllRollsForUser", Token);
            var Object = JsonConvert.DeserializeObject<List<AspNetRoles>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<AspNetRoles>();
        }

    }
}