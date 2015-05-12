using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class GameEntityCreator
{
	public Guid ID{ get; set;}
	private string realName;
	private List<string> semantics;
	private List<Guid> elements;

	public GameEntityCreator (Guid ID, string realName, List<string> semantics, List<Guid> elements)
	{
		this.ID = ID;
		this.realName = realName;
		this.semantics = semantics;
		this.elements = elements;
	}

	public GameEntity create (ref GameEntity ge)
	{
		ge.semantics = semantics;
		ge.go.name = realName;
		ge.elements = elements;
		ResourceLoader loader = ResourceLoader.getInstance ();
		foreach (Guid id in elements) {

			GameObject go=loader.getMeshStruct(id);
			GameObject goi=(GameObject) GameObject.Instantiate(go,Vector3.zero,Quaternion.identity);
			goi.transform.parent=ge.go.transform;
		}

		return ge;
	}
}

