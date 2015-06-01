using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class WorldCreator
{
	public Guid ID{ get; set; }
	string realName;
	Vector3 extents;
	Vector2 subdivision;
	List<SpawnPoint> spawnPoints;
	List<string> semantics;
	List<Guid> cells;

	public WorldCreator (Guid ID,string realName,Vector3 extents,Vector2 subdivision,List<SpawnPoint> spawnPoints,List<string> semantics,List<Guid> cells){
		this.ID = ID;
		this.realName = realName;
		this.extents = extents;
		this.subdivision = subdivision;
		this.spawnPoints = spawnPoints;
		this.semantics = semantics;
		this.cells = cells;

	}

	public World create(ref World world){
		world.realName = realName;
		world.extents = extents;
		world.subdivision = subdivision;
		world.spawnPoints = spawnPoints;
		world.semantics = semantics;

		ResourceLoader loader = ResourceLoader.getInstance ();
		foreach (Guid g in cells) {
			GameObject go = loader.getChunk(g).go;
			go.transform.SetParent(world.go.transform,false);

		}

		/*GameObject go = loader.getChunk(cells[41]).go;
		go.transform.SetParent(world.go.transform,false);*/

		return world;

	}
}



