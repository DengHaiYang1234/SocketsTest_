using UnityEngine;
using System.Collections;
using NetDatas;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SocketTest mSocket = new SocketTest();
		mSocket.ConnectServer("127.0.0.1", 8808);
		mSocket.SendMessage("服务器傻逼");
	}
}
