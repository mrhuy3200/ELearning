using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class CauHoiController : BaseController
    {
        // GET: GV/CauHoi
        public ActionResult Index()
        {
            using (ELearningDB db = new ELearningDB())
            {
                List<MonHoc> lstMH = new List<MonHoc>();
                lstMH = db.MonHocs.ToList();
                return View(lstMH);
            }

        }

        public ActionResult Details(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                CauHoi ch = db.CauHois.Find(id);
                CauHoiModel cauhoi = new CauHoiModel();
                cauhoi.MaCauHoi = ch.MaCauHoi;
                cauhoi.NoiDung = ch.NoiDung;
                cauhoi.BieuThuc = ch.BieuThuc;
                cauhoi.TenChuong = ch.Chuong.Name;
                cauhoi.DoKho = (int)ch.DoKho;
                cauhoi.GiangVienID = ch.MaGiangVien;
                cauhoi.TenGV = ch.GiangVien.HoVaTen;
                cauhoi.NgayTao = ch.NgayTao.Value.ToString("dd/MM/yyyy");
                cauhoi.CauA = ch.CauA;
                cauhoi.BieuThucA = ch.BieuThucA;
                cauhoi.CauB = ch.CauB;
                cauhoi.BieuThucB = ch.BieuThucB;
                cauhoi.CauC = ch.CauC;
                cauhoi.BieuThucC = ch.BieuThucC;
                cauhoi.CauD = ch.CauD;
                cauhoi.BieuThucD = ch.BieuThucD;
                cauhoi.DapAn = (int)ch.DapAn;
                if (ch.NgaySua == null)
                {
                    cauhoi.NgaySua = "";
                }
                else
                {
                    cauhoi.NgaySua = ch.NgaySua.Value.ToString("dd/MM/yyyy");
                }
                cauhoi.MaMonHoc = (int)ch.MaMonHoc;

                return View(cauhoi);
            }
        }

        public ActionResult CauHoi(int id)
        {
            ELearningDB db = new ELearningDB();
            ViewBag.MaMon = id;
            MonHoc MonHoc = db.MonHocs.Find(id);
            ViewBag.TenMon = MonHoc.TenMonHoc.ToString();
            ViewBag.MaMonHoc = MonHoc.MaMonHoc.ToString();
            return View();
        }

        [HttpGet]
        public ActionResult TaoMoi(int id)
        {
            ELearningDB db = new ELearningDB();
            CauHoiModel cauhoi = new CauHoiModel();
            var LstChuong = db.Chuongs.Where(x => x.MaMonHoc == id);
            List<ChuongModel> Chuongs = new List<ChuongModel>();
            foreach (var item in LstChuong)
            {
                ChuongModel chuong = new ChuongModel();
                chuong.ID = item.ID;
                chuong.Name = item.Name;
                Chuongs.Add(chuong);
            }
            ViewBag.Chuong = Chuongs;
            cauhoi.MaMonHoc = id;
            return View(cauhoi);
        }

        [HttpPost]
        public ActionResult TaoMoi(CauHoiModel cauhoi)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                TempData["Error"] = "Bạn chưa đăng nhập";
                return RedirectToAction("TrangChu", "Home");
            }
            else
            {
                if (session.loai != 2)
                {
                    TempData["Error"] = "Bạn chưa đăng nhập";
                    return RedirectToAction("TrangChu", "Home");
                }
            }
            using (ELearningDB db = new ELearningDB())
            {
                TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime utc = DateTime.UtcNow;
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                if (ModelState.IsValid)
                {
                    CauHoi ch = new CauHoi();
                    ch.NoiDung = cauhoi.NoiDung;
                    ch.BieuThuc = cauhoi.BieuThuc;
                    ch.CauA = cauhoi.CauA;
                    ch.BieuThucA = cauhoi.BieuThucA;
                    ch.CauB = cauhoi.CauB;
                    ch.BieuThucB = cauhoi.BieuThucB;
                    ch.CauC = cauhoi.CauC;
                    ch.BieuThucC = cauhoi.BieuThucC;
                    ch.CauD = cauhoi.CauD;
                    ch.BieuThucD = cauhoi.BieuThucD;
                    ch.DapAn = cauhoi.DapAn;
                    ch.DoKho = cauhoi.DoKho;
                    ch.TrangThai = true;
                    ch.NgayTao = now;
                    ch.MaMonHoc = cauhoi.MaMonHoc;
                    ch.MaGiangVien = session.ID;
                    ch.ChuongID = cauhoi.ChuongID;
                    db.CauHois.Add(ch);
                    db.SaveChanges();
                    return RedirectToAction("CauHoi", "CauHoi", new { id = cauhoi.MaMonHoc });
                }
                var LstChuong = db.Chuongs.Where(x => x.MaMonHoc == cauhoi.MaMonHoc);
                List<ChuongModel> Chuongs = new List<ChuongModel>();
                foreach (var item in LstChuong)
                {
                    ChuongModel chuong = new ChuongModel();
                    chuong.ID = item.ID;
                    chuong.Name = item.Name;
                    Chuongs.Add(chuong);
                }
                ViewBag.Chuong = Chuongs;

                return View(cauhoi);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                CauHoi cauhoi = db.CauHois.Find(id);
                CauHoiModel ch = new CauHoiModel();
                ch.MaCauHoi = cauhoi.MaCauHoi;
                ch.NoiDung = cauhoi.NoiDung;
                ch.BieuThuc = cauhoi.BieuThuc;
                ch.CauA = cauhoi.CauA;
                ch.BieuThucA = cauhoi.BieuThucA;
                ch.CauB = cauhoi.CauB;
                ch.BieuThucB = cauhoi.BieuThucB;
                ch.CauC = cauhoi.CauC;
                ch.BieuThucC = cauhoi.BieuThucC;
                ch.CauD = cauhoi.CauD;
                ch.BieuThucD = cauhoi.BieuThucD;
                ch.DapAn = (int)cauhoi.DapAn;
                ch.DoKho = (int)cauhoi.DoKho;
                ch.MaMonHoc = (int)cauhoi.MaMonHoc;
                var LstChuong = db.Chuongs.Where(x => x.MaMonHoc == ch.MaMonHoc);
                List<ChuongModel> Chuongs = new List<ChuongModel>();
                foreach (var item in LstChuong)
                {
                    ChuongModel chuong = new ChuongModel();
                    chuong.ID = item.ID;
                    chuong.Name = item.Name;
                    Chuongs.Add(chuong);
                }
                ViewBag.Chuong = Chuongs;

                return View(ch);
            }
        }

        [HttpPost]
        public ActionResult Update(CauHoiModel cauhoi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (ModelState.IsValid)
                {
                    TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime utc = DateTime.UtcNow;
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                    CauHoi ch = db.CauHois.Find(cauhoi.MaCauHoi);
                    ch.NoiDung = cauhoi.NoiDung;
                    ch.BieuThuc = cauhoi.BieuThuc;
                    ch.CauA = cauhoi.CauA;
                    ch.BieuThucA = cauhoi.BieuThucA;
                    ch.CauB = cauhoi.CauB;
                    ch.BieuThucB = cauhoi.BieuThucB;
                    ch.CauC = cauhoi.CauC;
                    ch.BieuThucC = cauhoi.BieuThucC;
                    ch.CauD = cauhoi.CauD;
                    ch.BieuThucD = cauhoi.BieuThucD;
                    ch.DapAn = cauhoi.DapAn;
                    ch.DoKho = cauhoi.DoKho;
                    ch.ChuongID = cauhoi.ChuongID;
                    ch.NgaySua = now;
                    db.SaveChanges();
                    return RedirectToAction("CauHoi", "CauHoi", new { id = ch.MaMonHoc });
                }
                return View(cauhoi);
            }
        }

        [HttpGet]
        public JsonResult GetAllQuestion(int id)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                var lstCauHoi = db.CauHois.Where(x => (x.MaGiangVien == session.ID || x.TrangThai == true) && x.MaMonHoc == id);
                List<CauHoiModel> CHs = new List<CauHoiModel>();
                foreach (var item in lstCauHoi)
                {
                    CauHoiModel ch = new CauHoiModel();
                    ch.MaCauHoi = item.MaCauHoi;
                    ch.NoiDung = item.NoiDung;
                    ch.BieuThuc = item.BieuThuc;
                    ch.TenChuong = item.Chuong.Name;
                    ch.DoKho = (int)item.DoKho;
                    ch.TrangThai = (bool)item.TrangThai;
                    ch.ChuongID = (long)item.ChuongID;
                    ch.TenGV = item.GiangVien.HoVaTen;

                    CHs.Add(ch);
                }
                return Json(CHs, JsonRequestBehavior.AllowGet);

            }
        }
        //GET: GV/CauHoi/GetCauHoi
        [HttpPost]
        public JsonResult GetCauHoi(CauHoiDeThiModel chdt) //tham số: mã môn học
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];

            using (ELearningDB db = new ELearningDB())
            {
                List<CauHoiModel> CHs = new List<CauHoiModel>();
                var lstCauHoiDeThi = db.CauHois.Where(x => x.DeThis.Any(s => s.MaDeThi == chdt.DeThiID));

                List<CauHoi> lstCH = db.CauHois.Where(x => x.MaGiangVien == session.ID && x.MaMonHoc == chdt.MaMonHoc).Except(lstCauHoiDeThi).ToList();
                foreach (var item in lstCH)
                {
                    CauHoiModel ch = new CauHoiModel();
                    ch.MaCauHoi = item.MaCauHoi;
                    ch.NoiDung = item.NoiDung;
                    ch.BieuThuc = item.BieuThuc;
                    ch.TenChuong = item.Chuong.Name;
                    ch.DoKho = (int)item.DoKho;
                    ch.TrangThai = (bool)item.TrangThai;
                    ch.DapAn = (int)item.DapAn;
                    ch.CauA = item.CauA;
                    ch.CauB = item.CauB;
                    ch.CauC = item.CauC;
                    ch.CauD = item.CauD;
                    ch.BieuThucA = item.BieuThucA;
                    ch.BieuThucB = item.BieuThucB;
                    ch.BieuThucC = item.BieuThucC;
                    ch.BieuThucD = item.BieuThucD;
                    ch.ChuongID = (long)item.ChuongID;
                    ch.TenGV = item.GiangVien.HoVaTen;

                    CHs.Add(ch);
                }
                return Json(CHs, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCauHoi_DeThi(int id) //param:MaDeThi
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                var lstCauHoi = db.CauHois.Where(x => x.DeThis.Any(s => s.MaDeThi == id));
                List<CauHoiModel> CauHois = new List<CauHoiModel>();
                foreach (var item in lstCauHoi)
                {
                    CauHoiModel ch = new CauHoiModel();
                    ch.MaCauHoi = item.MaCauHoi;
                    ch.NoiDung = item.NoiDung;
                    ch.BieuThuc = item.BieuThuc;
                    CauHois.Add(ch);
                }
                return Json(CauHois, JsonRequestBehavior.AllowGet);
            }
        }

        //Xóa câu hỏi
        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                CauHoi sv = db.CauHois.Find(id);
                if (sv == null)
                {
                    return Json(new { success = false });
                }
                db.CauHois.Remove(sv);
                db.SaveChanges();
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public JsonResult Delete_CauHoi_DeThi(CauHoiDeThiModel c)
        {
            using (ELearningDB db = new ELearningDB())
            {
                try
                {
                    var cauhoi = db.CauHois.Find(c.CauHoiID);
                    var dethi = db.DeThis.Find(c.DeThiID);
                    db.Entry(cauhoi).Collection("DeThi").Load();
                    cauhoi.DeThis.Remove(dethi);
                    db.SaveChanges();
                    return Json(new { success = true });

                }
                catch (Exception)
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpPost]
        public JsonResult Add_CauHoi_DeThi(CauHoiDeThiModel c)
        {
            using (ELearningDB db = new ELearningDB())
            {
                try
                {
                    var cauhoi = db.CauHois.Find(c.CauHoiID);
                    var dethi = db.DeThis.Find(c.DeThiID);
                    if (dethi.CauHois.Count < dethi.SoCauHoi)
                    {
                        if (dethi.CauHois.Contains(cauhoi))
                        {
                            return Json(new { success = false });
                        }
                        dethi.CauHois.Add(cauhoi);
                        db.SaveChanges();
                        return Json(new { success = true });
                    }
                    return Json(new { success = false });


                }
                catch (Exception)
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpPost]
        public JsonResult RandomCauHoiDeThi(CauHoiDeThiModel c)
        {
            using (ELearningDB db = new ELearningDB())
            {
                try
                {
                    Random ran = new Random();
                    var lstCauHoiDeThi = db.CauHois.Where(x => x.DeThis.Any(s => s.MaDeThi == c.DeThiID));
                    List<CauHoi> lstcauhoi = db.CauHois.Where(x => x.ChuongID == c.ChuongID && x.DoKho == c.DoKho).Except(lstCauHoiDeThi).ToList();
                    List<CauHoi> CauHois = new List<CauHoi>();
                    if (lstcauhoi.Count<c.SoCauHoi)
                    {
                        return Json(new { success = -1 });

                    }
                    for (int i = 0; i < c.SoCauHoi; i++)
                    {
                        int index = ran.Next(lstcauhoi.Count());
                        CauHoi cauhoi = lstcauhoi.ElementAt(index);
                        CauHois.Add(cauhoi);
                        lstcauhoi.RemoveAt(index);
                    }

                    var dethi = db.DeThis.Find(c.DeThiID);
                    foreach (var item in CauHois)
                    {
                        dethi.CauHois.Add(item);
                    }
                    db.SaveChanges();
                    return Json(new { success = true });

                }
                catch (Exception)
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpPost]
        public JsonResult Publish(CauHoi updatedCH)
        {
            using (ELearningDB db = new ELearningDB())
            {
                CauHoi cauhoi = db.CauHois.Find(updatedCH.MaCauHoi);
                if (cauhoi == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    cauhoi.TrangThai = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult Private(CauHoi updatedCH)
        {
            using (ELearningDB db = new ELearningDB())
            {
                CauHoi cauhoi = db.CauHois.Find(updatedCH.MaCauHoi);
                if (cauhoi == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    cauhoi.TrangThai = false;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }
    }
}