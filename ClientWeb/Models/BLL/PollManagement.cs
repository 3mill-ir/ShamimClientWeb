using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class PollManagement
    {
        public PollQuestionModel ActivePoll(string Profile) 
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/PollQuestion/GetPollActiveUser?username=" + Profile);
            var Object = JsonConvert.DeserializeObject<PollQuestionModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new PollQuestionModel();
        }
        public int PollParticipation(string Profile, int AnswerId, string IP)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/PollLog/AddPollingUser?username=" + Profile + "&IP=" + IP + "&ansId=" + AnswerId + "&device=Web");
            if (Result == "")
            {
                return 0;
            }
            return 1;
        }
    }
}