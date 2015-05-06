using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class Entity
{
	public GameObject go{ get; set; }
	public List<string> semantics{ get; set; }
	
	public Entity(){
		go = new GameObject ();
		semantics = new List<string> ();
	}


	public Entity(GameObject go){
		this.go = go;
		semantics = new List<string> ();
	}

	public Entity (GameObject go, List<string> semantics)
	{
		this.go = go;
		this.semantics = semantics;
	}

}