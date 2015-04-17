using UnityEngine;
using System;

	public class Logger
	{
	public enum Type : int {NONE=0,ERROR=1,WARNING=2,DEBUG=3,TRACE=4};

		private Logger ()
		{
		}

		public static Type logLvl = Type.NONE;

		public static void Log(object message,Type lvl){

			if(((int)lvl)<=((int)logLvl))
				Debug.Log(message);
		}
	}


