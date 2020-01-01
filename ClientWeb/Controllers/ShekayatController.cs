using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class ShekayatController : Controller
    {
        [PreRequirementCheckActionFilter]
        public ActionResult AddShekayat(string profile, string lang)
        {
            return View();
        }

        [HttpPost]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> AddShekayatPost(string profile, string lang, ShekayatModel model)
        {
            model.profile = profile;
            ShekayatManagement Shekayat = new ShekayatManagement();
            if (String.IsNullOrEmpty(model.Text))
            {
                ViewBag.ShekayatWarning = "لطفا برای ثبت شکایت فیلد متن درخواست را کامل کنید";
                return PartialView(model);
            }
            string Scale = await Shekayat.AddShekayat(model);
            ViewBag.ShekayatSuccess = !string.IsNullOrEmpty(Scale) ? " شکایت با موفقیت ثبت شد. کد پیگری شما : " + Scale : "شکایت با کد پیگیری قبلی " + model.TrackingCode + "با موفقیت ثبت شد";
            return PartialView(model);
        }

        [PreRequirementCheckActionFilter]
        public ActionResult ShekayatTracking(string profile, string lang)
        {
            return View();
        }

        [HttpPost]
        [PreRequirementCheckActionFilter]
        public ActionResult ShekayatTrackingPost(string profile, string lang, string ShekayatId)
        {
            ShekayatManagement tm = new ShekayatManagement();
            List<ShekayatInboxOutBox> model = tm.ShekayatTracking(profile, ShekayatId).ToList();
            if (model.Count < 1)
            {
                ViewBag.ShekayatError = "کد پیگیری وارد شده صحیح نمی باشد";
                return PartialView("AddShekayatPost");
            }
            return PartialView(model);
        }
    }
}