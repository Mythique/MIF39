using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class MeshStruct
{
	public Mesh mesh;
	public MeshRenderer renderer;

	public MeshStruct() {
		mesh = new Mesh();
		renderer = new MeshRenderer();
		renderer.enabled = true;

	}

	public MeshStruct(Mesh m, MeshRenderer r) {
		mesh = m;
		renderer = r;
	}
}



