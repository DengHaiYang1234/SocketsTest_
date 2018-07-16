using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			TcpClient tc = new TcpClient();

			IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12321);

			tc.Connect(ip);

			if(tc.Connected)
			{
				while(true)
				{
					string msg = Console.ReadLine();
					byte[] result = Encoding.UTF8.GetBytes(msg);
					tc.GetStream().Write(result, 0, result.Length);
				}
			}
		}
	}
}
