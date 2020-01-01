
using ClientWeb.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ClientWeb.Models.BLL
{
    public class FolderManagement
    {
        public FolderManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("GalleryPath", F_UserName, "AdminAddress", "FolderManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }

        #region user
        public List<string> UserLoadListFolders()
        {
            List<string> OBj = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_FoldersFile.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont = content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (List<string>)serializer.Deserialize(xmlReader);
                        return OBj;
                    }
                }
            }
        }
        #endregion
    }
}