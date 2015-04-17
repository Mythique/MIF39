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

	private Dictionary<Guid,MeshStruct> meshes;
	private Dictionary<Guid,Chunk> chunks;
	private Dictionary<Guid,Entity> entities;
	private Dictionary<Guid,Material> materials;
	private Dictionary<Guid,Texture2D> images;


	private ResourceLoader ()
	{
		meshes = new Dictionary<Guid,MeshStruct> ();
		chunks = new Dictionary<Guid, Chunk> ();
		entities = new Dictionary<Guid, Entity> ();
		materials = new Dictionary<Guid, Material> ();
		images = new Dictionary<Guid, Texture2D> ();
	}

	public static ResourceLoader getInstance(){
		if (loader == null)
			loader = new ResourceLoader ();
		return loader;
	}

	public bool load(){
		byte[] resource=ServerAnswerManager.getInstance().getAnswer();
		if (resource != null) {
			MemoryStream stream=new MemoryStream (resource);
			Resource res = ResourceReader.getInstance().readResource(stream);
			byte[] data = new byte[res.dataSize];
			stream.Read(data,0,res.dataSize);
			Debug.Log (res.nom);
			if(res.ID.Equals(MESH_GUID)){
				Debug.Log ("mesh");
				loadMesh(data, res.ID);
			}
			else if(res.ID.Equals(MAT_GUID)){
				Debug.Log ("mat");
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
			return true;
		}
		return false;
	}

	void loadMesh (byte[] data, Guid id)
	{
		MeshCreator mc = ResourceReader.getInstance().readMesh (new MemoryStream (data));
		MeshStruct tmp = mc.create();
		MeshStruct ms = getMeshStruct(id);
		ResourceCopier.getInstance().copy (tmp, ms);
	}

	void loadMaterial (byte[] data, Guid id)
	{
		MaterialCreator mc = ResourceReader.getInstance().readMaterial (new MemoryStream (data));
		Material tmp = mc.create ();
		Material m = getMaterial (id);
		ResourceCopier.getInstance().copy (tmp, m);
	}

	void loadEntity (byte[] data, Guid id)
	{
		EntityCreator ec = ResourceReader.getInstance().readEntity (new MemoryStream (data));
		Entity tmp = ec.create ();
		Entity e = getEntity (id);
		ResourceCopier.getInstance().copy (tmp, e);
	}

	void loadChunk (byte[] data, Guid id)
	{
		ChunkCreator cc = ResourceReader.getInstance().readChunk (new MemoryStream (data));
		Chunk tmp = cc.create ();
		Chunk c = getChunk (id);
		ResourceCopier.getInstance().copy (tmp, c);
	}

	void loadImage (byte[] data, Guid id)
	{
		ImageCreator ic = ResourceReader.getInstance().readImage (new MemoryStream (data));
		Texture2D tmp = ic.create ();
		Texture2D i = getImage (id);
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

	public Texture2D getImage( Guid id) {
		if (!images.ContainsKey (id)) {
			images[id] = new Texture2D(1,1);
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return images[id];
	}

}


