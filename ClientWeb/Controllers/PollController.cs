using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class PollController : Controller
    {
        public ActionResult PollQuestion(string profile)
        {
            PollManagement pl = new PollManagement();
            return PartialView(pl.ActivePoll(profile));
        }
        [HttpPost]
        public ActionResult PollQuestionPost(string profile, int PollAnswerId)
        {
            string ip = Request.UserHostAddress;
            PollManagement pl = new PollManagement();
            int scale;
            if (HttpContext.Request.Cookies["PSH2016l"] != null)
            {
                string IdString = HttpContext.Request.Cookies["PSH2016l"].Value;
                var LikeIds = IdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "" + PollAnswerId) == false)
                {
                    scale = pl.PollParticipation(profile, PollAnswerId, ip);
                    if (scale == 1)
                    {
                        Response.Cookies["PSH2016l"].Value = IdString + "-" + PollAnswerId;
                    }
                }
                else
                {
                    ViewBag.PollWarning = "کاربر گرامی شما قبلا در این نظر سنجی شرکت کرده اید";
                    return PartialView();
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("PSH2016l");
                cookie.Expires = DateTime.Now.AddDays(365);
                scale = pl.PollParticipation(profile, PollAnswerId, ip);
                if (scale == 1)
                {
                    cookie.Value = "" + PollAnswerId;
                }
                Response.Cookies.Add(cookie);
            }
            if (scale == 1)
            {
                ViewBag.PollSuccess = "نظر شما با موفقیت در سیستم ثبت شد";
                return PartialView();
            }
            else
            {
                ViewBag.PollError = "متاسفانه نظر شما در سیستم ثبت نشد. لطفا دوباره تلاش کنید";
                return PartialView();
            }
        }
    }
}