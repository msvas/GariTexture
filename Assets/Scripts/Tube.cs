using UnityEngine;
using System.Collections;

public class Tube : MonoBehaviour {

    public int id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        this.Notify(Messages.EnteredSection, new object[] { id });
    }

    void OnTriggerExit(Collider other) {
        this.Notify(Messages.LeftSection, new object[] { id });
    }
}
