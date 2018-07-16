using UnityEngine;
using System.Collections;
using ProtoBuf;


[ProtoContract]
public class NetModel
{
	[ProtoMember(1)]
	public int ID;
	[ProtoMember(2)]
	public string Commit;
	[ProtoMember(3)]
	public string Message;
}

