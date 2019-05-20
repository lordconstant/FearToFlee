using UnityEngine;
using System.Collections;

public class TouchHandler : MonoBehaviour {
	//How fast we need to swipe to register the swipe
	public Vector2 swipeTolerance;
	static bool m_swipeUp;
	static bool m_swipeDown;
	static bool m_swipeRight;
	static bool m_swipeLeft;
	static bool m_touchLeft;
	static bool m_touchRight;

	bool m_touching;
	bool m_swiped;
	float m_swipeTime;

	Vector2 m_touchStart;
	Vector2 m_touchVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_touchLeft = false;
		m_touchRight = false;

		if(Input.touchCount > 0){
			Touch firstTouch = Input.GetTouch(0);
			Vector2 touchScreenPos = firstTouch.position;
		
			//If first time touching
			if(!m_touching){
				//Stores initial touch pos & time of touch
				m_touchStart = touchScreenPos;
				m_swipeTime = Time.time;
				m_touchVelocity = new Vector2(0, 0);
			}else{
				//Calculating velocity of the swipe based on time and position between the first touch
				m_touchVelocity = touchScreenPos - m_touchStart;
				m_touchVelocity.y = m_touchVelocity.y/(Time.time-m_swipeTime);
				m_touchVelocity.x = m_touchVelocity.x/(Time.time-m_swipeTime);
			}

			//Stops the ability to swipe diagonally
			if(!m_swiped){
				if(m_touchVelocity.y * m_touchVelocity.y > m_touchVelocity.x * m_touchVelocity.x){
					if(m_touchVelocity.y > swipeTolerance.y){
						m_swipeUp = true;
						m_swiped = true;
					}else{
						m_swipeUp = false;
					}

					if(m_touchVelocity.y < -swipeTolerance.y){
						m_swipeDown = true;
						m_swiped = true;
					}else{
						m_swipeDown = false;
					}
				}else{
					if(m_touchVelocity.x > swipeTolerance.x){
						m_swipeRight = true;
						m_swiped = true;
					}else{
						m_swipeRight = false;
					}

					if(m_touchVelocity.x < -swipeTolerance.x){
						m_swipeLeft = true;
						m_swiped = true;
					}else{
						m_swipeLeft = false;
					}
				}
			}

			//If we are touching the left or right of the screen
			if(touchScreenPos.x < Screen.width/2){
				m_touchLeft = true;
			}else{
				m_touchRight = true;
			}

			m_touching = true;
		}else{
			//If we remove our finger we aren't doing anything
			m_touching = false;
			m_swipeDown = false;
			m_swipeUp = false;
			m_swipeRight = false;
			m_swipeLeft = false;
			m_swiped = false;
		}
	}

	public static bool TappedLeft(){
		return m_touchLeft;
	}

	public static bool TappedRight(){
		return m_touchRight;
	}

	public static bool SwipedUp(){
		return m_swipeUp;
	}

	public static bool SwipedDown(){
		return m_swipeDown;
	}

	public static bool SwipedRight(){
		return m_swipeRight;
	}

	public static bool SwipedLeft(){
		return m_swipeLeft;
	}
}
