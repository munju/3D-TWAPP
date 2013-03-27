using UnityEngine;
using System.Collections;

public class TabDetection : MonoBehaviour {

	public bool hittingFront = false;
	public bool hittingLeft = false;
	public bool hittingRight = false;
	
    void Start() {

    }
	
    void Update() {
		RaycastHit hit;
		Ray myRay = Camera.main.ViewportPointToRay(new Vector3(0.5F,0.5F,0));
		if (Physics.Raycast(myRay, out hit)){
			if (hit.collider.gameObject.name == "FirstPlane" || hit.collider.gameObject.name == "TwitterPlane1" || hit.collider.gameObject.name == "TwitterPlane2"){
				hittingLeft = false;
				hittingRight = false;
				hittingFront=true;
			} else if (hit.collider.gameObject.name == "ThirdPlane" || hit.collider.gameObject.name == "TwitterPlane3" || hit.collider.gameObject.name == "TwitterPlane4") {
				hittingFront = false;
				hittingRight = false;
				hittingLeft=true;
			} else if (hit.collider.gameObject.name == "SecondPlane" || hit.collider.gameObject.name == "TwitterPlane5" || hit.collider.gameObject.name == "TwitterPlane6") {
				hittingFront = false;
				hittingLeft = false;
				hittingRight=true;
			} else {
				hittingFront = false;
				hittingLeft = false;
				hittingRight = false;
			}
		}
    }
}