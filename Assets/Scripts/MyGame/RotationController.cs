using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour {
	//Speed at which game rotates
	public float speed = 2.0f;
	public GameObject floor;

	float m_gameSpeed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HandleTouch();

		//Used for handling mouse input for inEditor testing
		if(Input.GetMouseButton(0)){
			if(Input.mousePosition.x < Screen.width/2){
				floor.transform.Rotate(new Vector3(0, (-speed * m_gameSpeed) * Time.deltaTime, 0));
			}else{
				floor.transform.Rotate(new Vector3(0, (speed * m_gameSpeed) * Time.deltaTime, 0));
			}
		}
	}

	void HandleTouch(){
		if(floor){
			//Gets touchInput to rotate the game
			//Rotates faster as the game gets faster
			if(TouchHandler.TappedLeft()){
				floor.transform.Rotate(new Vector3(0, (-speed * m_gameSpeed) * Time.deltaTime, 0));
			}
			if(TouchHandler.TappedRight()){
				floor.transform.Rotate(new Vector3(0, (speed * m_gameSpeed) * Time.deltaTime, 0));
			}
		}
	}

	void UpdateGameSpeed(float gameSpeed){
		m_gameSpeed = gameSpeed;
	}
}
