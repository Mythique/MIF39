using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.IO;

public class TestMesh : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GameObject obj = new GameObject("obj");

		//DateTime ouverture = DateTime.Now;
		FileStream stream1 = File.Open(@"C:\Users\Tsubaki\Desktop\newSphereMesh.bin", FileMode.Open);
		//DateTime finOuvertue = DateTime.Now;
		//Debug.Log ("temps ouverture fichier :" + (finOuvertue- ouverture));
	
		//DateTime  readstreamdebut = DateTime.Now;
		ResourceReader.readResource (stream1);
		//DateTime  readstreamfin = DateTime.Now;
		//Debug.Log ("temps readstream :" + (readstreamdebut- readstreamfin));

		//DateTime  readmeshdebut = DateTime.Now;
		MeshCreator mc=ResourceReader.readMesh (stream1);
		//DateTime  readmeshfin = DateTime.Now;
		//Debug.Log ("temps readmesh :" + (readmeshdebut- readmeshfin));

		DateTime  createmeshdebut = DateTime.Now;
		mc.createMesh (obj);

		FileStream stream2 = File.Open(@"C:\Users\Tsubaki\Desktop\newSphereMat.bin", FileMode.Open);
		MaterialCreator matCret = ResourceReader.readMaterial (stream2);
		matCret.create (obj);

		FileStream stream3 = File.Open(@"C:\Users\Tsubaki\Desktop\newSphereImage.bin", FileMode.Open);
		ImageCreator imageCreat = ResourceReader.readImage (stream3);
		imageCreat.create (obj);		



		//reverse(obj)
		DateTime  createmeshfin = DateTime.Now;
		//Debug.Log ("temps create mesh :" + (createmeshdebut- createmeshfin));

		stream1.Close();
		//stream2.Close ();
		//stream3.Close ();
		//Client.Connect("192.168.1.116", 3000, "coucou hiboux");

	}
	 
	// Update is called once per frame
	void Update () {
		
	}
}
