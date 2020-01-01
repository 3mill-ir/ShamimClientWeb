using ClientWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Text;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using ClientWeb.Models.BLL;
using System.Net.Http.Headers;
using ClientWeb.CustomFilters;
using ClientWeb.Models.DataModels;
using GoogleRecaptcha;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace ClientWeb.Controllers
{
    public class AccountController : Controller
    {
        public string APIAddress = ConfigurationManager.AppSettings["APIAddress"];
        [AllowAnonymous]
        [PreRequirementCheckActionFilter]
        public ActionResult Login(string lang, string profile)
        {
            ViewBag.lang = lang;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, string profile, string lang)
        {
            RecaptchaV2Data t = new RecaptchaV2Data();
            t.Secret = "6LdY8AcUAAAAABJ0lfjKHh_a4b7M-zouo2w4qSOl";
            GoogleRecaptcha.RecaptchaV2 GC = new RecaptchaV2(t);
            if (GC.Verify().Success)
            {

                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        string postString = string.Format("grant_type=password&username={0}&password={1}", model.Username, model.Password);
                        HttpRequestMessage req = new HttpRequestMessage();
                        req.Content = new StringContent(postString, Encoding.UTF8, "application/x-www-form-urlencoded");
                        req.RequestUri = new Uri(APIAddress + "/token");
                        req.Method = HttpMethod.Post;
                        HttpResponseMessage response = await client.SendAsync(req);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            dynamic stuff = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                            string access_token = stuff.access_token;
                            //Cookie Add shode
                            HttpCookie myCookie = new HttpCookie("FB_TK");
                            DateTime now = DateTime.Now;
                            myCookie.Value = access_token;
                            myCookie.Expires = now.AddYears(50);
                            Response.Cookies.Set(myCookie);
                            TempData["LoginResult"] = "تبریک ! شما با موفقیت وارد سیستم شدید" + ",#13d213";
                            //return RedirectToAction("Index", "Home", new { lang = Lang, Type = "SpecialIndex" });
                            return RedirectToAction("PageDetail", "Page", new { lang = "FA", profile = profile, id = 0 });
                        }
                    }
                }
            }
            else
            {
                TempData["LoginResult"] = "خطا در تایید هویت" + ",red";
            }
            TempData["LoginResult"] = "نام کاربری یا کلمه عبور اشتباه است" + ",red";
            return RedirectToAction("PageDetail", "Page", new { lang = "FA", profile = profile, id = 0 });
        }

        [AllowAnonymous]
        [PreRequirementCheckActionFilter]
        public ActionResult RegisterUser(string lang, string profile)
        {
            ViewBag.lang = lang;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string profile)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = Tools.RegisterSendRequest(new RegisterfanbazarUserBindingModel() { UserName = model.UserName, CityId = model.CityId, CodeMelli = model.Ssn, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Password = model.Password, ParrentUserName = profile }, APIAddress + "/api/account/RegisterFanbazarUser", HttpMethod.Post);
                ViewBag.Alert = response.StatusCode == HttpStatusCode.OK ? "Success" : "Error";
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    ViewBag.AlertMessage = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }).Split(',');
                }
            }
            else
            {
                ViewBag.Alert = "Success";
                ViewBag.AlertMessage = "داده های ورودی معتبر نمی باشند,warning".Split(',');
            }
            return PartialView();
        }


        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult EditProfile(string Token, string profile, string lang)
        {
            ItemManagement im = new ItemManagement();
            var mn = new MenuManagement();
            if (profile == "sahibmall")
            {
                ViewBag.RegisterForm = Task.Run(() => im.UserDetailItemByType("UserRegister", Token, profile)).Result;
                if (ViewBag.RegisterForm == null)
                {
                    int MenuID = mn.GetFormByType(profile, "UserRegister").ID;
                    var temp = Task.Run(() => im.AddItem(new ItemPostDataModel() { F_MenuID = MenuID, Name = TempData["UserName"].ToString(), Description = "UserRegister" }, null, Token, profile)).Result;
                    ViewBag.RegisterForm = Task.Run(() => im.UserDetailItemByType("UserRegister", Token, profile)).Result;
                }
            }
            var Result = Tools.GetObjectFromRequest(APIAddress + "/api/account/GetUserDetailsForFanbazarUser", Token);
            var Object = JsonConvert.DeserializeObject<UserInformationDataModel>(Result, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            return View(Object != null ? Object : new UserInformationDataModel());
        }

        [HttpPost]
        [AuthLog]
        [ValidateAntiForgeryToken]
        [PreRequirementCheckActionFilter]
        public ActionResult EditProfile(UserInformationDataModel model, string Token, string profile, ItemPostDataModel Rmodel = null, string lang = "FA")
        {
            ViewBag.lang = TempData["lang"];
            if (ModelState.IsValid)
            {
                ItemManagement Item = new ItemManagement();
                if (Rmodel != null && profile == "sahibmall")
                {
                    foreach (var item in Rmodel.Attributes)
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
                    var EResult = Task.Run(() => Item.EditItemOrInitiate(Rmodel, null, Token, profile)).Result;
                }
                HttpStatusCode temp = Tools.SendRequestToUrl(model, APIAddress + "/api/account/PutProfileRegister", HttpMethod.Put, Token);
                ViewBag.Alert = temp == HttpStatusCode.OK ? "حساب کاربری شما با موفقیت ویرایش شد" + ",success" : "متاسفانه در فرآیند ویرایش حساب کاربری شما خطایی به وجود آمده، لطفا مجددا تلاش کنید" + ",danger";
            }
            else
                ViewBag.Alert = "فیلد ها را با ورودی های معتبر پر کنید" + ",warning";
            ItemManagement im = new ItemManagement();
            if (profile == "sahibmall")
                ViewBag.RegisterForm = Task.Run(() => im.GuestDetailItems(profile, "Form", Rmodel.ID)).Result;
            return View(model);
        }


        [AuthLog]
        public ActionResult ResetPassword(string Token)
        {
            return View();
        }

        [HttpPost]
        [AuthLog]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model, string Token)
        {
            if (ModelState.IsValid)
            {
                HttpStatusCode temp = Tools.SendRequestToUrl(model, APIAddress + "/api/Account/EditProfile", HttpMethod.Post, Token);
                ViewBag.Alert = temp == HttpStatusCode.OK ? "کلمه عبور شما با موفقیت ویرایش شد" + "success" : "متاسفانه در فرآیند ویرایش کلمه عبور شما خطایی به وجود آمده، لطفا مجددا تلاش کنید" + "danger";
            }
            return PartialView();
        }

        [AuthLog]
        [PreRequirementCheckActionFilter]
        public ActionResult ChangePassword(string Token, string profile, string lang)
        {
            ViewBag.lang = lang; TempData["lang"] = lang;
            return View();
        }

        [HttpPost]
        [AuthLog]
        [ValidateAntiForgeryToken]
        [PreRequirementCheckActionFilter]
        public ActionResult ChangePassword(ChangePasswordBindingModel model, string Token, string profile)
        {
            ViewBag.lang = TempData["lang"];
            if (ModelState.IsValid)
            {
                HttpStatusCode temp = Tools.SendRequestToUrl(model, APIAddress + "/api/Account/ChangePassword", HttpMethod.Post, Token);
                ViewBag.Alert = temp == HttpStatusCode.OK ? "کلمه عبور شما با موفقیت ویرایش شد" + ",success" : "متاسفانه در فرآیند ویرایش کلمه عبور شما خطایی به وجود آمده، لطفا مجددا تلاش کنید" + ",danger";
            }
            else
                ViewBag.Alert = "فیلد ها را با ورودی های معتبر پر کنید" + ",warning";
            return View();
        }

        [HttpPost]
        [AuthLog]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff(string profile = "admin")
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = new Uri(APIAddress + "api/Account/Logout");
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["FB_TK"].Value);
                req.Method = HttpMethod.Post;
                HttpResponseMessage response = await client.SendAsync(req);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpCookie myCookie = new HttpCookie("FB_TK");
                    myCookie.Expires = DateTime.Now.AddYears(-100);
                    Response.Cookies.Add(myCookie);
                }
                return RedirectToAction("PageDetail", "Page", new { lang = "FA", profile = profile, id = 0 });
            }
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}