using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class ItemController : Controller
    {
        #region User
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> UserListItem(string Token, string Type, string profile, string FromDate, string ToDate, string SearchText = "", string sortby = "", int? PageSize = 10, int? SearchType = 1, int? page = 1, int id = 0, string lang = "FA")
        {
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", profile, "UserListItem");
            ViewBag.F_MenuId = id;
            if (string.IsNullOrEmpty(FromDate)) FromDate = "1370/01/01";
            if (string.IsNullOrEmpty(ToDate)) ToDate = "1450/01/01";
            DateTime From; DateTime To;
            Tools.GetJalaliDateReturnDateTime(FromDate + " 00:00:00", out From);
            Tools.GetJalaliDateReturnDateTime(ToDate + " 00:00:00", out To);
            FilterItemDataModel model = new FilterItemDataModel() { FromTime = From, MenuId = id, PageNumber = page ?? default(int), PageSize = PageSize ?? default(int), search = SearchText, ToTime = To, Searchtype = SearchType ?? default(int), sortby = sortby, type = Type };
            ItemManagement im = new ItemManagement();
            var result = await im.UserListItems(model, Token);
            int total = result.Count;
            var pagedList = new StaticPagedList<ItemDataModel>(result.ToList(), page ?? default(int), PageSize ?? default(int), total);
            ViewBag.Type = Type; ViewBag.lang = lang;
            ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            ViewBag.Total = total; ViewBag.PageSize = PageSize; ViewBag.Type = Type; ViewBag.FromDate = FromDate; ViewBag.ToDate = ToDate;
            ViewBag.page = page; ViewBag.SearchText = SearchText; ViewBag.sortby = sortby; ViewBag.profile = profile; ViewBag.lang = lang; ViewBag.id = id;
            return View(pagedList);
        }
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult PostItem(string Token, string profile, string Type, string lang = "FA")
        {
            MenuManagement Category = new MenuManagement();
            ViewBag.Categories = Category.CategoriesComboBox(Token, Type.Split(',').ToList(), new List<string> { lang }, profile);
            ViewBag.Type = Type; ViewBag.lang = lang;
            ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            return View();
        }
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> PostItem(HttpPostedFileBase Image, ItemPostDataModel model, string Token, string profile, string Type, string lang = "FA")
        {
            model.Attributes.RemoveAll(item => item.Value == null);
            ItemManagement im = new ItemManagement();
            foreach (var item in model.Attributes)
            {
                if (item.File != null)
                {
                    string ImgStatus = Tools.ImageSave(item.File, "FormFilesPath", profile);
                    if (ImgStatus != "NotSaved")
                    {
                        item.File = null;
                        item.Value = ImgStatus;
                    }
                }
            }
            ViewBag.Alert = await im.AddItem(model, Image, Token, profile) == "OK" ? "عملیات ویرایش با موفقیت انجام شد" + ",success" : "خطا در عملیات ویرایش لطفا مجددا تلاش کنید" + ",danger";
            return RedirectToAction("UserListItem", "Item", new { Type = Type, lang = lang });
        }
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> PutItem(int ID, string Token, string Type, string profile, string lang = "FA")
        {
            MenuManagement Category = new MenuManagement();
            ViewBag.Categories = Category.CategoriesComboBox(Token, Type.Split(',').ToList(), new List<string> { lang }, profile);
            ViewBag.Type = Type; ViewBag.lang = lang;
            ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            ItemManagement im = new ItemManagement();
            return View(await im.UserDetailItems(Token, Type, ID));
        }
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> PutItem(HttpPostedFileBase Image, ItemPostDataModel model, string Token, string profile, string Type, string lang = "FA")
        {
            model.Attributes.RemoveAll(item => item.Value == null);
            foreach (var item in model.Attributes)
            {
                if (item.File != null)
                {
                    string ImgStatus = Tools.ImageSave(item.File, "FormFilesPath", profile);
                    if (ImgStatus != "NotSaved")
                    {
                        item.File = null;
                        item.Value = ImgStatus;
                    }
                }
            }
            ItemManagement im = new ItemManagement();
            ViewBag.Alert = await im.EditItem(model, Image, Token, profile) == "OK" ? "عملیات ویرایش با موفقیت انجام شد" + ",success" : "خطا در عملیات ویرایش لطفا مجددا تلاش کنید" + ",danger";
            return RedirectToAction("UserListItem", "Item", new { Type = Type, lang = lang });
        }

        [HttpPost]
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> DeleteMenu(int ID, int Page, string Token, string Type)
        {
            ItemManagement im = new ItemManagement();
            ViewBag.Alert = await im.DeleteItem(ID, Token) == "OK" ? "عملیات ویرایش با موفقیت انجام شد" + ",success" : "خطا در عملیات ویرایش لطفا مجددا تلاش کنید" + ",danger";
            return RedirectToAction("ListItem", Type);
        }

        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> UserItemDetail(string Token, string Type, int ID, string profile, string lang = "FA")
        {
            MenuManagement Category = new MenuManagement();
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", profile, "UserItemDetail");
            ItemManagement im = new ItemManagement(); ViewBag.lang = lang;
            ViewBag.Type = Type; ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            ViewBag.Categories = Category.CategoriesComboBox(Token, Type.Split(',').ToList(), new List<string> { lang }, profile);
            return View(await im.UserDetailItems(Token, Type, ID));
        }
        #endregion




        #region Guest
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> ListItem(string profile, string Type, string lang, int ID = 0,int page=1)
        {
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", profile, "ListItem");
            ItemManagement im = new ItemManagement();
            ViewBag.Category = Type == "FanbazarOffer" ? "عرضه ها" : Type == "FanbazarDemand" ? "تقاضا ها" : "شرکت ها";
            var FilterObject = new FilterItemDataModel() { type = Type, profile = profile, MenuId = ID, PageSize=12,PageNumber=page ,FromTime=null,ToTime=null};
            var result = await im.FilteredListItems(FilterObject);
            int total = result.Total;
            var pagedList = new StaticPagedList<ItemNew>(result.Items, page,12, total);
            ViewBag.Total = total;ViewBag.Type = Type;ViewBag.page = page;ViewBag.profile = profile;ViewBag.F_MenuID = ID; ViewBag.PageSize = 12;
            return View(pagedList);
        }
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> ItemDetail(string profile, string Type, int ID, string lang)
        {
            ViewBag.PrePath = Tools.ReturnPath("ItemImagePath", profile, "ItemDetail");
            ItemManagement im = new ItemManagement();
            ViewBag.Type = Type;
            ViewBag.Category = Type == "FanbazarOffer" ? "عرضه" : Type == "FanbazarDemand" ? "تقاضا" : "شرکت";
            return View(await im.GuestDetailItems(profile, Type, ID));
        }

        [PreRequirementCheckActionFilter]
        public ActionResult FormDetail(string profile, string Type, string lang, string FormName = "", int ID = 389)
        {
            ViewBag.ID = ID; ViewBag.FormName = FormName;
            return View();
        }

        [HttpPost]
        [PreRequirementCheckActionFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FormDetailPost(ItemPostDataModel model, string profile, string lang = "FA")
        {
            model.Attributes.RemoveAll(item => item.Value == null);
            ItemManagement im = new ItemManagement();
            model.Name = profile + " Submited Form";
            ViewBag.Alert = await im.AddItemForUser(model, null, profile) == "OK" ? "success" : "danger";
            return PartialView();
        }

        [PreRequirementCheckActionFilter]
        public ActionResult SearchItem(string profile, string Text, int CatID, string lang = "FA")
        {
            return View();
        }

        #endregion


        #region Helper
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> GetForm(string profile, string Token, string Type, int ID, string lang)
        {
            ItemManagement im = new ItemManagement();
            ViewBag.Type = Type; ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            return PartialView(await im.UserGetForm(profile, Token, Type, ID));
        }

        [AuthLog]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> GetTopDownForm(string profile, string Token, string Type, int ID, string lang)
        {
            ItemManagement im = new ItemManagement();
            ViewBag.Type = Type; ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            return PartialView(await im.UserGetForm(profile, Token, Type, ID));
        }

        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> GetFormForUser(string profile, string Type, int ID, string lang)
        {
            ItemManagement im = new ItemManagement();
            ViewBag.Type = Type; ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            return PartialView(await im.UserGetForm(profile, "", Type, ID));
        }
        //[AuthLog(Roles = "FanbazarUser")]
        [AuthLog]
        [HttpPost]
        public async Task<ActionResult> ImageUploader(string profile, string Token, string Type, int ID, List<HttpPostedFileBase> Images)
        {
            ItemManagement im = new ItemManagement();
            ViewBag.Type = Type; ViewBag.CurrentState = Type == "FanbazarOffer" ? "Offer" : Type == "FanbazarDemand" ? "Demand" : "Company";
            return PartialView(await im.SaveImageListForItem(Token, Type, ID, Images));
        }
        #endregion
    }
}