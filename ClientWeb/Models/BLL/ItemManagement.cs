using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Models.BLL
{
    public class ItemManagement
    {
        public string APIAddress = ConfigurationManager.AppSettings["APIAddress"];

        #region UserItems
        public async System.Threading.Tasks.Task<List<ItemDataModel>> UserListItems(FilterItemDataModel model, string Token)
        {
            var Result = await Tools.SendRequestToUrlGetObjectAsync(model, APIAddress + "/api/Item/PostListItemsForFanbazarUser", HttpMethod.Post, Token);
            var Object = JsonConvert.DeserializeObject<List<ItemDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<ItemDataModel>();
        }

        public async Task<ItemNew> UserDetailItems(string Token, string Type, int ItemID)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemDetailsForFanbazarUser?id=" + ItemID, Token);
            var Object = JsonConvert.DeserializeObject<ItemNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ItemNew();
        }

        public async Task<ItemNew> UserDetailItemByType(string Type,string Token,string profile)
        {
            var Result =await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemDetailsByType?Type=" + Type+ "&username=" + profile,Token);
            var Object = JsonConvert.DeserializeObject<ItemNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object;
        }
        public async Task<ItemNew> UserDetailItemByType(string Type, string UserID)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemDetailsByTypeForUser?Type=" + Type + "&UserID=" + UserID);
            var Object = JsonConvert.DeserializeObject<ItemNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object;
        }

        public async Task<MenuNew> UserGetForm(string profile, string Token, string Type, int MenuID)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetAttributesForItem?username=" + profile + "&menuid=" + MenuID);
            var Object = JsonConvert.DeserializeObject<MenuNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new MenuNew();
        }

        public async System.Threading.Tasks.Task<string> AddItem(ItemPostDataModel model, HttpPostedFileBase MyFile, string Token, string profile)
        {
            string ImgStatus = Tools.ImageSave(MyFile, "ItemImagePath");
            if (ImgStatus != "NotSaved")
                model.Image = ImgStatus;
            var result = await Tools.SendRequestToUrlAsync(model, APIAddress + "/api/Item/PostItem?username=" + profile, HttpMethod.Post, Token);
            if (result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";
        }

        public async System.Threading.Tasks.Task<string> AddItemForUser(ItemPostDataModel model, HttpPostedFileBase MyFile, string profile)
        {
            string ImgStatus = Tools.ImageSave(MyFile, "ItemImagePath");
            if (ImgStatus != "NotSaved")
                model.Image = ImgStatus;
            var result = await Tools.SendRequestToUrlAsync(model, APIAddress + "/api/Item/PostItemForUser?username=" + profile, HttpMethod.Post);
            if (result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";
        }

        public async System.Threading.Tasks.Task<string> EditItem(ItemPostDataModel model, HttpPostedFileBase MyFile, string Token, string profile)
        {
            string ImgStatus = Tools.ImageSave(MyFile, "ItemImagePath");
            if (ImgStatus != "NotSaved")
                model.Image = ImgStatus;
            var result = await Tools.SendRequestToUrlAsync(model, APIAddress + "/api/Item/PutItem?username=" + profile, HttpMethod.Put, Token);
            if (result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";

        }
        public async System.Threading.Tasks.Task<string> EditItemOrInitiate(ItemPostDataModel model, HttpPostedFileBase MyFile, string Token, string profile)
        {
            string ImgStatus = Tools.ImageSave(MyFile, "ItemImagePath");
            if (ImgStatus != "NotSaved")
                model.Image = ImgStatus;
            var result = await Tools.SendRequestToUrlAsync(model, APIAddress + "/api/Item/PutItemOrInitiate?username=" + profile, HttpMethod.Put, Token);
            if (result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";

        }

        public async System.Threading.Tasks.Task<string> DeleteItem(int ID, string Token)
        {
            var result = await Tools.SendRequestToUrlAsync(null, APIAddress + "/api/Item/DeleteItem" + ID, HttpMethod.Delete, Token);
            if (result == System.Net.HttpStatusCode.OK)
                return "OK";
            return "NOK";
        }

        #endregion

        #region GuestItems
        public async System.Threading.Tasks.Task<List<ItemDataModel>> GuestListItems(string profile, string Type)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemsForUser?type=" + Type + "&username=" + profile);
            var Object = JsonConvert.DeserializeObject<List<ItemDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<ItemDataModel>();
        }

        public async System.Threading.Tasks.Task<ItemNewPagedList> FilteredListItems(FilterItemDataModel model)
        {
            var Result = await Tools.SendRequestToUrlGetObjectAsync(model,APIAddress + "/api/Item/PostListItemsForGuest",HttpMethod.Post);
            var Object = JsonConvert.DeserializeObject<ItemNewPagedList>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ItemNewPagedList();
        }

        public async System.Threading.Tasks.Task<ItemNew> GuestDetailItems(string profile, string Type, int ItemID)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemDetailsForUser?id=" + ItemID + "&username=" + profile);
            var Object = JsonConvert.DeserializeObject<ItemNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ItemNew();
        }
        public async System.Threading.Tasks.Task<MenuNew> GuestDetailItemsByMenu(string profile, string Type, int ItemID,int F_MenuID)
        {
            var Result = await Tools.GetObjectFromRequestAsync(APIAddress + "/api/Item/GetItemDetailsByMenu?id=" + ItemID + "&username=" + profile + "&F_MenuID=" + F_MenuID);
            var Object = JsonConvert.DeserializeObject<MenuNew>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new MenuNew();
        }

        public async System.Threading.Tasks.Task<string> SaveImageListForItem(string Token, string type, int ID, List<HttpPostedFileBase> images)
        {
            var model = new List<string>();
            foreach (var item in images)
            {
                string ImgStatus = Tools.ImageSave(item, "ItemImagePath");
                if (ImgStatus != "NotSaved")
                    model.Add(ImgStatus);
            }
            var result = await Tools.SendRequestToUrlAsync(model, APIAddress + "/api/Item/PostItem?username=", HttpMethod.Post, Token);
            return result == System.Net.HttpStatusCode.OK ? "OK" : "NOK";
        }

        #endregion

    }
}