using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientWeb.Models.DataModels
{
    public class FolderModel
    {
        public string FolderName { get; set; }
        public int ExistingImagesCount { get; set; }
        public string InsertedFolderName { get; set; } //dar surate virayesh Esme jadide Folder
    }
}