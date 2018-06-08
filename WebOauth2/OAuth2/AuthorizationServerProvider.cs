using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebOauth2.Models;

namespace WebOauth2.OAuth2
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		/// <summary>
		/// 验证grant_type
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// 认证
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			await Task.Run(()=> {
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
				//查询数据库进行信息验证clientid省略
				UsersEntity user = MockDatabase.CheckUser(context.UserName, context.Password);
				if (user == null)
				{
					context.SetError("invalid_grant", "The username or password is incorrect");
					return;
				}

				//基于声明的认证，用来标识一个人的身份（如：姓名，邮箱等等），access_token中保存的信息
				var identity = new ClaimsIdentity(context.Options.AuthenticationType);
				identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
				identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
				identity.AddClaim(new Claim("TokenID", Guid.NewGuid().ToString("n")));
				identity.AddClaim(new Claim("UserId", user.UserID));
				identity.AddClaim(new Claim("Email", user.Email));

				//认证信息，可以说成证书信息
				var props = new AuthenticationProperties(new Dictionary<string, string>
				{
					{ "UserName", user.UserName},
				});
				var ticket = new AuthenticationTicket(identity, props);
				context.Validated(ticket);
			});
		}

		/// <summary>
		/// refresh_token
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
		{
			await Task.Run(() => {
				var newId = new ClaimsIdentity(context.Ticket.Identity);
				newId.RemoveClaim(newId.FindFirst("TokenID"));
				newId.AddClaim(new Claim("TokenID", Guid.NewGuid().ToString("n")));
				var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
				context.Validated(newTicket);
			});
		}

		/// <summary>
		/// 认证信息的返回到前台显示
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}
	}
}