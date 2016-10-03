using UnityEngine;
using System.Collections;
using System.IO;

public class TubeCreator : MonoBehaviour {

    public int tubeSize = 10;
    public float rotationRatio = 5;
    public GameObject tubePrefab;

    public int tubeId = 1;

	// Use this for initialization
	void Start () {
        if (tubePrefab != null) {
            Vector3 tubeRot = Vector3.up;
            float zPos = 0;
            while (File.Exists(Application.dataPath + "/Textures/tuboReal" + tubeId.ToString("D3") + ".jpg")) {
                //for (int i = 0; i < tubeSize; i++) {
                Vector3 tubePos = new Vector3(0, 0, zPos);
                GameObject newTube = (GameObject)Instantiate(tubePrefab, tubePos, Quaternion.FromToRotation(Vector3.up, Vector3.forward));
                newTube.GetComponent<TextureExtractor>().id = tubeId;
                newTube.transform.Rotate(tubeRot);
                tubeRot *= rotationRatio;
                zPos += 0.98f;
                //}
                tubeId++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
