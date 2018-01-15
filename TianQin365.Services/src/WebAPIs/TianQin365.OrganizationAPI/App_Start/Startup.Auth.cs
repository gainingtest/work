using Microsoft.Owin.Security.OAuth;
using Owin;

namespace TianQin365.OrganizationAPI
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}