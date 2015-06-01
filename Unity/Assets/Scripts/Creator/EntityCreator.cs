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
		this.scale = scale;
	}
	
	public Entity create (ref Entity entity)
	{
		GameObject go = entity.go;
		entity.gameEntity = meshId;
		go.name = realName;
		go.transform.SetParent(go.transform, false);
		go.transform.localPosition= position;
		go.transform.localRotation = rotation;
		go.transform.localScale = scale;
		entity.semantics = semantics;
		GameObject go2=ResourceLoader.getInstance ().getGameEntity(meshId).go;
		//go2.SetActive (false);
		GameObjectUtility.ChangeLayersRecursively(go2.transform,"cache");
		//go2.layer = LayerMask.NameToLayer ("cache");
		go.AddComponent<InstanciateEntity>();
		go.GetComponent<InstanciateEntity> ().instanceOf = meshId;


		return entity;
	}
}



