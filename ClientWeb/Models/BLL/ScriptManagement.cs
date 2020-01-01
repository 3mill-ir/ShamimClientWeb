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
    public class ScriptManagement
    {
        public ScriptManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("ScriptPath", F_UserName, "AdminAddress", "ScriptManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }

        #region user
        public List<ScriptsModel> UserLoadScripts()
        {
            List<ScriptsModel> OBj = new List<ScriptsModel>(); 
            //WebClient WebClient = new WebClient();
            //string YourContent = WebClient.DownloadString(Path + F_UserName + "_Scripts.xml");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_Scripts.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont= content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ScriptsModel>));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (List<ScriptsModel>)serializer.Deserialize(xmlReader);
                        return OBj;
                    }
                }
            }

       

            //List<ScriptsModel> OBj = new List<ScriptsModel>();
            //if (!System.IO.File.Exists(Path + "/" + F_UserName + "_Scripts.xml"))
            //{
            //    UserInitiateScripts();
            //}
            //var serializer = new XmlSerializer(typeof(List<ScriptsModel>));
            //using (var reader = XmlReader.Create(Path + "/" + F_UserName + "_Scripts.xml"))
            //{
            //    OBj = (List<ScriptsModel>)serializer.Deserialize(reader);
            //    return OBj;
            //}
        }

        private void UserSaveChangesScripts(List<ScriptsModel> model)
        {
            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Scripts.xml"))
            {
                serializer.Serialize(writer, model);
            }
        }

        //private void UserInitiateScripts()
        //{
        //    List<ScriptsModel> list = new List<ScriptsModel>();
        //    ScriptsModel LM = new ScriptsModel();
        //    LM.Name = "اسکریپت فوتر";
        //    LM.Script = "<script src=\"http://prayer.horuph.com/frame/?color0=FFFFFF&color1=FFFFFF&bgcolor=274472&inbgcolor=1F4C8C&az=1&tz=0&bdcolor=274472&border=1&curved=7&city=2-1\" type=\"text/javascript\" language=\"javascript\"></script>";
        //    list.Add(LM);
        //    ScriptsModel LM2 = new ScriptsModel();
        //    LM2.Name = "اسکریپت اول صفحات";
        //    LM2.Script = "<script type=\"text/javascript\" src=\"http://pichAk.Net/blogcod/random-photos/religious/random.js\"></script>";
        //    list.Add(LM2);
        //    ScriptsModel LM3 = new ScriptsModel();
        //    LM3.Name = "اسکریپت دوم صفحات";
        //    LM3.Script = "<iframe class=\"disabledRuzSpeech\" src=\"http://hadis.toolsir.com/hadis.php?color=000000&amp;bg=FFFFFF&amp;bc=FFFFFF&amp;m=1,2,3,4,5,6,7,8,9,10,11,12,13,14,&amp;time=none\" scrolling=\"no\" frameborder=\"0\" hspace=\"0\" align=\"center\" width=\"154\" height=\"160\" style=\"border:1px solid #FFFFFF;-webkit-border-radius: 4px;-moz-border-radius: 4px;border-radius: 4px;\"></iframe>";
        //    list.Add(LM3);
        //    UserSaveChangesScripts(list);
        //}
        #endregion
    }
}