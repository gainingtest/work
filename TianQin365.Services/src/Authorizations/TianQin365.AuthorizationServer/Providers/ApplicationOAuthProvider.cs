using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;

namespace TianQin365.AuthorizationServer.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;
            //if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
            //    context.TryGetFormCredentials(out clientId, out clientSecret))
            //{
            //    if (clientId == Clients.Client1.Id && clientSecret == Clients.Client1.Secret)
            //    {
            //        context.Validated();
            //    }
            //    else if (clientId == Clients.Client2.Id && clientSecret == Clients.Client2.Secret)
            //    {
            //        context.Validated();
            //    }
            //}
            //return Task.FromResult(0);

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = userManager.Find(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "用户名或密码不正确。");
                return Task.FromResult(0);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString("N")));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            foreach (var role in userManager.GetUserRoles(user))
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            //    identity.AddClaim(new Claim("urn:oauth:scope", scope));

            context.Validated(identity);

            return Task.FromResult(0);
        }

        //public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        //{
        //    //if (context.ClientId == Clients.Client1.Id)
        //    //{
        //    //    context.Validated(Clients.Client1.RedirectUrl);
        //    //}
        //    //else if (context.ClientId == Clients.Client2.Id)
        //    //{
        //    //    context.Validated(Clients.Client2.RedirectUrl);
        //    //}
        //    //return Task.FromResult(0);

        //    context.Validated();

        //    return Task.FromResult<object>(null);

        //    //return base.ValidateClientRedirectUri(context);
        //}

        //public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        //{
        //    var identity = new ClaimsIdentity(new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType), context.Scope.Select(x => new Claim("urn:oauth:scope", x)));

        //    context.Validated(identity);

        //    return Task.FromResult(0);

        //    //return base.GrantClientCredentials(context);
        //}

        //public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        //{
        //    foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
        //    {
        //        context.AdditionalResponseParameters.Add(property.Key, property.Value);
        //    }

        //    return Task.FromResult<object>(null);
        //}
    }
}