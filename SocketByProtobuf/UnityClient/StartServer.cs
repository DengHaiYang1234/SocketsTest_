using UnityEngine;
using System.Collections;

public class StartServer : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

		NetServer server = new NetServer();
		server.Start();
	}
	
	
}
