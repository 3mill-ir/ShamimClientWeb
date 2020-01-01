using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class ItemGalleryDataModel
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public Nullable<int> F_ItemID { get; set; }
        public string FileDescription { get; set; }
    }
}