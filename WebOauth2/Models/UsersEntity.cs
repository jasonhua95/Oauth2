using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOauth2.Models
{
	public class UsersEntity
	{
		public string UserID { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
	}
}