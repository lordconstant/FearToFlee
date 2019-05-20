using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	//Default speed of the game
	public float gameSpeed = 1.0f;
	//How fast you gain fear
	public float fearIncreaseRate = 2.0f;
	//Maximum amount of fear you can have
	public float maxFear = 10.0f;
	//How much speed you gain in relation to fear
	public float fearToSpeed = 0.5f;
	//How high the ScoreMultiplier is in relation to fear
	public float fearToScoreMultiplier = 1.0f;
	//How much fear is lost from a collectable
	public float fearRelief = 2.0f;
	//How much score is gained each increment
	public int scoreBaseIncreaseRate = 10;
	//How often score increments
	public float scoreIncreaseTime = 0.1f;
	//Mulplier marker on fear bar
	public GameObject fearUIMarker;
	public GameObject fearBar;

	public UnityEngine.UI.Text scoreText;

	public GameObject menuUI;
	public GameObject ingameUI;
	public GameObject fearSprite;
	public float m_fearBarMaxWidth;

	public GameObject pauseOverlay;

	int m_score = 0;
	float m_scoreMultiplier = 1.0f;
	//Current speed of the game
	float m_gameSpeed = 1.0f;
	//How much fear we have
	float m_fear = 1.0f;
	float m_scoreTimer = 0.0f;

	bool m_isPlaying;
	bool m_isPaused;

	// Use this for initialization
	void Start () {
		m_isPlaying = false;
		m_score = 0;
		m_gameSpeed = 0;

		//Calls the UpdateGameSpeed function on all child classes
		BroadcastMessage("UpdateGameSpeed", m_gameSpeed);

		//Calculating the size of our fear bar
		m_fearBarMaxWidth = fearBar.GetComponent<RectTransform>().rect.width - fearBar.GetComponent<HorizontalLayoutGroup>().padding.right - fearBar.GetComponent<HorizontalLayoutGroup>().padding.left;
		//Calculates spacing between markers
		int numMarkers = (int)(maxFear*fearToScoreMultiplier);
		float markerSpacing = m_fearBarMaxWidth/numMarkers;
		fearBar.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().spacing = markerSpacing;

		//Adds the necessary amount of markers and renames them to match multiplier
		for(int i = 0; i < numMarkers; i++){
			GameObject newMarker = Instantiate(fearUIMarker, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

			int numText = i+1;
			newMarker.GetComponentInChildren<UnityEngine.UI.Text>().text = "X" + numText.ToString();

			newMarker.transform.SetParent(fearBar.transform, false);

			//First marker is X1 and only used for spacing
			//Setting X scale to 0 to hide from view but still work with spacing
			if(i == 0){
				Vector3 tempScale = newMarker.transform.localScale;
				tempScale.x = 0;
				newMarker.transform.localScale = tempScale;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_isPlaying){
			m_scoreTimer += Time.deltaTime;

			if(m_scoreTimer >= scoreIncreaseTime){
				//Increases the score based on multiplier
				m_score += (int)(scoreBaseIncreaseRate * m_scoreMultiplier);
				m_scoreTimer = 0.0f;

				scoreText.text = m_score.ToString();
			}

			UpdateFear();
		}
	}

	void UpdateFear(){
		if(m_fear < maxFear){
			m_fear += Time.deltaTime * fearIncreaseRate;
		}

		if(m_fear > maxFear){
			m_fear = maxFear;
		}

		//Fear starts at 1, makes the fear bar start at 0
		float localFear = m_fear-1;
		//Calculates how much 1 fear is in relation to the maxWidth
		float fearUnit = (1/(maxFear-1)) * m_fearBarMaxWidth;
		//Resizes bar to match our current fear
		float fearBarSize = fearUnit * localFear;
		Vector2 tempSize = fearSprite.GetComponent<RectTransform>().sizeDelta;
		tempSize.x = fearBarSize;
		fearSprite.GetComponent<RectTransform>().sizeDelta = tempSize;

		//Calculates the current scoreMultiplier
		m_scoreMultiplier = m_fear * fearToScoreMultiplier;

		//Calcultes the current speed
		float newSpeed = gameSpeed + (m_fear * fearToSpeed);

		//Stops the game from moving at a rate less than 1
		if(newSpeed < 1.0f){
			newSpeed = 1.0f;
		}

		//Only updates the speed if increase is significant (Optimisation)
		if(newSpeed > m_gameSpeed + 0.05f || newSpeed < m_gameSpeed - 0.05f){
			SetGameSpeed(newSpeed);
		}
	}

	public void ReduceFear(){
		m_fear -= fearRelief;

		if(m_fear < 1.0f){
			m_fear = 1.0f;
		}

		UpdateFear();
	}

	public void SetGameSpeed(float newSpeed){
		m_gameSpeed = newSpeed;

		//Calls the UpdateGameSpeed function on all child classes
		BroadcastMessage("UpdateGameSpeed", m_gameSpeed);
	}

	public void startGame(){
		if(!m_isPlaying){
			m_isPlaying = true;

			m_fear = 1.0f;
			UpdateFear();

			SetGameSpeed(1);
		}
	}

	public void EndGame(){
		if(m_isPlaying){
			m_isPlaying = false;

			//Attempts to add our score to the high scores
			ScoreHandler.AddScore(m_score);
			PlayerPrefs.SetInt("LastScore", m_score);

			m_score = 0;
			m_scoreMultiplier = 1.0f;

			//Stops the game from playing
			SetGameSpeed(0);

			//Calls the Gameover function on all child classes
			BroadcastMessage("GameOver");
		}
	}

	public void TogglePause(){
		if(!m_isPaused){
			m_isPaused = true;
			m_isPlaying = false;
			pauseOverlay.SetActive(true);
			SetGameSpeed(0);
		}else{
			m_isPaused = false;
			m_isPlaying = true;
			pauseOverlay.SetActive(false);
			SetGameSpeed(1);
		}
	}
}
