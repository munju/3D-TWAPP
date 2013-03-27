using UnityEngine;
using System.Collections;
using System.IO;

public class CameraRotation : MonoBehaviour {

	// Skins
	public GUISkin mySkin;
	public GUISkin buttonSkin;
	
	// My Cube
	private ArrayList cube = new ArrayList();
	
	// Camera Objects
	private GameObject cameraParent;
	private Quaternion _rotationAdjustment;
	
	// Other
	private bool settings;
	private bool transCheck;
	private bool brightCheck;
	private bool updateCheck;
	private int count;
	
	// Drift Correction
	private float errorY;
	private float errorX;
	private float errorZ;
	
	// Slider Variables
	private float sliderValueTrans = 0.0f;
	private float sliderValueBright = 0.0f;
	private float sliderValueUpdates = 0.0f;
	
	// Tab Variables
	private TabDetection tab;
	private Camera1 cam1;
	private Camera2 cam2;	
	private Camera3 cam3;
	private float scrollCoef;
	private float tab1Position;
	private float startTab1dwn;
	private float startTab1up;
	private float tab2Position;
	private float startTab2dwn;
	private float startTab2up;
	private float tab3Position;
	private float startTab3dwn;
	private float startTab3up;
	
	#if UNITY_ANDROID
	// Setting up
	void Awake()
	{
		cameraParent = new GameObject( "camParent" ); // make a new parent and child the camera to it
		cameraParent.transform.position = transform.position;
		transform.parent = cameraParent.transform;
				
		// set the rotationAdjustment for the gyro and rotate the camera parent to get things in line
		_rotationAdjustment = new Quaternion( 0f, 0, 0, 1f );
		cameraParent.transform.eulerAngles = new Vector3( 0, 180, 0 );
		
		// setting up the small rotating cube for the settings screen
		GameObject lc = GameObject.Find("LittleCube");
		GameObject ic = GameObject.Find("InsideCube");
		lc.renderer.enabled = false;
		ic.renderer.enabled = false;
	}
	
	// Initialisation
	void Start () {
		
		// Creating an array with all the sides of the cube
		cube.Add("BottomPlane");
		cube.Add("BackPlane");
		cube.Add("FrontPlane");
		cube.Add("RightPlane");
		cube.Add("LeftPlane");
		cube.Add("LittleCube");
		
		// We don't want to start in the settings screen
		settings = false;
		
		// Initialisation of the tab scrolling variables
		startTab1dwn = 0;
		startTab1up = 3;
		tab1Position = 3;
		startTab2dwn = 0;
		startTab2up = 2;
		tab2Position = 2;
		startTab3dwn = 0;
		startTab3up = 2;
		tab3Position = 2;
		
		scrollCoef = 700;
		
		count = 0;
		
		// Starting the sensor we need
		SensorAndroid.startGyro();
		SensorAndroid.startRotation();
	}
	
	// Updating the camera position
	void Update () {
		if (!settings) {
			count++;
			//---------------------------------------------------------------------------
			//Calculating the rotation of the device
			Quaternion myRotation = SensorAndroid.getNormalizedQuaternionFromRotation() * _rotationAdjustment;
			transform.localRotation = myRotation;
			//---------------------------------------------------------------------------
			
			Vector3 temp = myRotation.eulerAngles;
			if (count > 5) {	
			if (80 < temp.x && temp.x < 100) {
				count = 0;
				settings = true;
				checkSliderValues();
				GameObject sp = GameObject.Find("SettingsPlane");
				sp.renderer.enabled = false;
				GameObject lc = GameObject.Find("LittleCube");
				lc.renderer.enabled = true;
				GameObject ic = GameObject.Find("InsideCube");
				ic.renderer.enabled = true;
				transform.localRotation = Quaternion.Euler( new Vector3(90,0,0)) * _rotationAdjustment;
			}
			}
		}
	}
	
	void OnGUI()
	{
		GUI.skin = mySkin;
		if (!settings) {
			
			tab = (TabDetection)FindObjectOfType(typeof(TabDetection));
			SwipeDetection swipe = (SwipeDetection)FindObjectOfType(typeof(SwipeDetection));
			
			if (tab.hittingFront == true) {
				cam1 = (Camera1)FindObjectOfType(typeof(Camera1));
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Down) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab1Position+dist) < startTab1up) {
						cam1.Up(dist);
						tab1Position = tab1Position + dist;
					} else if ((tab1Position+dist) > startTab1up) {
						cam1.Up(startTab1up-tab1Position);
						tab1Position = startTab1up;
					}else {/* Do Nothing */}
					swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
				}
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab1Position - dist) > startTab1dwn) {
						cam1.Down(dist);
						tab1Position = tab1Position - dist;
					} else if ((tab1Position - dist) < startTab1dwn) {
						cam1.Down(tab1Position-startTab1dwn);
						tab1Position = startTab1dwn;
					} else {/* Do Nothing */}
					swipe.lastSwipe= SwipeDetection.SwipeDirection.None;				
				}
			} else if (tab.hittingLeft == true) {
				cam2 = (Camera2)FindObjectOfType(typeof(Camera2));
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Down) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab2Position + dist) < startTab2up){
						cam2.Up(dist);
						tab2Position = tab2Position + dist;
					} else if ((tab2Position+dist) > startTab2up) {
						cam2.Up(startTab2up-tab2Position);
						tab2Position = startTab2up;
					} else {/* Do Nothing */}
					swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
				}
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
				   	if ((tab2Position - dist) > startTab2dwn){
						cam2.Down(dist);
						tab2Position = tab2Position - dist;
					} else if ((tab2Position - dist) < startTab2dwn) {
						cam2.Down(tab2Position-startTab2dwn);
						tab2Position = startTab2dwn;
					} else {/* Do Nothing */}
					swipe.lastSwipe= SwipeDetection.SwipeDirection.None;				
				}
			} else if (tab.hittingRight == true) {
				cam3 = (Camera3)FindObjectOfType(typeof(Camera3));
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Down) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab3Position+dist) < startTab3up) {
						cam3.Up(dist);
						tab3Position = tab3Position + dist;
					} else if ((tab3Position+dist) > startTab3up) {
						cam3.Up(startTab3up-tab3Position);
						tab3Position = startTab3up;
					}else {/* Do Nothing */}
					swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
				}
				if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab3Position - dist) > startTab3dwn) {
						cam3.Down(dist);
						tab3Position = tab3Position - dist;
					} else if ((tab3Position - dist) < startTab3dwn) {
						cam3.Down(tab3Position-startTab3dwn);
						tab3Position = startTab3dwn;
					} else {/* Do Nothing */}
					swipe.lastSwipe= SwipeDetection.SwipeDirection.None;				
				}
			} else {
			}
		} else {
			GUI.skin = buttonSkin;
			// Exit/Quit Button
			if( GUI.Button( new Rect( 20, Screen.height - 100, 120, 80 ), "Quit" ) )
			{
				settings = false;
				saveState();
				SensorAndroid.stopGyro();
				SensorAndroid.stopRotation();
				Application.Quit();
			}
			
			// Unfreeze Button
			if( GUI.Button( new Rect( Screen.width - 140, Screen.height - 100, 120, 80 ), "Un-freeze" ) )
			{//( Screen.width - 140, Screen.height - 100, 120, 80 )
				settings = false;
				saveState();
				GameObject sp = GameObject.Find("SettingsPlane");
				sp.renderer.enabled = true;
				GameObject lc = GameObject.Find("LittleCube");
				lc.renderer.enabled = false;
				GameObject ic = GameObject.Find("InsideCube");
				ic.renderer.enabled = false;
			}
			
			//--------------------------------------------------------------------------------------
			GUI.skin = mySkin;
			
			// Transparency Buttons
			if( GUI.Button( new Rect( 20, Screen.height/2-300, 120, 100 ), "-" ) )
			{
				if (sliderValueTrans == 0) {
				} else {
					sliderValueTrans = sliderValueTrans-0.1f;	
				}
			}
			
			if( GUI.Button( new Rect( Screen.width-140, Screen.height/2-300, 120, 100 ), "+" ) )
			{
				if (sliderValueTrans == 100) {
				} else {
					sliderValueTrans = sliderValueTrans+0.1f;	
				}
			}			
			
			GUI.Label(new Rect(Screen.width/2-150, Screen.height/2-350, 300, 80), "Transparency");
			sliderValueTrans = GUI.HorizontalSlider( new Rect(Screen.width/2-300, Screen.height/2-270, 600, 30), sliderValueTrans, 0.0f, 1.0f);
			
			adjustTransparency();
			
			//--------------------------------------------------------------------------------------
			
			// Brightness Buttons
			if( GUI.Button( new Rect( 20, Screen.height/2-100, 120, 100 ), "-" ) )
			{
				if (sliderValueBright == 0) {
				} else {
					sliderValueBright = sliderValueBright-1f;	
				}
			}
			
			if( GUI.Button( new Rect( Screen.width-140, Screen.height/2-100, 120, 100 ), "+" ) )
			{
				if (sliderValueBright == 100) {
				} else {
					sliderValueBright = sliderValueBright+1f;	
				}
			}			
			
			GUI.Label(new Rect(Screen.width/2-150, Screen.height/2-140, 300, 80), "Brightness");
			sliderValueBright = GUI.HorizontalSlider( new Rect(Screen.width/2-300, Screen.height/2-70, 600, 30), sliderValueBright, 0.0f, 8.0f);
			
			adjustBrightness();

			//--------------------------------------------------------------------------------------
			
			// Updates Buttons
			if( GUI.Button( new Rect( 20, Screen.height/2+120, 120, 100 ), "-" ) )
			{
				if (sliderValueUpdates == 0) {
				} else {
					sliderValueUpdates = sliderValueUpdates-1f;	
				}
			}
			
			if( GUI.Button( new Rect( Screen.width-140, Screen.height/2+120, 120, 100 ), "+" ) )
			{
				if (sliderValueUpdates == 100) {
				} else {
					sliderValueUpdates = sliderValueUpdates+1f;	
				}
			}
			
			GUI.Label(new Rect(Screen.width/2-250, Screen.height/2+70, 500, 80), "News Update per hour");
			sliderValueUpdates = GUI.HorizontalSlider( new Rect(Screen.width/2-300, Screen.height/2+150, 600, 30), sliderValueUpdates, 0.0f, 120.0f);
		}
	}
	
	//---------------------------------------------------------------------------------
	// Dealing with Sliders
	
	// Adjusting the transparency
	private void adjustTransparency() {
		foreach (string s in cube) {
			GameObject plane = GameObject.Find(s);
			Color transColor = plane.renderer.material.color;
			transColor.a = sliderValueTrans;
			plane.renderer.material.color = transColor;
		}
	}
	
	// Adjusting the brightness
	private void adjustBrightness() {
		GameObject pl = GameObject.Find("Main Light");
		pl.light.intensity = sliderValueBright;
	}
	
	// Saving the current state of the Sliders
	private void saveState() {
		writeFile(Application.persistentDataPath, "trans", sliderValueTrans+"");
		writeFile(Application.persistentDataPath, "bright", sliderValueBright+"");
		writeFile(Application.persistentDataPath, "updates", sliderValueUpdates+"");
	}
	
	// Checking Slider Values
	private void checkSliderValues() {
		// Updates
		if (doesFileExist(Application.persistentDataPath, "updates") == 0) {
			float updates = System.Convert.ToSingle(readFile(Application.persistentDataPath, "updates"));
			sliderValueUpdates = updates;
		} else if (doesFileExist(Application.persistentDataPath, "updates") == 1) {
			writeFile(Application.persistentDataPath, "updates", "60.0");
			sliderValueUpdates = 60f;
		}
		
		// Transparancy
		if (doesFileExist(Application.persistentDataPath, "trans") == 0) {
			float trans = System.Convert.ToSingle(readFile(Application.persistentDataPath, "trans"));
			sliderValueTrans = trans;
		} else if (doesFileExist(Application.persistentDataPath, "trans") == 1) {
			writeFile(Application.persistentDataPath, "trans", "0.5f");
			sliderValueTrans = 0.5f;
		}
		
		// Brightness
		if (doesFileExist(Application.persistentDataPath, "bright") == 0) {
			float bright = System.Convert.ToSingle(readFile(Application.persistentDataPath, "bright"));
			sliderValueBright = bright;
		} else if (doesFileExist(Application.persistentDataPath, "bright") == 1) {
			writeFile(Application.persistentDataPath, "bright", "2.9f");
			sliderValueBright = 2.9f;
		}
	}
	
	//---------------------------------------------------------------------------------
	// Location and Rotation Adjustments
	
	// Calculating where North is using the Magnetic Field Sensor
	private float getDirection(Vector3 mag) {
		float direction = 0f;
		if (mag.y == 0 && mag.x > 0) {
			direction = 0f;
		} else if (mag.y == 0 && mag.x < 0) {
			direction = 180f;
		} else if (mag.y > 0) {
			float temp = (Mathf.Atan(mag.x/mag.y))*180/Mathf.PI;
			direction = 90f - temp;
		} else if (mag.y < 0) {
			float temp = (Mathf.Atan(mag.x/mag.y))*180/Mathf.PI;
			direction = 270f - temp;
		}
		return direction;
	}
	
	//---------------------------------------------------------------------------------
	// File IO Stuff
		
	private int doesFileExist(string dirPath, string fileName) {
		int result;
		
		if (dirPath != null && dirPath.Length > 0) {
			string filePath = dirPath + "/" + fileName;
			if (File.Exists(filePath)) {
				result = 0;
			} else {
				result = 1;
			}
		} else {
			result = 2;
		}
		return result;
	}
	
	private string writeFile(string dirPath, string fileName, string output) {
		string result = "";
		
		if (dirPath != null && dirPath.Length > 0) {
			StreamWriter writer = File.CreateText(dirPath + "/" + fileName);
			writer.WriteLine(output);
			writer.Close();
			result = "done";
		} else {
			result = "error";
		}
		
		return result;
	}
	
	private string readFile(string dirPath, string fileName) {
		string result = "";
		
		if (dirPath != null && dirPath.Length > 0) {
			StreamReader reader = File.OpenText(dirPath + "/" + fileName);
			result = reader.ReadLine();
			reader.Close();
		} else {
			result = "error";
		}
		
		return result;
	}
#endif
}