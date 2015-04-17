using System;
using UnityEngine;

public class ResourceCopier
{
	private static ResourceCopier instance;

	private ResourceCopier ()
	{
	}

	public static ResourceCopier getInstance() {
		if (instance == null) {
			instance = new ResourceCopier();
		}
		return instance;
	}

	public void copy(MeshStruct from, MeshStruct to) {

	}

	public void copy(Material from, Material to) {
		
	}

	public void copy(Entity from, Entity to) {
		
	}

	public void copy(Chunk from, Chunk to) {
		
	}

	public void copy(Image from, Image to) {
		
	}
}