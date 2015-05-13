using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
public class GameEntityElement
{
	public Guid gameEntity;
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public List<String> semantics;
	public List<Guid> ressources;
	public String nom;



	public GameEntityElement (String nom, Guid GameEntity, Vector3 position, Quaternion rotation, Vector3 scale, List<String> semantics, List<Guid> ressources)
	{
		this.gameEntity =gameEntity;
		this.position=position;
		this.rotation=rotation;
		this.scale =scale;
		this.semantics=semantics;
		this.ressources=ressources;
	}
}

