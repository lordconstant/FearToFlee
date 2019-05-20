using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {
	public GameManager gameManager;
	public MenuManager menuManager;
	public PowerUpHandler powerHandler;

	public AudioClip hitClip;
	public AudioClip collectClip;

	// Use this for initialization
	void Start () {
		powerHandler.StartPowerHandler(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		powerHandler.UpdatePowerHandler();

		//Attemp to jump if we swipe up
		if(TouchHandler.SwipedUp()){
			powerHandler.UsePowerUp(powerHandler.jumpPower.powerName, audio);
		}
	}

	void OnTriggerEnter(Collider collision){
		//End the game if we hit an obstacle
		if(collision.transform.tag == "Obstacle"){
			Destroy(collision.gameObject);
			audio.PlayOneShot(hitClip);
			menuManager.SwitchToMenu();
			return;
		}

		//reduce our fear & destroy the object if we get a pill
		if(collision.transform.tag == "CollectablePill"){
			if(gameManager){
				gameManager.ReduceFear();
			}
			audio.PlayOneShot(collectClip);
			Destroy(collision.gameObject);
		}

		//Give ourselves a jump and destroy the powerUp
		if(collision.transform.tag == "JumpPowerUp"){
			powerHandler.jumpPower.amount++;
			Destroy(collision.gameObject);
			audio.PlayOneShot(collectClip);
		}
	}

	//Resets the amount of jumps we have
	void GameOver(){
		powerHandler.jumpPower.amount = 0;
	}

	public void UsePowerUp(string powerName){
		powerHandler.UsePowerUp(powerName, audio);
	}
}
