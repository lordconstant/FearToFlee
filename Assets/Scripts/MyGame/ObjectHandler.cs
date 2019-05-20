using UnityEngine;
using System.Collections;

public class ObjectHandler : MonoBehaviour {
	float m_speed = 1.0f;
	float m_gameSpeed = 1.0f;

	bool m_invincible;

	// Use this for initialization
	void Start () {
		m_invincible = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Move the object forward bsed on the gameSpeed
		Vector3 tempPos = transform.position;
		tempPos.z -= (m_speed * m_gameSpeed) * Time.deltaTime;
		transform.position = tempPos;

		//Destroys itself when its past the camera
		if(transform.position.z < -10){
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.GetComponent<ObjectHandler>() != null){
			//If the other object isn't invincible destroy it and make us invincicle
			//Used to only destroy one object if two spawn in each other
			if(!other.gameObject.GetComponent<ObjectHandler>().IsInvicible()){
				Destroy(other.gameObject);
				m_invincible = true;
			}
		}
	}

	public void SetSpeed(float speed){
		m_speed = speed;
	}

	public void UpdateGameSpeed(float gameSpeed){
		m_gameSpeed = gameSpeed;
	}

	public void GameOver(){
		Destroy(gameObject);
	}

	public bool IsInvicible(){
		return m_invincible;
	}
}
