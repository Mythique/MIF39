using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class Chunk
{
	public GameObject go { get; set; }
	public Vector2 indice { get; set; }

	public Chunk(){
		go = new GameObject ();
	}

	public Chunk(GameObject go){
		this.go = go;
	}


}



