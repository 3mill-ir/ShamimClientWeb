using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class AttributeGroupDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<double> Weight { get; set; }
        public string Image { get; set; }
        public Nullable<int> F_AttributeGroupID { get; set; }
        public Nullable<int> Depth { get; set; }
        public Nullable<int> F_MenuID { get; set; }
    }
    public class ListAttributeGroupDataModel
    {
        public List<AttributeGroupDataModel> AttributeList { get; set; }
        public ListAttributeGroupDataModel()
        {
            AttributeList = new List<AttributeGroupDataModel>();
        }
    }
}