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
    public class SliderManagement
    {
        public SliderManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("SliderPath", F_UserName, "AdminAddress", "SliderManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }

        public List<SliderModel> LoadSlider()
        {

            List<SliderModel> OBj = new List<SliderModel>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_Slider.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont = content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SliderModel>));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (List<SliderModel>)serializer.Deserialize(xmlReader);
                        return OBj;
                    }
                }
            }
        }

        public void EditSlider(SliderModel model, HttpPostedFileBase Img)
        {
            List<SliderModel> list = new List<SliderModel>();

            list.AddRange(LoadSlider());
            var FoundedObejct = list.FirstOrDefault(u => u.ID == model.ID);
            if (Img != null)
            {
                if (!string.IsNullOrEmpty(FoundedObejct.Img))
                {
                    string SpPath = Path + "/" + FoundedObejct.Img;
                    if (System.IO.File.Exists(SpPath))
                        System.IO.File.Delete(SpPath);
                }
                FoundedObejct.Img = Tools.ImageSave(Img, Path);
            }
            FoundedObejct.Title = model.Title;
            FoundedObejct.Priority = model.Priority;
            FoundedObejct.Link = model.Link;
            FoundedObejct.Description = model.Description;
            SaveChangesSlider(list);
        }

        public bool ChangeDisplaySlider(int ID)
        {
            List<SliderModel> list = new List<SliderModel>();
            list.AddRange(LoadSlider());
            var FoundedObejct = list.FirstOrDefault(u => u.ID == ID);
            if (FoundedObejct != null)
            {
                FoundedObejct.Display = !FoundedObejct.Display;
                SaveChangesSlider(list);
                return true;
            }
            else
                return false;
        }

        private void SaveChangesSlider(List<SliderModel> model)
        {

            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Slider.xml"))
            {
                serializer.Serialize(writer, model);
            }
        }

        private void InitiateSlider()
        {
            string webURL = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"];
            List<SliderModel> list = new List<SliderModel>();
            for (int i = 0; i < 4; i++)
            {
                SliderModel sm = new SliderModel();
                sm.ID = i;
                sm.Priority = i;
                sm.Display = false;
                sm.Title = "تیتر آزمایشی" + (i + 1);
                sm.Description = "محتوای خلاصه آزمایشی" + (i + 1);
                sm.Link = webURL;
                sm.Img = "";
                list.Add(sm);
            }
            SaveChangesSlider(list);
        }
    }
}