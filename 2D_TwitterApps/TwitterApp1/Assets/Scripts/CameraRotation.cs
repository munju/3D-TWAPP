using UnityEngine;
using System.Collections;
using System.IO;

public class CameraRotation : MonoBehaviour {

	// Skins
	public GUISkin mySkin;
	
	// Tab Variables
	private TabDetection tab;
	private Camera1 cam1;
	private Camera2 cam2;	
	private Camera3 cam3;
	private Camera4 cam4;
	private Camera5 cam5;	
	private Camera6 cam6;
	private float scrollCoef;
	private float startTab1dwn;
	private float startTab1up;
	private float tab1Position;
	private float startTab2dwn;
	private float startTab2up;
	private float tab2Position;
	private float startTab3dwn;
	private float startTab3up;
	private float tab3Position;
	private float startTab4dwn;
	private float startTab4up;
	private float tab4Position;
	private float startTab5dwn;
	private float startTab5up;
	private float tab5Position;
	private float startTab6dwn;
	private float startTab6up;
	private float tab6Position;
	
	private float smooth = 5.0f;
	private Vector3 endV;
	private Lerpy didLerp = Lerpy.None;
	private LerpyPosition lerpyPos = LerpyPosition.One;
		
	public enum LerpyPosition {
		One,
		Two,
		Three
	}
	
	public enum Lerpy {
		None,
		Left,
		Right
	}
	
	#if UNITY_ANDROID
	// Setting up
	void Awake()
	{

	
	}
	
	// Initialisation
	void Start () {
		
		// Initialisation of the tab scrolling variables
		startTab1dwn = 0;
		startTab1up = 150;
		tab1Position = 150;
		startTab2dwn = 0;
		startTab2up = 5;
		tab2Position = 5;
		startTab3dwn = 0;
		startTab3up = 9;
		tab3Position = 9;
		startTab4dwn = 0;
		startTab4up = 14;
		tab4Position = 14;
		
		scrollCoef = 700;
	}
	
	void Update () {
		if (didLerp == Lerpy.Right) {
			if (lerpyPos == LerpyPosition.One) {
				endV = new Vector3(-300,150,0);
				Vector3 temp =  Vector3.Lerp(transform.position, endV,  Time.deltaTime * smooth);
				transform.position = temp;
			} else if (lerpyPos == LerpyPosition.Two) {
				endV = new Vector3(-600,150,0);
				Vector3 temp =  Vector3.Lerp(transform.position, endV,  Time.deltaTime * smooth);
				transform.position = temp;				
			} else {/*nothing*/}
		} else if (didLerp == Lerpy.Left){
			if (lerpyPos == LerpyPosition.Two) {
				endV = new Vector3(0,150,0);
				Vector3 temp =  Vector3.Lerp(transform.position, endV,  Time.deltaTime * smooth);
				transform.position = temp;			
			} else if (lerpyPos == LerpyPosition.Three) {
				endV = new Vector3(-300,150,0);
				Vector3 temp =  Vector3.Lerp(transform.position, endV,  Time.deltaTime * smooth);
				transform.position = temp;						
			} else {/*nothing*/}
		} else {/*nothing*/}
	}
	
	void OnGUI() {
		GUI.skin = mySkin;
		
		if( GUI.Button( new Rect( Screen.width - 80, 0, 80, 80 ), "Exit" ) ){
			Application.Quit();
		}		
		tab = (TabDetection)FindObjectOfType(typeof(TabDetection));
		SwipeDetection swipe = (SwipeDetection)FindObjectOfType(typeof(SwipeDetection));
			
		// We are facing the Front Plane
		if (tab.hittingFront == true) {
			if( GUI.Button( new Rect( Screen.width - 80, Screen.height/2, 80, 80 ), ">" ) ){
				lerpyPos = LerpyPosition.One;
				didLerp = Lerpy.Right;
			}
			cam1 = (Camera1)FindObjectOfType(typeof(Camera1));
			cam2 = (Camera2)FindObjectOfType(typeof(Camera2));
			// Left Swiping Plane
			if (swipe.lastPlane == SwipeDetection.SwipePlane.Left) {
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
					swipe.lastPlane = SwipeDetection.SwipePlane.None;
				} else if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
					if ((tab1Position - dist) > startTab1dwn) {
						cam1.Down(dist);
						tab1Position = tab1Position - dist;
					} else if ((tab1Position - dist) < startTab1dwn) {
						cam1.Down(tab1Position-startTab1dwn);
						tab1Position = startTab1dwn;
					} else {/* Do Nothing */}
					swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
					swipe.lastPlane = SwipeDetection.SwipePlane.None;						
				} else {/* Do Nothing */}
			// Right Swiping Plane
			} else if (swipe.lastPlane == SwipeDetection.SwipePlane.Right) {
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
					swipe.lastPlane = SwipeDetection.SwipePlane.None;
				} else if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
					float dist = swipe.swipeDist;
					dist = dist/scrollCoef;
				   	if ((tab2Position - dist) > startTab2dwn){
						cam2.Down(dist);
						tab2Position = tab2Position - dist;
					} else if ((tab2Position - dist) < startTab2dwn) {
						cam2.Down(tab2Position-startTab2dwn);
						tab2Position = startTab2dwn;
					} else {/* Do Nothing */}
					swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
					swipe.lastPlane = SwipeDetection.SwipePlane.None;					
				} else {/* Do Nothing */}					
			} else {/* Do Nothing */}
		// We are facing the Left Plane
		} else if (tab.hittingLeft == true) {
			if( GUI.Button( new Rect( 0, Screen.height/2 , 80, 80 ), "<" ) ){
				lerpyPos = LerpyPosition.Three;
				didLerp = Lerpy.Left;
			}
			cam3 = (Camera3)FindObjectOfType(typeof(Camera3));
			cam4 = (Camera4)FindObjectOfType(typeof(Camera4));
			// Right Swiping Plane
				if (swipe.lastPlane == SwipeDetection.SwipePlane.Right) {
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
						swipe.lastPlane = SwipeDetection.SwipePlane.None;
					} else if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
						float dist = swipe.swipeDist;
						dist = dist/scrollCoef;
						if ((tab3Position - dist) > startTab3dwn) {
							cam3.Down(dist);
							tab3Position = tab3Position - dist;
						} else if ((tab3Position - dist) < startTab3dwn) {
							cam3.Down(tab3Position-startTab3dwn);
							tab3Position = startTab3dwn;
						} else {/* Do Nothing */}
						swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
						swipe.lastPlane = SwipeDetection.SwipePlane.None;						
					} else {/* Do Nothing */}
				// Left Swiping Plane
				} else if (swipe.lastPlane == SwipeDetection.SwipePlane.Left) {
					if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Down) {
						float dist = swipe.swipeDist;
						dist = dist/scrollCoef;
						if ((tab4Position + dist) < startTab4up){
							cam4.Up(dist);
							tab4Position = tab4Position + dist;
						} else if ((tab4Position+dist) > startTab4up) {
							cam4.Up(startTab4up-tab4Position);
							tab4Position = startTab4up;
						} else {/* Do Nothing */}
						swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
						swipe.lastPlane = SwipeDetection.SwipePlane.None;
					} else if (swipe.lastSwipe == SwipeDetection.SwipeDirection.Up) {
						float dist = swipe.swipeDist;
						dist = dist/scrollCoef;
				   		if ((tab4Position - dist) > startTab4dwn){
							cam4.Down(dist);
							tab4Position = tab4Position - dist;
						} else if ((tab4Position - dist) < startTab4dwn) {
							cam4.Down(tab4Position-startTab4dwn);
							tab4Position = startTab4dwn;
						} else {/* Do Nothing */}
						swipe.lastSwipe = SwipeDetection.SwipeDirection.None;
						swipe.lastPlane = SwipeDetection.SwipePlane.None;					
					} else {/* Do Nothing */}					
				} else {/* Do Nothing */}
		// We are facing the Right Plane
		} else if (tab.hittingRight == true) {
			if( GUI.Button( new Rect( Screen.width - 80, Screen.height/2 , 80, 80 ), ">" ) ){
				lerpyPos = LerpyPosition.Two;
				didLerp = Lerpy.Right;
			}
			if( GUI.Button( new Rect( 0, Screen.height/2 , 80, 80 ), "<" ) ){
				lerpyPos = LerpyPosition.Two;
				didLerp = Lerpy.Left;
			}			
			//cam5 = (Camera5)FindObjectOfType(typeof(Camera5));
			//cam6 = (Camera6)FindObjectOfType(typeof(Camera6));
		}
	}
	//---------------------------------------------------------------------------------
#endif
}
