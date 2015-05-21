using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Collections.Generic;

public class ResourceReader
{
	private static ResourceReader reader;

	private ResourceReader(){}

	public static ResourceReader getInstance(){
		if (reader == null)
			reader = new ResourceReader ();
		return reader;
	}


	public  Guid readGuid(Stream stream){
		byte[] guid = new byte[38];
		int nbRead = stream.Read (guid, 0, 38);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream guid");
			stream.Close ();
			return new Guid();
		}

		String gu = System.Text.Encoding.ASCII.GetString (guid);
		Debug.Log ("gu : " + gu);
		return new Guid(gu);
	}

	public  Guid readGuid16(Stream stream){
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

	public  Int64 readInt64(Stream stream){
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

	public  Int32 readInt32(Stream stream){
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

	public  UInt32 readUInt32(Stream stream){
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

	Vector2 readVector2ui (Stream stream)
	{
		return new Vector2 (readUInt32(stream),readUInt32(stream));
	}

	public  float readFloat(Stream stream)
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

	public Vector3 readVector3 (Stream stream)
	{
		return new Vector3 (readFloat (stream), readFloat (stream), readFloat (stream));
	}

	public Quaternion readQuaternion (Stream stream)
	{
		return new Quaternion (readFloat (stream), readFloat (stream), readFloat (stream), readFloat (stream));
	}

	public  String readString(Stream stream, int size){
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

	public  byte[] readByte(Stream stream, int size){
		byte[] nomByte = new byte[size];
		int nbRead = stream.Read (nomByte, 0, size);
		//Debug.Log ("nbRead: " + nbRead+", size: "+size);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream nom");
			stream.Close ();
			return nomByte;
		}
		
		return nomByte;
	}

	public  bool readBoolean(Stream stream)
	{
		byte[] tailleBool = new byte[1];
		int nbRead = stream.Read (tailleBool, 0, 1);
		if (nbRead == 0) 
		{
			//Debug.Log ("end of stream bool");
			stream.Close ();
			return false;
		}
		return BitConverter.ToBoolean (tailleBool, 0);
	}

	public  int[] readInt32Arrray(Stream stream, int size)
	{
		int[] tab = new int[size];
		for (int i = 0; i < size; i++) 
		{
			tab[i] = readInt32(stream);
		}
		return tab;
	}

	public  Triangle readTriangle (Stream stream)
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


	public  Resource readResource (Stream stream)
	{
		Guid ID = readGuid (stream);

		Int64 size = readInt64 (stream);

		String nom = readString(stream,(int) size);

		Int32 dataSize = readInt32(stream);

		return new Resource (ID,nom,dataSize);
	}

	public  MaterialGroup readMaterialGroup(Stream stream)
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

	public  TextureCreator readTexture(Stream stream)
	{

		//Debug.Log ("read texture");
		Guid imageID = readGuid(stream);
		//Debug.Log ("guid image : " + imageID);
		bool blendU = readBoolean (stream);
		//Debug.Log ("blendU image : " + blendU);
		bool blendV = readBoolean (stream);
		//Debug.Log ("blendV image : " + blendV);
		bool CC = readBoolean (stream);
		//Debug.Log ("CC image : " + CC);
		bool Clamp = readBoolean (stream);
		//Debug.Log ("v image : " + Clamp);
		float Base = readFloat (stream);
		//Debug.Log ("Base image : " + Base);
		float Gain = readFloat (stream);
		//Debug.Log ("Gain image : " + Gain);
		float BumpMult = readFloat (stream);
		//Debug.Log ("BumpMult image : " + BumpMult);
		float Boost = readFloat (stream);;
		//Debug.Log ("Boost image : " + Boost);
		Int32 TexRes = readInt32 (stream);
		//Debug.Log ("TexRes image : " + TexRes);
		Vector3 Position = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		//Debug.Log ("Position image : " + Position.x  + " " + Position.y  + " " + Position.z);
		Vector3 Scale = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		//Debug.Log ("Scale image : " + Scale.x  + " " + Scale.y  + " " + Scale.z);
		Vector3 Turbulence = new Vector3(readFloat(stream), readFloat(stream),readFloat(stream));
		//Debug.Log ("Turbulence image : " + Turbulence.x  + " " + Turbulence.y  + " " + Turbulence.z);
		TextureCreator.TextureChannel Channel = (TextureCreator.TextureChannel)readInt32 (stream);
		//return new TextureCreator ();
		return new TextureCreator(imageID, blendU, blendV, CC, Clamp, Base, Gain, BumpMult, Boost, TexRes, Position, Scale, Turbulence, Channel);
	}

	public  ColorRGB readRGB(Stream stream)
	{
		float R = readFloat (stream);
		float G = readFloat (stream);
		float B = readFloat (stream);
		//Debug.Log ("colorRGB : " + R + " ,"+ G + " ," + B);
		return new ColorRGB (R, G, B);
	}

	public  Dissolve readDissolve(Stream stream)
	{
		bool halo = readBoolean(stream);
		float factor = readFloat(stream);
		//Debug.Log ("Dissolve " + halo  + " ," + factor);
		return new Dissolve (halo, factor);
	}
	
	public  MaterialCreator readMaterial(Stream stream)
	{
		//readResource (stream);
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
		//Debug.Log ("color ok");//

		Dissolve Dissolve = readDissolve (stream);
		//Debug.Log ("Dissolve ok");
		float SpecularExponent = readFloat (stream);
		//Debug.Log ("spec ok" + SpecularExponent);
		float Sharpness = readFloat (stream);
		//Debug.Log ("sharp ok" + Sharpness);
		float OpticalDensity = readFloat (stream);
		//Debug.Log ("optical ok" + OpticalDensity);
		TextureCreator AmbientMap = readTexture (stream);
		TextureCreator DiffuseMap = readTexture (stream);
		TextureCreator SpecularMap = readTexture (stream);
		TextureCreator SpecularExponentMap = readTexture (stream);
		TextureCreator DissolveMap = readTexture (stream);
		TextureCreator DecalMap = readTexture (stream);
		TextureCreator DisplacementMap = readTexture (stream);
		TextureCreator BumpMap = readTexture (stream);
		TextureCreator ReflectionMap = readTexture (stream);
		int Illumination = readInt32(stream);
		//Debug.Log ("Illumination : " + Illumination); 
		//return new MaterialCreator (Diffuse, Specular, nom);
		return new MaterialCreator(nom, ID, Ambient, Diffuse, Specular, Emissive, Transmission,Dissolve,SpecularExponent,Sharpness,OpticalDensity,AmbientMap,DiffuseMap,SpecularMap,SpecularExponentMap,DissolveMap,DecalMap,DisplacementMap,BumpMap,ReflectionMap,Illumination);
	}


	public  MeshCreator readMesh(Stream stream)
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
	
	public  ImageCreator readImage(Stream stream)
	{
		Debug.Log ("Lecture image--------------------------------");
		//readResource (stream);

		Guid ID = readGuid (stream);
		Debug.Log ("guid image : " + ID);
		
		Int64 size = readInt64 (stream);
		Debug.Log ("size nom image : " + size);
		
		String nom = readString(stream,(int) size);
		Debug.Log ("nom image : " + nom);

		int width = readInt32 (stream);
		Debug.Log ("width : " + width);
		int height = readInt32 (stream);
		Debug.Log ("height : " + height);
		int depth = readInt32 (stream);
		Debug.Log ("depth : " + depth);
		int nbChannels = readInt32 (stream);
		Debug.Log ("nbChannels : " + nbChannels);
		int sizeData = width * height * depth /8;
		Debug.Log ("sizeData : " + sizeData);
		byte[] data = readByte (stream, sizeData);
		return new ImageCreator(width, height, depth, nbChannels, sizeData, data,ID);
	}

	public  EntityCreator readEntity (Stream stream)
	{
		Guid ID = readGuid (stream);
		Int64 size = readInt64 (stream);
		String nom = readString(stream,(int) size);

		size = readInt64 (stream);
		string realName = readString(stream,(int) size);

		Guid cell = readGuid (stream);
		Vector3 position = readVector3 (stream);
		Quaternion rotation = readQuaternion(stream);
		Vector3 scale = readVector3(stream);

		List<string> semantics =new List<string>();
		Int32 nbString = readInt32 (stream);
		for (int i = 0; i < nbString; i++) 
		{
			size = readInt64(stream);
			semantics.Add(readString(stream,(int) size));
		}
		Guid meshId = readGuid(stream);
		return new EntityCreator (ID, realName, cell, position, rotation, scale, semantics, meshId);
	}

	public GameEntityCreator readGameEntity (MemoryStream stream)
	{
		Debug.Log ("readGameEntity");

		Guid ID = readGuid (stream);
		Int64 size = readInt64 (stream);
		String nom = readString(stream,(int) size);


		size = readInt64 (stream);
		String realName = readString(stream,(int) size);
		Debug.Log ("realName : " +realName);

		List<string> semantics =new List<string>();

		//readInt32 (stream);
		Int32 nbString = readInt32 (stream);
		Debug.Log ("nbString : " +nbString);


		for (int i = 0; i < nbString; i++) 
		{
			size = readInt64(stream);
			Debug.Log ("size : " +size);
			String temp = readString(stream,(int) size);
			Debug.Log ("nom semantics : " + temp); 
			semantics.Add(temp);
		}


		List<GameEntityElement> elements =new List<GameEntityElement>();
		Int32 nbGuid = readInt32 (stream);
		Debug.Log ("nbGuid : " + nbGuid); 

		for (int i = 0; i < nbGuid; i++) 
		{
			elements.Add(readGameEntityElement(stream));
		}

		return new GameEntityCreator (ID, realName, semantics, elements);
	}

	GameEntityElement readGameEntityElement (MemoryStream stream)
	{
		Debug.Log ("readGameEntityElement");

		Int64 taille = readInt64(stream);
		String nom = readString(stream,(int)taille);
		Debug.Log("nom entityElement : " + nom);
			
		Guid id = readGuid (stream);
		Vector3 position = readVector3 (stream);
		Quaternion rotation = readQuaternion (stream);
		Vector3 scale = readVector3 (stream);

		List<string> semantics =new List<string>();
		Int32 nbString = readInt32 (stream);
		for (int i = 0; i < nbString; i++) 
		{
			Int64 size = readInt64(stream);
			semantics.Add(readString(stream,(int) size));
		}
		List<Guid> ressources =new List<Guid>();
		Int32 nbGuid = readInt32 (stream);
		for (int i = 0; i < nbGuid; i++) 
		{
			ressources.Add(readGuid(stream));
		}

		return new GameEntityElement (nom, id, position, rotation, scale, semantics, ressources);
	}

	public  ChunkCreator readChunk (Stream stream)
	{
		Guid ID = readGuid (stream);
		Int64 size = readInt64 (stream);
		String nom = readString(stream,(int) size);

		size = readInt64 (stream);
		string realName = readString(stream,(int) size);

		Guid world = readGuid (stream);
		Vector2 indice = readVector2ui (stream);
		Vector3 extents = readVector3 (stream);
		Vector3 position = readVector3 (stream);

		Int32 nbGo = readInt32 (stream);
		List<Guid> objects =new List<Guid>();
		for (int i = 0; i < nbGo; i++) 
		{
			objects.Add(readGuid(stream));
		}

		return new ChunkCreator (ID,realName,world,indice,extents,position,objects);
	}

	public WorldCreator readWorld (MemoryStream stream)
	{
		Guid ID = readGuid (stream);
		Int64 size = readInt64 (stream);
		String nom = readString(stream,(int) size);

		size = readInt64 (stream);
		string realName = readString(stream,(int) size);

		Vector3 extents = readVector3 (stream);
		Vector2 subdivision = readVector2ui (stream);

		Int32 nbSpawnPoint = readInt32 (stream);
		List<SpawnPoint> spawnPoints =new List<SpawnPoint>();
		for (int i = 0; i < nbSpawnPoint; i++) 
		{
			spawnPoints.Add(readSpawnPoint(stream));
		}

		List<string> semantics =new List<string>();
		Int32 nbString = readInt32 (stream);
		for (int i = 0; i < nbString; i++) 
		{
			Int64 si = readInt64(stream);
			semantics.Add(readString(stream,(int) si));
		}

		List<Guid> cells =new List<Guid>();
		Int32 nbGuid = readInt32 (stream);
		for (int i = 0; i < nbGuid; i++) 
		{
			cells.Add(readGuid(stream));
		}


		return new WorldCreator (ID,realName,extents,subdivision,spawnPoints,semantics,cells);
	}

	public SpawnPoint readSpawnPoint (MemoryStream stream)
	{
		Int64 s = readInt64 (stream);
		string realName = readString(stream,(int) s);
		s = readInt64 (stream);
		realName = readString(stream,(int) s);
		//Debug.Log (realName);
		Guid worldId = readGuid (stream);
		Vector3 location = readVector3 (stream);
		float size = readFloat(stream);

		List<string> semantics =new List<string>();
		Int32 nbString = readInt32 (stream);
		for (int i = 0; i < nbString; i++) 
		{
			Int64 si = readInt64(stream);
			semantics.Add(readString(stream,(int) si));
		}

		return new SpawnPoint (realName, worldId, location, size, semantics);
	}

	public LightCreator readLight(Stream stream)
	{
		Guid ID = readGuid (stream);
		Int64 size = readInt64 (stream);
		string realName = readString(stream, (int) size);
		LightCreator.Type type = (LightCreator.Type)readInt32 (stream);
		float intensity = readFloat(stream);
		Vector3 color = readVector3(stream);

		return new LightCreator (ID, realName, intensity, color, type);
	}
}
