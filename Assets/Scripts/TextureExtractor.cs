using UnityEngine;
using System.Collections;
using System.IO;

public class TextureExtractor : MonoBehaviour {

    private byte[] fileData;
    public int innerRadio = 100;

    void Start () {
        fileData = File.ReadAllBytes(Application.dataPath + "/Textures/testTube.jpg");
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        DrawCircle(tex, (int)tex.width/2, (int)tex.height/2, innerRadio, new Color(0, 0, 0));

        //Debug.Log(tex.GetPixel((int)tex.width / 2, 0));

        BuildTubeTexture(tex);

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);

        GetComponent<Renderer>().material.mainTexture = tex;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void BuildTubeTexture(Texture2D tex) {
        int newWidth = (int)Mathf.Ceil((tex.height / 2) - innerRadio);
        int newHeight = (int)Mathf.Ceil(2 * Mathf.PI * (tex.height / 2));

        Texture2D fixedTex = new Texture2D(newWidth, newHeight);


    }

    private void ClonePixel(Texture2D from, Texture2D to) {
        Color pixel = from.GetPixel(0, 0);
        to.SetPixel(0, 0, pixel);
    }

    private void DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
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
