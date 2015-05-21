using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class SpawnPoint
{
	public string realName;
	public Guid worldId;
	public Vector3 location;
	public float size;
	public List<string> semantics;

	public SpawnPoint(string realName, Guid worldId, Vector3 location, float size, List<string> semantics){
		this.realName = realName;
		this.worldId = worldId;
		this.location = location;
		this.size = size;
		this.semantics = semantics;

		}
}

