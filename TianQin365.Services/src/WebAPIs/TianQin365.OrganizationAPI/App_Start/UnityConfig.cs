using System.Web.Http;
using Microsoft.Practices.Unity;
using TianQin365.Organization.Application;
using Unity.WebApi;

namespace TianQin365.OrganizationAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ICompanyService, CompanyService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}