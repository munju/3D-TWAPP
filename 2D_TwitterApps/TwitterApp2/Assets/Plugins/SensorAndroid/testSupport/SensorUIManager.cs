using UnityEngine;
using System.Collections.Generic;


public class SensorUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	public GUIText guiTextField;
	
	// bools to hold the current state so we can toggle the sensors on/off
	private bool gyroOn;
	private bool rotationOn;
	private bool magneticOn;
	private bool lightOn;
	private bool proximityOn;
	private bool tiltOn;
	private bool gyroRawReturn;
	
	
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 75 : 35;
		float heightPlus = height + 6.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Check for Sensors" ) )
		{
			Debug.Log( "hasGyro: " + SensorAndroid.hasGyro() );
			Debug.Log( "hasRotation: " + SensorAndroid.hasRotationVector() );
			Debug.Log( "hasMagneticSensor: " + SensorAndroid.hasMagneticSensor() );
			Debug.Log( "hasProximitySensor: " + SensorAndroid.hasProximitySensor() );
			Debug.Log( "hasLightSensor: " + SensorAndroid.hasLightSensor() );
			Debug.Log( "hasDeviceTiltCapabilities: " + SensorAndroid.hasDeviceTiltCapabilities() );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Gyro" ) )
		{
			if( gyroOn )
				SensorAndroid.stopGyro();
			else
				SensorAndroid.startGyro();
			gyroOn = !gyroOn;
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Rotation" ) )
		{
			if( rotationOn )
				SensorAndroid.stopRotation();
			else
				SensorAndroid.startRotation();
			rotationOn = !rotationOn;
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Magnetic Sensor" ) )
		{
			if( magneticOn )
				SensorAndroid.stopMagneticSensor();
			else
				SensorAndroid.startMagneticSensor();
			magneticOn = !magneticOn;
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Light Sensor" ) )
		{
			if( lightOn )
				SensorAndroid.stopLightSensor();
			else
				SensorAndroid.startLightSensor();
			lightOn = !lightOn;
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Proximity Sensor" ) )
		{
			if( proximityOn )
				SensorAndroid.stopProximitySensor();
			else
				SensorAndroid.startProximitySensor();
			proximityOn = !proximityOn;
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Toggle Device Tilt Sensors" ) )
		{
			if( tiltOn )
				SensorAndroid.stopDeviceTiltSensors();
			else
				SensorAndroid.startDeviceTiltSensors();
			tiltOn = !tiltOn;
		}
		
	
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Toggle Raw Gyro Data" ) )
		{
			SensorAndroid.setReturnRawGyroData( !gyroRawReturn );
			gyroRawReturn = !gyroRawReturn;
			
			guiTextField.text = "returning raw gyro data: " + gyroRawReturn;
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Gyro Data" ) )
		{
			guiTextField.text = "gyro data: " + SensorAndroid.getGyroData();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Rotation Data" ) )
		{
			// rotation data can be either the raw Vector3 in radians or a normalized quaternion
			guiTextField.text = "rotation data: " + SensorAndroid.getRotationData();
			//guiTextField.text = "rotation data: " + SensorAndroid.getNormalizedQuaternionFromRotation();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Magnetic Data" ) )
		{
			guiTextField.text = "magnetic data: " + SensorAndroid.getMagneticData();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Light Value" ) )
		{
			guiTextField.text = "light value: " + SensorAndroid.getLightValue();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Proximity Value" ) )
		{
			guiTextField.text = "proximity value: " + SensorAndroid.getProximityValue();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Device Tilt Data" ) )
		{
			guiTextField.text = "device tilt data: " + SensorAndroid.getDeviceTilt();
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Next Scene" ) )
		{
			// stop all sensors when changing scenes!
			SensorAndroid.stopGyro();
			SensorAndroid.stopLightSensor();
			SensorAndroid.stopMagneticSensor();
			SensorAndroid.stopProximitySensor();
			
			Application.LoadLevel( "GyroCameraTestScene" );
		}
	}
#endif
}