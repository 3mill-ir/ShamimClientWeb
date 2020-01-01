using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using ClientWeb.Models.DataModels;
using ClientWeb.Models.BLL;
using System.Net.Http;

namespace ClientWeb.Models
{
    public class ContactManagement
    {
        public ContactManagement(string profile)
        {
            F_UserName = profile;
            Path = Tools.ReturnPathPhysicalMode("ContactPath", F_UserName, "AdminAddress", "ContactManagement()");
        }
        string Path { get; set; }
        string F_UserName { get; set; }
        public ContactManagementModel LoadContact()
        {
            ContactManagementModel OBj = new ContactManagementModel();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(Path + F_UserName + "_Contact.xml").Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string Cont = content.ReadAsStringAsync().Result;
                        System.IO.StringReader strReader = new System.IO.StringReader(Cont);
                        XmlSerializer serializer = new XmlSerializer(typeof(ContactManagementModel));
                        XmlTextReader xmlReader = new XmlTextReader(strReader);
                        OBj = (ContactManagementModel)serializer.Deserialize(xmlReader);
                        OBj.AddressInput = String.Join("", OBj.Address.ToList());
                        OBj.FaxInput = String.Join("", OBj.Fax.ToList());
                        OBj.PhoneInput = String.Join("", OBj.Phone.ToList());
                        OBj.EmailInput = String.Join("", OBj.Email.ToList());
                        return OBj;
                    }
                }
            }
        }

        public bool InitialContact(string F_username)
        {
            ContactManagementModel model = new ContactManagementModel();
            model.F_UserName = F_username;
            var serializer = new XmlSerializer(model.GetType());
            using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Contact.xml"))
            {

                serializer.Serialize(writer, model);
                return true;
            }


        }

        public bool UpdateContact(ContactManagementModel model)
        {
            ContactManagementModel OBj = new ContactManagementModel();
            if (System.IO.File.Exists(Path + "/" + F_UserName + "_Contact.xml"))
            {
                var serializer = new XmlSerializer(typeof(ContactManagementModel));
                using (var reader = XmlReader.Create(Path + "/" + F_UserName + "_Contact.xml"))
                {
                    OBj = (ContactManagementModel)serializer.Deserialize(reader);
                    OBj.AddressInput = String.Join("", OBj.Address.ToList());
                    OBj.FaxInput = String.Join("", OBj.Fax.ToList());
                    OBj.PhoneInput = String.Join("", OBj.Phone.ToList());
                    OBj.EmailInput = String.Join("", OBj.Email.ToList());

                }
            }
            model.F_UserName = OBj.F_UserName;
            model.F_UserId = OBj.F_UserId;
            model.AccountBanner = model.AccountBanner != null ? model.AccountBanner : OBj.AccountBanner;
            model.AndroidHomeImage = model.AndroidHomeImage != null ? model.AndroidHomeImage : OBj.AndroidHomeImage;
            model.AndroidThumbImage = model.AndroidThumbImage != null ? model.AndroidThumbImage : OBj.AndroidThumbImage;
            if (model.UserType == null)
                model.UserType = OBj.UserType;
            try
            {
                var serializer = new XmlSerializer(model.GetType());
                using (var writer = XmlWriter.Create(Path + "/" + F_UserName + "_Contact.xml"))
                {
                    if (model.EmailInput != null)
                        model.Email.AddRange(model.EmailInput.Split('\n'));
                    if (model.PhoneInput != null)
                        model.Phone.AddRange(model.PhoneInput.Split('\n'));
                    if (model.AddressInput != null)
                        model.Address.AddRange(model.AddressInput.Split('\n'));
                    if (model.FaxInput != null)
                        model.Fax.AddRange(model.FaxInput.Split('\n'));
                    model.FaxInput = null;
                    model.EmailInput = null;
                    model.AddressInput = null;
                    model.FaxInput = null;
                    model.PhoneInput = null;
                    serializer.Serialize(writer, model);
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

    }
}