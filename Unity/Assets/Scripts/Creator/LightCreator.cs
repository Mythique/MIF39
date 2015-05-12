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
	
	public GameObject create (ref GameObject light)
	{
		Light lightComp = light.AddComponent<Light>();
		lightComp.intensity = intensity;
		lightComp.color = color;
		lightComp.type = type;

		return light;
	}
}