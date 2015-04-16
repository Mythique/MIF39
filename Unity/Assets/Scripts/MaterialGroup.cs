using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class MaterialGroup 	
{
	String nom;
	Guid matId;
	Guid meshId;
	UInt32 nbFace;
	public List<Triangle> triangleListe;

	public MaterialGroup(Guid matId, Guid meshId, UInt32 nbFace, List<Triangle> triangleListe, String nom)
	{
		this.matId = matId;
		this.meshId = meshId;
		this.nbFace = nbFace;
		this.triangleListe = triangleListe;
		this.nom = nom;
	}

}