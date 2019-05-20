using UnityEngine;
using System.Collections;

[System.Serializable]
public class PowerUpHandler {
	public JumpPowerUp jumpPower;

	PowerUp m_activePower;
	GameObject m_objParent;

	// Use this for initialization
	public void StartPowerHandler (GameObject objParent) {
		m_activePower = jumpPower;
		m_objParent = objParent;
	}
	
	// Update is called once per frame
	public void UpdatePowerHandler () {
		jumpPower.PassiveUpdate(m_objParent, Time.deltaTime);

		if(m_activePower != null){
			m_activePower.UpdatePowerUp(m_objParent, Time.deltaTime);
		}
	}

	public void UsePowerUp(string powerName, AudioSource audSource){
		if(!m_activePower.IsActive()){
			switch(powerName){
			case "JumpPower":
				m_activePower = jumpPower;
				m_activePower.StartPowerUp(m_objParent, audSource);
				break;
			}
		}
	}
}
