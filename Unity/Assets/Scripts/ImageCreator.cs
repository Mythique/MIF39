using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ImageCreator
{
	int width;
	int height;
	int depth;
	int nbChannels;
	int size;
	byte[] data;

	public ImageCreator(int width, int height, int depth, int nbChannels, int size, byte[] data)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;
		this.nbChannels = nbChannels;
		this.size = size;
		this.data = data; 
	}

	public void create(GameObject obj)
	{
		int widthTot = 0;
		int heightTot = height;
		Texture2D t2d = new Texture2D (width, height);

		Debug.Log ("data : "+Convert.ToInt32(data[0]));
		Debug.Log ("data : "+Convert.ToInt32(data[1]));
		Debug.Log ("data : "+Convert.ToInt32(data[2]));
		Debug.Log ("data : "+Convert.ToInt32(data[3]));



		if (depth == 32)
		{
			for (int w = 0; w < size; w+=4)
			{
				//t2d.SetPixel(widthTot, heightTot, new Color(Convert.ToInt32(data[w]), Convert.ToInt32(data[w+1]),Convert.ToInt32(data[w+2]),Convert.ToInt32(data[w+3])));
				t2d.SetPixel(widthTot, heightTot,new Color32 (data [w+2], data [w+1], data [w], data [w+3]));
				widthTot++;
				if(widthTot == width)
				{
					widthTot = 0;
					heightTot--;
				}
			}
		}


		t2d.Apply ();
		File.WriteAllBytes(@"C:\Users\Tsubaki\Desktop\test.jpg", t2d.EncodeToJPG (100));
		obj.GetComponent<MeshRenderer> ().material.mainTexture = t2d;
	}
}
