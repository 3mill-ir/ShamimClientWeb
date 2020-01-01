using ClientWeb.CustomFilters;
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class HomeController : Controller
    {
        public string APIAddress = ConfigurationManager.AppSettings["APIAddress"];
        [PreRequirementCheckActionFilter]
        public ActionResult Index(string profile,string lang)
        {
            return View();
        }
    }
}