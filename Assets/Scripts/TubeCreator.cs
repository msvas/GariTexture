using UnityEngine;
using System.Collections;

public class TubeCreator : MonoBehaviour {

    public int tubeSize = 10;
    public float rotationRatio = 5;
    public GameObject tubePrefab;

	// Use this for initialization
	void Start () {
        if (tubePrefab != null) {
            Vector3 tubeRot = Vector3.up;
            float zPos = 0;
            for (int i = 0; i < tubeSize; i++) {
                Vector3 tubePos = new Vector3(0, 0, zPos);
                GameObject newTube = (GameObject) Instantiate(tubePrefab, tubePos, Quaternion.FromToRotation(Vector3.up, Vector3.forward));
                newTube.transform.Rotate(tubeRot);
                tubeRot *= rotationRatio;
                zPos += 0.98f;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
