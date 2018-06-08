using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebOauth2.Models;

namespace WebOauth2.OAuth2
{
	public class RefreshTokenProvider : AuthenticationTokenProvider
	{
		/// <summary>
		/// 生成 refresh_token
		/// </summary>
		public override void Create(AuthenticationTokenCreateContext context)
		{
			//这里的数据可以保存到数据库，通过数据库来获取数据
			ClaimsIdentity claimsIdentity = context.Ticket.Identity;
			Claim userIdClaim = claimsIdentity.FindFirst("UserId");
			Claim tokenIDClaim = claimsIdentity.FindFirst("TokenID");

			context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
			context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(100);
			context.SetToken(tokenIDClaim.Value);

			MockDatabase.refreshTokens[context.Token] = new ValueTuple<string, string> { Item1 = userIdClaim.Value, Item2 = context.SerializeTicket() };
		}

		/// <summary>
		/// 由 refresh_token 解析成 access_token
		/// </summary>
		public override void Receive(AuthenticationTokenReceiveContext context)
		{
			ValueTuple<string, string> value;
			if (MockDatabase.refreshTokens.TryRemove(context.Token, out value))
			{
				context.DeserializeTicket(value.Item2);
			}
		}
	}
}