using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class FailureFinder : MonoBehaviour {

    private float threshold = 0.1f;
    private List<Vector2> failures = new List<Vector2>();
    public TubeCreator creator;
    public GameObject pointPrefab;
    public int tubeId;

    public float ySize;
    public float xSize;

    public void analyzeTexture(Texture2D tex, int sectionB) {
        int[,] binTex = buildBinaryImage(tex);

        DebugMatrix(binTex);

        for (int i = 0; i <= sectionB; i++) {
            RunCircle(tex, binTex, (int)tex.width / 2, (int)tex.height / 2, i);
        }

        using (TextWriter tw = new StreamWriter("SQL_U.txt")) {
            for (int i = 0; i < tex.height; i++) {
                for (int j = 0; j < tex.width; j++) {
                    tw.Write(binTex[i, j]);
                }
                tw.WriteLine();
            }
        }

        DrawFailures(tex.width, tex.height);
        DebugFailures();
    }

    private int[,] buildBinaryImage(Texture2D tex) {
        Color[] pixels = tex.GetPixels();
        int[,] binTex = new int[tex.height, tex.width];
        //using (TextWriter tw = new StreamWriter("SQL_U.txt")) {
            for (int i = 0; i < tex.height; i++) {
                for (int j = 0; j < tex.width; j++) {
                    if (pixels[i * tex.width + j].grayscale > threshold)
                        binTex[i, j] = 1;
                    else
                        binTex[i, j] = 0;
                    //tw.Write(binTex[i, j]);
                }
                //tw.WriteLine();
            }
        //}

        
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
        return binTex;
    }

    private bool RunCircle(Texture2D tex, int[,] binImg, int cx, int cy, int r) {
        int x, y, px, nx, py, ny;
        bool end = false;
        int allWhite = 0;

        for (x = 0; x <= r; x++) {
            y = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            px = cx + x;
            nx = cx - x;
            py = cy + y;
            ny = cy - y;

            if (binImg[px, py] == 0) {
                //binImg[px, py] = 2;
                Vector2 tobeinserted = new Vector2(px, py);
                if (!failures.Contains(tobeinserted)) {
                    failures.Add(tobeinserted);
                    allWhite++;
                }
            }
            if (binImg[nx, py] == 0) {
                //binImg[nx, py] = 2;
                Vector2 tobeinserted = new Vector2(nx, py);
                if (!failures.Contains(tobeinserted)) {
                    failures.Add(tobeinserted);
                    allWhite++;
                }
            }
            if (binImg[px, ny] == 0) {
                //binImg[px, ny] = 2;
                Vector2 tobeinserted = new Vector2(px, ny);
                if (!failures.Contains(tobeinserted)) {
                    failures.Add(tobeinserted);
                    allWhite++;
                }
            }
            if (binImg[nx, ny] == 0) {
                //binImg[nx, ny] = 2;
                Vector2 tobeinserted = new Vector2(nx, ny);
                if (!failures.Contains(tobeinserted)) {
                    failures.Add(tobeinserted);
                    allWhite++;
                }
            }
        }
        //Debug.Log("all " + allWhite.ToString());
        //Debug.Log(4 * r);
        if (allWhite >= 4 * r) {
            end = true;
            for(int i = 0; i < allWhite; i ++) {
                failures.RemoveAt(failures.Count - 1);
            }
        }
        return end;        
    }

    private void DebugMatrix(int[,] matrix) {
        Color32[] colors = new Color32[matrix.Length];
        int j = 0;
        for (int i = 0; i < matrix.GetLength(1); i++) {
            for (int m = 0; m < matrix.GetLength(0); m++) {
                int k = matrix[m, i];
                colors[j] = new Color(k, k, k);
                j++;
            }
        }
        Texture2D tex = new Texture2D(matrix.GetLength(0), matrix.Length / matrix.GetLength(0));
        tex.SetPixels32(colors);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/Textures/binary.png", bytes);
    }

    private void DebugFailures() {
        string failuresPos = "";
        foreach (Vector2 fail in failures) {
            failuresPos += fail.x.ToString() + " " + fail.y.ToString() + Environment.NewLine;
        }
        File.WriteAllText("failures.txt", failuresPos);
    }

    private void DrawFailures(int width, int height) {
        foreach (Vector2 fail in failures) {
            Vector3 originalPos = creator.SectionPosition(tubeId);
            float x = ((fail.x / width) - 0.5f) * xSize * 4;
            float y = ((fail.y / height) - 0.5f) * ySize * 4;
            GameObject newPoint = (GameObject)Instantiate(pointPrefab, new Vector3(x, y, originalPos.z), Quaternion.FromToRotation(Vector3.up, Vector3.forward));
            //Gizmos.DrawSphere(new Vector3(x, y, originalPos.z), 1);
        }
    }
}




