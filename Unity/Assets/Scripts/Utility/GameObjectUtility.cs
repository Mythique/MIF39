using UnityEngine;
using System.Collections;
using System;

public class GameObjectUtility
{
			

	public static void ChangeLayersRecursively(Transform trans, string name)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(name);
		foreach(Transform child in trans)
		{            
			ChangeLayersRecursively(child,name);
		}
	}
}

