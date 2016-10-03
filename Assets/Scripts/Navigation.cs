using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

    private LineRenderer line;
    private Camera mainCam = Camera.current;

    // Use this for initialization
    void Start () {
        line = gameObject.AddComponent<LineRenderer>();
        line.SetColors(Color.yellow, Color.red);
        line.SetWidth(0.2F, 0.2F);
        line.SetVertexCount(3);
    }
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, mainCam.ScreenToWorldPoint(new Vector3(0, 0, 0)));
        line.SetPosition(1, mainCam.ScreenToWorldPoint(new Vector3(0, 1, 0)));
        line.SetPosition(2, mainCam.ScreenToWorldPoint(new Vector3(0, 2, 0)));
    }
}
