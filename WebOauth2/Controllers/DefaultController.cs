using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebOauth2.Controllers
{
	[Authorize]
	public class DefaultController : ApiController
	{
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
	}
}
