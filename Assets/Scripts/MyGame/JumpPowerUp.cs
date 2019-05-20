using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class JumpPowerUp : PowerUp {
	public Animator jumpAnim;
	public UnityEngine.UI.Text amountText;
	public AudioClip jumpClip;

	//Called while not active powerUp
	public override void PassiveUpdate(GameObject objParent, float deltaTime){
//		if(m_powerActive){
//			if(jumpAnim.GetBool("jump")){
//				if (jumpAnim.GetCurrentAnimationClipState(0))
//				{
//					m_powerActive = false;
//					jumpAnim.SetBool("jump", false);
//					Debug.Log("LANDED");
//				}
//			}
//		}

		if(amount > maxAmount){
			amount = maxAmount;
		}else if(amount < 0){
			amount = 0;
		}

		amountText.text = amount.ToString();
	}
	
	//Called when powerup first used
	public override void StartPowerUp(GameObject objParent, AudioSource audSource){
		if(amount > 0 && !m_powerActive){
			m_powerTimer = 0.0f;
			m_powerActive = true;
			amount--;
			jumpAnim.SetBool("jump", true);
			audSource.PlayOneShot(jumpClip);
		}
	}
	
	//Updates the power up while it is active
	public override void UpdatePowerUp(GameObject objParent, float deltaTime){
		if(m_powerActive){
			if(PowerTimer(deltaTime)){
				EndPowerUp(objParent);
			}
		}
	}
	
	//Called when the power up is deactivated
	public override void EndPowerUp(GameObject objParent){
		m_powerActive = false;
		jumpAnim.SetBool("jump", false);
	}
}
