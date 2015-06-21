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
	GameObject marshmallow;
	GameObject marc;
	public GameObject Chamallow;
	public float threshold = 0.01f;
	public float oldSensorValue = -1.0f;
	public float currentSensorValue = -1.0f;
	public float requiredValue = 0.5f;
	public bool Enabled = true;
	public Effector controlledDistance = new Effector ();
	public FuzzyLogic.Engine engine;
	public FuzzyLogicSolver.Engine solver;

	//Attention exemple de path a remplacer avec le path actuel
	private string MarshMallowDLLPath = @"C:\Users\Stu\Documents\FAC\Master\S2\MIF39\MIF39\MarshmallowDLL\MarshmallowDLL\bin\Debug\MarshmallowDLL.dll";

	private int timer;
	private GameObject wood;

	void Start ()
	{
		Assembly a = Assembly.LoadFile (MarshMallowDLLPath);

		object o = a.CreateInstance ("intelligenceDLL.Controller");

		Type type = a.GetType ("intelligenceDLL.Controller");

		engine = (FuzzyLogic.Engine) type.GetField ("theEngine").GetValue (o);
		solver = (FuzzyLogicSolver.Engine)type.GetField ("theSolver").GetValue (o);

		controlledDistance.Input = engine.Outputs ["Output"];

		GameObject.FindGameObjectWithTag("Fire").AddComponent(a.GetType("intelligenceDLL.Fire"));

		wood = createWood ();
		
		marc = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//marshmallow = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		marshmallow = Instantiate (Chamallow);

		GameObject stick = GameObject.CreatePrimitive (PrimitiveType.Cylinder);

		stick.transform.SetParent (marc.transform);
		stick.transform.position = new Vector3 (-0.209f, 0.393f, 0.76f);
		stick.transform.localScale = new Vector3 (0.1f,0.3f,0.1f);
		stick.transform.Rotate (new Vector3 (90f, 0f, 0f));

		marc.AddComponent<Rigidbody> ();
		
		marshmallow.transform.SetParent (marc.transform);
		
		marc.transform.position = new Vector3 (1, 0, 0);

		marshmallow.transform.GetChild(0).position = new Vector3 (1.664f, 0.634f, 0.773f);
		marshmallow.transform.GetChild(0).localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		marshmallow.transform.GetChild(0).Rotate (new Vector3 (0, 0, 90));

		Type movingHuman = a.GetType ("intelligenceDLL.Effector");

		marc.AddComponent(movingHuman);

		controlledDistance.mInfo = marc.GetComponent (movingHuman).GetType ().GetMethod ("Updated");

		controlledDistance.obj = marc.GetComponent (movingHuman);

		Type cookingSensor = a.GetType ("intelligenceDLL.Sensor");

		marshmallow.AddComponent (cookingSensor);

		Component c = marshmallow.GetComponent ("Sensor");

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

		engine.Inputs ["Input"].Value = newInput;
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
