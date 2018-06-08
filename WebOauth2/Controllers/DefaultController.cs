using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebOauth2.Models;
using WebOauth2.OAuth2;

namespace WebOauth2.Controllers
{
	[ApiAuthorize]
	public class DefaultController : ApiController
	{
		private UsersEntity _usersEntity;
		/// <summary>
		/// 获取用户信息
		/// </summary>
		private UsersEntity usersEntity
		{
			get
			{
				return _usersEntity ?? (_usersEntity = HttpContext.Current.User.CurrentUser());
			}
		}

		[AllowAnonymous]
		[HttpGet]
		public string GetAllowTest()
		{
			return "测试Allow GET数据";
		}

		[HttpGet]
		public string GetTest()
		{
			return "测试GET数据";
		}

		[HttpPost]
		public string GetPost()
		{
			return "测试Post数据";
		}

		[HttpPost]
		public string ChangePassword()
		{
			MockDatabase.ChangePassword(usersEntity);
			return "测试修改密码数据，需要重新验证";
		}
	}
}
