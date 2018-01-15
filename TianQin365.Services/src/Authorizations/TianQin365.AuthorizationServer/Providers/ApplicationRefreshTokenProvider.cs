using System;
using System.Collections.Concurrent;
using Microsoft.Owin.Security.Infrastructure;

namespace TianQin365.AuthorizationServer.Providers
{
    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, string> refreshTokens = new ConcurrentDictionary<string, string>();

        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(1);

            context.SetToken(context.SerializeTicket());
            refreshTokens[context.Token] = context.SerializeTicket();
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (refreshTokens.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
        }
    }
}