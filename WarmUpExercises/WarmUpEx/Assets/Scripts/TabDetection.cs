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
		foreach (Touch thisTouch in Input.touches) {
			Ray myRay = Camera.main.ScreenPointToRay(thisTouch.position);
			if (Physics.Raycast(myRay, out hit)){
				if (hit.collider.gameObject.name == "FrontPlane" || hit.collider.gameObject.name == "PoemPlane1"){
					hittingLeft = false;
					hittingRight = false;
					hittingFront=true;
				} else if (hit.collider.gameObject.name == "LeftPlane" || hit.collider.gameObject.name == "PoemPlane2") {
					hittingFront = false;
					hittingRight = false;
					hittingLeft=true;
				} else if (hit.collider.gameObject.name == "RightPlane" || hit.collider.gameObject.name == "PoemPlane3") {
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
}