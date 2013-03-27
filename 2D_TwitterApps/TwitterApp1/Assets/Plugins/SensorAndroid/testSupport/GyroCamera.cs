using UnityEngine;
using System.Collections;


// Note: this rig is designed to be used in LandscapeLeft orientation
// Demonstrates very simple use of the gyro data.  This setup will work well for most simple
// situations where the user does not do through extreme y values (tiling the device all the way face up
// or down). If you need full 360 degree movement the gyro data needs to be corrected using the accelerometer and/or
// the magnetometer data
public class GyroCamera : MonoBehaviour
{
	public GUIText text;
	private Quaternion _rotationAdjustment;
	private GameObject cameraParent;
	

#if UNITY_ANDROID
	void Awake()
	{
		var cameraParent = new GameObject( "camParent" ); // make a new parent and child the camera to it
		cameraParent.transform.position = transform.position;
		transform.parent = cameraParent.transform;
		
		// set the rotationAdjustment for the gyro and rotate the camera parent to get things in line
		_rotationAdjustment = new Quaternion( 0f, 0, 0, 1f );
		cameraParent.transform.eulerAngles = new Vector3( 0, 180, 0 );
	}
	
	
	void Start()
	{
		SensorAndroid.startGyro();
	}
	

	void Update()
	{
		var tilt = SensorAndroid.getGyroData();
		text.text = string.Format( "x: {0}, y: {1}, z: {2}", tilt.x, tilt.y, tilt.z );
		transform.localRotation = Quaternion.Euler( tilt ) * _rotationAdjustment;
	}
	
	
	public void OnGUI()
	{
		if( GUI.Button( new Rect( Screen.width - 80, 0, 80, 80 ), "Reset" ) )
		{
			SensorAndroid.stopGyro();
			SensorAndroid.startGyro();
		}
		
		
		if( GUI.Button( new Rect( Screen.width - 80, Screen.height - 80, 80, 80 ), "Next Scene" ) )
		{
			// stop all sensors when changing scenes!
			SensorAndroid.stopGyro();
			Application.LoadLevel( "RotationCameraTestScene" );
		}
	}
#endif
}