using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshCreator
{
	List<Vector3> vertices;
	List<Triangle> triangleListe;
	List<Vector3> normales;
	List<Vector2> textures;
	List<MaterialGroup> matGroup;
	public Guid id;

	public MeshCreator()
	{
		vertices = new List<Vector3> ();
		triangleListe = new List<Triangle> ();
		normales = new List<Vector3> ();
		textures = new List<Vector2> ();
		matGroup = new List<MaterialGroup>();
		id = new Guid ();
	}

	public MeshCreator(Guid id, List<Vector3> vertices, List<Triangle> triangleListe, List<Vector3> normales,List<Vector2> textures, List<MaterialGroup> matGroup)
	{
		this.vertices = vertices;
		this.triangleListe = triangleListe;
		this.normales = normales;
		this.textures = textures;
		this.matGroup = matGroup;
		this.id = id;
	}

	public int indice(List<Vector3> vers,List<Vector3> norms,List<Vector2> text,Vector3 v,Vector3 n,Vector2 t){
		for (int i=0; i<norms.Count; i++) {
			if(vers[i].Equals(v)&&norms[i].Equals(n))//&&text[i].Equals(t))
				return i;
		}
		return -1;
	}

	public int indice(List<Vector3> vers,List<Vector2> text,Vector3 v,Vector2 t){
		//Debug.Log (vers.Count);
		//Debug.Log (text.Count);
		for (int i=0; i<vers.Count; i++) {
			if(vers[i].Equals(v)&&text[i].Equals(t))
				return i;
		}
		return -1;
	}

	public int indice(List<Vector3> vers,List<Vector3> norms,Vector3 v,Vector3 n){
		for (int i=0; i<norms.Count; i++) {
			if(vers[i].Equals(v)&&norms[i].Equals(n))
				return i;
		}
		return -1;
	}

	public int indice(List<Vector3> vers,Vector3 v){
		for (int i=0; i<vers.Count; i++) {
			if(vers[i].Equals(v))
				return i;
		}
		return -1;
	}

	public Asset create(ref Asset asset)
	{

		GameObject obj = asset.go;
		obj.AddComponent<MeshFilter>();
		obj.AddComponent<MeshRenderer>();
		Logger.Debug ("Create Mesh");
		bool has_norm = normales.Count > 0;
		//Debug.Log ("normales du mesh " + has_norm);
		bool has_text = textures.Count > 0;
		List<int> triangles = new List<int>();
		List<Vector3> norms = new List<Vector3>();
		List<Vector3> vers = new List<Vector3>();
		List<Vector2> text = new List<Vector2>();
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;

		int ind=0;
		List<int> tsansmat = new List<int>();
		foreachTriangle(triangleListe,ref ind, has_norm, has_text,ref vers,ref norms,ref text,ref triangles,ref tsansmat);


		List<List<int>> groups = new List<List<int>> ();
		for (int j=0; j<matGroup.Count; j++) {
			List<int> ts = new List<int>();
			MaterialGroup mat=matGroup[j];
			foreachTriangle (mat.triangleListe,ref ind, has_norm, has_text,ref vers,ref norms,ref text,ref triangles,ref ts);
			groups.Add(ts);

		}

		mesh.vertices = vers.ToArray ();
		mesh.triangles = triangles.ToArray();
		if(has_norm)
			mesh.normals=norms.ToArray();
		if(has_text)
			mesh.uv = text.ToArray ();

		Material[] listeMats = new Material[matGroup.Count];

		mesh.subMeshCount = matGroup.Count+1;
		for (int j=0; j<matGroup.Count; j++) {
			mesh.SetTriangles(groups[j].ToArray(),j);

			// Récupération des mats du cache
			Logger.Debug("Chargement Mat");
			listeMats[j] = ResourceLoader.getInstance().getMaterial(matGroup[j].getMatId());

		}
		mesh.SetTriangles(tsansmat.ToArray(),matGroup.Count);
		//mesh.RecalculateBounds();

		//mesh.Optimize();

		mesh.name = id.ToString();
		MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer> ();
		meshRenderer.materials = listeMats;
		mesh.RecalculateNormals ();

		asset.loaded = true;
		return asset;
	}


	public void foreachTriangle(List<Triangle> triangleListe,ref int ind,bool has_norm,bool has_text,ref List<Vector3> vers,ref List<Vector3> norms ,ref List<Vector2> text,ref List<int> triangles,ref List<int> ts){
		TimeSpan all = new TimeSpan(0);
		foreach (Triangle t in triangleListe) 
		{
			for(int i = 0; i < t.getVerticeIndexes().Length; i++)
			{
				int indic=-1;
				//indic=getIndic(has_norm,has_text,vers,norms,text,t,i);
				if(indic==-1){
					if(has_norm&&t.m_hasNormals){
						norms.Add(normales[t.getNormaleIndexeAt(i)]);
					}
					else{
						norms.Add(new Vector3(0f,0f,0f));
					}
					if(has_text&&t.m_hasTexcoords){
						text.Add(textures[t.getTextureIndexeAt(i)]);
					}
					else{
						text.Add(new Vector2(0f,0f));
					}
					vers.Add (vertices[t.getVerticeIndexeAt(i)]);
					indic=ind++;
				}
				
				triangles.Add(indic);
				ts.Add (indic);
				
			}
		}
		//Debug.Log ("temps recherche dans liste triangle: "+all);
	}

	public int getIndic(bool has_norm, bool has_text ,List<Vector3> vers, List<Vector3> norms , List<Vector2> text ,Triangle t,int i) {
		int indic=-1;
		if(has_norm&&has_text){
			indic=indice(vers,norms,text,vertices[t.getVerticeIndexeAt(i)],normales[t.getNormaleIndexeAt(i)],textures[t.getTextureIndexeAt(i)]);
		}
		else if(has_norm){
			indic=indice(vers,norms,vertices[t.getVerticeIndexeAt(i)],normales[t.getNormaleIndexeAt(i)]);
		}
		else if(has_text){
			indic=indice(vers,text,vertices[t.getVerticeIndexeAt(i)],textures[t.getTextureIndexeAt(i)]);
		}
		else{
			indic=indice(vers,vertices[t.getVerticeIndexeAt(i)]);
		}

		return indic;
	}
}
