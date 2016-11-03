using UnityEngine;
using System.Collections;
using System.IO;

public class FailureFinder
    {

    private float threshold = 0.5f;

    public void analyzeTexture(Texture2D tex) {
        Color[] pixels = tex.GetPixels();
        bool[,] binTex = new bool[tex.width, tex.height];
        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
                if (pixels[i * tex.height + j].grayscale > threshold)
                    binTex[i, j] = true;
                else
                    binTex[i, j] = false;
            }
        }
        /*
        for (x = 0; x <= r; x++) {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++) {
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
        */
        /*
        string debug = "";
        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
               debug += binTex[i, j].ToString() + " ";
            }
            debug += "\n";
        }
        File.WriteAllText("debug.txt", debug);
        */
    }
}
