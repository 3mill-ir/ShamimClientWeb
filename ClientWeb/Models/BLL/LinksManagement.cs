using ClientWeb.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ClientWeb.Models.BLL
{
    public class LinksManagement
    {
        public LinksManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("LinksPath", F_UserName, "AdminAddress", "LinksManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }

        private void SaveChangesLinks(List<LinkModel> model)
        {


            System.IO.Directory.CreateDirectory(Path);
            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Links.xml"))
            {
                serializer.Serialize(writer, model);
            }

        }

        private void InitiateLinks()
        {
            List<LinkModel> list = new List<LinkModel>();
            for (int i = 0; i < 4; i++)
            {
                LinkModel LM = new LinkModel();
                LM.ID = i;
                LM.Link = "http://www.leader.ir/fa";
                LM.Type = "Main";
                LM.Image = "Rahbari.jpg";
                list.Add(LM);
            }
            for (int i = 4; i < 9; i++)
            {
                LinkModel LM = new LinkModel();
                LM.ID = i;
                LM.Link = "http://www.leader.ir/fa";
                LM.Type = "Footer";
                LM.Text = "پایگاه اطلاع رسانی مقام معظم رهبری";
                list.Add(LM);
            }
            SaveChangesLinks(list);
        }

        #region user
        public List<LinkModel> UserLoadLinks()
        {
            List<LinkModel> OBj = new List<LinkModel>();
            //WebClient WebClient = new WebClient();
            //string YourContent = WebClient.DownloadString(Path + F_UserName + "_Links.xml");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_Links.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont= content.ReadAsStringAsync().Result;

                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<LinkModel>));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (List<LinkModel>)serializer.Deserialize(xmlReader);
                        return OBj;
                    }
                }
            }
        }

        private void UserSaveChangesLinks(List<LinkModel> model)
        {



            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Links.xml"))
            {
                serializer.Serialize(writer, model);
            }
        }

        private void UserInitiateLinks(string profile)
        {
            List<LinkModel> list = new List<LinkModel>();
            for (int i = 0; i < 4; i++)
            {
                LinkModel LM = new LinkModel();
                LM.ID = i;
                LM.Link = "http://www.leader.ir/fa";
                LM.Type = "Main";
                LM.Image = "Rahbari.jpg";
                list.Add(LM);
            }
            for (int i = 4; i < 9; i++)
            {
                LinkModel LM = new LinkModel();
                LM.ID = i;
                LM.Link = "http://www.leader.ir/fa";
                LM.Type = "Footer";
                LM.Text = "پایگاه اطلاع رسانی مقام معظم رهبری";
                list.Add(LM);
            }
            UserSaveChangesLinks(list);
        }
        #endregion
    }
}