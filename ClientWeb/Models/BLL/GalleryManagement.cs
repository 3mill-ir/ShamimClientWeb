
using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ClientWeb.Models.BLL
{
    public class GalleryManagement
    {
        public GalleryManagement(string profile)
        {
            F_UserName = profile;
            FullPath = Tools.ReturnPathPhysicalMode("GalleryPath", F_UserName, "AdminAddress", "GalleryManagement()");
    
        }
        string FullPath { get; set; }
        string F_UserName { get; set; }



        #region user
        public List<GalleryModelAdmin> GetGalleryByFolderName(string FolderName, int pageNumber, int pageSize, out int total)
        {
            // sub string baraye in ast ke alamate ~ az avvale masir hazf shavad ta ax ha nemayesh dade shavand
            var temp = UserLoadPhotos().Where(u => u.Type == FolderName).ToPagedList(pageNumber, pageSize);
            total = temp.TotalItemCount;
            foreach (var item in temp)
            {
                item.Path = item.Type + "/" + item.Path;
            }
            return temp.ToList();
        }

        public List<GalleryModelAdmin> UserLoadPhotos()
        {
            List<GalleryModelAdmin> OBj = new List<GalleryModelAdmin>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(FullPath + F_UserName + "_ImagesFile.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont = content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<GalleryModelAdmin>));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (List<GalleryModelAdmin>)serializer.Deserialize(xmlReader);
                        return OBj;
                    }
                }
            }
        }
        #endregion
    }
}