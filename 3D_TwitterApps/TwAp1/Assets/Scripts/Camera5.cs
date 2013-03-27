using UnityEngine;
using System.Collections;

public class Camera5 : MonoBehaviour {

	public void Up (float distance) {
		transform.position += new Vector3(0,0.1F,0);
	}
	
	public void Down (float distance) {
		transform.position += new Vector3(0,-0.1F,0);	
	}
}
