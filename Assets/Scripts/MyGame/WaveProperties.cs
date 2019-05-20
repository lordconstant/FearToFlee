using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("WaveDifficulty")]
public class WaveProperties{
	public enum waveTypes{NoWave, SinWave, CosWave, PingPongWave, TanWave, PerlinWave};
	public string waveName;
	//How long the wave will last
	public float waveDuration = 10;
	//The frequency of the wave
	public float waveXYFreq = 5;
	//The frequency of the Z wave
	public float waveZFreq = 5;
	//The frequency of the Modifier wave
	public float waveXYModFreq = 5;
	//The Amplitude of the Modifier wave
	public float waveXYAmp = 5;
	//The Amplitude of the Z Wave
	public float waveZAmp = 5;
	//Spawn rate of the wave
	public float obstacleSpawnRate = 0.2f;
	//How many objects will be spawned each call
	public int spawnCount = 1;
	//Percentage based distance offset of objects based on equal distance apart as 100
	[Range (0, 100)]
	public float obstacleOffset = 100;
	//What zWave we will be using
	public waveTypes zWave = waveTypes.SinWave;
	//What Modifier wave we will be using
	public waveTypes xyWave = waveTypes.SinWave;
	//Chance of a collectable spawning in relation to an obstacle
	[Range (0, 100)]
	public float collectableSpawnChance = 5.0f;
	//Chance of a PowerUp spawning in relation to an obstacle
	[Range (0, 100)]
	public float PowerUpSpawnChance = 1.0f;

	//Any gameobjects in these arrays need to be stored in a resource folder!
	[XmlIgnore]
	public List<GameObject> waveObstacle = new List<GameObject>();
	[XmlIgnore]
	public List<GameObject> waveCollectables = new List<GameObject>();
	[XmlIgnore]
	public List<GameObject> wavePowerUps = new List<GameObject>();

	//Used for storing object names for serialization
	[XmlArray("ObsNames"),XmlArrayItem("ObsName")]
	public string[] m_obsNames;
	[XmlArray("ColsNames"),XmlArrayItem("ColsName")]
	public string[] m_collNames;
	[XmlArray("PowerUpNames"),XmlArrayItem("PowerUpName")]
	public string[] m_powerUpNames;

	float m_waveTimer = 0.0f;
	float m_waveSpawnTimer = 0.0f;
	
	bool m_isWaveFinished = false;
	bool m_isWaveReady = false;

	public WaveProperties(){
	}

	public void ResetSpawnTimer(){
		m_waveSpawnTimer = 0.0f;
	}
	
	public bool UpdateWave(float deltaTime){
		//Increases timers based on delta time
		m_waveTimer += deltaTime;
		m_waveSpawnTimer += deltaTime;
		
		if(m_waveTimer > waveDuration){
			m_isWaveFinished = true;
		}

		//If we can spawn a new object return true otherwise false
		if(m_waveSpawnTimer > obstacleSpawnRate){
			return true;
		}else{
			return false;
		}
	}
	
	public bool IsWaveFinished(){
		return m_isWaveFinished;
	}
	
	public bool IsWaveReady(){
		return m_isWaveReady;
	}
	
	// Run this before starting a wave
	public void PrepareWave(){
		m_waveTimer = 0.0f;
		m_waveSpawnTimer = 0.0f;
		m_isWaveFinished = false;
		m_isWaveReady = true;
	}

	//Call this before Serialzing the class
	public void OnBeforeSerialize(){
		//Moves the gameobjects into string arrays for serialization
		if(waveObstacle != null){
			m_obsNames = new string[waveObstacle.Count];

			for(int i = 0; i < waveObstacle.Count; i++){
				if(waveObstacle[i] != null){
					m_obsNames[i] = waveObstacle[i].transform.name;
				}else{
					m_obsNames[i] = "";
				}
			}
		}

		if(waveCollectables != null){
			m_collNames = new string[waveCollectables.Count];
			
			for(int i = 0; i < m_collNames.Length; i++){
				if(waveCollectables[i] != null){
					m_collNames[i] = waveCollectables[i].name;
				}else{
					m_collNames[i] = "";
				}
			}
		}

		if(wavePowerUps != null){
			m_powerUpNames = new string[wavePowerUps.Count];

			for(int i = 0; i < m_powerUpNames.Length; i++){
				if(wavePowerUps[i] != null){
					m_powerUpNames[i] = wavePowerUps[i].name;
				}else{
					m_powerUpNames[i] = "";
				}
			}
		}
	}

	//Call this after Deserializing the class
	public void OnAfterDeserialize(){
		//Converts the name arrays back to gameobject array by loading the gameobjects by name
		waveObstacle = new List<GameObject>();
		if(m_obsNames != null){
			for(int i = 0; i < m_obsNames.Length; i++){
				GameObject foundObj = Resources.Load(m_obsNames[i]) as GameObject;

				waveObstacle.Add(foundObj);
			}
		}

		waveCollectables = new List<GameObject>();
		if(m_collNames != null){
			for(int i = 0; i < m_collNames.Length; i++){
				GameObject foundObj = Resources.Load(m_collNames[i]) as GameObject;
				
				waveCollectables.Add(foundObj);
			}
		}

		wavePowerUps = new List<GameObject>();
		if(m_powerUpNames != null){
			for(int i = 0; i < m_powerUpNames.Length; i++){
				GameObject foundObj = Resources.Load(m_powerUpNames[i]) as GameObject;
				
				wavePowerUps.Add(foundObj);
			}
		}
	}
}
