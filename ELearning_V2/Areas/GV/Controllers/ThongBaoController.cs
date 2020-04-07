using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class ThongBaoController : BaseController
    {
        // GET: GV/ThongBao
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetThongBao_TheoLopID(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstTB = db.ThongBaos.Where(x => x.LopID == id);
                List<ThongBaoModel> ThongBaos = new List<ThongBaoModel>();
                foreach (var item in lstTB)
                {
                    ThongBaoModel thongbao = new ThongBaoModel();
                    thongbao.ID = item.ID;
                    thongbao.NoiDung = item.NoiDung;
                    thongbao.NgayTao = item.NgayTao.Value.ToString("dd/MM/yyyy");
                    ThongBaos.Add(thongbao);
                }
                return Json(ThongBaos, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Insert(ThongBaoModel thongbao)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session==null || session.loai!=2)
            {
                return Json(new { success = false });
            }

            using (ELearningDB db = new ELearningDB())
            {
                TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime utc = DateTime.UtcNow;
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                ThongBao tb = new ThongBao();
                tb.NoiDung = thongbao.NoiDung;
                tb.NgayTao = now;
                tb.CreatedBy = session.ID;
                tb.LopID = thongbao.LopID;
                db.ThongBaos.Add(tb);
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(ThongBaoModel thongbao)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null || session.loai != 2)
            {
                return Json(new { success = false });
            }
            using (ELearningDB db = new ELearningDB())
            {
                ThongBao tb = db.ThongBaos.Find(thongbao.ID);
                if (tb==null)
                {
                    return Json(new { success = false });
                }
                db.ThongBaos.Remove(tb);
                db.SaveChanges();
            }
            return Json(new { success = true });

        }
    }
}