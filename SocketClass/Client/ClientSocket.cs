using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
	class ClientSocket
	{
		Socket clientSocket;
		int port = 12321;

		public ClientSocket()
		{
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		void Connent()
		{
			clientSocket.Connect(new IPEndPoint(IPAddress.Any,port));
		}
	}
}
