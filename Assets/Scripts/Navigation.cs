using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Navigation : MonoBehaviour, IObserver {

    private Camera mainCam;

    public GameObject tubeCreator;
    private TubeCreator tubes;

    public GameObject buttonPrefab;

    private bool tubesToUpdate = true;
    private Vector3 basePos = new Vector3(Screen.width - 100, 80, 0);

    private List<GameObject> tubeSections;

    public Sprite redLine;
    public Sprite normalLine;

    // Use this for initialization
    void Start () {
        mainCam = Camera.current;
        tubeSections = new List<GameObject>();
        tubes = tubeCreator.GetComponent<TubeCreator>();
        this.RegisterTo(Messages.EnteredSection);
        this.RegisterTo(Messages.LeftSection);
    }
	
	// Update is called once per frame
	void Update () {
        if (tubesToUpdate) {
            for (int i = 0; i < tubes.tubeId - 1; i++) {
                GameObject newButton = (GameObject)Instantiate(buttonPrefab, basePos, Quaternion.identity);
                newButton.transform.SetParent(transform.parent);

                RectTransform rt = (RectTransform)newButton.transform;
                basePos.y += rt.rect.height;

                Vector3 sectionPos = tubes.SectionPosition(i);
                newButton.GetComponentInChildren<Button>().onClick.AddListener(() => { MoveToSection(sectionPos); });

                tubeSections.Add(newButton);
            }
            tubesToUpdate = false;
        }
    }

    public void MoveToSection(Vector3 position) {
        GameObject cam = GameObject.Find("Camera");
        cam.transform.position = position;
        cam.GetComponent<CameraController>().move = false;
    }

    public void OnNotification(object sender, Messages subject, params object[] args) {
        if(subject == Messages.EnteredSection) {
            int tubeId = (int)args[0] - 1;
            tubeSections[tubeId].GetComponentInChildren<Image>().sprite = redLine;
        }
        else if(subject == Messages.LeftSection) {
            int tubeId = (int)args[0] - 1;
            tubeSections[tubeId].GetComponentInChildren<Image>().sprite = normalLine;
        }
    }
}
