using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
public class ResourceLoader
{
	public static readonly Guid MAT_GUID=new Guid("3c50697c-543a-0e7f-6ab6-23ae942b73cc");
	public static readonly Guid MESH_GUID=new Guid("4394cb82-98f0-f0ef-1e3a-e4956402ace7");

	private ResourceLoader ()
	{
		chunk = new Queue<byte[]> ();
		entite = new Queue<byte[]> ();
		mesh = new Queue<byte[]> ();
		mat = new Queue<byte[]> ();
		image = new Queue<byte[]> ();
	}

	public static ResourceLoader getInstance(){
		if (loader == null)
			loader = new ResourceLoader ();
		return loader;
	}
	
	private static ResourceLoader loader;
	private Queue<byte[]> chunk;
	private Queue<byte[]> entite;
	private Queue<byte[]> mesh;
	private Queue<byte[]> mat;
	private Queue<byte[]> image;


	public void load(){
		byte[] resource=ServerAnswerManager.getInstance().getAnswer();
		if (resource != null) {
			MemoryStream stream=new MemoryStream (resource);
			Resource res = ResourceReader.readResource(stream);
			byte[] data = new byte[res.dataSize];
			stream.Read(data,0,res.dataSize);
			Debug.Log (res.nom);
			if(res.ID.Equals(MESH_GUID)){
				Debug.Log ("mesh");
				mesh.Enqueue(data);
			}
			else if(res.ID.Equals(MAT_GUID)){
				Debug.Log ("mat");
				mat.Enqueue(data);
			}
		}

	}

}


