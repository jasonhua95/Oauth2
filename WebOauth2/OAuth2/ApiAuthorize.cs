using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using WebOauth2.Models;

namespace WebOauth2.OAuth2
{
	/// <summary>
	/// 自定义验证类
	/// </summary>
	public class ApiAuthorizeAttribute : AuthorizeAttribute
	{
		//重写基类的验证方式，加入我们自定义的Ticket验证,url后面都需要加上TokenID
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			if (!SkipAuthorization(actionContext)) {
				var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;

				if (claimsIdentity == null || !IsAuthorized(actionContext))
				{
					HandleUnauthorizedRequest(actionContext);
				}
				else
				{
					Claim tokenIDClaim = claimsIdentity.FindFirst("TokenID");

					//数据库匹配
					if (tokenIDClaim == null || !MockDatabase.refreshTokens.ContainsKey(tokenIDClaim.Value))
					{
						HandleUnauthorizedRequest(actionContext);
					}
				}
			}
		}

		/// <summary>
		/// 自定义错误信息
		/// </summary>
		/// <param name="actionContext"></param>
		protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
		{
			base.HandleUnauthorizedRequest(actionContext);

			var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
			response.StatusCode = HttpStatusCode.Unauthorized;
			var content = new
			{
				code = 401,
				message = "无效的令牌(token)",
				data = new { }
			};
			response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
		}

		/// <summary>
		/// 跳过AllowAnonymous
		/// </summary>
		/// <param name="actionContext"></param>
		/// <returns></returns>
		private bool SkipAuthorization(HttpActionContext actionContext)
		{
			bool flag = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
				|| actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
			return flag;
		}
	}
}