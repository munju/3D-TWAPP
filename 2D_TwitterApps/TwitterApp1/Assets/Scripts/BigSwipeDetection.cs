/*
Swipe gesture for iPhone. Add this script and a GUIText component to an empty GO
With the GO selected in the inspector set:
 
swipeLength ---> this is how long you want the swipe to be. 25 pixels seems ok
swipeVariance ---> this is how far the drag can go 'off line'. 5 pixels either way seems ok
timeToSwipe ---> if you leave this at 0 then there is no timer. If you set it then this is how long
                    the user has to complete the swipe gesture
 
You can swipe as many fingers left or right and it will only pick up one of them
It will then allow further swipes when that finger has been lifed from the screen.
Typically its swipe > lift finger > swipe .......
 
Be aware that it sometimes does not pick up the iPhoneTouchPhase.Ended
This is either a bug in the logic (plz test) or as the TouchPhases are notoriously
inaccurate its could well be this or it could be iPhone 1.6 given that it is quirky with touches.
Anyhow it does not affect the working of the class
other than a dead once in a while which then rectifies itself on the next swipe so
no big deal.
 
No need for orientation as it will respect whatever you set.
*/
 
using UnityEngine;
using System.Collections;
 
public class TouchInfo
{
    public Vector2 touchPosition;
    public bool swipeComplete;
    public float timeSwipeStarted;
}
 
 
public class BigSwipeDetection : MonoBehaviour {
 
//public member vars
public int swipeLength;
public int swipeVariance;
public float timeToSwipe;
//private member vars
private GUIText swipeText;
private TouchInfo[] touchInfoArray;
private int activeTouch = -1;
 
//methods
    void Start()
    {
        //get a reference to the GUIText component
        swipeText = (GUIText) GetComponent(typeof(GUIText));
        touchInfoArray = new TouchInfo[5];
    }   
 
    void Update()
    {
        //touch count is a bit dodgy at the moment so add the extra check to see if there are no more than 5 touches
        if(Input.touchCount > 0 && Input.touchCount < 6)
        {
            foreach(Touch touch in Input.touches)
            {
                if(touchInfoArray[touch.fingerId] == null)
                        touchInfoArray[touch.fingerId] = new TouchInfo();
                       
                if(touch.phase == TouchPhase.Began)
                {
                    touchInfoArray[touch.fingerId].touchPosition = touch.position;
                    touchInfoArray[touch.fingerId].timeSwipeStarted = Time.time;
                }
                //check if withing swipe variance      
                if(touch.position.y > (touchInfoArray[touch.fingerId].touchPosition.y + swipeVariance))
                {
                    touchInfoArray[touch.fingerId].touchPosition = touch.position;
                }
                if(touch.position.y < (touchInfoArray[touch.fingerId].touchPosition.y - swipeVariance))
                {
                    touchInfoArray[touch.fingerId].touchPosition = touch.position;
                }
                //swipe right
                if((touch.position.x > touchInfoArray[touch.fingerId].touchPosition.x + swipeLength) && !touchInfoArray[touch.fingerId].swipeComplete
                    && activeTouch == -1)
                {
                    SwipeComplete("swipe right  ",  touch);
					Debug.Log("swipe right");
                }
                //swipe left
                if((touch.position.x < touchInfoArray[touch.fingerId].touchPosition.x - swipeLength) && !touchInfoArray[touch.fingerId].swipeComplete
                    && activeTouch == -1)
                {
                    SwipeComplete("swipe left  ",  touch);
					Debug.Log("swipe left");
                }
                //when the touch has ended we can start accepting swipes again
                if(touch.fingerId == activeTouch && touch.phase == TouchPhase.Ended)
                {
                    //Debug.Log("Ending " + touch.fingerId);
                    //if more than one finger has swiped then reset the other fingers so
                    //you do not get a double/triple etc. swipe
                    foreach(Touch touchReset in Input.touches)
                    {
                        touchInfoArray[touch.fingerId].touchPosition = touchReset.position; 
                    }
                    touchInfoArray[touch.fingerId].swipeComplete = false;
                    activeTouch = -1;
                }
            }          
        }   
    }
   
    void SwipeComplete(string messageToShow, Touch touch)
    {
        //Debug.Log(Time.time - touchInfoArray[touch.fingerId].timeSwipeStarted);
        Reset(touch);
        if(timeToSwipe == 0.0f || (timeToSwipe > 0.0f && (Time.time - touchInfoArray[touch.fingerId].timeSwipeStarted) <= timeToSwipe))
        {
            swipeText.text = messageToShow;
            //Do something here
        }
    }
   
    void Reset(Touch touch)
    {
        activeTouch = touch.fingerId;
        touchInfoArray[touch.fingerId].swipeComplete = true;       
    }
   
}
