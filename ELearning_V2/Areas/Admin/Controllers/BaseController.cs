using ELearning_V2.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ELearning_V2.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null || session.loai != 1)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "" }));
            }
            base.OnActionExecuted(filterContext);

        }
    }
}