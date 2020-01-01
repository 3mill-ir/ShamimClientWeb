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
    public class PostManagement
    {
        public List<PostDataModel> LatestNewsByMenuID(int Menu_ID, int count, string Profile)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetRecentPostsUser?menuid=" + Menu_ID + "&count=" + count + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }

        public async Task<List<PostDataModel>> SearchInPosts(string SearchBox, string Profile)
        {
            var Result = await Tools.GetObjectFromRequestAsync(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetSearched?searchString=" + SearchBox + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }

        public List<PostDataModel> ListPost(int Menu_ID, string Profile, int page = 1, int PageSize = 10)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetMenuPostsUser?id=" + Menu_ID + "&pagesize=" + PageSize + "&pagenumber=" + page + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }
        public async Task<ListPostDataModel> ListPost(FilterPostDatamodel model, string profile)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/postListPostsuser?username=" + profile, HttpMethod.Post);
            var Object = JsonConvert.DeserializeObject<ListPostDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ListPostDataModel();
        }

        public List<PostDataModel> ListTagsPost(int TagsId, string Profile, int page = 1, int PageSize = 10)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/tag/GetTagPostsUser?id=" + TagsId + "&pagesize=" + PageSize + "&pagenumber=" + page + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }


        public PostDataModel PostDetail(string Profile, string lang, int id)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetPostDetailsUser?username=" + Profile + "&lang=" + lang + "&id=" + id);
            var Object = JsonConvert.DeserializeObject<PostDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new PostDataModel();
        }
        public string LikePost(int postId)
        {
            return Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "api/Post/GetLike?id=" + postId);
        }
        public string DisLikePost(int postId)
        {
            return Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "api/Post/GetDislike?id=" + postId);
        }


        public List<PostDataModel> RecentPosts(int Menu_ID, int count, string Profile)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetRecentPostsUser?menuid=" + Menu_ID + "&count=" + count + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }

        public List<PostDataModel> PopularPosts(int Menu_ID, int count, string Profile)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Post/GetPopularPostsUser?menuid=" + Menu_ID + "&count=" + count + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<PostDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<PostDataModel>();
        }
    }
}