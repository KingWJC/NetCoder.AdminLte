using System.Web;
using System.Web.Mvc;

namespace NetCode.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandlerGlobalError());
        }
    }
}
