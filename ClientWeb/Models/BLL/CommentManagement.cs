using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientWeb.Models.DataModels;
using System.Net.Http;
using System.Configuration;

namespace ClientWeb.Models.BLL
{
    public class CommentManagement
    {
        public string LikeComment(int commentId, bool flag)
        {
            return flag ? Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Comment/Getlike?id=" + commentId) : Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Comment/Getlikes?id=" + commentId);
        }

        public string DisLikeComment(int commentId, bool flag)
        {
            return flag? Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Comment/GetDislike?id=" + commentId): Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Comment/GetDislikes?id=" + commentId);
        }

        public string AddPostComment(CommentDataModel model)
        {
            var Result = Tools.SendRequestToUrl(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Comment/PostComment", HttpMethod.Post);
            if (Result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";
        }
    }
}