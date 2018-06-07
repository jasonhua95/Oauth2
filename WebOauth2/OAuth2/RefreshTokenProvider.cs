using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOauth2.OAuth2
{
	public class RefreshTokenProvider : AuthenticationTokenProvider
	{
		private static ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

		/// <summary>
		/// 生成 refresh_token
		/// </summary>
		public override void Create(AuthenticationTokenCreateContext context)
		{
			//这里的数据可以保存到数据库，通过数据库来获取数据
			context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
			context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(100);

			context.SetToken(Guid.NewGuid().ToString("n"));
			_refreshTokens[context.Token] = context.SerializeTicket();
		}

		/// <summary>
		/// 由 refresh_token 解析成 access_token
		/// </summary>
		public override void Receive(AuthenticationTokenReceiveContext context)
		{
			string value;
			if (_refreshTokens.TryRemove(context.Token, out value))
			{
				context.DeserializeTicket(value);
			}
		}
	}
}