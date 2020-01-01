using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class PageController : Controller
    {

        [PreRequirementCheckActionFilter]
        public async Task<ActionResult> PageDetail(string profile,string lang,int id)
        {
            if (id == -1)
            {
                return View("Wellcome");
            }
            PageManagement pm = new PageManagement();
       
            var page = await pm.DetailPage(id, profile, lang);

            if (page != null)
            {
                if (!string.IsNullOrEmpty(page.ActionContent))
                {
                    if (page.ActionContent.EndsWith(".html"))
                    {
                        page.ActionContent = Tools.GetHtmldetail("DetailHTMLFilePath", page.ActionContent, profile);
                        if (page.ActionContent.StartsWith("<p><a href="))
                        {
                            var temp = page.ActionContent.Split('"');
                            if (temp.Count() > 1 & Uri.IsWellFormedUriString(temp[1], UriKind.Absolute))
                            {
                                return Redirect(temp[1]);
                            }
                        }

                        ViewBag.F_MenuId = id;
                        return View("xPageDetail", page);
                    }
                    return View(page.ActionContent);
            
                }
            }
            return View("NotFound");
        }


    }
}