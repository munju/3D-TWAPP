using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID
public class SensorAndroid
{
	private static AndroidJavaObject _plugin;
	
		
	static SensorAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.SensorPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}

	
	#region capability checks

	// checks to see if the device has a gyroscope
	public static bool hasGyro()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasGyro" );
	}


	// checks to see if the device has rotation vector support (request API level 9, Android 2.3+)
	public static bool hasRotationVector()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasRotation" );
	}

	
	// checks to see if the device has a magnetic sensor
	public static bool hasMagneticSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasMagneticSensor" );
	}

	
	// checks to see if the device has a light sensor
	public static bool hasLightSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasLightSensor" );
	}
	
	
	// checks to see if the device has a proximity sensor
	public static bool hasProximitySensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasProximitySensor" );
	}

	
	// checks to see if the device has the required sensors to calculate device tilt (magnetometer and accelerometer)
	public static bool hasDeviceTiltCapabilities()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _plugin.Call<bool>( "hasDeviceTiltCapabilities" );
	}
	
	#endregion
	
	
	#region start/stop methods
	
	// starts up the gyro listener
	public static void startGyro()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startGyro" );
	}
	
	
	// stops the gyro listener
	public static void stopGyro()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopGyro" );
	}
	
	
	// starts up the rotation vector listener
	public static void startRotation()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startRotation" );
	}
	
	
	// stops the rotation listener
	public static void stopRotation()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopRotation" );
	}
	
	
	// toggles if the gyro data returned should be the raw velocity or accumulated angle
	public static void setReturnRawGyroData( bool returnRawGyroData )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Set<bool>( "returnRawGyroData", returnRawGyroData );
	}	

	
	// starts up the magnetic sensor listener
	public static void startMagneticSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startMagneticSensor" );
	}
	
	
	// stops the magnetic sensor listener
	public static void stopMagneticSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopMagneticSensor" );
	}
	
	
	// starts up the light sensor listener
	public static void startLightSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startLightSensor" );
	}
	
	
	// stops the light sensor listener
	public static void stopLightSensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopLightSensor" );
	}
	
	
	// starts up the proximity sensor listener
	public static void startProximitySensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startProximitySensor" );
	}
	
	
	// stops the proximity sensor listener
	public static void stopProximitySensor()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopProximitySensor" );
	}
	
	
	// starts up the device tilt listeners
	public static void startDeviceTiltSensors()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "startDeviceTiltSensors" );
	}
	
	
	// stops the device tilt listeners
	public static void stopDeviceTiltSensors()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopDeviceTiltSensors" );
	}
	
	#endregion
	
	
	#region data getters	
	
	// when returnRawGyroData is false (default), the output of the gyroscope is integrated over time to calculate an angle.
	// hen returnRawGyroData is true, all values are in radians/second and measure the rate of rotation around the X, Y and Z axis
	public static Vector3 getGyroData()
	{
		if( Application.platform != RuntimePlatform.Android )
			return Vector3.zero;
		
		var data = _plugin.Get<float[]>( "gyroData" );
		return new Vector3( data[0], data[1], data[2] );
	}
	
	
	// returns the device rotation in radians
	public static Vector3 getRotationData()
	{
		if( Application.platform != RuntimePlatform.Android )
			return Vector3.zero;
		
		var data = _plugin.Get<float[]>( "rotationData" );
		return new Vector3( data[0], data[1], data[2] );
	}
	
	
	// uses the rotationData to produce a quaternion.  Useful for applying directly to the camear.
	public static Quaternion getNormalizedQuaternionFromRotation()
	{
		if( Application.platform != RuntimePlatform.Android )
			return Quaternion.identity;
		
		return fromVector( getRotationData() );
		
		// we can do this on the Android side also but their method has an oddity I haven't figured out yet
		//var data = _plugin.Call<float[]>( "getNormalizedQuaternionFromRotation" );
		//return new Quaternion( data[1], data[2], data[3], data[0] );
	}
	
	
	// helper to get a unit Quaternion from the rotation vector
	private static Quaternion fromVector( Vector3 v )
	{
		var q = new Quaternion( 0, 0, 0, 0 );
		
		// standard quart from rotation vector
		q.w = Mathf.Sqrt( 1 - v.x * v.x - v.y * v.y - v.z * v.z );
		q.x = v.x;
		q.y = v.y;
		q.z = v.z;
		
		// Sqrt can produce NaN so guard against it
		if( float.IsNaN( q.w ) )
			q.w = 0;
		
		// correct the quaternion
		q *= Quaternion.Euler( 90, 0, 0 );
		
		q.y *= -1;
		
		// swap w for q components
		var oldW = q.w;
		q.w = q.x;
		q.x = oldW;
		
		return q;
	}
	
	
	// all values are in micro-Tesla (uT) and measure the ambient magnetic field in the X, Y and Z axis
	public static Vector3 getMagneticData()
	{
		if( Application.platform != RuntimePlatform.Android )
			return Vector3.zero;
		
		var data = _plugin.Get<float[]>( "magneticData" );
		return new Vector3( data[0], data[1], data[2] );
	}
	
	
	// ambient light level in SI lux units
	public static float getLightValue()
	{
		if( Application.platform != RuntimePlatform.Android )
			return 0;
		
		return _plugin.Get<float>( "lightData" );
	}

	
	// proximity sensor distance measured in centimeters.  Note: Some proximity sensors only support a binary near or far measurement
	public static float getProximityValue()
	{
		if( Application.platform != RuntimePlatform.Android )
			return 0;
		
		return _plugin.Get<float>( "proximityData" );
	}
	
	
	// mock gyro-like data for devices that only have a magnetic sensor and accelerometer.  values returned are essentially
	// pitch, roll and yaw in degrees
	public static Vector3 getDeviceTilt()
	{
		if( Application.platform != RuntimePlatform.Android )
			return Vector3.zero;
		
		var data = _plugin.Call<float[]>( "getDeviceTilt" );
		return new Vector3( data[0], data[1], data[2] );
	}
	
	#endregion

}
#endif
