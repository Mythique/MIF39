using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class CookingSensor : MonoBehaviour
{
	public SimpleMarshmallow controllerToCall;
	Fire fire;
	// Use this for initialization
	void Start ()
	{
		fire = GameObject.FindGameObjectWithTag ("Fire").GetComponent<Fire> ();
		if (fire == null) {
			Debug.LogError ("You must assign the tag \"Fire\" to the fire !");
			Application.Quit ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{		
		float d = Vector3.Distance (transform.position, fire.transform.position);

		if (d == 0f) {
			Debug.LogError ("Cannot divide by 0");
			d = 0.00001f;
			//Application.Quit ();
		}

		controllerToCall.sensorValueChanged (fire.intensity / d);
	}

	[Serializable]
	public class SensorEvent : UnityEvent<float>
	{
	};

}
