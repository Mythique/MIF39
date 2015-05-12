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
		Logger.logFile = logFile;
		//t.Start ();
		//t2.Start ();
		ms = ResourceLoader.getInstance ().getMeshStruct (new System.Guid ("2aeee241-1309-c2e1-69d1-0d3485185752"));
		ms.transform.parent = obj.transform;

		ResourceLoader loader = ResourceLoader.getInstance ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		loader.load ();
		//Client.getInstance ().Disconnect();
	}


	void testLoad (){
		ms = ResourceLoader.getInstance ().getMeshStruct (new System.Guid ("2aeee241-1309-c2e1-69d1-0d3485185752"));
		

	}

	void resourceLoad (){
		ResourceLoader loader = ResourceLoader.getInstance ();
		while (true) {
			Thread.Sleep (10);
			while(loader.load());
		}

	}

	void Update(){
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

