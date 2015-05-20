using UnityEngine;
using System.Collections;
using System;

public class InstanciateEntity : MonoBehaviour {
	bool done = false;
	public bool affiche=false;
	public Guid instanceOf;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (done)
			return;
		GameEntity ge = ResourceLoader.getInstance ().getGameEntity (instanceOf);
		GameObject tmp = ge.go;

		if(affiche)
			Debug.Log ("Update GameObject");
		if(ge.isLoaded)
		{
			Debug.Log("Clone gameEntity");
			GameObject go = (GameObject) GameObject.Instantiate(tmp, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(gameObject.transform, false); 
			go.SetActive(true);
			GameObjectUtility.ChangeLayersRecursively(go.transform,"Default");
			//go.layer = LayerMask.NameToLayer ("Default");
			done = true;
		}

	}

}