using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Server
{
	public class ServerControl
	{
		Socket serverSocket;
		List<Socket> clientList;

		public ServerControl()
		{
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			clientList = new List<Socket>();
		}

		public void Start()
		{
			serverSocket.Bind(new IPEndPoint(IPAddress.Any,12321));
			serverSocket.Listen(10);

			try
			{
				Console.WriteLine("服务器启动成功");
			}
			catch
			{
				Console.WriteLine("服务器启动失败");
			}

			//主线程之外在开辟一个线程处理多个客户端的连接.
			Thread acceptThread = new Thread(Accept);
			acceptThread.IsBackground = true;
			acceptThread.Start();
		}

		void Accept()
		{
			//接收客户端的信息，会挂起当前线程.若不使用线程，那么若有第二个客户端连接，则会挂起。
			Socket client = serverSocket.Accept();
			//将EndPoint抽象类转换为实际类IPEndPoint
			IPEndPoint ipPoint = client.RemoteEndPoint as IPEndPoint;
			Console.WriteLine("IP{0}【{1}】连接成功",ipPoint.Address,ipPoint.Port);
			clientList.Add(client);


			//Recieve（）也会挂起。所以新开一个线程接收客户端信息
			Thread receiveThread = new Thread(Receive);
			receiveThread.IsBackground = true;
			receiveThread.Start(client);


			Accept();
		}

		void Receive(Object obj)
		{
			Socket client = obj as Socket;
			IPEndPoint ipEndpoint = client.RemoteEndPoint as IPEndPoint;
			try
			{
				//接收消息的消息为byte数组，需做类型转换
				byte[] bytes = new byte[1024];
				int msglen = client.Receive(bytes);
				string msg = Encoding.UTF8.GetString(bytes, 0, msglen);
				Console.WriteLine("IP{0}【{1}】:{2}", ipEndpoint.Address, ipEndpoint.Port, msg);
				string clientMsg = string.Format("IP{0}【{1}】:{2}", ipEndpoint.Address, ipEndpoint.Port, msg);
				//client.Send(Encoding.UTF8.GetBytes("11111"));
				//Send("111",client);
				Broadcast(client,clientMsg);
				Receive(client);
			}
			catch
			{
				Console.WriteLine("IP{0}【{1}】已断开", ipEndpoint.Address, ipEndpoint.Port);
				string clientMsg = string.Format("IP{0}【{1}】已退出", ipEndpoint.Address, ipEndpoint.Port);
				Broadcast(client, clientMsg);
				clientList.Remove(client);
			}
		}

		void Send(string msg,Object obj)
		{
			Socket clientSocket = obj as Socket;
			clientSocket.Send(Encoding.UTF8.GetBytes(msg));
		}


		//广播给除了当前给我发消息的其他客户端。
		void Broadcast(Socket clientOther,string msg)
		{
			foreach(var client in clientList)
			{
				if(client != clientOther)
				{
					client.Send(Encoding.UTF8.GetBytes(msg));
				}
			}
		}
	}
}
