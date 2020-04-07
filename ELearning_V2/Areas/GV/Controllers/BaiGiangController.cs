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
    public class BaiGiangController : BaseController
    {

        private static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public ActionResult QuanLyBaiGiang()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                var lstBG = db.BaiGiangs.Where(x => x.MaGiangVien == session.ID).ToList();
                List<BaiGiangModel> BaiGiangs = new List<BaiGiangModel>();
                foreach (var item in lstBG)
                {
                    BaiGiangModel bg = new BaiGiangModel();
                    bg.MaBaiGiang = item.MaBaiGiang;
                    bg.TenBaiGiang = item.TenBaiGiang;
                    bg.TrangThai = item.TrangThai;
                    BaiGiangs.Add(bg);
                }
                return View(BaiGiangs);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BaiGiangModel bg)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (ModelState.IsValid)
                {
                    var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                    BaiGiang baigiang = new BaiGiang();
                    baigiang.TenBaiGiang = bg.TenBaiGiang;
                    baigiang.NoiDung = bg.NoiDung;
                    baigiang.URL = bg.URL;
                    baigiang.MaGiangVien = session.ID;
                    baigiang.TrangThai = true;
                    if (baigiang.URL != null)
                    {
                        string removestr = "watch?v=";
                        string replacestr = "embed/";
                        string str1 = baigiang.URL;
                        if (str1.Contains("youtube.com/watch?v="))
                        {
                            string str2 = ReplaceFirst(str1, removestr, replacestr);
                            baigiang.URL = str2;
                        }
                    }

                    db.BaiGiangs.Add(baigiang);
                    db.SaveChanges();
                    return RedirectToAction("QuanLyBaiGiang", "BaiGiang");
                }
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                BaiGiang baigiang = db.BaiGiangs.Find(id);
                BaiGiangModel bg = new BaiGiangModel();
                bg.MaBaiGiang = baigiang.MaBaiGiang;
                bg.TenBaiGiang = baigiang.TenBaiGiang;
                bg.NoiDung = baigiang.NoiDung;
                bg.MaGiangVien = baigiang.MaGiangVien;
                bg.TenGiangVien = baigiang.GiangVien.HoVaTen;
                bg.URL = baigiang.URL;
                bg.TrangThai = baigiang.TrangThai;
                ViewBag.NoiDung = baigiang.NoiDung;
                return View(bg);

            }
        }

        public ActionResult Edit(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                BaiGiang baigiang = db.BaiGiangs.Find(id);
                BaiGiangModel bg = new BaiGiangModel();
                bg.MaBaiGiang = baigiang.MaBaiGiang;
                bg.NoiDung = baigiang.NoiDung;
                bg.URL = baigiang.URL;
                bg.TenBaiGiang = baigiang.TenBaiGiang;

                return View(bg);
            }
        }

        [HttpPost]
        public ActionResult Edit(BaiGiangModel baigiang)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (ModelState.IsValid)
                {
                    BaiGiang bg = db.BaiGiangs.Find(baigiang.MaBaiGiang);
                    if (baigiang.URL != null)
                    {
                        string removestr = "watch?v=";
                        string replacestr = "embed/";
                        string str1 = baigiang.URL;
                        if (str1.Contains("youtube.com/watch?v="))
                        {
                            string str2 = ReplaceFirst(str1, removestr, replacestr);
                            bg.URL = str2;
                        }
                    }
                    bg.TenBaiGiang = baigiang.TenBaiGiang;
                    bg.NoiDung = baigiang.NoiDung;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = baigiang.MaBaiGiang });
                }
                return View(baigiang);
            }
        }

        public ActionResult Delete(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                BaiGiang bg = db.BaiGiangs.Find(id);
                if (bg == null)
                {
                    TempData["error"] = "<script>alert('Không tìm thấy bài giảng cần xóa');</script>";
                    return RedirectToAction("QuanLyBaiGiang", "BaiGiang");
                }
                db.BaiGiangs.Remove(bg);
                db.SaveChanges();
                return RedirectToAction("QuanLyBaiGiang", "BaiGiang");
            }
        }

        public ActionResult ChangeStatus(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                BaiGiang bg = db.BaiGiangs.Find(id);
                if (bg == null)
                {
                    TempData["error"] = "<script>alert('Không tìm thấy bài giảng');</script>";
                    return RedirectToAction("QuanLyBaiGiang", "BaiGiang");
                }
                bg.TrangThai = !bg.TrangThai;
                db.SaveChanges();
                return RedirectToAction("QuanLyBaiGiang", "BaiGiang");
            }

        }
        //get bài giảng theo lớp
        public JsonResult GetBaiGiang(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                List<BaiGiang> lstBG = db.BaiGiangs.Where(x => x.Lops.Any(c => c.MaLop == id)).ToList();
                List<BaiGiangModel> BGs = new List<BaiGiangModel>();
                foreach (var item in lstBG)
                {
                    BaiGiangModel bg = new BaiGiangModel();
                    bg.MaBaiGiang = item.MaBaiGiang;
                    bg.TenBaiGiang = item.TenBaiGiang;
                    bg.NoiDung = item.NoiDung;
                    bg.MaGiangVien = item.MaGiangVien;
                    bg.TrangThai = item.TrangThai;
                    bg.URL = item.URL;
                    BGs.Add(bg);
                }
                return Json(BGs, JsonRequestBehavior.AllowGet);
            }
        }

        //Lấy tất cả bài giảng theo mã giảng viên
        public JsonResult GetAllBaiGiang(int id)
        {

            using (ELearningDB db = new ELearningDB())
            {
                var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                var baigiangs = db.BaiGiangs.Where(x => x.Lops.Any(c => c.MaLop == id)).ToList();
                var baigiangID = baigiangs.Select(x => x.MaBaiGiang).ToArray();
                List<BaiGiang> lstBG = db.BaiGiangs.Where(x => x.MaGiangVien == session.ID && !baigiangID.Contains(x.MaBaiGiang)).ToList();
                List<BaiGiangModel> BGs = new List<BaiGiangModel>();
                foreach (var item in lstBG)
                {
                    BaiGiangModel bg = new BaiGiangModel();
                    bg.MaBaiGiang = item.MaBaiGiang;
                    bg.TenBaiGiang = item.TenBaiGiang;
                    bg.NoiDung = item.NoiDung;
                    bg.MaGiangVien = item.MaGiangVien;
                    bg.TrangThai = item.TrangThai;
                    bg.URL = item.URL;
                    BGs.Add(bg);
                }
                return Json(BGs, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult AddBaiGiangToLop(BaiGiangModel data)
        {
            using (ELearningDB db = new ELearningDB())
            {
                BaiGiang bg = db.BaiGiangs.Find(data.MaBaiGiang);
                if (bg == null)
                {
                    return Json(new { success = false });
                }
                Lop l = db.Lops.Find(data.MaLop);
                l.BaiGiangs.Add(bg);
                db.SaveChanges();
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public JsonResult RemoveBaiGiang(BaiGiangModel data)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop l = db.Lops.Find(data.MaLop);
                BaiGiang bg = db.BaiGiangs.Find(data.MaBaiGiang);
                if (l != null && bg != null)
                {
                    bg.Lops.Remove(l);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
        }
    }
}

