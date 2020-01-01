using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClientWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
 "PageRout",
 "{profile}/{lang}/Page/PageDetail/{id}",
 new { controller = "Page", action = "PageDetail" }, namespaces: new[] { "ClientWeb.Controllers" }
 );
            routes.MapRoute(
"PostRout",
"{profile}/{lang}/Post/{action}/{id}",
new { controller = "Post", action = "ListPost", id = UrlParameter.Optional }, namespaces: new[] { "ClientWeb.Controllers" }
);
            routes.MapRoute(
"ItemRout",
"{profile}/{lang}/{controller}/{action}/{id}",
new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "ClientWeb.Controllers" }
);

            //            routes.MapRoute(
            //"SearchRout",
            //"{profile}/{lang}/Post/Search",
            //new { controller = "Post", action = "Search" }, namespaces: new[] { "ClientWeb.Controllers" }
            //);
            //            routes.MapRoute(
            //"PostDetailRout",
            //"{profile}/Post/PostDetail/{lang}/{id}",
            //new { controller = "Post", action = "PostDetail" }, namespaces: new[] { "ClientWeb.Controllers" }
            //);
            routes.MapRoute(
"UtilityRoutRout",
"Utility/{action}",
new { controller = "Utility", action = "Index" }, namespaces: new[] { "ClientWeb.Controllers" }
);
            routes.MapRoute(
"profileonly1", // Route name
"{profile}", // URL with parameters
new { controller = "Home", action = "Index", profile = ConfigurationManager.AppSettings["MainProfile"] }, namespaces: new[] { "ClientWeb.Controllers" } // Parameter defaults
);

            routes.MapRoute(
"profileonly", // Route name
"{profile}/{lang}", // URL with parameters
new { controller = "Home", action = "Index", profile = ConfigurationManager.AppSettings["MainProfile"] }, namespaces: new[] { "ClientWeb.Controllers" } // Parameter defaults
);
            routes.MapRoute(
                name: "Default",
                url: "{profile}/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", profile = ConfigurationManager.AppSettings["MainProfile"] },
                namespaces: new[] { "ClientWeb.Controllers" }
            );



        }
    }
}
