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
    public class TicketManagement
    {
        public async Task<string> AddTicket(UserTicketModel model)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Ticket/PostTicket", HttpMethod.Post);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public List<TicketInboxModel> TicketTracking(string profile, string TicketID)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Ticket/GetTicketTracking?profile=" + profile + "&TicketId=" + TicketID);
            var Object = JsonConvert.DeserializeObject<List<TicketInboxModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<TicketInboxModel>();
        }



    }
}