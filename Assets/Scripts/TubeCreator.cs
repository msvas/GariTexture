using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class TubeCreator : MonoBehaviour {

    public int tubeSize = 10;
    public float rotationRatio = 5;
    public GameObject tubePrefab;
    public int linesRatio = 10;

    private List<GameObject> sections;

    public int tubeId = 1;
    public Material mat;

    private bool executed = false;

	// Use this for initialization
	void Start () {
        sections = new List<GameObject>();
        if (tubePrefab != null) {
            Vector3 tubeRot = Vector3.up;
            float zPos = 0;
            while (File.Exists(Application.dataPath + "/Textures/tuboReal" + tubeId.ToString("D3") + ".jpg")) {
                //for (int i = 0; i < tubeSize; i++) {
                Vector3 tubePos = new Vector3(0, 0, zPos);
                GameObject newTube = (GameObject)Instantiate(tubePrefab, tubePos, Quaternion.FromToRotation(Vector3.up, Vector3.forward));
                sections.Add(newTube);

                newTube.GetComponent<TextureExtractor>().ySize = newTube.GetComponent<MeshRenderer>().bounds.size.y;
                newTube.GetComponent<TextureExtractor>().xSize = newTube.GetComponent<MeshRenderer>().bounds.size.x;

                newTube.GetComponent<TextureExtractor>().id = tubeId;
                newTube.GetComponent<TextureExtractor>().creator = this;
                newTube.GetComponent<Tube>().id = tubeId;
                newTube.transform.Rotate(tubeRot);
                tubeRot *= rotationRatio;
                zPos += 0.98f;
                //}
                tubeId++;
            }
        }
	}

    public Vector3 SectionPosition(int id) {
        if(id < sections.Count)
            return sections[id].transform.position;
        else {
            return Vector3.zero;
        }
    }

    private void CreateConnections() {
        int maxpnts = sections[0].GetComponent<LaserAnalyser>().MaxPoints();
        int interval = maxpnts / linesRatio;
        for (int i = 1; i < sections.Count; i ++) {
            for (int point = 0; point < maxpnts; point += interval) {
                Vector2 pointA = sections[i - 1].GetComponent<LaserAnalyser>().PointPos(point);
                Vector2 pointB = sections[i].GetComponent<LaserAnalyser>().PointPos(point);
                Vector3 pointA3 = new Vector3(pointA.x, pointA.y, sections[i - 1].transform.position.z);
                Vector3 pointB3 = new Vector3(pointB.x, pointB.y, sections[i].transform.position.z);
                //Debug.Log(pointA3);
                //Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 5), Color.red);
                DrawLine(pointA3, pointB3);
            }
        }
    }

    // Update is called once per frame
    void Update () {

    }

    private bool allReady() {
        bool ready = false;
        foreach(GameObject section in sections) {
            ready = section.GetComponent<LaserAnalyser>().isReady();
            if (!ready)
                break;
        }
        return ready;
    }

    private void DrawLine(Vector3 pA, Vector3 pB) {
        GL.Begin(GL.LINES);
        mat.SetPass(0);
        GL.Color(Color.red);
        GL.Vertex(pA);
        GL.Vertex(pB);
        GL.End();
    }

    public void PostRender() {
        if (allReady()) {
            CreateConnections();
            executed = true;
        }
    }
}
