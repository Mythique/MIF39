using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using System.IO;

public class TestMesh : MonoBehaviour {

	// Use this for initialization
	public GameObject obj;
	void Start () {
		/*List<Vector3> vertice = new List<Vector3> ();
		vertice.Add(new Vector3(0f,0f,0f));
		vertice.Add(new Vector3(1f,0f,0f));
		vertice.Add(new Vector3(0f,1f,0f));
		vertice.Add(new Vector3(1f,1f,0f));

		List<Vector3> norms = new List<Vector3> ();
		norms.Add(new Vector3(0f,0f,1f));

		List<Vector2> textures = new List<Vector2> ();
		textures.Add(new Vector2(0f,0f));

		int[] v = {0,1,2};
		int[] vn = {0,0,0};
		Triangle t=new Triangle(v,vn,vn);

		int[] v2 = {1,3,2};
		int[] vn2 = {0,0,0};
		Triangle t2=new Triangle(v2,vn2,vn2);

		List<Triangle> triangles = new List<Triangle> ();
		triangles.Add(t);
		triangles.Add(t2);

		MeshCreator crea = new MeshCreator (vertice, triangles, norms, textures);
		Mesh me=crea.createMesh();*/

		/*Mesh m = new Mesh ();
		List<Vector3> norms = new List<Vector3>();
		List<Vector3> vers = new List<Vector3>();
		List<Vector2> text = new List<Vector2>();

		List<Vector3> vertice = new List<Vector3> ();
		vertice.Add(new Vector3(0f,0f,0f));
		vertice.Add(new Vector3(1f,0f,0f));
		vertice.Add(new Vector3(0f,1f,0f));
		vertice.Add(new Vector3(1f,1f,0f));

		norms.Add(new Vector3(0f,0f,1f));
		norms.Add(new Vector3(0f,0f,1f));
		norms.Add(new Vector3(0f,0f,1f));
		norms.Add(new Vector3(0f,0f,1f));

		text.Add (new Vector2 (0f, 0f));
		text.Add (new Vector2 (1f, 0f));
		text.Add (new Vector2 (0f, 1f));
		text.Add (new Vector2 (1f, 1f));

		m.vertices = vertice.ToArray ();
		m.normals = norms.ToArray();
		m.uv = text.ToArray ();

		int[] v = {0,1,2};
		int[] v2 = {1,3,2};

		int[] triangles = {0,1,2,1,3,2};
		m.triangles = triangles;
		m.subMeshCount = 2;
		m.SetTriangles (v, 0);
		m.SetTriangles (v2, 1);

		obj.AddComponent<MeshFilter> ().mesh = m;
		obj.GetComponent<MeshRenderer> ().enabled = true;*/
		//obj.GetComponent<Renderer> ().material.color = Color.white;




		byte[] b = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15};
		Guid guid = new Guid (b);
		Int32 sizeName = 6;
		String name = "coucou";
		Int32 sizeData = 42;

		Stream str = new MemoryStream();
		byte[] bytes = System.Text.Encoding.ASCII.GetBytes ("{"+guid.ToString()+"}");
		str.Write (bytes, 0, 38);
		str.Write (BitConverter.GetBytes (sizeName), 0, 4);
		bytes = System.Text.Encoding.ASCII.GetBytes (name);
		str.Write (bytes, 0, sizeName);
		str.Write (BitConverter.GetBytes (sizeData), 0, 4);    
		str.Flush ();
		str.Seek(0, SeekOrigin.Begin);

		//DateTime ouverture = DateTime.Now;
		FileStream stream1 = File.Open(@"C:\Users\Tsubaki\Desktop\textMesh.bin", FileMode.Open);
		//DateTime finOuvertue = DateTime.Now;
		//Debug.Log ("temps ouverture fichier :" + (finOuvertue- ouverture));
	
		//DateTime  readstreamdebut = DateTime.Now;
		ResourceReader.readStream (stream1);
		//DateTime  readstreamfin = DateTime.Now;
		//Debug.Log ("temps readstream :" + (readstreamdebut- readstreamfin));

		//DateTime  readmeshdebut = DateTime.Now;
		MeshCreator mc=ResourceReader.readMesh (stream1);
		//DateTime  readmeshfin = DateTime.Now;
		//Debug.Log ("temps readmesh :" + (readmeshdebut- readmeshfin));

		DateTime  createmeshdebut = DateTime.Now;

		FileStream stream2 = File.Open(@"C:\Users\Tsubaki\Desktop\textMat.bin", FileMode.Open);
		MaterialCreator matCret = ResourceReader.readMaterial (stream2);
		matCret.create (obj);

		//FileStream stream3 = File.Open(@"C:\Users\Tsubaki\Desktop\textImage.bin", FileMode.Open);
		//TextureCreator TextureCreat = ResourceReader.readTexture (stream3);
		//matCret2.create (obj);		

		mc.createMesh (obj);

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
