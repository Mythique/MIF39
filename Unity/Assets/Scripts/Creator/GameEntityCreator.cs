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
	private List<GameEntityElement> elements;

	public GameEntityCreator (Guid ID, string realName, List<string> semantics, List<GameEntityElement> elements)
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
		foreach (GameEntityElement elem in elements) {
			GameObject elemGo =new GameObject();
			elemGo.transform.position=elem.position;
			elemGo.transform.rotation=elem.rotation;
			elemGo.transform.localScale=elem.scale;
			elemGo.transform.SetParent(ge.go.transform, false);
			foreach(Guid id in elem.ressources)
			{ 
				if(id.CompareTo(new Guid())!=0){
					GameObject des= ResourceLoader.getInstance().getMeshStruct(id);
					des.SetActive(false);
					GameObject go=new GameObject ();
					go.AddComponent<Instanciate>();
					go.GetComponent<Instanciate>().instanceOf = id;
					go.GetComponent<Instanciate>().ge = ge;

					GameObject goi=go;//(GameObject) GameObject.Instantiate(go,Vector3.zero,Quaternion.identity);
					goi.transform.SetParent(elemGo.transform, false);
				}
			}
		}

		return ge;
	}
}

