using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TianQin365.Common.WebAPI
{
    public class BaseAuthorize : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var r = base.IsAuthorized(actionContext);

            if (r)
            {
                //var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                //var name = claimsIdentity.Name;
                //var userID = claimsIdentity.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
                //var roles = claimsIdentity.Claims.Where(m => m.Type == ClaimTypes.Role).Select(m => m.Value).ToList();
                ////var scopes = claimsIdentity.Claims.Where(c => c.Type == "urn:oauth:scope").ToList();
                //var phoneNumber = claimsIdentity.Claims.First(m => m.Type == ClaimTypes.MobilePhone).Value;

                if (Roles == null || Roles.Length == 0) return true;
                if (Roles.Any(HttpContext.Current.User.IsInRole)) return true;
            }

            return r;
        }

        public override void OnAuthorization(HttpActionContext httpActionContext)
        {
            //string controllerName = httpActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //string actionName = httpActionContext.ActionDescriptor.ActionName;
            // TODO:获取Controller/Action的角色并保存到Roles

            base.OnAuthorization(httpActionContext);
        }
    }
}
