﻿using ProtocolData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			ClientControl client = new ClientControl();
			client.Connect("127.0.0.1", 12321);

			client.SendMsg();



		}
	}
}
