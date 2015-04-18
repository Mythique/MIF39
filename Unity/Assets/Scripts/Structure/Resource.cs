using System;
public class Resource
{
	public Guid ID;
	public String nom;
	public Int32 dataSize;

	public Resource (Guid ID, String nom, Int32 dataSize)
	{
		this.ID = ID;
		this.nom = nom;
		this.dataSize = dataSize;
	}
}


