using UnityEngine;
using System.Collections;

public class Camera4 : MonoBehaviour {

	public void Up (float distance) {
		transform.position += new Vector3(0,distance,0);
	}
	
	public void Down (float distance) {
		transform.position += new Vector3(0,(distance*-1.0f),0);
	}
}
