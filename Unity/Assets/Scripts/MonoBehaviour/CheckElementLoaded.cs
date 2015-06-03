using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CheckElementLoaded : MonoBehaviour {
	public List<GameObject> list;
	public bool ready=false;
	public GameEntityElement element;

	void Start(){
		//list=new List<GameObject>();
	}

	void Update(){
		if (ready) {
			foreach(GameObject go in list){
				if(!go.GetComponent<Instanciate>().done)
					return;
			}

			element.isLoaded=true;
			ready=false;
		}

	}

	public void add(GameObject go){
		/*if(list==null)
			list=new List<GameObject>();*/
		list.Add (go);
	}
}

