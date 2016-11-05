using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class FailureFinder {

    private float threshold = 0.1f;
    private List<Vector2> failures = new List<Vector2>();

    public void analyzeTexture(Texture2D tex, int sectionA, int sectionB) {
        int[,] binTex = buildBinaryImage(tex);

        DebugMatrix(binTex);

        for (int i = sectionA; i <= sectionB; i++) {
            Vector2 failurePos = RunCircle(tex, binTex, (int)tex.width / 2, (int)tex.height / 2, i);
            if(failurePos.x != -1) {
                failures.Add(failurePos);
                Debug.Log(failurePos);
            }
        }
    }

    private int[,] buildBinaryImage(Texture2D tex) {
        Color[] pixels = tex.GetPixels();
        int[,] binTex = new int[tex.width, tex.height];
        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
                if (pixels[i * tex.height + j].grayscale > threshold)
                    binTex[i, j] = 1;
                else
                    binTex[i, j] = 0;
            }
        }
        return binTex;
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

    private Vector2 RunCircle(Texture2D tex, int[,] binImg, int cx, int cy, int r) {
        int x, y, px, nx, py, ny;

        for (x = 0; x <= r; x++) {
            y = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            px = cx + x;
            nx = cx - x;
            py = cy + y;
            ny = cy - y;
                
            if (binImg[px, py] == 0)
                return new Vector2(px, py);
            if (binImg[nx, py] == 0)
                return new Vector2(nx, py);
            if (binImg[px, ny] == 0)
                return new Vector2(px, ny);
            if (binImg[nx, ny] == 0)
                return new Vector2(nx, ny);
        }
        return new Vector2(-1, -1);
    }

    private void DebugMatrix(int[,] matrix) {
        Color32[] colors = new Color32[matrix.Length];
        int j = 0;
        foreach(int i in matrix) {
            colors[j] = new Color(i, i, i);
            j++;
        }
        Texture2D tex = new Texture2D(matrix.GetLength(0), matrix.Length / matrix.GetLength(0));
        tex.SetPixels32(colors);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/Textures/binary.png", bytes);
    }
}


