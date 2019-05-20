using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveHandler{
	public WaveDifficulty[] difficulties;
	int m_curDiff;

	List<WaveProperties> m_waveOptions;
	WaveProperties m_curWave;

	public void StartHandler(){
		//Loads in the waves and then runs their deserialzation function
		difficulties = Serializer.Load<WaveDifficulty[]>("WaveDifficulties.xml");

		for(int i = 0; i < difficulties.Length; i++){
			difficulties[i].OnAfterDeserialize();
		}

		RestartHandler();
	}

	public void RestartHandler(){
		//Resets the wave options then prepares the first wave
		m_waveOptions = new List<WaveProperties>();

		m_curDiff = 0;

		AddToWaves(difficulties[m_curDiff].waves);
		PickNewWave();
	}

	public string UpdateHandler(float deltaTime){
		//Increases difficulty based on duration of current difficulty
		if(difficulties != null){
			if(difficulties[m_curDiff].UpdateDifficulty(deltaTime)){
				if(m_curDiff < difficulties.Length-1){
					m_curDiff++;
					AddToWaves(difficulties[m_curDiff].waves);
				}
			}

			if(m_curWave != null){
				if(m_curWave.IsWaveFinished()){
					PickNewWave();
				}
			}
		}else{
			if(m_curDiff != 0){
				m_curDiff = 0;
			}else{
				Debug.Log("No Difficulties Exist");
			}
		}

		return m_curWave.waveName;
	}

	public WaveProperties GetCurWave(){
		return m_curWave;
	}

	void PickNewWave(){
		//Randomly picks a new wave from allowed waves and then preps it for the game
		int chosenWave = Random.Range(0, m_waveOptions.Count);
		m_curWave = m_waveOptions[chosenWave];
		m_curWave.PrepareWave();
	}

	void AddToWaves(WaveProperties[] newWaves){
		for(int i = 0; i < newWaves.Length; i++){
			m_waveOptions.Add(newWaves[i]);
		}
	}
}
