﻿using UnityEngine;
using System.Collections;
using System;

public class Instanciate : MonoBehaviour {
	public bool done = false;
	public Guid instanceOf;
	public String id;
	public String setId;

	// Use this for initialization
	void Start () {
		setId = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (done)
			return;

		id = instanceOf.ToString ();
		if (setId != "") {
			instanceOf = new Guid(setId);
			setId="";
		}
		Asset asset = ResourceLoader.getInstance ().getAsset (instanceOf);
		GameObject tmp = asset.go;
		tmp.SetActive (true);
		if(asset.loaded)
		{
			GameObject go = (GameObject) GameObject.Instantiate(tmp, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(gameObject.transform, false); 
			go.SetActive(true);
			done = true;
		}
		tmp.SetActive (false);
	}

}