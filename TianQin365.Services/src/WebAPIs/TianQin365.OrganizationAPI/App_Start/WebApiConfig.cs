using System.Web.Http;
using TianQin365.Common.Encrypt;
using TianQin365.Common.WebAPI;

namespace TianQin365.OrganizationAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // 通讯加密处理程序
            config.MessageHandlers.Add(new EncryptionProcessingHandler());

            // 过滤器
            config.Filters.Add(new BaseAuthorize());
        }
    }
}
