using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Test : MonoBehaviour {

	void Start ()
	{
		NetModel item = new NetModel()
		{
			ID = 1,
			Commit = "DHY",
			Message = "Protobuf"
		};

		byte[] temp = Serialize(item);  //将信息转换为流文件（二进制文件）
		Debug.Log(temp.Length);
		NetModel result = DeSerialize(temp);//将二进制文件转化为UTF8格式
		Debug.Log(result.Message);
	}

	private byte[] Serialize(NetModel model)
	{
		try
		{
			//使用之后就回收
			using (MemoryStream ms = new MemoryStream())
			{
				ProtoBuf.Serializer.Serialize<NetModel>(ms, model);
				byte[] result = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(result, 0, result.Length);
				return result;
			}
		}

		catch(Exception ex)
		{
			Debug.Log("序列化失败：" + ex.ToString());
			return null;
		}
	}
	

	private NetModel DeSerialize(byte[] msg)
	{
		try
		{
			using (MemoryStream ms = new MemoryStream())
			{
				ms.Write(msg, 0, msg.Length);

				ms.Position = 0;

				NetModel result = ProtoBuf.Serializer.Deserialize<NetModel>(ms);

				return result;
			}
		}

		catch(Exception ex)
		{
			Debug.Log("反序列化失败：" + ex.ToString());
			return null;
		}
	}


}
