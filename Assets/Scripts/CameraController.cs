using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Range(1, 10)]
    public float speed = 1f;

    private int tubeSize;

    private Vector3 pos = new Vector3(0, 0, 0);

    public bool move = true;

	// Use this for initialization
	void Start () {
        tubeSize = GameObject.FindGameObjectWithTag("TubeCreator").GetComponent<TubeCreator>().tubeSize;
    }
	
	// Update is called once per frame
	void Update () {
        if (move) {
            if (gameObject.transform.position.z < tubeSize) {
                pos.z += speed / 1000 * Time.deltaTime;
                transform.Translate(pos);
            } else {
                transform.Translate(0, 0, -tubeSize);
            }
        }
	}
}
