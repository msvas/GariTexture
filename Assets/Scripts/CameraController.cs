using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Range(0, 1)]
    public float speed = 0.5f;

    private Vector3 pos = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        pos.z += speed * Time.deltaTime;
        transform.Translate(pos);
	}
}
