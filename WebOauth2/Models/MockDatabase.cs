using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOauth2.Models
{
	/// <summary>
	/// 模拟数据库
	/// </summary>
	public class MockDatabase
	{
		//refreshToken保存到数据库,这个保存到数据库，SSO，服务器横向多看
		public static ConcurrentDictionary<string, ValueTuple<string, string>> refreshTokens = new ConcurrentDictionary<string, ValueTuple<string, string>>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static UsersEntity CheckUser(string userName, string password)
		{
			UsersEntity user = null;
			if (userName != null && password != null)
			{
				user = new UsersEntity();
				user.UserID = userName;
				user.UserName = userName;
				user.Email = "test@163.com";
			}
			return user;
		}

		/// <summary>
		/// 修改密码删除关于此用户的所有token
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool ChangePassword(UsersEntity user) {
			bool result = true;
			foreach (KeyValuePair<string, ValueTuple<string, string>> kvPair in refreshTokens)
			{
				ValueTuple<string, string> token;
				if (kvPair.Value.Item1 == user.UserID)
				{
					refreshTokens.TryRemove(kvPair.Key, out token);
				}
			}
			return result;
		}
	}
}