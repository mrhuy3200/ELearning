using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult TrangChu()
        {
            var user = (TaiKhoan)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (TempData["ActiveResult"] != null)
            {
                ViewBag.ActiveResult = TempData["ActiveResult"].ToString();
            }
            if (TempData["RegisterResult"] != null)
            {
                ViewBag.ActiveResult = TempData["RegisterResult"].ToString();
            }
            if (TempData["Error"] != null)
            {
                ViewBag.ActiveResult = TempData["Error"].ToString();
            }
            var User = (TaiKhoan)Session["User"];
            if (User.Role == 4)
            {
                ELearningDB db = new ELearningDB();
                var Lops = db.Courses.Where(x => x.Price == 0).ToList();
                return View("GuessHomePage", Lops);
            }
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
    }
}