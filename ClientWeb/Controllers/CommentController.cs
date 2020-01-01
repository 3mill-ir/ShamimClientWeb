using ClientWeb.Models.BLL;
using ClientWeb.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientWeb.Controllers
{
    public class CommentController : Controller
    {
        [HttpPost]
        public ActionResult AddPostComment(int postid,string Commenttext ,int CommentID=-1)
        {
            CommentManagement post = new CommentManagement();
            {
                if (!String.IsNullOrEmpty(Commenttext))
                {
                    CommentDataModel model = new CommentDataModel();
                    model.Text = Commenttext;
                    model.F_PostsID = postid;
                    if (CommentID != -1)
                        model.F_CommentsID = CommentID;
                    string scale = post.AddPostComment(model);
                    if (scale == "OK")
                        ViewBag.AddCommentSuccess = "نظر شما با موفقیت ثبت شد";
                    else
                        ViewBag.AddCommentFailed = "متاسفانه ثبت نظر شما با خطا رو به رو شد";
                }
                else
                    ViewBag.AddCommentWarning = "لطفا ابتدا دیدگاه خود را وارد نموده سپس آن را ثبت نمایید";
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult LikePostComment(int CommentId)
        {
            CommentManagement post = new CommentManagement();
            if (HttpContext.Request.Cookies["PSH2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["PSH2016O"].Value;
                var LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PCL" + CommentId) == false)
                {
                    Response.Cookies["PSH2016O"].Value = LikeIdString + "-PCL" + CommentId;
                    ViewBag.CMLCount = post.LikeComment(CommentId, true);
                }
                else
                    ViewBag.CMLCount = post.LikeComment(CommentId, false);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("PSH2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCL" + CommentId;
                Response.Cookies.Add(cookie);
                ViewBag.CMLCount = post.LikeComment(CommentId, true);
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult DisLikePostComment(int CommentId)
        {
            CommentManagement post = new CommentManagement();
            if (HttpContext.Request.Cookies["PSH2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["PSH2016O"].Value;
                var LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PCD" + CommentId) == false)
                {
                    Response.Cookies["PSH2016O"].Value = LikeIdString + "-PCD" + CommentId;
                    ViewBag.CMLCount = post.DisLikeComment(CommentId, true);
                }
                else
                    ViewBag.CMLCount = post.DisLikeComment(CommentId, false);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("PSH2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCD" + CommentId;
                Response.Cookies.Add(cookie);
                ViewBag.CMLCount = post.DisLikeComment(CommentId, true);
            }
            return PartialView();
        }
    }
}