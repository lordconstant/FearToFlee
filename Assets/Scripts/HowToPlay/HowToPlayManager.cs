using UnityEngine;
using System.Collections;

public class HowToPlayManager : MonoBehaviour {
	public GameObject[] slides = new GameObject[4];
	public MenuManager menuManager;
	public float swipeDelay = 0.5f;
	int m_activeSlide;

	float m_swipeTimer;
	bool m_activeScene;

	// Use this for initialization
	void Start () {
		m_activeScene = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_activeScene){
			m_swipeTimer += Time.deltaTime;

			if(m_swipeTimer > swipeDelay){
				if(TouchHandler.SwipedLeft()){
					PrevSlide();
					m_swipeTimer = 0.0f;
				}

				if(TouchHandler.SwipedRight()){
					NextSlide();
					m_swipeTimer = 0.0f;
				}
			}
		}
	}

	public bool ShowHowToPlay(){
		//If we have a how to play it returns true otherwise false
		if(slides.Length > 0){
			m_activeSlide = 0;
			slides[0].SetActive(true);
			m_swipeTimer = 0.0f;
			m_activeScene = true;
			return true;
		}else{
			return false;
		}
	}

	public void NextSlide(){
		if(m_activeSlide < slides.Length-1){
			slides[m_activeSlide].SetActive(false);
			m_activeSlide++;
			slides[m_activeSlide].SetActive(true);
		}else{
			//Return to menu if we are at end
			slides[m_activeSlide].SetActive(false);
			m_activeScene = false;
			menuManager.SwitchToMenu();
		}
	}

	public void PrevSlide(){
		if(m_activeSlide > 0){
			slides[m_activeSlide].SetActive(false);
			m_activeSlide--;
			slides[m_activeSlide].SetActive(true);
		}else{
			//Returns to the menu if we are at start
			slides[m_activeSlide].SetActive(false);
			m_activeScene = false;
			menuManager.SwitchToMenu();
		}
	}
}
