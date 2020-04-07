using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class CommentController : BaseController
    {
        // GET: GV/Comment
        private string getNamebyID(long id)
        {
            ELearningDB db = new ELearningDB();
            TaiKhoan tk = db.TaiKhoans.Find(id);
            if (tk.Role==2)
            {
                string name = tk.GiangVien.HoVaTen;
                return name;
            }
            else
            {
                if (tk.Role==3)
                {
                    string name = tk.HocVien.HoVaTen;
                    return name;
                }
            }
            return "";
        }


        public JsonResult getAllComment(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                List<Comment> lstCom = db.Comments.Where(x => x.ClassID == id).ToList();
                List<CommentModel> Coms = new List<CommentModel>();
                foreach (var item in lstCom)
                {
                    CommentModel com = new CommentModel();
                    com.ID = item.ID;
                    com.NoiDung = item.NoiDung;
                    com.CreateDate = item.CreateDate.Value.ToString("dd/MM/yyyy hh:mm tt");
                    com.CreateBy = item.CreateBy;
                    com.ClassID = item.ClassID;
                    com.HoTen = getNamebyID(item.TaiKhoan.ID);
                    List<Reply> lstRep = db.Replies.Where(x => x.CommentID == item.ID).ToList();
                    List<CommentModel> reps = new List<CommentModel>();
                    foreach (var rep in lstRep)
                    {
                        CommentModel r = new CommentModel();
                        r.ID = rep.ID;
                        r.NoiDung = rep.NoiDung;
                        r.CreateDate = rep.CreateDate.Value.ToString("dd/MM/yyyy hh:mm tt");
                        r.CreateBy = rep.CreateBy;
                        r.HoTen = getNamebyID(rep.TaiKhoan.ID);
                        reps.Add(r);
                    }
                    com.Reps = reps;
                    Coms.Add(com);
                }
                return Json(Coms, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult removeComment(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Comment cmt = db.Comments.Find(id);
                if (cmt!=null)
                {
                    db.Comments.Remove(cmt);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });

                }
            }
        }

        [HttpPost]
        public JsonResult removeReply(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Reply rep = db.Replies.Find(id);
                if (rep != null)
                {
                    db.Replies.Remove(rep);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });

                }
            }

        }

        [HttpPost]
        public JsonResult InsertCmt(CommentModel comment)
        {
            if (comment!=null)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime utc = DateTime.UtcNow;
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                    var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                    Comment com = new Comment();
                    com.NoiDung = comment.NoiDung;
                    com.CreateDate = now;
                    com.CreateBy = session.ID;
                    com.ClassID = comment.ClassID;
                    db.Comments.Add(com);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new { success = false });

            }
        }

        [HttpPost]
        public JsonResult InsertRep(CommentModel reply)
        {
            if (reply != null)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime utc = DateTime.UtcNow;
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                    var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                    Reply rep = new Reply();
                    rep.NoiDung = reply.NoiDung;
                    rep.CreateDate = now;
                    rep.CreateBy = session.ID;
                    rep.CommentID = (long)reply.ParentCommentID;
                    db.Replies.Add(rep);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new { success = false });

            }
        }


        //public JsonResult getCommentByID(long id)
        //{
        //    using (ELearningDB db = new ELearningDB())
        //    {
        //        Comment c = db.Comment.Find(id);
        //        CommentModel com = new CommentModel();
        //        com.ID = c.ID;
        //        com.ParentCommentID = c.ParentCommentID;
        //        com.NoiDung = c.NoiDung;
        //        com.CreateDate = c.CreateDate.Value;
        //        com.CreateBy = c.CreateBy;
        //        com.ClassID = c.ClassID;
        //        com.HoTen = getNamebyID(c.TaiKhoan.ID);
        //        List<Comment> lstcon = c.Con.ToList();
        //        List<long> cons = new List<long>();
        //        foreach (var con in lstcon)
        //        {
        //            long ID = con.ID;
        //            cons.Add(ID);
        //        }
        //        com.conID = cons;
        //        return Json(com, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}