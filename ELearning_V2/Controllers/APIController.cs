using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ELearning_V2.Controllers
{
    public class APIController : ApiController
    {
        [HttpPost]
        public string UploadClassImage()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/ClassImage");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    var path = Path.Combine(sPath, hpf.FileName);
                    hpf.SaveAs(path);
                    iUploadedCnt = iUploadedCnt + 1;
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                    //    // SAVE THE FILES IN THE FOLDER.
                    //    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    //    
                    //}
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return "OK";
            }
            else
            {
                return "Fail";
            }
        }

        [HttpPost]
        public string UploadLessionImage()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/LessionImage");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    var path = Path.Combine(sPath, hpf.FileName);
                    hpf.SaveAs(path);
                    iUploadedCnt = iUploadedCnt + 1;
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                    //    // SAVE THE FILES IN THE FOLDER.
                    //    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    //    
                    //}
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return "OK";
            }
            else
            {
                return "Fail";
            }
        }
        [HttpPost]
        public string UploadUserImage()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/UserImage");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    var path = Path.Combine(sPath, hpf.FileName);
                    hpf.SaveAs(path);
                    iUploadedCnt = iUploadedCnt + 1;
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    //if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    //{
                    //    // SAVE THE FILES IN THE FOLDER.
                    //    hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                    //    
                    //}
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return "OK";
            }
            else
            {
                return "Fail";
            }
        }


    }
}
