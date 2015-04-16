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

	public void ask(int type, List<Guid> param)
	{
		bool read = false;
		int msgSize;
		Stream stm = tcpclnt.GetStream();
		byte[] t = BitConverter.GetBytes (type);
		stm.Write (t, 0, t.Length);

		foreach(Guid i in param) {
			byte[] guid = i.ToByteArray();
			stm.Write(guid, 0, guid.Length);
		}
		ServerAnswerManager.getInstance ().addContents (stm);
	}

	 
	public static void Connect(string ipAdresse, int port)
	{
		/*bool wait = true;
		bool lecture = false;
		//*/
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
			/*Stream stm = tcpclnt.GetStream();
			Debug.Log("Stream okok");


			ASCIIEncoding asen= new ASCIIEncoding();
			byte[] ba=asen.GetBytes(str);
			byte[] size = BitConverter.GetBytes(ba.Length);

			/*Debug.Log("Transmitting size of string : " + ba.Length);
			stm.Write(size,0,size.Length);
			Debug.Log("Transmitting string");
			stm.Write(ba,0,ba.Length);

			uint tailleMessage = 0;

			while(wait)
			{
				Debug.Log ("en attente d'une réponse");
				if(stm.CanRead && !lecture)
				{
					byte[] tailleMessageByte = new byte[4];
					stm.Read(tailleMessageByte, 0, 4);
					tailleMessage = BitConverter.ToUInt32(tailleMessageByte, 0);
					Debug.Log ("taille message : " + tailleMessage);
					lecture = true;
				}
				if(stm.CanRead && lecture)
				{
					byte[] MessageByte = new byte[tailleMessage];
					stm.Read(MessageByte, 0, (int)tailleMessage);
					string message = BitConverter.ToString(MessageByte,0);
					Debug.Log ("message reçu : " + message);
					wait = false;
				}

			}
			tcpclnt.Close();
			//*/
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
		Connect("192.168.1.108", 3000);

		List<Guid> testList = new List<Guid>();
		Guid testGuid = new Guid ("test");
		testList.Add (testGuid);
		int testType = -1;

		bool continued = true;

		while(continued) 
		{
			ask(testType, testList);
			if(ServerAnswerManager.getInstance().getAnswerTest()){
				Debug.Log("Test réussi!!!!");
				continued = false;
			}
		}
		Disconnect ();
	}
}
