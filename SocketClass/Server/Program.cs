using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using NetDatas;

namespace SocketServer
{
	class Program
	{
		private static byte[] result = new byte[1024];
		private const int port = 8808;
		private static string IP = "127.0.0.1";
		private static Socket socketServer;

		static void Main(string[] args)
		{
			IPAddress ip = IPAddress.Parse(IP);
			IPEndPoint ipEndpoint = new IPEndPoint(ip, port);
			socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socketServer.Bind(ipEndpoint);
			socketServer.Listen(100);
			Console.WriteLine("启动监听{0}成功", socketServer.LocalEndPoint.ToString());

			Thread thread = new Thread(ClientConnectListen);
			thread.Start();
			Console.ReadLine();

		}

		private static void ClientConnectListen()
		{
			Socket clientSocket = socketServer.Accept();
			Console.WriteLine("客户端{0}连接成功", clientSocket.RemoteEndPoint.ToString());

			ByteBuffer buffer = new ByteBuffer();
			buffer.WriteString("Connected Server");
			clientSocket.Send(WriteMessage(buffer.ToBytes()));

			Thread thread = new Thread(RecieveMessage);
			thread.Start(clientSocket);
		}

		private static byte[] WriteMessage(byte[] message)
		{
			MemoryStream ms = null;
			using (ms = new MemoryStream())
			{
				ms.Position = 0;
				BinaryWriter writer = new BinaryWriter(ms);
				ushort msglen = (ushort)message.Length;
				writer.Write(msglen);
				writer.Write(message);
				writer.Flush();
				return ms.ToArray();
			}
		}

		private static void RecieveMessage(object clientSocket)
		{
			Socket mClientSocket = (Socket)clientSocket;
			try
			{
				int receiveNumber = mClientSocket.Receive(result);
				Console.WriteLine("接受客户端{0}消息，长度为{1}", mClientSocket.RemoteEndPoint.ToString(), receiveNumber);
				ByteBuffer buff = new ByteBuffer(result);

				int len = buff.ReadShort();

				string data = buff.ReadString();
				Console.WriteLine("数据内容：{0}", data);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				mClientSocket.Shutdown(SocketShutdown.Both);
				mClientSocket.Close();
			}
		}
	}
}
