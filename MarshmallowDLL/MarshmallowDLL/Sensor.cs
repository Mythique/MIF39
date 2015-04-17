using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Reflection;

namespace intelligenceDLL{
	public class Sensor : MonoBehaviour
	{

		Fire fire;
		public MethodInfo controllerMethod;
		public object controller;

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

			object[] f = new object[1];
			f [0] = fire.intensity / d;

			controllerMethod.Invoke (controller, f);
		}

		public SensorEvent OnValueChanged;

		[Serializable]
		public class SensorEvent : UnityEvent<float>
		{
		};

	}
}