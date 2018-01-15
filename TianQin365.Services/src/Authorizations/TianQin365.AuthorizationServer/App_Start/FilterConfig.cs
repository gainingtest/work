using System.Web;
using System.Web.Mvc;

namespace TianQin365.AuthorizationServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
