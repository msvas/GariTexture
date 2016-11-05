using UnityEngine;
using System.Collections;
using System.IO;

public class TextureExtractor : MonoBehaviour {

    private byte[] fileData;
    public int innerRadio = 100;
    private Texture2D result;

    private FailureFinder analyzer;

    public int id;

    void Start () {
        if(File.Exists(Application.dataPath + "/Textures/tuboReal" + id.ToString("D3") + ".jpg")) { 
            fileData = File.ReadAllBytes(Application.dataPath + "/Textures/tuboReal" + id.ToString("D3") + ".jpg");
            //fileData = File.ReadAllBytes(Application.dataPath + "/Textures/testBinary.png");
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            analyzer = new FailureFinder();
            analyzer.analyzeTexture(tex, innerRadio, 120);
      
            DrawCircle(tex, (int)tex.width / 2, (int)tex.height / 2, innerRadio, new Color(0, 0, 0));

            //Debug.Log(tex.GetPixel((int)tex.width / 2, 0));

            BuildTubeTexture(tex);

            // Encode texture into PNG
            byte[] bytes = result.EncodeToPNG();

            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + "/Textures/SavedScreen.png", bytes);

            result.Apply();

            GetComponent<Renderer>().material.mainTexture = result;
            id++;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void BuildTubeTexture(Texture2D tex) {
        int radius = tex.height / 2;
        int newHeight = (int)Mathf.Ceil(radius - innerRadio);
        int newWidth = 4 * radius;

        result = new Texture2D(newWidth, newHeight);

        RunCircle(radius, tex);
    }

    private void ClonePixel(Texture2D from, int xorig, int yorig, int xdest, int ydest) {
        Color pixel = from.GetPixel(xorig, yorig);
        result.SetPixel(xdest, ydest, pixel);
    }

    private void RunCircle(int r, Texture2D tex) {
        int x, j;
        float factor;
        float k, z, m, n, lastk, lastz, lastm, lastn;

        int xcenter = tex.width / 2;
        int ycenter = tex.height / 2;

        for (j = innerRadio; j <= r; j++) {
            factor = (float)r / (float)j;
            k = 0;
            m = 2 * r;
            n = 2 * r; 
            z = 4 * r;
            lastk = 0;
            lastm = 0;
            lastz = 0;
            lastn = 0;
            //factor = 1;
            for (x = 0; x <= j; x++) {
                int y = (int)Mathf.Ceil(Mathf.Sqrt(j * j - x * x));

                ClonePixel(tex, xcenter + x, ycenter + y, (int)k, j - innerRadio); //1st
                ClonePixel(tex, xcenter - x, ycenter + y, (int)z, j - innerRadio); //4th
                ClonePixel(tex, xcenter + x, ycenter - y, (int)m, j - innerRadio); //2nd
                ClonePixel(tex, xcenter - x, ycenter - y, (int)n, j - innerRadio); //3rd
                lastk = k;
                lastm = m;
                lastz = z;
                lastn = n;
                k += factor;
                z -= factor;
                m -= factor; 
                n += factor;
                FixBlankPixel(lastk, k, tex, xcenter + x, ycenter + y, j - innerRadio, true);
                FixBlankPixel(lastz, z, tex, xcenter - x, ycenter + y, j - innerRadio, false);
                FixBlankPixel(lastm, m, tex, xcenter + x, ycenter - y, j - innerRadio, false);
                FixBlankPixel(lastn, n, tex, xcenter - x, ycenter - y, j - innerRadio, true);
            }
        }
    }

    private void FixBlankPixel(float last, float now, Texture2D tex, int xorig, int yorig, int ydest, bool right) {
        if (right) {
            if (((int)now - (int)last) >= 2) {
                for (int i = (int)last + 1; i < (int)now; i++) {
                    ClonePixel(tex, xorig, yorig, i, ydest);
                }
            }
        }
        else {
            if (((int)last - (int)now) >= 2) {
                for (int i = (int)last - 1; i > (int)now; i--) {
                    ClonePixel(tex, xorig, yorig, i, ydest);
                }
            }
        }
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
