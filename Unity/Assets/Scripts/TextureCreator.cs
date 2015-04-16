using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class TextureCreator
{
	public enum TextureChannel: int { CHAN_R, CHAN_G, CHAN_B, CHAN_M, CHAN_L, CHAN_Z };

	Guid imageID;
	bool blendU, blendV, CC, Clamp;
	float Base, Gain, BumpMult, Boost;
	Int32 TexRes;
	Vector3 Position, Scale, Turbulence;
	TextureChannel Channel;

	public TextureCreator()
	{
	}

	public TextureCreator(Guid imageID, bool blendU, bool blendV, bool CC, bool Clamp,
	                      float Base, float Gain, float BumpMult, float Boost,
	                      Int32 TexRes,
	                      Vector3 Position, Vector3 Scale, Vector3 Turbulence,
	                      TextureChannel Channel)
	{
		this.imageID = imageID;
		this.blendU = blendU;
		this.blendV = blendV;
		this.CC = CC;
		this.Clamp = Clamp;
		this.Base = Base;
		this.Gain = Gain;
		this.BumpMult = BumpMult;
		this.Boost = Boost;
		this.TexRes = TexRes;
		this.Position = Position;
		this.Scale = Scale;
		this.Turbulence = Turbulence;
		this.Channel = Channel;
	}

	public Texture create ()
	{
		return new Texture();
	}
}
