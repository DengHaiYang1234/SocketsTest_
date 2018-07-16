using UnityEngine;
using System.Collections;
using NetDatas;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class SocketTest
{
	private static byte[] result = new byte[1024];
	private static Socket clientSocket;

	public bool IsConnected = false;

	public SocketTest()
	{
		clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	}

	public void ConnectServer(string ip,int port)
	{
		IPAddress mIP = IPAddress.Parse(ip);
		IPEndPoint ipEndPoint = new IPEndPoint(mIP,port);
		
		try
		{
			clientSocket.Connect(ipEndPoint);
			IsConnected = true;
			Debug.Log("服务器已经连接成功");
		}
		catch
		{
			IsConnected = false;
			Debug.Log("连接服务器失败");
			return;
		}

		int receiveLength = clientSocket.Receive(result);  //将接收到的数据存入result缓存区中
		ByteBuffer buffer = new ByteBuffer(result);
		int len = buffer.ReadShort();
		string data = buffer.ReadString();
		Debug.Log("服务器返回数据：" + data);

	}

	public void SendMessage(string data)
	{
		if(IsConnected == false)
		{
			return;
		}

		try
		{
			ByteBuffer buffer = new ByteBuffer();
			buffer.WriteString(data);
			clientSocket.Send(WriteMessage(buffer.ToBytes()));
		}
		catch
		{
			IsConnected = false;
			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
		}
	}

	private  byte[] WriteMessage(byte[] message)
	{
		MemoryStream ms = null;
		using (ms = new MemoryStream()) //using执行完成后。执行完成后，程序会自动回收资源！
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

}
