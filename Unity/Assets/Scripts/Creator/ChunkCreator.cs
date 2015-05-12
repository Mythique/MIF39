using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class ChunkCreator
{
	public Guid id{ get; set; }
	string realName;
	Guid world;
	Vector2 indice;
	Vector3 extents;
	Vector3 position;
	List<Guid> objects;

	public ChunkCreator (Guid id, string realName, Guid world, Vector2 indice, Vector3 extents, Vector3 position, List<Guid> objects)
	{
		this.id = id;
		this.realName = realName;
		this.world = world;
		this.indice = indice;
		this.extents = extents;
		this.position = position;
		this.objects = objects;
	}

	public Chunk create (ref Chunk chunk)
	{
		GameObject go = chunk.go;
		chunk.objects = objects;
		go.name = realName;
		go.transform.localScale = extents;
		go.transform.position = position;
		chunk.indice = indice;

		foreach (Guid id in objects) {

			Entity entity=ResourceLoader.getInstance().getEntity(id);
			GameObject go2=GameObject.Instantiate(entity.go,Vector3.zero,Quaternion.identity);
			go2.transform.parent=go.transform;
		}

		return chunk;
	}
}



