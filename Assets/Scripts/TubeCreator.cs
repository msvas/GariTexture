using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class TubeCreator : MonoBehaviour {

    public int tubeSize = 10;
    public float rotationRatio = 5;
    public GameObject tubePrefab;

    private List<GameObject> sections;

    public int tubeId = 1;

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

                newTube.GetComponent<TextureExtractor>().ySize = newTube.GetComponent<MeshRenderer>().bounds.extents.y * 2;
                newTube.GetComponent<TextureExtractor>().xSize = newTube.GetComponent<MeshRenderer>().bounds.extents.x * 2;

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

    // Update is called once per frame
    void Update () {
	
	}
}
