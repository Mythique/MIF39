using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class ServerAnswerManager{

	public enum Type : int {COMPRESSED=1, SHARED_R=2, ID_CHUNK=4, ID_OBJ=8, ID_ASSET=16, ID_THINGIE=32, TEST =225};
	/*
	 * Attributs
	 */
	//Instance unique
	private static ServerAnswerManager instance;

	private Queue<byte[]> ressourceQueue;
	private bool testResult;

	/*
	 * Méthodes
	 */
	private ServerAnswerManager(){
		ressourceQueue = new Queue<byte[]> ();
		testResult = false;
	}

	public static ServerAnswerManager getInstance(){

		if (instance == null)
			instance = new ServerAnswerManager ();
		return instance;
	}

	public bool addContents(Stream ste){

		Logger.Debug ("Add Contents");
		int type, length;
		type = ResourceReader.getInstance().readInt32 (ste);
		Debug.Log ("Type: "+type);
		length = ResourceReader.getInstance().readInt32 (ste);
		Debug.Log ("Taille: "+length);

		switch (type) {
		case (int)Type.SHARED_R : 
			byte[] contenu = new byte[length];
			int sommeRead=0;
			do{
				int nbRead=ste.Read(contenu, sommeRead, length-sommeRead);
				sommeRead+=nbRead;
			}
			while(sommeRead!=length);

			Debug.Log("------------------------------------------- :"+sommeRead+" "+length);
			ressourceQueue.Enqueue(contenu);
			break;

		case (int) Type.TEST :
			testResult = true;
			break;
		default :
				return false;
				break;

		}
		return true;
	}

	public bool getAnswerTest(){
		if (testResult) {
			testResult = false;
			return true;
		} else {
			return false;
		}

	}

	public byte[] getAnswer(){
		Logger.Trace ("Server Answer Manager/ nombre de ressource non lu: "+ressourceQueue.Count);
		if(ressourceQueue.Count!=0)
			return ressourceQueue.Dequeue();

		return null;
	}
}
