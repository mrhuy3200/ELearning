using ELearning_V2.Areas.GV.Models;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class BaiGiangController : Controller
    {
        // GET: BaiGiang
        public JsonResult GetBaiGiang_TheoMaLop(int id)
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
                    BGs.Add(bg);
                }
                return Json(BGs, JsonRequestBehavior.AllowGet);

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

    }
}