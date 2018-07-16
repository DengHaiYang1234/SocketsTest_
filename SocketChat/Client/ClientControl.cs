using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
	public class ClientControl
	{
		delegate void Func();

		Socket clientSocket;

		public ClientControl()
		{
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Connect(string ip,int port)
		{
			clientSocket.Connect(ip,port);

			try
			{
				Console.WriteLine("连接服务器成功");
				Thread newThread = new Thread(Receive);
				newThread.IsBackground = true;
				newThread.Start();
			}
			catch
			{
				Console.WriteLine("连接服务器失败");
			}
		}

		public void Send()
		{
			Console.WriteLine("请输入，输入quite结束");
			string msg = Console.ReadLine();
			while (msg != "quite")
			{
				clientSocket.Send(Encoding.UTF8.GetBytes(msg));
				msg = Console.ReadLine();
			}
			
		}

		void Receive()
		{

			try
			{
				byte[] msg = new byte[1024];
				int msgLen = clientSocket.Receive(msg);
				string receiveMsg = Encoding.UTF8.GetString(msg, 0, msgLen);
				Console.WriteLine("服务器返回消息：" + receiveMsg);
				Receive();
			}

			catch
			{
				Console.WriteLine("服务器拒绝");
			}

		}
	}
}
