using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class TagsManagement
    {
        public List<TagDataModel> ListTags(string Username, int PostID)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetPostTagsUser?id=" + PostID + "&username=" + Username);
            var Object = JsonConvert.DeserializeObject<List<TagDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<TagDataModel>();
        }
        public List<PostDataModel> ListTagPosts(string Username, int TagID, int page = 1, int PageSize = 10)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Tag/GetTagPostsUser?id=" + TagID + "&username=" + Username);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }
    }
}