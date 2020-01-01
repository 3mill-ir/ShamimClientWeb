using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ContactManagementModel
    {
        public ContactManagementModel()
        {
            Email = new List<string>();
            Address = new List<string>();
            Phone = new List<string>();
            Fax = new List<string>();
        }
        public string F_UserName { get; set; }
        public int? F_UserId { get; set; }
        public string Telegram { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public List<string> Email { get; set; }
        public List<string> Address { get; set; }
        public List<string> Phone { get; set; }
        public List<string> Fax { get; set; }
        public string EmailInput { get; set; }
        public string AddressInput { get; set; }
        public string PhoneInput { get; set; }
        public string FaxInput { get; set; }
        public string AccountBanner { get; set; }
        public string AndroidThumbImage { get; set; }
        public string AndroidHomeImage { get; set; }
        public string UserType { get; set; }
    }
}