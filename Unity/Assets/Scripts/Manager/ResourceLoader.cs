using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.IO;
public class ResourceLoader
{
	public static readonly Guid MAT_GUID=new Guid("3c50697c-543a-0e7f-6ab6-23ae942b73cc");
	public static readonly Guid MESH_GUID=new Guid("4394cb82-98f0-f0ef-1e3a-e4956402ace7");
	public static readonly Guid ENTITY_GUID=new Guid();
	public static readonly Guid CHUNK_GUID=new Guid();
	public static readonly Guid IMAGE_GUID=new Guid("0685f590-f83a-0e1f-d272-2a46b8321d24");

	private static ResourceLoader loader;

	private Dictionary<Guid,GameObject> meshes;
	private Dictionary<Guid,Chunk> chunks;
	private Dictionary<Guid,Entity> entities;
	private Dictionary<Guid,Material> materials;
	private Dictionary<Guid,Texture2D> images;

	private Queue<Guid> objAcreer;


	private ResourceLoader ()
	{
		meshes = new Dictionary<Guid,GameObject> ();
		chunks = new Dictionary<Guid, Chunk> ();
		entities = new Dictionary<Guid, Entity> ();
		materials = new Dictionary<Guid, Material> ();
		images = new Dictionary<Guid, Texture2D> ();
		objAcreer = new Queue<Guid> ();
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
			Logger.Debug (res.nom);
			Logger.Debug (res.ID);
			if(res.ID.Equals(MESH_GUID)){
				loadMesh(data);
			}
			else if(res.ID.Equals(MAT_GUID)){
				loadMaterial(data);
			}
			else if(res.ID.Equals(ENTITY_GUID)) {
				loadEntity(data);
			}
			else if(res.ID.Equals(CHUNK_GUID)) {
				loadChunk(data);
			}
			else if(res.ID.Equals(IMAGE_GUID)) {
				loadImage(data);
			}
			return true;
		}
		return false;
	}

	void loadMesh (byte[] data)
	{
		MeshCreator mc = ResourceReader.getInstance().readMesh (new MemoryStream (data));
		Logger.Debug ("MeshCreator créé");
		GameObject ms = getMeshStruct(mc.id);
		Logger.Debug ("Get MeshStruct ok");
		GameObject tmp = mc.create(ref ms);
		Logger.Debug ("Create Mesh ok");
		//ResourceCopier.getInstance().copy (tmp, ms);
	}

	void loadMaterial (byte[] data)
	{
		MaterialCreator mc = ResourceReader.getInstance().readMaterial (new MemoryStream (data));
		Material m = getMaterial (mc.id);
		Material tmp = mc.create (ref m);
		//ResourceCopier.getInstance().copy (tmp, m);
	}

	void loadEntity (byte[] data)
	{
		EntityCreator ec = ResourceReader.getInstance().readEntity (new MemoryStream (data));
		Entity e = getEntity (ec.id);
		Entity tmp = ec.create (ref e);
		//ResourceCopier.getInstance().copy (tmp, e);
	}

	void loadChunk (byte[] data)
	{
		ChunkCreator cc = ResourceReader.getInstance().readChunk (new MemoryStream (data));
		Chunk c = getChunk (cc.id);
		Chunk tmp = cc.create (ref c);
		//ResourceCopier.getInstance().copy (tmp, c);
	}

	void loadImage (byte[] data)
	{
		ImageCreator ic = ResourceReader.getInstance().readImage (new MemoryStream (data));
		Texture2D i = getImage (ic.id);
		Texture2D tmp = ic.create (ref i);
		//ResourceCopier.getInstance().copy (tmp, i);
	}

	/* Si l'id n'est pas dans la map, alors on crée un nouveau MeshStruct et on le donne
		 * Juste avant de retourner, on fait la requete au serveur via le client
		 * Si l'id est dans la map, on le donne et finish
		 */
	public GameObject getMeshStruct(Guid id) {
		Logger.Debug("getMeshStruct "+id.ToString());
		if (!meshes.ContainsKey (id)) {
			/*=lock(objAcreer){
				objAcreer.Enqueue(id);
			}*/
			//Logger.Debug("Enqueue "+id.ToString());
			/*while(!meshes.ContainsKey (id)){
				Thread.Sleep(100);
			}*/
			GameObject obj =new GameObject();
			obj.AddComponent<MeshFilter>();
			obj.AddComponent<MeshRenderer>();
			meshes[id]=obj;
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		Logger.Debug("end getMeshStruct "+id.ToString());
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
			materials[id] = new Material(Shader.Find("Standard"));
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		Logger.Debug("Fin Chargement Mat");
		return materials[id];
	}

	public Texture2D getImage( Guid id) {
		if (!images.ContainsKey (id)) {
			Texture2D tex =new Texture2D(1,1);
			tex.name="vouvou";
			images[id] = tex;
			Client.getInstance().ask(ServerAnswerManager.Type.SHARED_R, id);
		}
		return images[id];
	}

	public void addObj(GameObject obj ,Guid id){
		meshes[id]=obj;
		Logger.Debug("Add GameObj "+id.ToString()+":"+(meshes.ContainsKey(id)));

	}

	public Guid dequeueObjACreer(){
		Logger.Trace("Count "+objAcreer.Count);
		if (objAcreer.Count != 0) {
			Guid resu=Guid.Empty;
			lock(objAcreer){
				resu= objAcreer.Dequeue();
			}
			Logger.Debug("Dequeue "+resu.ToString());
			return resu;
		}
			
		
		return Guid.Empty;
	}

}


