using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class GalleryController : Controller
    {
        [PreRequirementCheckActionFilter]
        public ActionResult GalleryItems(string profile, int? page, string FolderName = "", string lang="FA")
        {
            FolderManagement FM = new FolderManagement(profile);
            var Folders = FM.UserLoadListFolders(); ViewBag.Folders = Folders;
            string FolderArgument = FolderName != "" ? FolderName : Folders.Count > 0 ? Folders[0] : null;
            ViewBag.PrePath = Tools.ReturnPathPhysicalMode("GalleryPath", profile, "AdminAddress", "GetGallery()");
            ViewBag.FolderName = FolderArgument;
            ViewBag.profile = profile;ViewBag.lang = lang;
            GalleryManagement GM = new GalleryManagement(profile);
            int pageSize = 12;int pageNumber = (page ?? 1);int total;
            var pagedList = new StaticPagedList<GalleryModelAdmin>(GM.GetGalleryByFolderName(FolderArgument, pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
    }
}