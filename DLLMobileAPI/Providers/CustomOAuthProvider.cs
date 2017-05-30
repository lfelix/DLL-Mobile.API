using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.IO;
using DLLMobileAPI.Business;
using System.Net;
using Newtonsoft.Json;

namespace DLLMobileAPI.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var form = await context.Request.ReadFormAsync();
            var deviceId = form["device_id"];
            UserBusiness business = new UserBusiness();
            ApplicationUser user = business.Authtenticate(context.UserName, context.Password);
            if (user != null)
            {
                if (business.AuthenticatedInAnotherDevice(deviceId, user.Id))
                {
                    context.Rejected();
                    context.SetError("concurrency_fail", "This username is already connected from other device.");
                    return;
                }
            }
            else
            {
                context.Rejected();
                context.SetError("invalid_grant", "The user name or password is incorrect");
                return;
            }

            var ticket = new AuthenticationTicket(SetClaimsIdentity(context, user), new AuthenticationProperties());
            context.Validated(ticket);
            business.SetLoginActivity(deviceId, user.Id);

            return;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, ApplicationUser user)
        {
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));

            //todo: arrumar isso
            //var userRoles = context.OwinContext.Get<DefaultUserManager>().GetRoles(user.Id);
            var userRoles = new List<string>();
            userRoles.Add("admins");
            foreach (var role in userRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }
    }
}