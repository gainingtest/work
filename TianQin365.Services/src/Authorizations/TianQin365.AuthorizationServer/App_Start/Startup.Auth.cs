using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TianQin365.AuthorizationServer.Providers;
using TianQin365.Common.OAuth;

namespace TianQin365.AuthorizationServer
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // 将数据库上下文和用户管理器配置为对每个请求使用单个实例
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // 使应用程序可以使用 Cookie 来存储已登录用户的信息
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // 使应用程序可以使用不记名令牌来验证用户身份
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                TokenEndpointPath = new PathString(Paths.TokenPath),
                //AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20), // 默认20mins
                ApplicationCanDisplayErrors = true,
#if DEBUG
                AllowInsecureHttp = true,
#endif
                Provider = new ApplicationOAuthProvider(), // Authorization server provider which controls the lifecycle of Authorization Server
                RefreshTokenProvider = new ApplicationRefreshTokenProvider() // Refresh token provider which creates and receives referesh token
            });
        }
    }
}