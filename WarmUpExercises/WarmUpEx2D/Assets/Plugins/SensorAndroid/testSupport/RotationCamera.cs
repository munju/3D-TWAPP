using UnityEngine;
using System.Collections;


// Note: this rig is designed to be used in LandscapeLeft orientation
// Demonstrates very simple use of the gyro data.  This setup will work well for most simple
// situations where the user does not do through extreme y values (tiling the device all the way face up
// or down). If you need full 360 degree movement the gyro data needs to be corrected using the accelerometer and/or
// the magnetometer data
public class RotationCamera : MonoBehaviour
{
	public GUIText text;
	private Quaternion _rotationAdjustment;
	private GameObject cameraParent;
	

#if UNITY_ANDROID	
	void Start()
	{
		SensorAndroid.startRotation();
	}

	
	void Update()
	{
		var tilt = SensorAndroid.getNormalizedQuaternionFromRotation();
		text.text = string.Format( "x: {0:0.0}, y: {1:0.0}, z: {2:0.0}, w: {3:0.0}", tilt.x, tilt.y, tilt.z, tilt.w );
		transform.rotation = tilt;
	}
	
	
	public void OnGUI()
	{
		if( GUI.Button( new Rect( Screen.width - 80, Screen.height - 80, 80, 80 ), "Next Scene" ) )
		{
			// stop all sensors when changing scenes!
			SensorAndroid.stopRotation();
			Application.LoadLevel( "SensorTestScene" );
		}
	}
#endif
}