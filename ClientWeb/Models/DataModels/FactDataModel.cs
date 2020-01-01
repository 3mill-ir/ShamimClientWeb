using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class FactDataModel
    {
        public int UsersCount { get; set; }
        public int CompaniesCount { get; set; }
        public int ProductsCount { get; set; }
        public int RequestsCount { get; set; }
    }
}