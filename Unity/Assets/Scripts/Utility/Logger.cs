using UnityEngine;
using System;
using System.IO;

public class Logger
{
public enum Type : int {NONE=0,ERROR=1,WARNING=2,DEBUG=3,TRACE=4};

	private Logger ()
	{
	}

	public static Type logLvl = Type.NONE;
	public static String logFile="";

	private static void Log(object message,Type lvl){


		if (((int)lvl) <= ((int)logLvl)) {
			UnityEngine.Debug.Log (message);
			if(logFile!=null&&!logFile.Equals("")){
			DateTime date= DateTime.Now;
			File.AppendAllText(logFile,date.ToString()+": "+message.ToString()+"\n");
			}
		}
	}

	public static void Trace(object message){
		Log ("TRACE: "+message.ToString(), Type.TRACE);
	}

	public static void Debug(object message){
		Log ("DEBUG: "+message.ToString(), Type.DEBUG);
	}
	
	public static void Warning(object message){
		Log ("WARNING: "+message.ToString(), Type.WARNING);
	}

	public static void Error(object message){
		Log ("ERROR: "+message.ToString(), Type.ERROR);
	}
}


