using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Range(1, 10)]
    public float speed = 1f;

    private Vector3 pos = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        pos.z += speed / 1000 * Time.deltaTime;
        transform.Translate(pos);
	}
}
