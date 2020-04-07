using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class HomeGVController : Controller
    {
        // GET: GV/Home
        public ActionResult TrangChu()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null || session.loai != 2)
            {
                return RedirectToAction("Index", "Login", new { area="" });
            }
            using (ELearningDB db = new ELearningDB())
            {
                GiangVien gv = db.GiangViens.Find(session.ID);
                Session["Hoten"] = gv.HoVaTen;
            }
            
            return View();
        }
    }
}