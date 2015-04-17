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

		int type, length;
		type = ResourceReader.readInt32 (ste);
		Debug.Log ("Type: "+type);
		length = ResourceReader.readInt32 (ste);
		Debug.Log ("Taille: "+length);
		List<Guid> li = new List<Guid>(length);

		switch (type) {
		case (int)Type.SHARED_R : 
			byte[] contenu = new byte[length];
			ste.Read(contenu, 0, length);
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
		Debug.Log ("count: "+ressourceQueue.Count);
		if(ressourceQueue.Count!=0)
			return ressourceQueue.Dequeue();

		return null;
	}
}
