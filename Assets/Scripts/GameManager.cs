using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour {

    public GameObject tubecreator;
    private TubeCreator creator;

    // Use this for initialization
    void Start() {
        creator = tubecreator.GetComponent<TubeCreator>();
        MessageSystem.InitTable();
    }

    // Update is called once per frame
    void Update() {
    }

    void OnPostRender() {
        creator.PostRender();
    }
}