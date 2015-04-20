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
	public Guid id;

	public ImageCreator(int width, int height, int depth, int nbChannels, int size, byte[] data,Guid id)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;
		this.nbChannels = nbChannels;
		this.size = size;
		this.data = data;
		this.id = id;
	}

	public Texture2D create(ref Texture2D t2d)
	{
		Logger.Debug("Texture2D create "+t2d.name);
		int widthTot = 0;
		int heightTot = height;
		t2d.Resize (width, height);

		if (depth == 32)
		{
			for (int w = 0; w < size; w+=4)
			{
				//t2d.SetPixel(widthTot, heightTot, new Color(Convert.ToInt32(data[w]), Convert.ToInt32(data[w+1]),Convert.ToInt32(data[w+2]),Convert.ToInt32(data[w+3])));
				t2d.SetPixel(widthTot, heightTot,new Color32 (data [w+2], data [w+1], data [w], data [w+3]));
				widthTot++;
				if(widthTot == width-1)
				{
					widthTot = 0;
					heightTot--;
				}
			}
		}
		t2d.Apply ();
		Logger.Debug("end Texture2D create");

		File.WriteAllBytes(@"C:\Users\Unity\Desktop\test.jpg", t2d.EncodeToJPG (100));
		return t2d;

	}
}
