using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientWeb.Models.BLL;

namespace ClientWeb.CustomFilters
{
    public class PreRequirementCheckActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string profile = filterContext.ActionParameters["profile"].ToString();




            string baseUrl = HttpContext.Current.Request.Url.Host;

            var UserSettingProfile = Tools.GetWebsiteProfileSetting(profile,baseUrl);
            if (UserSettingProfile == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {
                    controller = "Error",
                    action = "NotFound"
                }));
                return;
            }

        
            filterContext.Controller.TempData["UserName"] = profile;
            string Token = filterContext.HttpContext.Request.Cookies["FB_TK"] != null ? filterContext.HttpContext.Request.Cookies["FB_TK"].Value : "";
            filterContext.Controller.TempData["FanBazar_User"] = Tools.IsAuthenticated(Token);

            string VirtualPath = Tools.ReturnPathVirtualMode("WebsiteThemePath", profile, "WebAddress", "ForTheme");
            filterContext.Controller.TempData["WebsiteThemePath"] = System.Configuration.ConfigurationManager.AppSettings["WebAddress"] + VirtualPath + UserSettingProfile.WebsiteTheme + "/";
            filterContext.Controller.TempData["WebsiteThemeVirtualPath"] = VirtualPath;




            string[] langs=UserSettingProfile.WebsiteThemeLangs.Split(',');
            int Id;
            if (langs.Count() > 1)
            {
                Id = -1;
            }
            else
            {
                Id = 0;
            }
            string[] lang_direction;

            LanguageManagement ln = new LanguageManagement();
            var L = ln.Loadlanguage();
          var langList =L.Where(u=> Array.Exists(langs, s => s.StartsWith(u.Language))).ToList();
            filterContext.Controller.TempData["LangList"] = langList;
            if (filterContext.ActionParameters["lang"]==null)
            {
                lang_direction = langs[0].Split('-');
                filterContext.Controller.TempData["lang"] = lang_direction[0];
                filterContext.Result = new  RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {
                    controller = "Page",
                    action = "PageDetail",
                    profile = profile,
                    lang = lang_direction[0],
                    id = Id,
                }));
         

                filterContext.Controller.TempData["ThemeDirection"] = lang_direction[1];
                return;
            }
            filterContext.Controller.TempData["lang"] = filterContext.ActionParameters["lang"];
            var _find = Array.Find(langs, u => u.Contains(filterContext.ActionParameters["lang"] as string));
            lang_direction = _find.Split('-');
            filterContext.Controller.TempData["LangDirection"] = lang_direction[1];


            // filterContext.Controller.TempData["UserTheme"] = Tools.CacheHtml("WebsiteThemePath", "/shared/_Layout.html", profile);
        }
    }
}