using ClientWeb.Models.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.CustomFilters
{
    public class AuthLogAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public AuthLogAttribute()
        {
            View = "NotAuthorized";
        }

        public string View { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsUserAuthorized(filterContext);
        }
        private void IsUserAuthorized(ActionExecutingContext filterContext)
        {
            bool RoleAuth = false;
            string Token = filterContext.HttpContext.Request.Cookies["FB_TK"] != null ? filterContext.HttpContext.Request.Cookies["FB_TK"].Value : "";
            var  _acc  = Tools.IsAuthenticated(Token);
            if (!string.IsNullOrEmpty(_acc.UserName))
            {
                filterContext.Controller.ViewBag.UserName = _acc.UserName;
                filterContext.ActionParameters["Token"] = Token;
                if (!string.IsNullOrEmpty(Roles))
                {
                    RollManagement roll = new RollManagement();
                    var UserRolls = roll.AllRollsOfUser(Token);
                    if (UserRolls.Count>0)
                    {
                        var RoleArray = Roles.Split(',');
                        if (UserRolls.Any(u => Array.Exists(RoleArray, s => s.Equals(u.Name))))
                        {
                            string profile = filterContext.ActionParameters["profile"].ToString();
                            if (!string.IsNullOrEmpty(_acc.F_ParrentUserName))
                            {
                                if (_acc.F_ParrentUserName.ToLower() == profile.ToLower())
                                {
                                    RoleAuth = true;
                                }
                            }
                 
                        }
                    }
                }
                else
                {
                    RoleAuth = true;
                }
            }
            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(_acc.UserName)  || RoleAuth == false)
            {
                var vr = new ViewResult();
                vr.ViewName = View;
                ViewDataDictionary dict = new ViewDataDictionary();
                dict.Add("Message", "Sorry you are not Authorized to Perform this Action");
                vr.ViewData = dict;
                var result = vr;
                filterContext.Result = result;
            }
        }
    }
}