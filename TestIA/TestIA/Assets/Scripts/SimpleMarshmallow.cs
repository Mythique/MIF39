using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using DesignPatterns;

using FuzzyLogic;

using Conjunction = FuzzyLogicSolver.TNorm;
using Disjunction = FuzzyLogicSolver.SNorm;
using Activation = FuzzyLogicSolver.TNorm;
using Accumulation = FuzzyLogicSolver.SNorm;
using System.Reflection;

public abstract class AIController : MonoBehaviour
{
	public abstract void sensorValueChanged (float f);
}

[Serializable]
public class Effector
{
	public OutputVariable Input { get; set; }

	//public MovingHuman human;

	public MethodInfo mInfo;
	public object obj;

	public void Update ()
	{
		double value = Input.Value;
		if (!double.IsNaN (value)) {
			float v = (float)value;
			//human.UpdateDistance(v);

			object[] f = new object[1];
			f[0] = v;

			/*Debug.Log (obj);
			Debug.Log (mInfo);*/

			Debug.Log ("Telling marc to move : " + v);
			mInfo.Invoke(obj, f);
			//[AJOUT]Ici indiquer au server que ma position a change 
		}
	}

	public OutputEvent OnValueChanged;
	[Serializable]
	public class OutputEvent : UnityEvent<float>
	{
	}
}

public class SimpleMarshmallow : MonoBehaviour
{
	public GameObject marshmallow;
	public GameObject marc;
	public float threshold = 0.01f;
	public float oldSensorValue = -1.0f;
	public float currentSensorValue = -1.0f;
	public float requiredValue = 0.5f;
	public bool Enabled = true;
	public Effector controlledDistance = new Effector ();
	public FuzzyLogic.Engine engine;
	public FuzzyLogicSolver.Engine solver;
	//public SimpleMarshmallowController controller;

	private int timer;
	private GameObject wood;

	void Start ()
	{
		Assembly a = Assembly.LoadFile (@"C:\Users\Unity\Documents\StuffToExport\MarshmallowDLL\MarshmallowDLL\bin\Debug\MarshmallowDLL.dll");

		object o = a.CreateInstance ("MarshmallowDLL.SimpleMarshmallowController");

		Type type = a.GetType ("MarshmallowDLL.SimpleMarshmallowController");

		engine = (FuzzyLogic.Engine) type.GetField ("theEngine").GetValue (o);
		solver = (FuzzyLogicSolver.Engine)type.GetField ("theSolver").GetValue (o);

		controlledDistance.Input = engine.Outputs ["Distance"];

		GameObject.FindGameObjectWithTag("Fire").AddComponent(a.GetType("MarshmallowDLL.Fire"));

		wood = createWood ();
		
		marc = GameObject.CreatePrimitive (PrimitiveType.Cube);
		marshmallow = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		
		marc.AddComponent<Rigidbody> ();
		
		marshmallow.transform.SetParent (marc.transform);
		
		marc.transform.position = new Vector3 (1, 0, 0);
		marshmallow.transform.position = new Vector3 (-0.026f, 0.063f, 0.711f);
		marshmallow.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

		Type movingHuman = a.GetType ("MarshmallowDLL.MovingHuman");

		marc.AddComponent(movingHuman);

		controlledDistance.mInfo = marc.GetComponent (movingHuman).GetType ().GetMethod ("UpdateDistance");

		controlledDistance.obj = marc.GetComponent (movingHuman);

		Type cookingSensor = a.GetType ("CookingSensor");

		marshmallow.AddComponent (cookingSensor);

		FieldInfo controllerFi = cookingSensor.GetField ("controller");

		Component c = marshmallow.GetComponent ("CookingSensor");
	
		MethodInfo sensorValInfo = this.GetType ().GetMethod ("sensorValueChanged");

		FieldInfo controller = c.GetType ().GetField ("controllerMethod");

		controller.SetValue (c, sensorValInfo);

		c.GetType ().GetField ("controller").SetValue (c, this);
	}

	void FixedUpdate ()
	{
		if (Enabled) {
			solver.Process();
			controlledDistance.Update ();
			//DEMANDER AU SERVER DES INFOS
		}
		if (timer > 500) {
			GameObject.Destroy(wood);
			wood = createWood ();
			timer = 0;
		}
		timer ++;
	}

	public void sensorValueChanged (float v)
	{
		//Debug.Log ("Sensor Value changed to " + v);
		currentSensorValue = v;

		float newInput = v - requiredValue;

		engine.Inputs ["FeltIntensity"].Value = newInput;
		//controller.theEngine.Inputs ["lol ca marche pas normalement"].Value = v - requiredValue;
		Debug.Log ("Sensor value : " + newInput);
		oldSensorValue = v;
	}

	public void requiredIntensityChanged (float value)
	{
		/*bool oldEnabled = Enabled;
		Enabled = false;
		requiredValue = value;
		Enabled = oldEnabled;*/
	}

	public GameObject createWood ()
	{
		GameObject wood = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		wood.transform.position = new Vector3 (UnityEngine.Random.Range (-10, 10), UnityEngine.Random.Range (2, 10), UnityEngine.Random.Range (-10, 10));
		wood.AddComponent<Rigidbody> ();
		wood.GetComponent<Rigidbody> ().mass = 0.3f;
		wood.tag = "Tree";
		return wood;
	}
}
