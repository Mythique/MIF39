using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
public class ResourceLoader
{
	public static readonly Guid MAT_GUID=new Guid("3c50697c-543a-0e7f-6ab6-23ae942b73cc");
	public static readonly Guid MESH_GUID=new Guid("4394cb82-98f0-f0ef-1e3a-e4956402ace7");
	public static readonly Guid ENTITY_GUID=new Guid();
	public static readonly Guid CHUNK_GUID=new Guid();
	public static readonly Guid IMAGE_GUID=new Guid();

	private static ResourceLoader loader;
	private Queue<byte[]> chunk;
	private Queue<byte[]> entite;
	private Queue<byte[]> mesh;
	private Queue<byte[]> mat;
	private Queue<byte[]> image;

	private Dictionary<Guid,MeshStruct> meshes;
	private Dictionary<Guid,Chunk> chunks;
	private Dictionary<Guid,Entity> entities;
	private Dictionary<Guid,Material> materials;
	private Dictionary<Guid,Image> images;


	private ResourceLoader ()
	{
		meshes = new Dictionary<Guid,MeshStruct> ();
		chunks = new Dictionary<Guid, Chunk> ();
		entities = new Dictionary<Guid, Entity> ();
		materials = new Dictionary<Guid, Material> ();
		images = new Dictionary<Guid, Image> ();

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
				loadMesh(data, res.ID);
			}
			else if(res.ID.Equals(MAT_GUID)){
				Debug.Log ("mat");
				mat.Enqueue(data);
				loadMaterial(data, res.ID);
			}
			else if(res.ID.Equals(ENTITY_GUID)) {
				loadEntity(data, res.ID);
			}
			else if(res.ID.Equals(CHUNK_GUID)) {
				loadChunk(data, res.ID);
			}
			else if(res.ID.Equals(IMAGE_GUID)) {
				loadImage(data, res.ID);
			}
		}
	}

	void loadMesh (byte[] data, Guid id)
	{
		MeshCreator mc = ResourceReader.readMesh (new MemoryStream (data));
		MeshStruct tmp = mc.create();
		MeshStruct ms = getMeshStruct(id);
		ResourceCopier.getInstance().copy (tmp, ms);
	}

	void loadMaterial (byte[] data, Guid id)
	{
		MaterialCreator mc = ResourceReader.readMaterial (new MemoryStream (data));
		Material tmp = mc.create ();
		Material m = getMaterial (id);
		ResourceCopier.getInstance().copy (tmp, m);
	}

	void loadEntity (byte[] data, Guid id)
	{
		EntityCreator ec = ResourceReader.readEntity (new MemoryStream (data));
		Entity tmp = ec.create ();
		Entity e = getEntity (id);
		ResourceCopier.getInstance().copy (tmp, e);
	}

	void loadChunk (byte[] data, Guid id)
	{
		ChunkCreator cc = ResourceReader.readChunk (new MemoryStream (data));
		Chunk tmp = cc.create ();
		Chunk c = getChunk (id);
		ResourceCopier.getInstance().copy (tmp, c);
	}

	void loadImage (byte[] data, Guid id)
	{
		ImageCreator ic = ResourceReader.readImage (new MemoryStream (data));
		Image tmp = ic.create ();
		Image i = getImage (id);
		ResourceCopier.getInstance().copy (tmp, i);
	}

	/* Si l'id n'est pas dans la map, alors on crée un nouveau MeshStruct et on le donne
		 * Juste avant de retourner, on fait la requete au serveur via le client
		 * Si l'id est dans la map, on le donne et finish
		 */
	public MeshStruct getMeshStruct(Guid id) {
		if (!meshes.ContainsKey (id)) {
			meshes[id] = new MeshStruct();
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return meshes[id];
	}

	public Chunk getChunk(Guid id) {
		if (!chunks.ContainsKey (id)) {
			chunks[id] = new Chunk();
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return chunks[id];
	}

	public Entity getEntity(Guid id) {
		if (!entities.ContainsKey (id)) {
			entities[id] = new Entity();
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return entities[id];
	}

	public Material getMaterial(Guid id) {
		if (!materials.ContainsKey (id)) {
			materials[id] = new Material("");
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return materials[id];
	}

	public Image getImage( Guid id) {
		if (!images.ContainsKey (id)) {
			images[id] = new Image();
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return images[id];
	}

}


