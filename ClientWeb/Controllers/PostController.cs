using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using PagedList;
using System.Threading.Tasks;

namespace ClientWeb.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> ListPost(string profile, string lang, int id, string FromDate, string ToDate, string SearchText = "", string sortby = "DateNew", int? PageSize = 10, int? Type = 1, int? page = 1)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            ViewBag.F_MenuId = id;
            if (string.IsNullOrEmpty(FromDate))
                FromDate = "1370/01/01";
            if (string.IsNullOrEmpty(ToDate))
                ToDate = "1450/01/01";
            DateTime From; DateTime To;
            Tools.GetJalaliDateReturnDateTime(FromDate + " 00:00:00", out From);
            Tools.GetJalaliDateReturnDateTime(ToDate + " 00:00:00", out To);
            FilterPostDatamodel model = new FilterPostDatamodel() { FromTime = From, Language = lang, MenuId = id, PageNumber = page ?? default(int), PageSize = PageSize ?? default(int), search = SearchText, ToTime = To, type = Type ?? default(int), sortby = sortby };
            PostManagement post = new PostManagement();

            var result = await post.ListPost(model, profile);
            int total = result.count;
            var pagedList = new StaticPagedList<PostDataModel>(result.List, page ?? default(int), PageSize ?? default(int), total);
            ViewBag.Total = total;
            ViewBag.PageSize = PageSize;
            ViewBag.Type = Type;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.page = page;
            ViewBag.SearchText = SearchText;
            ViewBag.sortby = sortby;
            ViewBag.profile = profile;
            ViewBag.lang = lang;
            ViewBag.id = id;

            return View(pagedList);
        }


        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> Search(string profile, string lang, int? id, string FromDate, string ToDate, string SearchText = "", string sortby = "", int? PageSize = 10, int? Type = 1, int? page = 1)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            ViewBag.F_MenuId = id;
            if (string.IsNullOrEmpty(FromDate))
                FromDate = "1370/01/01";
            if (string.IsNullOrEmpty(ToDate))
                ToDate = "1450/01/01";
            DateTime From; DateTime To;
            Tools.GetJalaliDateReturnDateTime(FromDate + " 00:00:00", out From);
            Tools.GetJalaliDateReturnDateTime(ToDate + " 00:00:00", out To);
            FilterPostDatamodel model = new FilterPostDatamodel() { FromTime = From, Language = lang, MenuId = id ?? default(int), PageNumber = page ?? default(int), PageSize = PageSize ?? default(int), search = SearchText, ToTime = To, type = Type ?? default(int), sortby = sortby };
            PostManagement post = new PostManagement();

            var result = await post.ListPost(model, profile);
            int total = result.count;
            var pagedList = new StaticPagedList<PostDataModel>(result.List, page ?? default(int), PageSize ?? default(int), total);
            ViewBag.Total = total;
            ViewBag.PageSize = PageSize;
            ViewBag.Type = Type;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.page = page;
            ViewBag.SearchText = SearchText;
            ViewBag.sortby = sortby;
            ViewBag.profile = profile;
            ViewBag.lang = lang;
            ViewBag.id = id;
            LanguageManagement ln = new LanguageManagement();
            var langs = ln.Loadlanguage();
            List<SelectListItem> LangItems = new List<SelectListItem>();
            foreach (var item in langs)
                LangItems.Add(new SelectListItem() { Text = item.LanguageName + "-" + item.Language, Value = item.Language });
            ViewBag.langlist = new SelectList(LangItems, "Value", "Text", lang);
            MenuManagement mn = new MenuManagement();
            var menus = mn.LoadMenu(profile, "DynamicPost",lang);
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

            ViewBag.menulist = new SelectList(menuItem, "Value", "Text");

            return View(pagedList);
        }

        [PreRequirementCheckActionFilter]
        public ActionResult TagsPost(string profile,string lang, int TagId, string TagsText, int page = 1, int Pagesize = 10)
        {
            LanguageManagement ln = new LanguageManagement();
            var langs = ln.Loadlanguage();
            List<SelectListItem> LangItems = new List<SelectListItem>();
            foreach (var item in langs)
                LangItems.Add(new SelectListItem() { Text = item.LanguageName + "-" + item.Language, Value = item.Language });
            ViewBag.langlist = new SelectList(LangItems, "Value", "Text");
            MenuManagement mn = new MenuManagement();
            var menus = mn.LoadMenu(profile, "DynamicPost","FA");
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

            ViewBag.menulist = new SelectList(menuItem, "Value", "Text");
            ViewBag.Tag = TagsText;
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            ViewBag.TagId = TagId;
            PostManagement post = new PostManagement();
            var result = post.ListTagsPost(TagId, profile, page, Pagesize);
            var pagedList = new StaticPagedList<PostDataModel>(result.ToList(), page, Pagesize, result.Count);
            return View(pagedList);
        }
        [PreRequirementCheckActionFilter]
        public ActionResult PostDetail(string profile, string lang, int id)
        {
            ViewBag.profile = profile;
            ViewBag.lang = lang;
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement post = new PostManagement();
            return View(post.PostDetail(profile, lang, id));
        }

        [HttpPost]
        public ActionResult LikePost(int PostId, int NumberOfLikes)
        {
            ViewBag.Count = NumberOfLikes;
            PostManagement post = new PostManagement();
            if (HttpContext.Request.Cookies["PSH2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["PSH2016O"].Value;
                var LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PL" + PostId) == false)
                {
                    Response.Cookies["PSH2016O"].Value = LikeIdString + "-PL" + PostId;
                    ViewBag.Count = post.LikePost(PostId);
                }
                else
                    ViewBag.Count = NumberOfLikes;
            }
            else
            {
                HttpCookie cookie = new HttpCookie("PSH2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PL" + PostId;
                Response.Cookies.Add(cookie);
                ViewBag.Count = post.LikePost(PostId);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult DisLikePost(int PostId, int NumberOfDisLikes)
        {
            ViewBag.Count = NumberOfDisLikes;
            PostManagement post = new PostManagement();
            if (HttpContext.Request.Cookies["PSH2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["PSH2016O"].Value;
                var LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PD" + PostId) == false)
                {
                    Response.Cookies["PSH2016O"].Value = LikeIdString + "-PD" + PostId;
                    ViewBag.Count = post.DisLikePost(PostId);
                }
                else
                    ViewBag.Count = NumberOfDisLikes;
            }
            else
            {
                HttpCookie cookie = new HttpCookie("PSH2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PD" + PostId;
                Response.Cookies.Add(cookie);
                ViewBag.Count = post.DisLikePost(PostId);
            }
            return PartialView();
        }



        #region partialViews
        public ActionResult NewsSlider(string profile)
        {
            MenuManagement m = new MenuManagement();
            ViewBag.Categories = m.MenuListByType("dynamic", profile);
            return PartialView();
        }
        public ActionResult NewsSliderGet(string profile, int F_MenuID = -1)
        {
            PostManagement pm = new PostManagement();
            return PartialView(pm.LatestNewsByMenuID(F_MenuID, 4, profile));
        }

        [ChildActionOnly]
        public ActionResult RecentPosts(string profile, int F_MenuID)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement pm = new PostManagement();
            return PartialView(pm.RecentPosts(F_MenuID, 4, profile));
        }


        [ChildActionOnly]
        public ActionResult RecentPostsHome(string profile, int F_MenuID,int count=4)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement pm = new PostManagement();
            return PartialView(pm.RecentPosts(F_MenuID, count, profile));
        }
        
        [ChildActionOnly]
        public ActionResult RecentPostsHomesecond(string profile, int F_MenuID, int count = 4)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement pm = new PostManagement();
            return PartialView(pm.RecentPosts(F_MenuID, count, profile));
        }
        [ChildActionOnly]
        public ActionResult RecentPostsHomeThird(string profile, int F_MenuID, int count = 4)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement pm = new PostManagement();
            return PartialView(pm.RecentPosts(F_MenuID, count, profile));
        }


        [ChildActionOnly]
        public ActionResult BreakingNews(string profile, int F_MenuID)
        {
            PostManagement pm = new PostManagement();
             return PartialView(Tools.ViewPath(profile, "BreakingNews","Post"),pm.RecentPosts(F_MenuID, 4, profile));
     
        }
        [ChildActionOnly]
        public ActionResult PopularPosts(string profile, int F_MenuID)
        {
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("DynamicPageImages", profile, "AdminAddress", "PostListView");
            PostManagement pm = new PostManagement();
            return PartialView(pm.PopularPosts(F_MenuID, 4, profile));
        }
        #endregion



    }
}