using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using ProtocolData;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
	class SocketServer
	{
		Socket socket;

		public SocketServer()
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Start()
		{
			socket.Bind(new IPEndPoint(IPAddress.Any,12321));
			socket.Listen(10);

			Thread threadAccept = new Thread(Accept);
			threadAccept.IsBackground = true;
			threadAccept.Start();
		}

		public void Accept()
		{
			Socket client = socket.Accept();

			Thread threadReceive = new Thread(Receive);
			threadReceive.IsBackground = true;
			threadReceive.Start(client);

			Accept();
		}

		public void Receive(object obj)
		{
			Socket client = obj as Socket;

			byte[] msg = new byte[1024 * 1024];
			int msgLen = client.Receive(msg);

			MemoryStream stream = new MemoryStream(msg, 0, msgLen);
			BinaryFormatter formatter = new BinaryFormatter();
			ProtocolGame protocolGame =  formatter.Deserialize(stream) as ProtocolGame;


			switch(protocolGame.model)
			{
				case 1://登陆
					switch(protocolGame.ope)
					{
						case 1://注册

							break;
						case 2://登陆

							break;
						case 3://找回密码

							break;
					}
					break;
				case 2://聊天
					break;
				case 3://战斗
					break;
			}

			Receive(obj);
		}



	}
}
