using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using WebOauth2.Models;

namespace WebOauth2.OAuth2
{
	/// <summary>
	/// 获取用户信息类
	/// </summary>
	public static class UserExtension
	{
		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <param name="principal"></param>
		/// <returns></returns>
		public static UsersEntity CurrentUser(this IPrincipal principal)
		{
			UsersEntity user = null;
			var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
			if (claimsIdentity != null)
			{
				Claim userIdClaim = claimsIdentity.FindFirst("UserId");
				Claim emailClaim = claimsIdentity.FindFirst("Email"); 
				if (userIdClaim != null)
				{
					user = new UsersEntity();
					user.UserID = userIdClaim.Value;
					user.Email = emailClaim.Value;
				}
			}

			return user;
		}
	}
}