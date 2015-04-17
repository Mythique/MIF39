using UnityEngine;
using System.Threading;
using System.Collections;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Client.getInstance ().Connect ();
		Thread t = new Thread (resourceLoad);
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

