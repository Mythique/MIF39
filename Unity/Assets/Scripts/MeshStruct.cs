using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

class MeshStruct
{
	public Mesh mesh;
	public MeshRenderer renderer;

	public MeshStruct() {}

	public MeshStruct(Mesh m, MeshRenderer r) {
		mesh = m;
		renderer = r;
	}
}



