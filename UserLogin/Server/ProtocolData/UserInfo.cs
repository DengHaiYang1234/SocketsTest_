using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolData
{
	public class UserInfo
	{
		//可做MD5加密
		string act;
		string pwd;

		public string Act
		{
			get { return act; }
			set { act = value; }
		}

		public string Pwd
		{
			get { return pwd; }
			set { pwd = value; }
		}

		//public UserInfo(string UserAct,string UserPwd)
		//{
		//	act = UserAct;
		//	pwd = UserPwd;
		//	MD5Encrypt16(pwd);
		//}

		//string MD5Encrypt16(string password)
		//{
		//	var md5 = new MD5CryptoServiceProvider();
		//	string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(password)),4,8);

		//	t2 = t2.Replace("-", "");
		//	return t2;

		//}
	}
}
