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
            if (TempData["ActiveResult"]!=null)
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

            return View();
        }

        public ActionResult Home()
        {
            return View();
        }
    }
}