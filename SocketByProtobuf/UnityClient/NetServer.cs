using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System;

public class NetServer
{
	//public static readonly NetServer Instance = new NetServer();

	Socket serverSocket;

	int maxClient = 10;

	int port = 12321;

	Stack<NetUserToken> pools;

	public NetServer()
	{
		serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		serverSocket.Bind(new IPEndPoint(IPAddress.Any,port));
	}

	public void Start()
	{
		serverSocket.Listen(maxClient);
		Debug.Log("Server is Ok");

		pools = new Stack<NetUserToken>(maxClient);

		for(int i = 0;i< maxClient; i++)
		{
			NetUserToken userToken = new NetUserToken();
			pools.Push(userToken);
		}

		serverSocket.BeginAccept(AsyncAccept,null);
	}

	/// <summary>
	/// 异步接收客户端的连接
	/// </summary>
	/// <param name="result"></param>
	void AsyncAccept(IAsyncResult result)
	{
		try
		{
			Socket client = serverSocket.EndAccept(result);

			Debug.Log("有客户端连接:" + client.RemoteEndPoint);
			NetUserToken userToken = pools.Pop();
			userToken.socket = client;
			BeginReceive(userToken);
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}

	/// <summary>
	/// 异步监听消息
	/// </summary>
	/// <param name="userToken"></param>
	void BeginReceive(NetUserToken userToken)
	{
		try
		{
			userToken.socket.BeginReceive(userToken.buffer, 0, userToken.buffer.Length, SocketFlags.None, EndReceive, userToken);
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}


	void EndReceive(IAsyncResult result)
	{
		try
		{
			NetUserToken userToken = result.AsyncState as NetUserToken;

			int len = userToken.socket.EndReceive(result);

			if(len > 0)
			{
				byte[] data = new byte[len];
				Buffer.BlockCopy(userToken.buffer, 0, data, 0,len);
				userToken.Receive(data);
				BeginReceive(userToken);
			}
		}

		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}


}
