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
    public class TicketController : Controller
    {
        [PreRequirementCheckActionFilter]
        public ActionResult AddTicket(string profile, string lang)
        {
            return View();
        }

        [HttpPost]
        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> AddTicketPost(string profile, string lang, UserTicketModel model,HttpPostedFileBase MediaFile)
        {
            model.profile = profile;
            TicketManagement Tickets = new TicketManagement();
            if (String.IsNullOrEmpty(model.Content))
            {
                ViewBag.TicketWarning = "لطفا برای ارسال درخواست فیلد متن درخواست را کامل کنید";
                return PartialView(model);
            }
            //if (MediaFile != null)
            //{
            //    string ImgStatus = Tools.ImageSave(MediaFile, "TicketMediaUploadPath", profile);
            //    if (ImgStatus != "NotSaved")
            //    {
            //        model.MediaBox.Add(new Media() { FileName = ImgStatus,FilePath= Tools.ReturnPathPhysicalMode("TicketMediaUploadPath", profile, "WebAddress", "AddTicketPost()"),ContentLength=MediaFile.ContentLength,ContentType=MediaFile.ContentType });
            //    }
            //}
            string Scale = await Tickets.AddTicket(model);
            ViewBag.TicketSuccess = " درخواست با موفقیت ثبت شد. کد پیگری شما : " + Scale;
            return PartialView(model);
        }

        [PreRequirementCheckActionFilter]
        public ActionResult TicketTracking(string profile, string lang)
        {
            return View();
        }

        [HttpPost]
        [PreRequirementCheckActionFilter]
        public ActionResult TicketTrackingPost(string profile, string lang, string TicketId)
        {
            TicketManagement tm = new TicketManagement();
            List<TicketInboxModel> model = tm.TicketTracking(profile,TicketId).ToList();
            if (model.Count < 1)
            {
                ViewBag.TicketError = "کد پیگیری وارد شده صحیح نمی باشد";
                return PartialView("AddTicketPost");
            }
            return PartialView(model);
        }

        public FileResult TicketMediaDownload(string path, string Key)
        {
            if (Key == "MobileAppTicket")
            {
                string filepath = Server.MapPath(@"~/App_Data/TicketMediaUpload/" + path);
                if (System.IO.File.Exists(filepath))
                {
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = path,
                        Inline = true,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());
                    return File(filedata, System.Net.Mime.MediaTypeNames.Application.Octet);
                }
            }
            return null;
        }
    }
}