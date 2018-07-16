using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class NetUserToken 
{
	public Socket socket;

	public byte[] buffer;

	public NetUserToken()
	{
		buffer = new byte[1024];
	}


	/// <summary>
	/// 接收消息
	/// </summary>
	public void Receive(byte[] bytes)
	{
		Debug.Log("接收到客户端的消息");
	}


	/// <summary>
	/// 发送消息
	/// </summary>
	public void Send()
	{

	}


}
