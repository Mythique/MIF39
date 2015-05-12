using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class EntityCreator
{
	public Guid id {get; set;}
	string realName;
	Guid cell;
	Vector3 position;
	Quaternion rotation;
	Vector3 scale;
	List<string> semantics;
	Guid meshId;
	
	public EntityCreator(Guid id, string realName, Guid cell, Vector3 position, Quaternion rotation, Vector3 scale, List<string> semantics, Guid meshId)
	{
		this.id = id;
		this.realName = realName;
		this.cell = cell;
		this.position = position;
		this.rotation = rotation;
		this.semantics = semantics;
		this.meshId = meshId;
	}
	
	public Entity create (ref Entity entity)
	{
		GameObject go = entity.go;
		go.name = realName;
		go.transform.position = position;
		go.transform.rotation = rotation;
		go.transform.localScale = scale;
		entity.semantics = semantics;
		//TODO
		GameObject go2=ResourceLoader.getInstance ().getGameEntity(meshId).go;
		GameObject go3=GameObject.Instantiate(go2,Vector3.zero,Quaternion.identity);
		go3.transform.parent = go.transform;

		return entity;
	}
}



