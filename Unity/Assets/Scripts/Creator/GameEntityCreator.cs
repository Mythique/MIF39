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
			elemGo.transform.SetParent(ge.go.transform, false);
			elemGo.transform.localPosition=elem.position;
			elemGo.transform.localRotation=elem.rotation;
			elemGo.transform.localScale=elem.scale;
			elemGo.AddComponent<CheckElementLoaded>().element=elem;
			CheckElementLoaded check=elemGo.GetComponent<CheckElementLoaded>();
			check.list = new List<GameObject> ();


			foreach(Guid id in elem.ressources)
			{ 
				if(id.CompareTo(new Guid())!=0){
					Asset asset=ResourceLoader.getInstance().getAsset(id);
					GameObject des= asset.go;
					des.SetActive(false);
					GameObject go=new GameObject ();
					go.AddComponent<Instanciate>();
					go.GetComponent<Instanciate>().instanceOf = id;

					check.add(go);

					GameObject goi=go;//(GameObject) GameObject.Instantiate(go,Vector3.zero,Quaternion.identity);
					goi.transform.SetParent(elemGo.transform, false);
				}
			}
			check.ready=true;

		}

		return ge;
	}
}

