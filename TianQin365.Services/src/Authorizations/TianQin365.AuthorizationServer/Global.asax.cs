using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TianQin365.Common;
using TianQin365.Common.Models;
using TianQin365.Common.OAuth;

namespace TianQin365.AuthorizationServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Paths.AuthorizePath = "/api/Account/ExternalLogin";
            Paths.TokenPath = "/Token";

            Global.ConfigPath = Server.MapPath(Global.ConfigPath);
            Config.Initialize();
        }
    }
}
