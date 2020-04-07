using ELearning_V2.Areas.Admin.Models;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.Admin.Controllers
{
    public class MonHocController : BaseController
    {
        public ActionResult DanhSachMonHoc()
        {
            ELearningDB db = new ELearningDB();
            List<MonHoc> MonHocs = db.MonHocs.ToList();
            return View(MonHocs);
        }

        [HttpPost]
        public JsonResult Create(MonHocModel mh)
        {
            try
            {
                ELearningDB db = new ELearningDB();
                MonHoc mon = new MonHoc();
                mon.TenMonHoc = mh.TenMonHoc;
                db.MonHocs.Add(mon);
                db.SaveChanges();
                return Json(new { success = true });

            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        public ActionResult Chuong(int id)
        {
            ELearningDB db = new ELearningDB();
            var m = db.Chuongs.Where(x => x.MaMonHoc == id);
            ViewBag.MaMonHoc = id;
            return View(m);
        }

        [HttpPost]
        public JsonResult CreateChuong(ChuongModel c)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (c != null)
                {
                    Chuong chuong = new Chuong();
                    chuong.Name = c.Name;
                    chuong.MaMonHoc = c.MaMonHoc;
                    db.Chuongs.Add(chuong);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
        }

        public ActionResult Delete(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Chuong c = db.Chuongs.Find(id);
                int mamon = (int)c.MaMonHoc;
                if (c != null)
                {
                    db.Chuongs.Remove(c);
                    db.SaveChanges();
                    return RedirectToAction("Chuong", "MonHoc", new { id = mamon });
                }
                ViewBag.error = "Xóa không thành công";
                return RedirectToAction("Chuong", "Chuong", new { id = mamon });
            }
        }

        public JsonResult GetChuong(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstChuong = db.Chuongs.Where(x => x.MaMonHoc == id);
                List<ChuongModel> Chuongs = new List<ChuongModel>();
                foreach (var item in lstChuong)
                {
                    ChuongModel chuong = new ChuongModel();
                    chuong.ID = item.ID;
                    chuong.Name = item.Name;
                    Chuongs.Add(chuong);
                }
                return Json(Chuongs, JsonRequestBehavior.AllowGet);
            }
        }

    }
}