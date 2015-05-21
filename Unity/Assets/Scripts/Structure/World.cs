using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class World

{
	public string realName;
	public Vector3 extents;
	public Vector2 subdivision;
	public List<SpawnPoint> spawnPoints;
	public List<string> semantics;
	public GameObject go;


		public World ()
		{
		go = new GameObject ();
		}
}


