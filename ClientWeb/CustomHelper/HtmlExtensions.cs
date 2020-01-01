using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ClientWeb.CustomHelper
{
    public static class HtmlExtensions 
    {


        public static string ReturnPathPhysicalMode(this HtmlHelper helper,string ConfigPath, string F_UserName, string DomainAddress, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = ConfigurationManager.AppSettings[DomainAddress] + string.Format(section[ConfigPath], F_UserName);
            return Path;
        }



        //public static MvcHtmlString returnAdminPath(this HtmlHelper helper, string partialUrl, object model = null)
        //{
        //    string cachedPath;

        //    // return cached copy if exists


        //    // download remote data
        //    var webClient = new WebClient();
        //    var partialUri = new Uri(partialUrl);
        //    string partialData = "";// webClient.DownloadString(partialUrl);
        //    using (HttpClient client = new HttpClient())
        //    {
        //        using (HttpResponseMessage response = client.GetAsync(partialUri).Result)
        //        {
        //            using (HttpContent content = response.Content)
        //            {
        //                partialData = content.ReadAsStringAsync().Result;
        //            }
        //        }
        //    }


        //    // save cached copy locally
        //    var partialLocalName = Path.ChangeExtension(partialUri.LocalPath.Replace('/', '_'), "cshtml");
        //    var partialMappedPath = helper.ViewContext.RequestContext.HttpContext.Server.MapPath("ll" + partialLocalName);
        //    File.WriteAllText(partialMappedPath, partialData, System.Text.Encoding.UTF8);

        //    // add to cache
        ////    _remotePartialsMappingCache.Add(partialUrl, partialLocalName);

        //    return helper.Partial("ll" + partialLocalName, model);
        //}
    }
}