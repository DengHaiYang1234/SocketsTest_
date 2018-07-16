using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ProtocolData;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Client
{
	class ClientControl
	{
		Socket client;

		public ClientControl()
		{
			client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Connect(string ip,int port)
		{
			client.Connect(ip, port);

			Thread threadReceive = new Thread(Receive);
			threadReceive.IsBackground = true;
			threadReceive.Start();
		}

		public void Send(ProtocolGame game)
		{
			//实例化流
			MemoryStream stream = new MemoryStream();
			//二进制转换
			BinaryFormatter formatter = new BinaryFormatter();
			//序列化
			formatter.Serialize(stream,game);
			//将序列化的流文件转换为数组发送
			byte[] msg = stream.GetBuffer();

			client.Send(msg);
		}

		public void SendMsg()
		{
			UserInfo useInfos = new UserInfo();
			Console.WriteLine("请输入用户名：");
			useInfos.Act = Console.ReadLine();
			Console.WriteLine("请输入密码：");
			useInfos.Pwd = Console.ReadLine();

			ProtocolGame game = new ProtocolGame();
			game.model = 1;
			game.ope = 1;
			game.data = useInfos;

			Send(game);
		}

		void Receive()
		{
			byte[] msg = new byte[1024 * 1024];
			int msgLen = client.Receive(msg);

			Console.WriteLine("服务器返回的消息：" + Encoding.UTF8.GetString(msg,0,msgLen));

			//MemoryStream stream = new MemoryStream(msg);
			//BinaryFormatter formatter = new BinaryFormatter();
			//ProtocolGame game = formatter.Deserialize(stream) as ProtocolGame;


			Receive();
		} 
	}


}
