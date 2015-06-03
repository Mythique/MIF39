using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class LightCreator
{
	public enum Type: int { Point, Direction, Surface };

	public Guid id{ get; set; }
	string realName;
	float intensity;
	Vector3 color;
	Type type;

	public LightCreator (Guid id, string realName, float intensity, Vector3 color, Type type)
	{
		this.id = id;
		this.realName = realName;
		this.intensity = intensity;
		this.color = color;
		this.type = type;
	}
	
	public Asset create (ref Asset asset)
	{
		Debug.Log ("light intensity : " + intensity);
		Light lightComp = asset.go.AddComponent<Light>();
		lightComp.intensity = intensity;
		lightComp.color =new Color(color.x,color.y,color.z);
		lightComp.type = type==Type.Point?LightType.Point:type==Type.Direction?LightType.Directional:type==Type.Surface?LightType.Area:LightType.Point;
		lightComp.enabled = true;

		asset.loaded = true;
		return asset;
	}
}