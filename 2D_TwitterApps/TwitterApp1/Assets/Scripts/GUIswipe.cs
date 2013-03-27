using UnityEngine;
using System.Collections;


[ExecuteInEditMode] 

public class GUIswipe : MonoBehaviour {
    
    // Internal variables for managing touches and drags
	private int selected = -1;
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;
	private float previousDelta = 0f;
	private const float inertiaDuration = 0.5f;
	
    public Vector2 scrollPosition;
	public bool moved = false;


    void Update()
    {
		if (Input.touchCount != 1)
		{
			selected = -1;

			if ( scrollVelocity != 0.0f )
			{
				// slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
				scrollPosition.x += frameVelocity * Time.deltaTime;
				if (scrollPosition.x > 10) {
					Debug.Log("Scroll Position: "+ scrollPosition);
					moved = true;
				}
				
				// after N seconds, we've stopped
				if (t >= inertiaDuration) scrollVelocity = 0.0f;
			}
			return;
		}
		
		Touch touch = Input.touches[0];
		if (touch.phase == TouchPhase.Began)
		{
			selected = 1;
			previousDelta = 0.0f;
			scrollVelocity = 0.0f;
			moved = false;
		}
		else if (touch.phase == TouchPhase.Canceled)
		{
			selected = -1;
			previousDelta = 0f;
			moved = false;
		}
		else if (touch.phase == TouchPhase.Moved)
		{
			// dragging
			selected = -1;
			previousDelta = touch.deltaPosition.x;
			scrollPosition.x += touch.deltaPosition.x;
			if (scrollPosition.x > 10) {
				moved = true;
				Debug.Log("Scroll Position: "+ scrollPosition);
			}
		}
		else if (touch.phase == TouchPhase.Ended)
		{
            // Was it a tap, or a drag-release?
            if ( selected > -1 )
            {
				moved = false;
	            //Debug.Log("Player selected row " + selected);
            }
			else
			{
				// impart momentum, using last delta as the starting velocity
				// ignore delta < 10; precision issues can cause ultra-high velocity
				if (Mathf.Abs(touch.deltaPosition.y) >= 10) 
					scrollVelocity = (int)(touch.deltaPosition.x / touch.deltaTime);
				timeTouchPhaseEnded = Time.time;
			}
		}
		
	}
}
 