using UnityEngine;
using System.Collections;

public class ColorRGB
{
	float R;
	float G;
	float B;

	public ColorRGB(float R, float G, float B)
	{
		this.R = R;
		this.G = G;
		this.B = B;
	}

	public float getR()
	{
		return R;
	}
	public float getG()
	{
		return G;
	}
	public float getB()
	{
		return B;
	}
}
