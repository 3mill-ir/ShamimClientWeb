using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class UserInformationDataModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CodeMelli { get; set; }
        public int F_CityID { get; set; }
        public int F_StateID { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Tell { get; set; }
        public FanbazarFactDataModel FanbazarFacts { get; set; }
    }

    public class WebsiteProfileSetting
    {
        public string WebsiteTheme { get; set; }
        public string WebsiteThemeLangs { get; set; }
        public string WebSiteURL { get; set; }
    }
}