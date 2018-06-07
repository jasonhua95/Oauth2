using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebOauth2.OAuth2;

[assembly: OwinStartup(typeof(WebOauth2.Startup))]
namespace WebOauth2
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
			ConfigAuth(app);

			HttpConfiguration config = new HttpConfiguration();
			WebApiConfig.Register(config);
			app.UseCors(CorsOptions.AllowAll);
			app.UseWebApi(config);
		}

		public void ConfigAuth(IAppBuilder app)
		{
			OAuthAuthorizationServerOptions option = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"), //获取 access_token 授权服务请求地址
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1), //access_token 过期时间
				Provider = new AuthorizationServerProvider(), //access_token 相关授权服务
				RefreshTokenProvider = new RefreshTokenProvider() //refresh_token 授权服务
			};
			app.UseOAuthAuthorizationServer(option);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
		}
	}
}