using System.Web.Mvc;

namespace ELearning_V2.Areas.GV
{
    public class GVAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GV_default",
                "GV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "ELearning_V2.Areas.GV.Controllers" }
            );
        }
    }
}