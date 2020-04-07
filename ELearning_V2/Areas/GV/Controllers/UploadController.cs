using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class UploadController : Controller
    {
        // GET: GV/Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ContentResult Upload()
        {
            string path = Server.MapPath("~/Assets/Image/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string key in Request.Files)
            {
                HttpPostedFileBase postedFile = Request.Files[key];
                postedFile.SaveAs(path + postedFile.FileName);
            }

            return Content("Success");
        }
    }
}