using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Models.BLL
{
    public class MenuManagement
    {
        public List<MenuDataModel> MenuListByType(string Type, string Profile)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Menues/GetMenuTypeUser?type=" + Type + "&username=" + Profile);
            var Object = JsonConvert.DeserializeObject<List<MenuDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<MenuDataModel>();
        }
        public List<MenuDataModel> LoadMenu(string Profile, string type, string Lang)
        {
            var typelist = HttpUtility.ParseQueryString("");
            type.Split(',').ToList().ForEach(s => typelist.Add("type", s));
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/menues/GetMenuAll?username=" + Profile + "&status=true&" + typelist + "&lang=" + Lang);
            var Object = JsonConvert.DeserializeObject<List<MenuDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new List<MenuDataModel>();
        }

        public MenuDataModel GetFormByType(string profile, string Type)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/menues/GetFormByType?Type=" + Type + "&profile=" + profile);
            var Object = JsonConvert.DeserializeObject<MenuDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new MenuDataModel();
        }
        public SelectList getmenus(string Profile, string Lang, string type)
        {
            MenuManagement mn = new MenuManagement();
            var menus = mn.LoadMenu(Profile, type, Lang);
            List<SelectListItem> menuItem = new List<SelectListItem>();
            foreach (var item in menus)
            {
                var NameObj = item;
                string MenuName = "";
                while (NameObj != null && NameObj.F_MenuID != null)
                {
                    MenuName = NameObj.Name + " < " + MenuName;
                    NameObj = menus.FirstOrDefault(i => i.ID == NameObj.F_MenuID);
                }
                MenuName = NameObj != null ? MenuName + NameObj.Name : MenuName;
                menuItem.Add(new SelectListItem() { Text = string.Join(" > ", MenuName.Split('<').Reverse()), Value = item.ID + "" });
            }

            return new SelectList(menuItem, "Value", "Text");
        }



        public ListMenuPostCountDataModel CountMenuPost(int MenuId)
        {
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/Menues/GetMenuPostCount?menuid=" + MenuId);
            var Object = JsonConvert.DeserializeObject<ListMenuPostCountDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return Object != null ? Object : new ListMenuPostCountDataModel();
        }

        public SelectList CategoriesComboBox(string Token, List<string> type, List<string> lang, string username)
        {
            List<SelectListItem> F_MenuIDs = new List<SelectListItem>();
            var typelist = HttpUtility.ParseQueryString("");
            type.ForEach(s => typelist.Add("type", s));
            var langlist = HttpUtility.ParseQueryString("");
            lang.ForEach(s => langlist.Add("lang", s));
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/menues/GetMenuAll?username=" + username + "&status=true&" + typelist + "&" + langlist);
            var Object = JsonConvert.DeserializeObject<List<MenuDataModel>>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) as List<MenuDataModel>;
            foreach (var item in Object)
            {
                var NameObj = item;
                string MenuName = "";
                bool isfirst = true;
                while (NameObj != null)
                {
                    MenuName = isfirst ? NameObj.Name : NameObj.Name + " > " + MenuName;
                    NameObj = Object.FirstOrDefault(i => i.ID == NameObj.F_MenuID);
                    isfirst = false;
                }
                F_MenuIDs.Add(new SelectListItem() { Text = MenuName, Value = item.ID + "" });
            }
            return new SelectList(F_MenuIDs, "Value", "Text");
        }


    }
}