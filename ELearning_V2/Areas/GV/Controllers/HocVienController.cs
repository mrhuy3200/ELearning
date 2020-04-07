using ELearning_V2.Areas.GV.Models;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class HocVienController : Controller
    {
        // GET: GV/HocVien
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSV(string key)
        {
            if (key == "987654321")
            {
                ELearningDB db = new ELearningDB();
                List<HocVien> lstSV = db.HocViens.ToList();
                List<HocVienModel> HVs = new List<HocVienModel>();
                foreach (var item in lstSV)
                {
                    HocVienModel hv = new HocVienModel();
                    hv.HoVaTen = item.HoVaTen;
                    hv.ID = item.ID;
                    HVs.Add(hv);
                }
                return Json(HVs, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false });
            }

        }
    }
}