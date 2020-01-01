using ClientWeb.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ClientWeb.Models.BLL
{
    public class PopUpManagement
    {
        public PopUpManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("PopUpPath", F_UserName, "AdminAddress", "PopUpManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }

        public PopUpModel LoadPopUp()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_PopUp.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont = content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(PopUpModel));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        return (PopUpModel)serializer.Deserialize(xmlReader);
                    }
                }
            }
        }
    }
}