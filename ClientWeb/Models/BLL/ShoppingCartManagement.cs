using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ClientWeb.Models.BLL
{
    public class ShopManagement
    {
        public ShopPagedList ShopList(string profile,string ShopStatus, int pageNumber, int pageSize, string Token)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/GetListShop?ShopStatus=" + ShopStatus + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&profile=" + profile, Token);
            var Object = JsonConvert.DeserializeObject<ShopPagedList>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ShopPagedList();
        }
        public Shop ShopDetail(string profile, int ShopID, string Token)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/GetShopDetail?profile=" + profile + "&ShopID=" + ShopID, Token);
            var Object = JsonConvert.DeserializeObject<Shop>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new Shop();
        }
        public async Task<string> AddShop(List<ShopItem> model,string profile,string Token)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/PostShop?profile=" + profile, HttpMethod.Post,Token);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public async Task<string> EditShopItem(List<ShopItem> model, string Token)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/PutEditShopItem", HttpMethod.Put, Token);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public async Task<string> EditShop(Shop model, string Token)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/PutEditShop", HttpMethod.Put, Token);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public async Task<string> ChangeStateShop(Shop model, string Token)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(model, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/PostChangeShopState", HttpMethod.Post, Token);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";
        }

        public async Task<string> DeleteShopItem(int F_ShopID, string Token)
        {
            string Result = await Tools.SendRequestToUrlGetObjectAsync(null, ConfigurationManager.AppSettings["APIAddress"] + "/api/Shop/PostDeleteShop?F_ShopID=" + F_ShopID, HttpMethod.Post, Token);
            var Object = JsonConvert.DeserializeObject<string>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : "";

        }

    }
}