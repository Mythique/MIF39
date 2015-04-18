using UnityEngine;
using System.Collections;

public class TestLogger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Logger.logLvl = Logger.Type.DEBUG;
		Logger.logFile = "C:/Users/lucas/Desktop/log.txt";
		Logger.Debug("test debug");
		Logger.Error("test error");
		Logger.Trace("test trace");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
