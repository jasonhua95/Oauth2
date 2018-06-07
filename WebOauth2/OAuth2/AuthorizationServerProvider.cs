using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebOauth2.OAuth2
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
			return Task.FromResult<object>(null);
		}
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			await Task.Run(()=> {
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
				if (context.UserName != "admin" && context.Password != "123456")
				{
					context.SetError("invalid_grant", "The username or password is incorrect");
					return;
				}
				var identity = new ClaimsIdentity(context.Options.AuthenticationType);
				identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
				identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
				identity.AddClaim(new Claim("sub", context.UserName));
				context.Validated(identity);
			});
		}
	}
}