using UnityEngine;
using System.Collections;
using System;

public class MaterialCreator
{
	String nom;
	Guid ID;
	ColorRGB Ambient;
	ColorRGB Diffuse;
	ColorRGB Specular;
	ColorRGB Emissive;
	ColorRGB Transmission;
	Dissolve Dissolve;
	float SpecularExponent;
	float Sharpness;
	float OpticalDensity;
	Texture AmbientMap;
	Texture DiffuseMap;
	Texture SpecularMap;
	Texture SpecularExponentMap;
	Texture DissolveMap;
	Texture DecalMap;
	Texture DisplacementMap;
	Texture BumpMap;
	Texture ReflectionMap;
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
	                       Texture AmbientMap,
	                       Texture DiffuseMap,
	                       Texture SpecularMap,
	                       Texture SpecularExponentMap,
	                       Texture DissolveMap,
	                       Texture DecalMap,
	                       Texture DisplacementMap,
	                       Texture BumpMap,
	                       Texture ReflectionMap,
	                       int Illumination)
	{
		this.nom = nom;
		this.ID = ID;
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
	
	public void create(GameObject obj)
	{
		obj.GetComponent<MeshRenderer> ().material = new Material (Shader.Find("Standard"));
		obj.GetComponent<MeshRenderer> ().material.SetColor("_Color", new Color(Diffuse.getR(), Diffuse.getG(), Diffuse.getB()));
		obj.GetComponent<MeshRenderer> ().material.SetColor("_SpecColor", new Color(Specular.getR(), Specular.getG(), Specular.getB()));
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
