using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TianQin365.AuthorizationServer.Startup))]
namespace TianQin365.AuthorizationServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}