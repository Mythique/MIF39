using UnityEngine;
using System.Collections;
using System;

public class Instanciate : MonoBehaviour {
	bool done = false;
	public Guid instanceOf;
	public GameEntity ge; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (done)
			return;
		GameObject tmp = ResourceLoader.getInstance ().getMeshStruct (instanceOf);
		tmp.SetActive (true);
		if(tmp.GetComponent<MeshFilter>().sharedMesh != null)
		{
			GameObject go = (GameObject) GameObject.Instantiate(tmp, Vector3.zero, Quaternion.identity);
			go.transform.SetParent(gameObject.transform, false); 
			go.SetActive(true);
			done = true;
			ge.isLoaded=true;
		}
		tmp.SetActive (false);
	}
}