using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour {
	public int maxSize = 10;
	public GameObject scoreSlot;

	GameObject[] m_scoreSlots;

	// Use this for initialization
	void Start () {
		m_scoreSlots = new GameObject[maxSize];
		ScoreHandler.setMaxSize(maxSize);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PopulateScoreBoard(){
		//Wipes the scoreBoard and the populates it with the saved highscores
		ClearScoreBoard();

		ScoreHandler.scores = ScoreHandler.GetScores();

		ScoreHandler.scores = ScoreHandler.SortScores(ScoreHandler.scores);

		for(int i = 0; i < maxSize; i++){
			GameObject newSlot = Instantiate(scoreSlot, transform.position, Quaternion.identity) as GameObject;
			newSlot.transform.SetParent(transform, false);

			if(i < ScoreHandler.scores.Length){
				if(ScoreHandler.scores[i] > 0){
					newSlot.GetComponentInChildren<UnityEngine.UI.Text>().text = ScoreHandler.scores[i].ToString();
				}else{
					newSlot.GetComponentInChildren<UnityEngine.UI.Text>().text = "---";
				}
			}else{
				newSlot.GetComponentInChildren<UnityEngine.UI.Text>().text = "---";
			}

			m_scoreSlots[i] = newSlot;
		}
	}

	public void ClearScoreBoard(){
		for(int i = 0; i < m_scoreSlots.Length; i++){
			if(m_scoreSlots[i]){
				Destroy(m_scoreSlots[i]);
				m_scoreSlots[i] = null;
			}
		}
	}
}
