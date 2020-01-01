using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class AttributeDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> F_AttributeGroupID { get; set; }
        public string ComponentType { get; set; }
        public Nullable<int> F_AttributeItemID { get; set; }
        public string Icon { get; set; }
        public string TextColor { get; set; }
        public ICollection<AttributeItemDataModel> AttributeItem1 { get; set; }
    }
}