using UnityEngine;
using System.Collections;

public abstract class PowerUp {
	//Used for finding a powerup icon: powerName_Icon
	public string powerName;
	//Number of powerup held
	public int amount = 2;

	public int maxAmount = 3;
	//How long the powerup effects last
	public float duration;

	protected float m_powerTimer;

	protected bool m_powerActive = false;

	//Called while not active powerUp
	public abstract void PassiveUpdate(GameObject objParent, float deltaTime);

	//Called when powerup first used
	public abstract void StartPowerUp(GameObject objParent, AudioSource audSource);

	//Updates the power up while it is active
	public abstract void UpdatePowerUp(GameObject objParent, float deltaTime);

	//Called when the power up is deactivated
	public abstract void EndPowerUp(GameObject objParent);

	protected bool PowerTimer(float deltaTime){
		m_powerTimer += deltaTime;

		if(m_powerTimer >= duration){
			return true;
		}else{
			return false;
		}
	}

	public bool IsActive(){
		return m_powerActive;
	}
}
