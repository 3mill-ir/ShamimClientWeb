using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class AttributeItemDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> F_AttributeID { get; set; }
        public ICollection<Attribute> Attribute { get; set; }
    }
}