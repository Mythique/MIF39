using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class ServerAnswerManager{

	public enum Type : int {COMPRESSED=1, SHARED_R=2, EMPTY=8};
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

		//Logger.Debug ("Add Contents");
		int type, length;
		type = ResourceReader.getInstance().readInt32 (ste);
		//Debug.Log ("Type: "+type);
		length = ResourceReader.getInstance().readInt32 (ste);
		//Debug.Log ("Taille: "+length);

		byte[] contenu =new byte[1];
		if (length != 0) {
			contenu = new byte[length];
			int sommeRead = 0;
			do {
					int nbRead = ste.Read (contenu, sommeRead, length - sommeRead);
					sommeRead += nbRead;
			} while(sommeRead!=length);

		}

		switch (type) {
		case (int)Type.SHARED_R : 
			ressourceQueue.Enqueue(contenu);
			break;

		case (int) Type.EMPTY :
			Debug.Log("empty");
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
