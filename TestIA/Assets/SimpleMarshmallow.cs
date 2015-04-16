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

[Serializable]
public class Sensor
{
	private GameObject marshmallow;

	public Sensor (GameObject marshmallow)
	{
		this.marshmallow = marshmallow;	
	}

	public double GetOutput ()
	{
		//GameObject marshmallow = GameObject.FindGameObjectWithTag ("Marshmallow");
		GameObject fire = GameObject.FindGameObjectWithTag ("Fire");
		Fire f = fire.GetComponent<Fire> ();
		if (f == null)
			return -1;

		double d = Vector3.Distance (marshmallow.transform.position, fire.transform.position);
		if (d == 0)
			return -1;
		return f.intensity / d;
	}
}

[Serializable]
public class Effector
{
	public OutputVariable Input { get; set; }

	//public MovingHuman human;

	public void Update ()
	{
		double value = Input.Value;
		if (!double.IsNaN (value)) {
			float v = (float)value;
			//human.UpdateDistance(v);
			OnValueChanged.Invoke(v);
			//[AJOUT]Ici indiquer au server que ma position a change 
		}
	}

	public OutputEvent OnValueChanged;
	[Serializable]
	public class OutputEvent : UnityEvent<float>
	{
	}
}

public class SimpleMarshmallowController
{
	public FuzzyLogic.Engine theEngine;
	public FuzzyLogicSolver.Engine theSolver;

	public static bool amCreated;
	private static SimpleMarshmallowController instance;

	//
	void createEngineDescription ()
	{
		theEngine = new FuzzyLogic.Engine ();

		theEngine.Create <FuzzyLogic.InputVariable>
			(
				"Name", "FeltIntensity",
				"Minimum", 0,
				"Maximum", 1,
				"Terms", theEngine.Create <FuzzyLogic.Curve>
				(
				"Name", "Cold",
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 0, "Value", 1, "InTangent", 0, "OutTangent", 0),
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 1, "Value", 0, "InTangent", 0, "OutTangent", 0)
		),
				"Terms", theEngine.Create <FuzzyLogic.Curve>
				(
				"Name", "Hot",
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 0, "Value", 0, "InTangent", 0, "OutTangent", 0),
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 1, "Value", 1, "InTangent", 0, "OutTangent", 0)
		)
		);
		
		theEngine.Create <FuzzyLogic.OutputVariable>
			(
				"Name", "Distance",
				"Minimum", -1,
				"Maximum", 1,
				"Terms", theEngine.Create<FuzzyLogic.Curve>
				(
				"Name", "Closer",
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", -1, "Value", 1, "InTangent", 0, "OutTangent", 0),
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 1, "Value", 0, "InTangent", 0, "OutTangent", 0)
		),
				"Terms", theEngine.Create<FuzzyLogic.Curve>
				(
				"Name", "Further",
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", -1, "Value", 0, "InTangent", 0, "OutTangent", 0),
				"Keyframes", theEngine.Create <FuzzyLogic.Keyframe> ("Time", 1, "Value", 1, "InTangent", 0, "OutTangent", 0)
		)
		);
		
		theEngine.Create<FuzzyLogic.RuleBlock>
			(
				"Name", "SimpleCooking",
				"Conclusions", theEngine.Create <FuzzyLogic.Rule>
				(
				"Antecedent", theEngine.Create <FuzzyLogic.Antecedent>
				(
				"Expression", theEngine.Create <FuzzyLogic.Proposition>
				(
				"Variable", theEngine.Inputs ["FeltIntensity"],
				"Term", theEngine.Inputs ["FeltIntensity"] ["Cold"]
		)
		),
				"Consequent", theEngine.Create <FuzzyLogic.Consequent>
				(
				"Conclusions", theEngine.Create <FuzzyLogic.Proposition>
				(
				"Variable", theEngine.Outputs ["Distance"],
				"Term", theEngine.Outputs ["Distance"] ["Closer"]
		)
		),
				"Weight", 1
		),
				"Conclusions", theEngine.Create <FuzzyLogic.Rule>
				(
				"Antecedent", theEngine.Create <FuzzyLogic.Antecedent>
				(
				"Expression", theEngine.Create <FuzzyLogic.Proposition>
				(
				"Variable", theEngine.Inputs ["FeltIntensity"],
				"Term", theEngine.Inputs ["FeltIntensity"] ["Hot"]
		)
		),
				"Consequent", theEngine.Create <FuzzyLogic.Consequent>
				(
				"Conclusions", theEngine.Create <FuzzyLogic.Proposition>
				(
				"Variable", theEngine.Outputs ["Distance"],
				"Term", theEngine.Outputs ["Distance"] ["Further"]
		)
		),
				"Weight", 1
		)
		);
	}

	private SimpleMarshmallowController ()
	{
		FuzzyLogicSolver.SNormFactory snormFactory = FuzzyLogicSolver.SNormFactory.Get ();
		FuzzyLogicSolver.TNormFactory tnormFactory = FuzzyLogicSolver.TNormFactory.Get ();
		FuzzyLogicSolver.DefuzzyfierFactory defuzzifierFactory = FuzzyLogicSolver.DefuzzyfierFactory.Get ();
		createEngineDescription ();
		theSolver = FuzzyLogicSolver.Converter.Convert (theEngine);
		theSolver.Configure (
					null,
					null,
					tnormFactory.Create ("Minimum"),
					snormFactory.Create ("Maximum"),
					defuzzifierFactory.Create ("Centroid", "5")
		);

		amCreated = true;
	}

	public void Process ()
	{
		theSolver.Process ();
	}

	public static SimpleMarshmallowController getInstance(){
		if (amCreated) {
			return instance;
		} else {
			return instance = new SimpleMarshmallowController();
		}
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
	public Sensor controllerSensor;
	public Effector controlledDistance = new Effector();
	public SimpleMarshmallowController controller;

	private int timer;
	private GameObject wood;

	void Start ()
	{
		controller = SimpleMarshmallowController.getInstance ();//[AJOUT]Demander au serveur la totalité de mes infos, les objets qui me composent etc engros tout ce qui suit
		/// D'ici
		controlledDistance.Input = controller.theEngine.Outputs ["Distance"];

		wood = createWood ();

		marc = GameObject.CreatePrimitive (PrimitiveType.Cube);
		marshmallow = GameObject.CreatePrimitive (PrimitiveType.Sphere);

		marc.AddComponent<Rigidbody> ();

		marshmallow.transform.SetParent (marc.transform);

		marc.transform.position = new Vector3 (1, 0, 0);
		marshmallow.transform.position = new Vector3 (-0.026f, 0.063f, 0.711f);
		marshmallow.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

		marc.AddComponent<MovingHuman>();
		marshmallow.AddComponent<CookingSensor> ();
		CookingSensor s = marshmallow.GetComponent<CookingSensor> ();
		s.controllerToCall = this;
	
		controlledDistance.OnValueChanged.AddListener (marc.GetComponent<MovingHuman> ().UpdateDistance);
		//controlledDistance.human = marc.GetComponent<MovingHuman> ();
		/// A ici
	}

	void FixedUpdate ()
	{
		if (Enabled) {
			controller.Process ();
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
		controller.theEngine.Inputs ["FeltIntensity"].Value = v - requiredValue;
		//controller.theEngine.Inputs ["lol ca marche pas normalement"].Value = v - requiredValue;
		oldSensorValue = v;
	}

	public void requiredIntensityChanged (float value)
	{
		bool oldEnabled = Enabled;
		Enabled = false;
		requiredValue = value;
		Enabled = oldEnabled;
	}

	public GameObject createWood(){
		GameObject wood = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		wood.transform.position = new Vector3 (UnityEngine.Random.Range (-10, 10), UnityEngine.Random.Range (2, 10), UnityEngine.Random.Range (-10, 10));
		wood.AddComponent<Rigidbody> ();
		wood.GetComponent<Rigidbody> ().mass = 0.3f;
		wood.tag = "Tree";
		return wood;
	}
}
