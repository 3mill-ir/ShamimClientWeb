using ClientWeb.CustomFilters;
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
    public class ShoppingCartController : Controller
    {
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult ShoppingCartList(string Token, string profile, string lang, int page = 1)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("ItemImagePath", profile, "WebAddress", "ListItem");
            ShopManagement sp = new ShopManagement();
            return View(sp.ShopList(profile,"Initiated", page, 4, Token));
        }
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult OrderedList(string Token, string profile, string lang, int page = 1)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("ItemImagePath", profile, "WebAddress", "ListItem");
            ShopManagement sp = new ShopManagement();
            return View(sp.ShopList(profile, "Ordered", page, 10, Token));
        }

        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult OrdereDetail(string Token, string profile, string lang, int ShopID)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("ItemImagePath", profile, "WebAddress", "ListItem");
            ShopManagement sp = new ShopManagement();
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/account/GetUserDetailsForFanbazarUser", Token);
            TempData["Account"] = JsonConvert.DeserializeObject<UserInformationDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return View(sp.ShopDetail(profile, ShopID, Token));
        }
        [HttpGet]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult AddCartItem(string Token, string profile, string lang, int F_ItemID)
        {
            ShopManagement sp = new ShopManagement();
            var temp = sp.AddShop(new List<ShopItem>() { new ShopItem() { F_ItemID = F_ItemID, Count = 1 } }, profile, Token);
            return RedirectToAction("ShoppingCartList", "ShoppingCart");
        }

        [HttpPost]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult EditCartItem(string Token, string profile, string lang, List<ShopItem> model)
        {
            ShopManagement sp = new ShopManagement();
            var temp = sp.EditShopItem(model, Token);
            return RedirectToAction("ShoppingCartList", "ShoppingCart");
        }

        [HttpPost]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult DeleteCartItem(string Token, string profile, string lang, int ShoppingItemID)
        {
            ShopManagement sp = new ShopManagement();
            var temp = sp.DeleteShopItem(ShoppingItemID, Token);
            return RedirectToAction("ShoppingCartList", "ShoppingCart");
        }

        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult AcceptShopping(string Token, string profile, string lang)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("ItemImagePath", profile, "WebAddress", "ListItem");
            ShopManagement sp = new ShopManagement();
            var Result = Tools.GetObjectFromRequest(ConfigurationManager.AppSettings["APIAddress"] + "/api/account/GetUserDetailsForFanbazarUser", Token);
            TempData["Account"] = JsonConvert.DeserializeObject<UserInformationDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return View(sp.ShopList(profile, "Initiated", 1, 4, Token));
        }

        [HttpPost]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult SetDelivery(string Token, string profile, string lang, Shop model)
        {
            ShopManagement sp = new ShopManagement();
            var temp = sp.EditShop(model, Token);
            return RedirectToAction("PayCost", "ShoppingCart");
        }

        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult PayCost(string Token, string profile, string lang)
        {
            ShopManagement sp = new ShopManagement();
            return View(sp.ShopList(profile, "Initiated", 1, 4, Token));
        }

        [HttpPost]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> FinishShopping(string Token, string profile, string lang, Shop model)
        {
            ShopManagement sp = new ShopManagement();
            model.State = "Ordered";
            model.Payed = model.PayType == "پرداخت حضوری" ? false : true;
            var temp =await sp.EditShop(model, Token);
            var result = sp.ShopList(profile, "Ordered", 1, 4, Token);
            return View(result);
        }
    }
}