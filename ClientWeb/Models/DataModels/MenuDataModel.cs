using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Models.DataModels
{
    public class MenuDataModel
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }
        [Display(Name = "نام منو")]
        public string Name { get; set; }
        [Display(Name = "توضیحات")]
        [AllowHtml]
        public string Description { get; set; }
        [Display(Name = "وزن")]
        public double Weight { get; set; }
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        [Display(Name = "زبان")]
        public string Language { get; set; }
        [Display(Name = "زیر منوی")]
        public Nullable<int> F_MenuID { get; set; }
        [Display(Name = "شناسه کاربر")]
        public string F_UserID { get; set; }
        [Display(Name = "عکس منو")]
        public string Image { get; set; }
        [Display(Name = "محتوای صفحه")]
        public string Type { get; set; }
        [Display(Name = "کلمات کلیدی")]
        public string MetaKeywords { get; set; }
        [Display(Name = "توضیحات")]
        public string MetaDescription { get; set; }
        [Display(Name = "عنوان صفحه")]
        public string MetaTittle { get; set; }
        [Display(Name = "آدرس سئو")]
        public string MetaSeoName { get; set; }
        [Display(Name = "در فوتر نمایش داده شود")]
        public bool DisplayInFooter { get; set; }
        [Display(Name = "در ساید بار نمایش داده شود")]
        public bool DisplayInSidebar { get; set; }
    }


    public class ListMenuPostCountDataModel
    {
        public ListMenuPostCountDataModel()
        {
            MenuCount = new List<MenuPostCountDataModel>();
        }
        public string Name { get; set; }
        public List<MenuPostCountDataModel> MenuCount { get; set; }
    }
    public class MenuPostCountDataModel
    {
        public string MenuName { get; set; }
        public int PostCount { get; set; }

        public int MenuId { get; set; }

    }
}