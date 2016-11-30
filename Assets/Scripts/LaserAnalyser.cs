using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LaserAnalyser : FailureFinder {

    private List<Vector2> points = new List<Vector2>();
    private byte[] fileData;
    private bool ready = false;

    private int texwidth;
    private int texheight;

    public void LaserAnalysis() {
        if (File.Exists(Application.dataPath + "/Textures/laser" + tubeId.ToString("D3") + ".jpg")) {
            fileData = File.ReadAllBytes(Application.dataPath + "/Textures/laser" + tubeId.ToString("D3") + ".jpg");
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            texwidth = tex.width;
            texheight = tex.height;

            int[,] binTex = buildBinaryImage(tex);
            FindPoints(binTex);

            ready = true;

            using (TextWriter tw = new StreamWriter("SQL_U.txt")) {
                for (int i = tex.height - 1; i >= 0; i--) {
                    for (int j = 0; j < tex.width; j++) {
                        tw.Write(binTex[i, j]);
                    }
                    tw.WriteLine();
                }
            }

            DrawGeometry(tex.width, tex.height);
        }
    }

    private void FindPoints(int[,] binIMG) {
        for (int i = 0; i < binIMG.GetLength(1); i++) {
            for (int m = 0; m < binIMG.GetLength(0); m++) {
                if (binIMG[m, i] == 0) {
                    points.Add(new Vector2(i, m));
                }
            }
        }
    }

    private void DrawGeometry(int width, int height) {
        foreach (Vector2 point in points) {
            Vector3 originalPos = creator.SectionPosition(tubeId);
            float x = ((point.x / width) - 0.5f) * xSize;
            float y = ((point.y / height) - 0.5f) * ySize;
            //Debug.Log(x.ToString() + " " + y.ToString());
            GameObject newPoint = (GameObject)Instantiate(pointPrefab, new Vector3(x, y, originalPos.z), Quaternion.FromToRotation(Vector3.up, Vector3.forward));
            newPoint.transform.parent = transform;
        }
    }

    public int MaxPoints() {
        return points.Count;
    }

    public Vector2 PointPos(int pointId) {
        if(pointId < points.Count) {
            float x = ((points[pointId].x / texwidth) - 0.5f) * xSize;
            float y = ((points[pointId].y / texheight) - 0.5f) * ySize;
            return new Vector2(x, y);
        }
        else {
            return new Vector2(-1, -1);
        }
    }

    public bool isReady() {
        return ready;
    }
}
