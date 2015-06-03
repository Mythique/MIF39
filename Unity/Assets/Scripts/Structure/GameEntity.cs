using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class GameEntity
{
	public GameObject go{ get; set; }
	public bool isLoaded{ 
		get{
			if(elements==null)
				return false;
			foreach(GameEntityElement gee in elements){
				if(!gee.isLoaded)
					return false;
			}
			return true;
		}
	}
	public List<string> semantics{ get; set; }
	public List<GameEntityElement> elements{ get; set; }

	public GameEntity(){
		go = new GameObject ();
		semantics = new List<string> ();
		//elements = new List<GameEntityElement> ();
	}
}



