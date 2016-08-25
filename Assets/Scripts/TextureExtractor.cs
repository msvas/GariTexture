﻿using UnityEngine;
using System.Collections;

public class TextureExtractor : MonoBehaviour {

    public TextAsset imageAsset;

    void Start () {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageAsset.bytes);
        DrawCircle(tex, (int)tex.width/2, (int)tex.height/2, 5, new Color(0, 0, 0));



        GetComponent<Renderer>().material.mainTexture = tex;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;
        Color32[] tempArray = tex.GetPixels32();

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                tempArray[py * tex.width + px] = col;
                tempArray[py * tex.width + nx] = col;
                tempArray[ny * tex.width + px] = col;
                tempArray[ny * tex.width + nx] = col;
            }
        }
        tex.SetPixels32(tempArray);
        tex.Apply();
    }
}
