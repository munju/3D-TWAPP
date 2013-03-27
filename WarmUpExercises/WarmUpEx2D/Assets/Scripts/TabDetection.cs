using UnityEngine;
using System.Collections;

public class TabDetection : MonoBehaviour {

	public bool hittingFirst = false;
	public bool hittingSecond = false;
	public bool hittingThird = false;
	
    void Start() {

    }
	
    void Update() {
		RaycastHit hit;
		Ray myRay = Camera.main.ViewportPointToRay(new Vector3(0.5F,0.5F,0));
		if (Physics.Raycast(myRay, out hit)){
			if (hit.collider.gameObject.name == "FirstPlane" || hit.collider.gameObject.name == "TwitterPlane1"){
				hittingSecond = false;
				hittingThird = false;
				hittingFirst=true;
			} else if (hit.collider.gameObject.name == "SecondPlane" || hit.collider.gameObject.name == "TwitterPlane2") {
				hittingFirst = false;
				hittingThird = false;
				hittingSecond=true;
			} else if (hit.collider.gameObject.name == "ThirdPlane" || hit.collider.gameObject.name == "TwitterPlane3") {
				hittingFirst = false;
				hittingSecond = false;
				hittingThird=true;
			} else {
				hittingFirst = false;
				hittingSecond = false;
				hittingThird = false;
			}
		}
    }
}