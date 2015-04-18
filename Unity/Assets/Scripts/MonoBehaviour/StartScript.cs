using UnityEngine;
using System.Threading;
using System.Collections;

public class StartScript : MonoBehaviour {

	public string ip="192.168.1.108";
	public int port=3000;
	public Logger.Type logLvl=Logger.Type.NONE;
	public string logFile="";
	// Use this for initialization
	void Start () {
		Client.getInstance ().Connect (ip,port);
		Thread t = new Thread (resourceLoad);
		Logger.logLvl = logLvl;
		Logger.logFile = logFile;
		t.Start ();
	}

	void resourceLoad (){
		ResourceLoader loader = ResourceLoader.getInstance ();
		while (true) {
			Thread.Sleep (10);
			while(loader.load());
		}

	}

}

