using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class ServerAnswerManager{

	enum Type : int {COMPRESSED=1, SHARED_R=2, ID_CHUNK=4, ID_OBJ=8, ID_ASSET=16, ID_THINGIE=32, TEST =225};
	/*
	 * Attributs
	 */
	//Instance unique
	private static ServerAnswerManager instance;

	private static Queue<List<Guid>> cellsIdQueue;
	private static Queue<List<Guid>> assetsIdQueue;
	private static Queue<List<Guid>> objIdQueue;
	private Queue<byte[]> ressourceQueue;
	private bool testResult;

	/*
	 * Méthodes
	 */
	private ServerAnswerManager(){
		cellsIdQueue = new Queue<List<Guid>>();
		assetsIdQueue = new Queue<List<Guid>>();
		objIdQueue = new Queue<List<Guid>>();
		ressourceQueue = new Queue<byte[]> ();
		testResult = false;
	}

	public static ServerAnswerManager getInstance(){

		if (instance == null)
			instance = new ServerAnswerManager ();
		return instance;
	}
	public bool addContents(Stream ste){

		int type, length, i;
		type = ResourceReader.readInt32 (ste);
		length = ResourceReader.readInt32 (ste);
		List<Guid> li = new List<Guid>(length);

		switch (type) {

		case (int)Type.ID_CHUNK:
			for (i = 0; i < (length/38); i++) {
				li.Add (ResourceReader.readGuid (ste));
			}
			cellsIdQueue.Enqueue(li);
			break;
		
	
		case (int)Type.ID_ASSET :
			for(i = 0; i < (length/38); i++){
				li.Add(ResourceReader.readGuid(ste));
			}
			assetsIdQueue.Enqueue(li);
			break;
		
		case (int)Type.ID_OBJ :
			for(i = 0; i < (length/38); i++){
				li.Add(ResourceReader.readGuid(ste));
			}
			objIdQueue.Enqueue(li);
			break;

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
	public List<Guid> getAnswer(int type){

		switch (type) {

		case (int)Type.ID_CHUNK:
			return cellsIdQueue.Dequeue ();
			break;

		case (int)Type.ID_ASSET:
			return assetsIdQueue.Dequeue ();
			break;

		case (int) Type.ID_OBJ:
			return objIdQueue.Dequeue ();
			break;
		default :
			return new List<Guid> ();
		}
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
		return ressourceQueue.Dequeue ();
	}
}
