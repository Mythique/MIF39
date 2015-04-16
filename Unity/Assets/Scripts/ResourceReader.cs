using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class ResourceReader
{
	static int cpt = 0;

	public static Guid readGuid(Stream stream){
		byte[] guid = new byte[38];
		int nbRead = stream.Read (guid, 0, 38);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream guid");
			stream.Close ();
			return new Guid();
		}
		String gu = System.Text.Encoding.ASCII.GetString (guid);
		return new Guid(gu);
	}

	public static Guid readGuid16(Stream stream){
		byte[] guid = new byte[16];
		int nbRead = stream.Read (guid, 0, 16);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream guid");
			stream.Close ();
			return new Guid();
		}

		return new Guid(guid);
	}

	public static Int64 readInt64(Stream stream){
		byte[] tailleNom = new byte[8];
		int nbRead = stream.Read (tailleNom, 0, 8);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream int64");
			stream.Close ();
			return -1;
		}
		return BitConverter.ToInt64 (tailleNom, 0);
	}

	public static Int32 readInt32(Stream stream){
		byte[] tailleNom = new byte[4];
		int nbRead = stream.Read (tailleNom, 0, 4);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream int32");
			stream.Close ();
			return -1;
		}
		return BitConverter.ToInt32 (tailleNom, 0);
	}

	public static UInt32 readUInt32(Stream stream){
		byte[] tailleNom = new byte[4];
		int nbRead = stream.Read (tailleNom, 0, 4);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream int32");
			stream.Close ();
			return 0;
		}
		return BitConverter.ToUInt32 (tailleNom, 0);
	}

	public static float readFloat(Stream stream)
	{
		byte[] taillefloat = new byte[4];
		int nbRead = stream.Read (taillefloat, 0, 4);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream float");
			stream.Close ();
			return -1;
		}
		return BitConverter.ToSingle (taillefloat, 0);
	}

	public static String readString(Stream stream, int size){
		byte[] nomByte = new byte[size];
		int nbRead = stream.Read (nomByte, 0, size);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream nom");
			stream.Close ();
			return "";
		}
		
		return System.Text.Encoding.ASCII.GetString (nomByte);
	}

	public static bool readBoolean(Stream stream)
	{
		byte[] tailleBool = new byte[2];
		int nbRead = stream.Read (tailleBool, 0, 2);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream bool");
			stream.Close ();
			return false;
		}
		return BitConverter.ToBoolean (tailleBool, 0);
	}

	public static int[] readInt32Arrray(Stream stream, int size)
	{
		int[] tab = new int[size];
		for (int i = 0; i < size; i++) 
		{
			tab[i] = readInt32(stream);
		}
		return tab;
	}

	public static Triangle readTriangle (Stream stream)
	{

		bool m_hasNormals = readBoolean(stream);
		/*if (cpt <= 10) 
		{
			Debug.Log ("m_hasNormals : " + m_hasNormals);
		}*/


		bool m_hasTexcoords = readBoolean(stream);
		/*if (cpt <= 10) 
		{
			Debug.Log ("m_hasTexcoords : " + m_hasTexcoords);
			cpt++;
		}*/


		int[] m_vertexIndices = readInt32Arrray(stream, 3);
		int[] m_normalIndices = readInt32Arrray(stream, 3);
		int[] m_texcoordIndices = readInt32Arrray(stream, 3);

		return new Triangle (m_vertexIndices, m_normalIndices, m_texcoordIndices, m_hasNormals, m_hasTexcoords);
	}

	public static void readStream (Stream stream)
	{
		Guid ID = readGuid (stream);
		//Debug.Log ("guid : " + ID);

		Int64 size = readInt64 (stream);
		//Debug.Log ("size nom : " + size);

		String nom = readString(stream,(int) size);
		//Debug.Log ("nom : " + nom);

		Int32 dataSize = readInt32(stream);
		//Debug.Log ("data size : " + dataSize);
	}

	public static MaterialGroup readMaterialGroup(Stream stream)
	{
		Int64 size = readInt64 (stream);
		//Debug.Log ("size nom material group: " + size);
		
		String nom = readString(stream,(int) size);
		//Debug.Log ("nom material group: " + nom);

		Guid matId = readGuid (stream);
		Guid meshId = readGuid (stream);
		UInt32 nbFace = readUInt32(stream);
		//Debug.Log ("nbFace materialgroup : " + nbFace);

		List<Triangle> triangleListe = new List<Triangle> ();

		for (int i = 0; i < nbFace; i++) 
		{
			triangleListe.Add(readTriangle(stream));
		}

		return new MaterialGroup (matId, meshId, nbFace, triangleListe, nom);
	}

	public static TextureCreator readTexture(Stream stream)
	{
		Guid imageID = readGuid16(stream);
		Debug.Log ("guid image : " + imageID);
		bool blendU = readBoolean (stream);
		bool blendV = readBoolean (stream);
		bool CC = readBoolean (stream);
		bool Clamp = readBoolean (stream);
		float Base = readFloat (stream); 
		float Gain = readFloat (stream);
		float BumpMult = readFloat (stream);
		float Boost = readFloat (stream);;
		Int32 TexRes = readInt32 (stream);
		Vector3 Position = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		Vector3 Scale = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		Vector3 Turbulence = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		TextureCreator.TextureChannel Channel = (TextureCreator.TextureChannel)readInt32 (stream);
		return new TextureCreator ();
		//return new TextureCreator(imageID, blendU, blendV, CC, Clamp, Base, Gain, BumpMult, Boost, TexRes, Position, Scale, Turbulence, Channel);
	}

	public static ColorRGB readRGB(Stream stream)
	{
		float R = readFloat (stream);
		float G = readFloat (stream);
		float B = readFloat (stream);
		//Debug.Log ("colorRGB : " + R + " ,"+ G + " ," + B);
		return new ColorRGB (R, G, B);
	}

	public static Dissolve readDissolve(Stream stream)
	{
		bool halo = readBoolean (stream);
		float factor = readFloat(stream);
		//Debug.Log ("Dissolve" + halo  + " ," + factor);
		return new Dissolve (halo, factor);
	}
	
	public static MaterialCreator readMaterial(Stream stream)
	{
		readStream (stream);
		//Debug.Log ("matérial créator");
		Guid ID = readGuid (stream);
		//Debug.Log ("guid material : " + ID);

		Int64 size = readInt64 (stream);
		//Debug.Log ("size nom material : " + size);
		
		String nom = readString(stream,(int) size);
		//Debug.Log ("nom material : " + nom);

		ColorRGB Ambient = readRGB (stream);
		ColorRGB Diffuse = readRGB (stream);
		//Debug.Log ("specular");
		ColorRGB Specular = readRGB (stream);
		ColorRGB Emissive = readRGB (stream);
		ColorRGB Transmission = readRGB (stream);
		//Debug.Log ("color ok");

		Dissolve Dissolve = readDissolve (stream);
		//Debug.Log ("Dissolve ok");

		float SpecularExponent = readFloat (stream);
		//Debug.Log ("spec ok" + SpecularExponent);
		float Sharpness = readFloat (stream);
		//Debug.Log ("sharp ok" + Sharpness);
		float OpticalDensity = readFloat (stream);
		//Debug.Log ("optical ok" + OpticalDensity);
		//TextureCreator AmbientMa = readTexture (stream);
		//Debug.Log ("AmbientMa");
		//Texture AmbientMap = AmbientMa.create ();
		//TextureCreator DiffuseMa = readTexture (stream);
		//Debug.Log ("diffuseMa");
		//Texture DiffuseMap = DiffuseMa.create ();
		//TextureCreator SpecularMa= readTexture (stream);
		//Texture SpecularMap = SpecularMa.create ();
		//TextureCreator SpecularExponentMa = readTexture (stream);
		//Texture SpecularExponentMap = SpecularExponentMa.create ();
		//TextureCreator DissolveMa = readTexture (stream);
		//Texture DissolveMap = DissolveMa.create ();
		//TextureCreator DecalMa = readTexture (stream);
		//Texture DecalMap = DecalMa.create ();
		//TextureCreator DisplacementMa = readTexture (stream);
		//Texture DisplacementMap = DisplacementMa.create ();
		//TextureCreator BumpMa = readTexture (stream);
		//Texture BumpMap = BumpMa.create ();
		//TextureCreator ReflectionMa = readTexture (stream);
		//Texture ReflectionMap = ReflectionMa.create ();
		//int Illumination = readInt32(stream);
		//Debug.Log ("Illumination : " + Illumination); 
		return new MaterialCreator (Diffuse, Specular, nom);
		//return new MaterialCreator(ID, Ambient, Diffuse, Specular, Emissive, Transmission,Dissolve,SpecularExponent,Sharpness,OpticalDensity,AmbientMap,DiffuseMap,SpecularMap,SpecularExponentMap,DissolveMap,DecalMap,DisplacementMap,BumpMap,ReflectionMap,Illumination);
	}


	public static MeshCreator readMesh(Stream stream)
	{
		List<Vector3> vertices = new List<Vector3> ();
		List<Triangle> triangleListe = new List<Triangle>();
		List<Vector3> normales = new List<Vector3>();
		List<Vector2> textures = new List<Vector2>();
		List<MaterialGroup> mg = new List<MaterialGroup> ();
		float x;
		float y;
		float z;

		Guid ID = readGuid (stream);
		//Debug.Log ("guid mesh : " + ID);
		
		Int64 size = readInt64 (stream);
		//Debug.Log ("size nom mesh : " + size);
		
		String nom = readString(stream,(int) size);
		//Debug.Log ("nom : mesh " + nom);

		Int32 nbVertices = readInt32(stream);
		//Debug.Log ("nb vertice : " + nbVertices);

		//lecture des points + stockage dans liste
		for (int i = 0; i < nbVertices; i++) 
		{
			x = readFloat(stream);
			y = readFloat(stream);
			z = readFloat(stream);
			vertices.Add(new Vector3(x,y,z));
		}

		Int32 nbTexVertices = readInt32(stream);
		//Debug.Log ("nb tex vertice : " + nbTexVertices);

		//lecture des textures + stockage dans liste
		for (int i = 0; i < nbTexVertices; i++)
		{
			x = readFloat(stream);
			y = readFloat(stream);
			textures.Add(new Vector2(x,y));
		}

		Int32 nbNormals = readInt32(stream);
		//Debug.Log ("nb normale : " + nbNormals);

		//lecture des normales + stockage dans liste
		for (int i = 0; i < nbNormals; i++) 
		{
			x = readFloat(stream);
			y = readFloat(stream);
			z = readFloat(stream);
			normales.Add(new Vector3(x,y,z));
		}

		Int32 nbFaces = readInt32(stream);
		//Debug.Log ("nb face : " + nbFaces);

		//lecture des triangles + stockage dans liste
		for (int i = 0; i < nbFaces; i++) 
		{
			triangleListe.Add(readTriangle(stream));
		}


		Int32 nbMaterialGroup = readInt32 (stream);
		//Debug.Log ("nb material groups : " + nbMaterialGroup);

		for (int i = 0; i < nbMaterialGroup; i++) 
		{
			mg.Add(readMaterialGroup (stream));
		}
		//Debug.Log (triangleListe.Count);
		return new MeshCreator(ID, vertices, triangleListe, normales, textures, mg);
	}
}
