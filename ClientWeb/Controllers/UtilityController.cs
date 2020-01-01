using ClientWeb.CustomFilters;
using ClientWeb.Models;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class UtilityController : Controller
    {
        public string APIAddress = ConfigurationManager.AppSettings["APIAddress"];
        #region Layout

        [ChildActionOnly]
        public ActionResult LanguageList()
        {
            LanguageManagement mn = new LanguageManagement();
            return PartialView(mn.Loadlanguage());
        }

        [ChildActionOnly]
        public ActionResult PageTitle(int F_MenuID, string Type = "List")
        {
            ViewBag.Type = Type == "List" ? "" : " جزئیات ";
            return PartialView(Tools.MenuParents(F_MenuID));
        }

        [ChildActionOnly]
        public ActionResult MainMenu(string profile, string lang)
        {
            TempData["UserName"] = profile;
            ViewBag.lang = lang;
            MenuManagement mn = new MenuManagement();
            TempData["Menu"] = mn.LoadMenu(profile, "StaticPost,DynamicPost,NoneStaticDynamic", lang);
            return PartialView(TempData["Menu"]);
        }


        public JsonResult MenuList(string profile, string lang = "FA", string type = "DynamicPost")
        {
            MenuManagement mn = new MenuManagement();
            return Json(mn.getmenus(profile, lang, type), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Footer(string profile)
        {
            ViewBag.Menu = TempData["Menu"];
            ViewBag.profile = profile;
            return PartialView();
        }

        public ActionResult Footer_Links(string profile)
        {
            LinksManagement lm = new LinksManagement(profile);
            return PartialView(lm.UserLoadLinks().Where(u => u.Type == "Footer"));
        }

        public ActionResult Footer_Statistics(string profile)
        {
            ScriptManagement sm = new ScriptManagement(profile);
            return PartialView(sm.UserLoadScripts().FirstOrDefault(u => u.DisplayPlace == "اسکریپت فوتر"));
        }

        public ActionResult Footer_ContactUs(string profile)
        {
            ContactManagement cm = new ContactManagement(profile);
            return PartialView(cm.LoadContact());
        }

        #endregion

        #region Index

        public ActionResult PopUp(string profile)
        {
            PopUpManagement PM = new PopUpManagement(profile);
            return PartialView(PM.LoadPopUp());
        }
        public ActionResult Slider(string profile)
        {
            ViewBag.ImgPath = Tools.ReturnPathPhysicalMode("SliderPath", profile, "AdminAddress", "Slider()");
            ViewBag.Profile = profile;
            SliderManagement SM = new SliderManagement(profile);
            var Slides = SM.LoadSlider().Where(u => u.Display == true).OrderBy(u => u.Priority);
            foreach (var item in Slides)
            {
                if (!string.IsNullOrEmpty(item.Title) && item.Title.Count() > 60)
                {
                    item.Title = item.Title.Substring(0, 60);
                    item.Title = item.Title.Remove(item.Title.LastIndexOf(item.Title.Split(' ').Last())) + " ...";
                }
                if (!string.IsNullOrEmpty(item.Description) && item.Description.Count() > 140)
                {
                    item.Description = item.Description.Substring(0, 140);
                    item.Description = item.Description.Remove(item.Description.LastIndexOf(item.Description.Split(' ').Last())) + " ...";
                }
            }
            return PartialView(Slides);
        }

        public ActionResult RelatedVideos(string profile)
        {
            return PartialView();
        }
        public ActionResult SideLinks(string profile)
        {
            LinksManagement lm = new LinksManagement(profile);
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("LinksPath", profile, "AdminAddress", "SideLinks()");
            return PartialView(lm.UserLoadLinks().Where(u => u.Type == "Main"));
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string profile, string SearchBox = "")
        {
            PostManagement pm = new PostManagement();
            return View(await pm.SearchInPosts(SearchBox, profile));
        }

        [ChildActionOnly]
        public ActionResult RuzPic(string profile)
        {
            ScriptManagement sm = new ScriptManagement(profile);
            return PartialView(sm.UserLoadScripts().FirstOrDefault(u => u.DisplayPlace == "اسکریپت اول صفحات"));
        }

        [ChildActionOnly]
        public ActionResult RuzWord(string profile)
        {
            ScriptManagement sm = new ScriptManagement(profile);
            return PartialView(sm.UserLoadScripts().FirstOrDefault(u => u.DisplayPlace == "اسکریپت دوم صفحات"));
        }

        [ChildActionOnly]
        public ActionResult ImageSlider(string profile)
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult TagsList(string profile, int PostID)
        {
            TagsManagement tm = new TagsManagement();
            return PartialView(tm.ListTags(profile, PostID));
        }


        [ChildActionOnly]
        public ActionResult MenuPostCounter(int F_MenuID)
        {
            ViewBag.currentid = F_MenuID;
            MenuManagement tm = new MenuManagement();
            return PartialView(tm.CountMenuPost(F_MenuID));
        }
        #endregion



        #region FanBazar
        public ActionResult SpecialMenu(string profile, string CurrentState, string lang)
        {
            ViewBag.CurrentState = CurrentState;
            ViewBag.lang = lang;
            return PartialView();
        }
        [AuthLog]
        public ActionResult UserIntroduction(string profile, string Token)
        {
            var Result = Tools.GetObjectFromRequest(APIAddress + "/api/account/GetUserDetailsForFanbazarUser", Token);
            var Object = JsonConvert.DeserializeObject<UserInformationDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return PartialView(Object != null ? Object : new UserInformationDataModel());
        }

        [AuthLog]
        public ActionResult ShoppingCart(string profile, string Token)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("ItemImagePath", profile, "WebAddress", "ListItem");
            ShopManagement sm = new ShopManagement();
            return PartialView(sm.ShopList(profile, "Initiated", 1, 3, Token));
        }

        public ActionResult TopBarContact(string profile)
        {
            ContactManagement cm = new ContactManagement(profile);
            return PartialView(cm.LoadContact());
        }

        public ActionResult Facts(string profile)
        {
            var Result = Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetCountItems?username=" + profile);
            var Object = JsonConvert.DeserializeObject<FanbazarFactDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return PartialView(Object != null ? Object : new FanbazarFactDataModel());
        }
        public JsonResult GetCity(string Id, int SelectedId = 0)
        {
            int StateId = -1;
            int.TryParse(Id, out StateId);
            return Json(Tools.GetCity(SelectedId, StateId));
        }

        public ActionResult Introduction()
        {
            return PartialView();
        }
        public ActionResult LatestCompanies(string profile)
        {
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", "", "LatestItems");
            var Object = JsonConvert.DeserializeObject<List<ItemDataModel>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralLatestItems?" + "type=FanbazarCompany&count=4" + "&username=" + profile), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return PartialView(Object != null ? Object : new List<ItemDataModel>());
        }
        public ActionResult LatestItems(string profile)
        {
            var OfferObject = JsonConvert.DeserializeObject<List<ItemDataModel>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralLatestItems?" + "type=FanbazarOffer&count=6" + "&username=" + profile), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var DemandObject = JsonConvert.DeserializeObject<List<ItemDataModel>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralLatestItems?" + "type=FanbazarDemand&count=6" + "&username=" + profile), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var PopularObject = JsonConvert.DeserializeObject<List<ItemDataModel>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralPopularItems?" + "type=FanbazarOffer&count=6" + "&username=" + profile), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            ViewBag.LatestOffers = OfferObject != null ? OfferObject : new List<ItemDataModel>();
            ViewBag.LatestDemands = DemandObject != null ? DemandObject : new List<ItemDataModel>();
            ViewBag.PopularItems = PopularObject != null ? PopularObject : new List<ItemDataModel>();
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", "", "LatestItems");
            return PartialView();
        }

        [PreRequirementCheckActionFilter]
        public ActionResult ItemBox(string Color, string CategoryName, int count, int ID, string profile, string lang = "FA", int Adad = 0)
        {
            var LatestItems = JsonConvert.DeserializeObject<List<ItemNew>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralLatestItems?" + "type=FanbazarOffer,FanbazarOfferDemand &count=" + count + "&username=" + profile + "&F_MenuID=" + ID), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            var PopularItems = JsonConvert.DeserializeObject<List<ItemNew>>(Tools.GetObjectFromRequest(APIAddress + "/api/Item/GetGeneralPopularItems?" + "type=FanbazarDemand,FanbazarOfferDemand&count=" + count + "&username=" + profile + "&F_MenuID=" + ID), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            ViewBag.LatestItems = LatestItems != null ? LatestItems : new List<ItemNew>();
            ViewBag.MostVisitedItems = PopularItems != null ? PopularItems : new List<ItemNew>();
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", "", "LatestItems");
            ViewBag.Color = Color; ViewBag.CategoryName = CategoryName; ViewBag.Adad = Adad;
            return PartialView();
        }
        #endregion
    }
}