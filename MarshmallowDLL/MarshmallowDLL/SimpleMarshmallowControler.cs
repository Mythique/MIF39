using UnityEngine;

namespace MarshmallowDLL
{
	public class SimpleMarshmallowController
	{
		public FuzzyLogic.Engine theEngine;
		public FuzzyLogicSolver.Engine theSolver;
		public static bool amCreated;
	
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
	
		public SimpleMarshmallowController ()
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
		
			Debug.Log ("Created SimpleMarshmallowControler");
		}
	
		public void Process ()
		{
			theSolver.Process ();
		}
	}
}