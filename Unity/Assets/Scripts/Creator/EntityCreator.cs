using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class EntityCreator
{
	public Guid id;

	public EntityCreator(Guid id){
		this.id = id;
	}

	public Entity create (ref Entity entity)
	{
		throw new NotImplementedException ();
	}
}



