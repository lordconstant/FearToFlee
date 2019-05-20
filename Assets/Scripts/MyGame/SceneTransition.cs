using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour {
	public float fadeInDuration;
	public float fadeWaitDuration;
	public float fadeOutDuration;
	public UnityEngine.UI.Image fadeImage;

	float m_fadeTimer;
	bool m_fadingIn;
	bool m_fadeWaiting;
	bool m_fadingOut;
	bool m_fading;

	// Use this for initialization
	void Start () {
		m_fadeTimer = 0.0f;
		m_fading = false;
		m_fadingIn = false;
		m_fadeWaiting = false;
		m_fadingOut = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_fading){
			m_fadeTimer += Time.deltaTime;
			if(m_fadingIn){
				Color fadeCol = fadeImage.color;
				fadeCol.a += 1/(fadeInDuration)*Time.deltaTime;
				fadeImage.color = fadeCol;

				//If we have faded in start waiting
				if(m_fadeTimer > fadeInDuration){
					m_fadingIn = false;
					m_fadeWaiting = true;
				}
			}
			if(m_fadeWaiting){
				//If we have finished waiting fadeOut
				if(m_fadeTimer > fadeInDuration + fadeWaitDuration){
					m_fadeWaiting = false;
					m_fadingOut = true;
				}
			}

			if(m_fadingOut){
				Color fadeCol = fadeImage.color;
				fadeCol.a -= 1/(fadeOutDuration)*Time.deltaTime;
				fadeImage.color = fadeCol;

				//If we have faded out stop the transition
				if(m_fadeTimer > fadeInDuration + fadeWaitDuration + fadeOutDuration){
					m_fadingOut = false;
					m_fading = false;
				}
			}
		//Defaults the alpha back to 0 when fading is finished
		}else if(fadeImage.color.a != 0){
			Color fadeCol = fadeImage.color;
			fadeCol.a = 0;
			fadeImage.color = fadeCol;
		}
	}

	//Call the begin a fading transition
	public void StartFade(){
		m_fading = true;
		m_fadingIn = true;
		m_fadingOut = false;
		m_fadeWaiting = false;
		m_fadeTimer = 0.0f;
	}

	public bool FadingIn(){
		return m_fadingIn;
	}

	public bool FadingOut(){
		return m_fadingOut;
	}
}
