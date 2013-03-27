using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {
	public RaycastHit hit;
	
	void Update() {	
		foreach (Touch thisTouch in Input.touches) {
			if (thisTouch.phase == TouchPhase.Ended){
				Ray myRay = Camera.main.ScreenPointToRay(thisTouch.position);
				if (Physics.Raycast(myRay, out hit)){
					if (hit.collider.gameObject.name == this.name){
						SensorAndroid.stopGyro();
						SensorAndroid.stopRotation();
						Application.Quit();
					}
				}
			}
		}
	}
}