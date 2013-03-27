using UnityEngine;
using System.Collections;
     
public class SwipeDetection : MonoBehaviour { 

	// Values to set:
    public float comfortZone = 70.0f;
    public float minSwipeDist = 14.0f;
    public float maxSwipeTime = 0.5f;
	public float swipeDist = 0.0f;
     
    private float startTime;
	private Vector2 startPos;
	private bool couldBeSwipe;
       
    public enum SwipeDirection {            
		None,
        Up,
        Down
    }
	
	public enum SwipePlane {
		None,
		Left,
		Right
	}
        
	public SwipePlane lastPlane = SwipeDetection.SwipePlane.None;
	public SwipeDirection lastSwipe = SwipeDetection.SwipeDirection.None;
    public float lastSwipeTime;
       
    void  Update() {
		
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
          
            switch (touch.phase) {
                    case TouchPhase.Began:
                        lastSwipe = SwipeDetection.SwipeDirection.None;
                                            lastSwipeTime = 0;
                        couldBeSwipe = true;
                        startPos = touch.position;
                        startTime = Time.time;
                        break;
                   
                    case TouchPhase.Moved:
                        if (Mathf.Abs(touch.position.x - startPos.x) > comfortZone) {
                            //Debug.Log("Not a swipe. Swipe strayed " + (int)Mathf.Abs(touch.position.x - startPos.x) +
                            //          "px which is " + (int)(Mathf.Abs(touch.position.x - startPos.x) - comfortZone) +
                            //          "px outside the comfort zone.");
                            couldBeSwipe = false;
                        } else {
                            float swipeTime = Time.time - startTime;
                            swipeDist = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                       		testForPlane(touch);
                            if ((swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
                                // It's a swiiiiiiiiiiiipe!
                                float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                           
                                // If the swipe direction is positive, it was an upward swipe.
                                // If the swipe direction is negative, it was a downward swipe.
                                if (swipeValue > 0)
                                    lastSwipe = SwipeDetection.SwipeDirection.Up;
                                else if (swipeValue < 0)
                                    lastSwipe = SwipeDetection.SwipeDirection.Down;
                           
                                // Set the time the last swipe occured, useful for other scripts to check:
                                lastSwipeTime = Time.time;
							}			
						}
                        break;
                    case TouchPhase.Ended:
                        if (couldBeSwipe) {
                            float swipeTime = Time.time - startTime;
                            swipeDist = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                       		testForPlane(touch);
                            if ((swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
                                // It's a swiiiiiiiiiiiipe!
                                float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                           
                                // If the swipe direction is positive, it was an upward swipe.
                                // If the swipe direction is negative, it was a downward swipe.
                                if (swipeValue > 0)
                                    lastSwipe = SwipeDetection.SwipeDirection.Up;
                                else if (swipeValue < 0)
                                    lastSwipe = SwipeDetection.SwipeDirection.Down;
                           
                                // Set the time the last swipe occured, useful for other scripts to check:
                                lastSwipeTime = Time.time;
                                //Debug.Log("Found a swipe!  Direction: " + lastSwipe);        
					}        
				}        
				break;
			}
		}
	}

	void testForPlane(Touch touch) {
		RaycastHit hit;
		Ray myRay = Camera.main.ScreenPointToRay(touch.position);
		if (Physics.Raycast(myRay, out hit)){
			if (hit.collider.gameObject.name == "TwitterPlane1" || hit.collider.gameObject.name == "TwitterPlane4" || hit.collider.gameObject.name == "TwitterPlane5"){
				lastPlane = SwipeDetection.SwipePlane.Left;
			} else if (hit.collider.gameObject.name == "TwitterPlane2" || hit.collider.gameObject.name == "TwitterPlane3" || hit.collider.gameObject.name == "TwitterPlane6"){
				lastPlane = SwipeDetection.SwipePlane.Right;
			} else {
				lastPlane = SwipeDetection.SwipePlane.None;
			}
		}
	}
}
