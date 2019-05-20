using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class MenuManager : MonoBehaviour {
	public enum scenes{Menu, Game, LeaderBoards, HowToPlay, Credits};

	const float ADTIME = 300.0f;

	public GameObject menuUI;
	public GameObject inGameUI;
	public GameObject leaderBoardUI;
	public GameObject howToPlayUI;
	public GameObject creditsUI;
	public SceneTransition sceneTrans;
	public GameManager gameManager;
	public HowToPlayManager howToPlayManager;
	public UnityEngine.UI.Text lastScoreText;
	public UnityEngine.UI.Text bestScoreText;
	
	scenes m_curScene;
	bool m_sceneChanged;
	float m_adTimer;

	void Awake(){
		if(Advertisement.isSupported){
			Advertisement.allowPrecache = true;
			Advertisement.Initialize ("40627");
		}
	}

	void Start(){
		m_curScene = scenes.Menu;
		ScoreHandler.CheckForReset ();
		bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
		lastScoreText.text = PlayerPrefs.GetInt("LastScore", 0).ToString();
		m_adTimer = 300.0f;
	}
	
	void SwitchScene(scenes chosenScene){
		//Sets the current scene and starts a fading transition
		
		m_curScene = chosenScene;
		sceneTrans.StartFade();
		m_sceneChanged = false;
	}
	
	void Update(){
		m_adTimer += Time.deltaTime;

		//If the scene hasn't changed yet, check if fading out
		if(!m_sceneChanged){
			if(sceneTrans.FadingOut()){
				//Disabling all scene ui canvas's
				menuUI.GetComponent<Canvas>().enabled = false;
				inGameUI.GetComponent<Canvas>().enabled = false;
				leaderBoardUI.GetComponent<Canvas>().enabled = false;
				howToPlayUI.GetComponent<Canvas>().enabled = false;
				creditsUI.GetComponent<Canvas>().enabled = false;
				
				m_sceneChanged = true;
				
				switch(m_curScene){
				case scenes.Game:
					//Fades to the game music
					AudioHandler.instance.SwitchTo(AudioHandler.MusicState.GAME);
					//Activates the UI
					inGameUI.GetComponent<Canvas>().enabled = true;
					gameManager.startGame();
					break;
				case scenes.Menu:
					menuUI.GetComponent<Canvas>().enabled = true;
					gameManager.EndGame();
					bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
					lastScoreText.text = PlayerPrefs.GetInt("LastScore", 0).ToString();
					AudioHandler.instance.SwitchTo(AudioHandler.MusicState.MENU);
					break;
				case scenes.LeaderBoards:
					leaderBoardUI.GetComponent<Canvas>().enabled = true;
					leaderBoardUI.GetComponent<LeaderBoard>().PopulateScoreBoard();
					break;
				case scenes.HowToPlay:
					//If a how to play exists the scene will change overwise we stay
					if(howToPlayManager.ShowHowToPlay()){
						howToPlayUI.GetComponent<Canvas>().enabled = true;
					}else{
						menuUI.GetComponent<Canvas>().enabled = true;
					}
					break;
				case scenes.Credits:
					creditsUI.GetComponent<Canvas>().enabled = true;
					break;
				}
			}
		}
	}
	
	//Functions added to work with buttons (Buttons can't take an enum as a parameter)
	public void SwitchToMenu(){
		SwitchScene(scenes.Menu);
		if(m_adTimer > ADTIME){
			//StartCoroutine (ShowAdWhenReady());
			if(Advertisement.isReady()){
				Advertisement.Show ();
			}
			
			m_adTimer = 0.0f;
		}
	}
	
	public void SwitchToGame(){
		SwitchScene(scenes.Game);
	}
	
	public void SwitchToLeaderBoards(){
		SwitchScene(scenes.LeaderBoards);
	}
	
	public void SwitchToHowToPlay(){
		SwitchScene(scenes.HowToPlay);
	}
	
	public void SwitchToCredits(){
		SwitchScene(scenes.Credits);
	}
	
	public void ExitGame(){
		Application.Quit();
	}

	IEnumerator ShowAdWhenReady()
	{
		while (!Advertisement.isReady ())
			yield return null;
		
		Advertisement.Show ();
	}
}
