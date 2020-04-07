using System.Web.Mvc;

namespace NetCode.Web.Areas.Base_SysManage
{
    public class Base_SysManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Base_SysManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Base_SysManage_default",
                "Base_SysManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}