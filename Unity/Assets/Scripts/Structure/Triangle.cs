using UnityEngine;
using System.Collections;

public class Triangle
{
	public bool m_hasNormals;
	public bool m_hasTexcoords;
	int [] verticeIndexes;
	int [] normalIndexes;
	int [] textIndexes;

	public Triangle()
	{
		verticeIndexes = new int[3];
		normalIndexes = new int[3];
		textIndexes = new int[3];
		m_hasNormals = false;
		m_hasTexcoords = false;
	}
		
	public Triangle(int [] verticeIndexes, int [] normalIndexes, int [] textIndexes, bool m_hasNormals, bool m_hasTexcoords)
	{
		this.verticeIndexes = verticeIndexes;
		this.normalIndexes = normalIndexes;
		this.textIndexes = textIndexes;
		this.m_hasNormals = m_hasNormals;
		this.m_hasTexcoords = m_hasTexcoords;
	}

	public int[] getVerticeIndexes()
	{
		return this.verticeIndexes;
	}

	public int getVerticeIndexeAt(int index)
	{
		if (index < verticeIndexes.Length && index >= 0) {
			return verticeIndexes[index];
		} else {
			return -1;
		}
	}

	public int getNormaleIndexeAt(int index)
	{
		if (index < normalIndexes.Length && index >= 0) {
			return normalIndexes[index];
		} else {
			return -1;
		}
	}

	public int getTextureIndexeAt(int index)
	{
		if (index < textIndexes.Length && index >= 0) {
			return textIndexes[index];
		} else {
			return -1;
		}
	}


}
