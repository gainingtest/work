using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TianQin365.OrganizationAPI.Startup))]
namespace TianQin365.OrganizationAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //ConfigureWebApi(app);
        }
    }
}