using UnityEngine;
using System.Collections;
using System.IO;

public class FailureFinder
    {

    private float threshold = 0.5f;

    public void analyzeTexture(Texture2D tex) {
        Color[] pixels = tex.GetPixels();
        bool[,] binTex = new bool[tex.width, tex.height];
        for(int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
                if (pixels[i * tex.height + j].grayscale > threshold)
                    binTex[i, j] = true;
                else
                    binTex[i, j] = false;
            }
         }
    }
}
