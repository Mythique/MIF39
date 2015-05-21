using UnityEngine;
using System;
using System.Threading;
using System.Collections;

public class StartScript : MonoBehaviour {
	
	public string ip="192.168.1.116";
	public int port=3000;
	public Logger.Type logLvl=Logger.Type.NONE;
	public string logFile="";
	public GameObject obj;
	private GameObject ms;
	// Use this for initialization
	void Start ()
	{

		Client.getInstance ().Connect (ip,port);

		//Thread t = new Thread (resourceLoad);
		//Thread t2 = new Thread (testLoad);
		Logger.logLvl = logLvl;
		//Logger.logFile = logFile;
		//t.Start ();
		//t2.Start ();

		//ms = ResourceLoader.getInstance ().getMeshStruct (new System.Guid ("02777adf-d977-abc5-53a1-3225eb60470a"));

		//ms = ResourceLoader.getInstance ().getGameEntity (new System.Guid ("212c94ef-7a57-2f25-0db9-d1eaa96c2506")).go;

		//ms = ResourceLoader.getInstance ().getEntity (new System.Guid ("011c8ad0-af3a-ba7e-30f5-24b6dedda0d1")).go;

		//ms = ResourceLoader.getInstance ().getChunk (new System.Guid ("cd9bcf09-99a1-92f5-9729-53e79b50873f")).go;
		//ms = ResourceLoader.getInstance ().getChunk (new System.Guid ("0688cdf6-a57c-3e81-6efb-f785e0ece2e1")).go;
		//ms = ResourceLoader.getInstance ().getChunk (new System.Guid ("07458ae8-1b92-ebe6-64ca-0c4ea248d0c4")).go;




		ms = ResourceLoader.getInstance ().getWorld (new System.Guid ("a6423f97-c6a7-7898-73ef-92bb0ffeee1d")).go;
		ms.transform.parent = obj.transform;


		//Client.getInstance ().Disconnect();
	}


	/*void testLoad (){
		ms = ResourceLoader.getInstance ().getMeshStruct (new System.Guid ("2aeee241-1309-c2e1-69d1-0d3485185752"));
		

	}*/

	void resourceLoad (){
		ResourceLoader loader = ResourceLoader.getInstance ();
		while (true) {
			Thread.Sleep (10);
			while(loader.load());
		}

	}

	void Update(){
		ResourceLoader loader = ResourceLoader.getInstance ();
		loader.load ();
		/*Guid id = ResourceLoader.getInstance ().dequeueObjACreer ();
		if (id != Guid.Empty) {
			Logger.Debug("Create GameObj "+id.ToString());
			GameObject obj=new GameObject();
			obj.AddComponent<MeshFilter>();
			obj.AddComponent<MeshRenderer>();
			ResourceLoader.getInstance ().addObj(obj,id);
		}

		if (ms != null) {
			ms.transform.parent = obj.transform;
		}*/

	}

}

