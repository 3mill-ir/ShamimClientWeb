using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class RegisterfanbazarUserBindingModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CodeMelli { get; set; }
        public int CityId { get; set; }
        public string ParrentUserName { get; set; }
    }
}