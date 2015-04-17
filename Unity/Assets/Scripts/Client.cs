using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class Client: MonoBehaviour
{	
	private static TcpClient tcpclnt;

	public void ask(int type,Guid param)
	{
		bool read = false;
		int msgSize;
		Stream stm = tcpclnt.GetStream();
		byte[] t = BitConverter.GetBytes (type);
		stm.Write (t, 0, t.Length);
		stm.Write (BitConverter.GetBytes(38), 0, 4);
		byte[] guid = System.Text.Encoding.ASCII.GetBytes ("{"+param.ToString()+"}");
		Debug.Log (System.Text.Encoding.ASCII.GetString(guid));
		stm.Write(guid, 0, guid.Length);
		stm.Flush ();
		ServerAnswerManager.getInstance ().addContents (stm);
	}

	 
	public static void Connect(string ipAdresse, int port)
	{
		try 
		{
			tcpclnt = new TcpClient();
			Debug.Log("Connecting.....");

			if(ipAdresse != "")
			{
				Debug.Log(ipAdresse);
				tcpclnt.Connect(ipAdresse,port);
			}
			else
			{
				tcpclnt.Connect(IPAddress.Any,port);
			}

			Debug.Log("Addresse okok");
		}
		
		catch (Exception e)
		{
			Debug.Log("Error..... " + e.StackTrace);
		}
	}

	public static void Disconnect() {
		try 
		{
			tcpclnt.Close();
		}
		catch(Exception e)
		{
			Debug.Log("Error disconnecting");
		}
	}

	void Start(){
		Connect("192.168.1.168", 3000);

		//Guid testGuid = new Guid ("60633b7c-3d7e-d298-384a-22a37ecc723a");
		//Guid testGuid = new Guid ("2c8f0939-2be1-0b49-0178-8e89bd4f6986");
		//Guid testGuid = new Guid ("d45925f5-9829-fe00-cf43-13b6ef24e539");

		//int testType = (int)ServerAnswerManager.Type.SHARED_R;

		//bool continued = true;
		//ask(testType, testGuid);
		//Disconnect ();

	}

	void Update(){
		ResourceLoader.getInstance().load ();
	}

}
