using UnityEngine;
using System.Collections;
using System;

public class MaterialCreator
{
	String nom;
	public Guid id;
	ColorRGB Ambient;
	ColorRGB Diffuse;
	ColorRGB Specular;
	ColorRGB Emissive;
	ColorRGB Transmission;
	Dissolve Dissolve;
	float SpecularExponent;
	float Sharpness;
	float OpticalDensity;
	TextureCreator AmbientMap;
	TextureCreator DiffuseMap;
	TextureCreator SpecularMap;
	TextureCreator SpecularExponentMap;
	TextureCreator DissolveMap;
	TextureCreator DecalMap;
	TextureCreator DisplacementMap;
	TextureCreator BumpMap;
	TextureCreator ReflectionMap;
	int Illumination;
	
	public MaterialCreator(ColorRGB Diffuse, ColorRGB Specular, String nom)
	{
		this.Diffuse = Diffuse;
		this.Specular = Specular;
		this.nom = nom;
	}
	
	public MaterialCreator(String nom,
	                       Guid ID,
	                       ColorRGB Ambient,
	                       ColorRGB Diffuse,
	                       ColorRGB Specular,
	                       ColorRGB Emissive,
	                       ColorRGB Transmission,
	                       Dissolve Dissolve,
	                       float SpecularExponent,
	                       float Sharpness,
	                       float OpticalDensity,
	                       TextureCreator AmbientMap,
	                       TextureCreator DiffuseMap,
	                       TextureCreator SpecularMap,
	                       TextureCreator SpecularExponentMap,
	                       TextureCreator DissolveMap,
	                       TextureCreator DecalMap,
	                       TextureCreator DisplacementMap,
	                       TextureCreator BumpMap,
	                       TextureCreator ReflectionMap,
	                       int Illumination)
	{
		this.nom = nom;
		this.id = ID;
		this.Ambient = Ambient;
		this.Diffuse = Diffuse;
		this.Specular = Specular;
		this.Emissive = Emissive;
		this.Transmission = Transmission;
		this.Dissolve = Dissolve;
		this.SpecularExponent = SpecularExponent;
		this.Sharpness = Sharpness;
		this.OpticalDensity = OpticalDensity;
		this.AmbientMap = AmbientMap;
		this.DiffuseMap = DiffuseMap;
		this.SpecularMap = SpecularMap;
		this.SpecularExponentMap = SpecularExponentMap;
		this.DissolveMap = DissolveMap;
		this.DecalMap = DecalMap;
		this.DisplacementMap = DisplacementMap;
		this.BumpMap = BumpMap;
		this.ReflectionMap = ReflectionMap;
		this.Illumination = Illumination;
	}
	
	public Material create(ref Material material)
	{
		if(Transmission.getR ()==1){
			material.shader = Shader.Find ("Transparent/Diffuse");
		}
		else{
			material.shader = Shader.Find ("Diffuse");
		}

		material.SetColor("_Color", new Color(Diffuse.getR(), Diffuse.getG(), Diffuse.getB()));
		material.SetColor("_SpecColor", new Color(Specular.getR(), Specular.getG(), Specular.getB()));

		if(!AmbientMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(AmbientMap.getImageID());
		if (!DiffuseMap.getImageID ().Equals (new Guid ()))
			material.SetTexture("_MainTex", ResourceLoader.getInstance().getImage(DiffuseMap.getImageID()));
		if(!SpecularMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(SpecularMap.getImageID());
		if(!SpecularExponentMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(SpecularExponentMap.getImageID());
		if(!DissolveMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(DissolveMap.getImageID());
		if(!DecalMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(DecalMap.getImageID());
		if(!DisplacementMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(DisplacementMap.getImageID());
		if(!BumpMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(BumpMap.getImageID());
		if(!ReflectionMap.getImageID().Equals(new Guid()))
			ResourceLoader.getInstance().getImage(ReflectionMap.getImageID());

		return material;


		/*int taille = obj.GetComponent<MeshRenderer> ().materials.Length;
		Material[] mats;
		if (taille == 1 && obj.GetComponent<MeshRenderer> ().materials [0].name == "New Material (Instance)") 
		{
			 mats = new Material[taille];
			 mats[0] = new Material (Shader.Find("Standard"));
			mats[0].color = new Color(Diffuse.getR(), Diffuse.getG(), Diffuse.getB());
		}
		else 
		{
			mats = new Material[taille+1];
			mats[0] = new Material (Shader.Find("Standard"));
			mats[0].color = new Color(Diffuse.getR(), Diffuse.getG(), Diffuse.getB());
			//Debug.Log ("parcours des materials");
			for (int i = 0; i < taille; i++)
			{
				//Debug.Log (obj.GetComponent<MeshRenderer> ().materials[i].name);
				mats[i+1] = obj.GetComponent<MeshRenderer> ().materials[i];
			}

		}
		obj.GetComponent<MeshRenderer> ().materials = mats;
		*/
		
	}
	
}
