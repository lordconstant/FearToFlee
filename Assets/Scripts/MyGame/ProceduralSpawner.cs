using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ProceduralSpawner : MonoBehaviour {
	//Delegate for handling the different wave functions
	//Freq = frequency of the wave/Offset = how far along the wave to move/Amp = Amplitude of the wave/wMod = Wave Modifier - This adds an extra wave to the current wave/freq and amp of the extra wave
	public delegate float waveFunction(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1);
	//Radius of the cylinder to spawn around
	public float spawnRadius = 5.0f;
	//Default movement speed of objects in game
	public float moveSpeed = 0.5f;
	//How much the gameSpeed effects the ProceduralSpawner
	public float gameSpeedMultiplier = 0.5f;
	public Text waveText;

	float m_gameSpeed = 1.0f;
	WaveHandler m_waveHandler;

	// Use this for initialization

	void Start () {
		m_waveHandler = new WaveHandler();
		m_waveHandler.StartHandler();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_waveHandler != null){
			WaveProperties curWave = m_waveHandler.GetCurWave();

			//If the game is playing
			if(m_gameSpeed > 0){
				waveText.text = m_waveHandler.UpdateHandler(Time.deltaTime);
			}

			if(curWave != null){
				//Update wave based on games speed
				if(curWave.UpdateWave(Time.deltaTime * (m_gameSpeed * gameSpeedMultiplier))){
					GameObject newObj = null;

					//Calculates the spacing around the cylinder for multiple objects
					float diff = (2*Mathf.PI)/curWave.spawnCount;
					diff = diff/100 * curWave.obstacleOffset;

					for(int i = 0; i < curWave.spawnCount; i++){
						//Picks a random number then decides what to spawn based on the spawn chances of the wave
						float choice = Random.Range(0.0f, 100.0f);

						if(choice >= curWave.collectableSpawnChance && choice >= curWave.PowerUpSpawnChance){
							if(curWave.waveObstacle.Count != 0){
								//Selects a random obstacle and then calculates the offset based on the spacing calculated earlier
								newObj = SpawnNewObject(curWave.waveObstacle[Random.Range(0, curWave.waveObstacle.Count)], diff*i, diff*i, curWave);
							}
						}else if(choice < curWave.collectableSpawnChance && choice > curWave.PowerUpSpawnChance){
							if(curWave.waveCollectables.Count != 0){
								newObj = SpawnNewObject(curWave.waveCollectables[Random.Range(0, curWave.waveCollectables.Count)], diff*i, diff*i, curWave);
							}
						}else if(choice < curWave.PowerUpSpawnChance){
							if(curWave.wavePowerUps.Count != 0){
								newObj = SpawnNewObject(curWave.wavePowerUps[Random.Range(0, curWave.wavePowerUps.Count)], diff*i, diff*i, curWave);
							}
						}

						//If we managed to spawn an object rotate them to be on the ground and set their speeds
						if(newObj){
							newObj.transform.up = newObj.transform.position - new Vector3(transform.position.x, transform.position.y, newObj.transform.position.z);

							newObj.GetComponent<ObjectHandler>().SetSpeed(moveSpeed);
							newObj.GetComponent<ObjectHandler>().UpdateGameSpeed(m_gameSpeed);

							newObj.transform.parent = transform;
						}
					}

					curWave.ResetSpawnTimer();
				}
			}
		}
	}

	//Returns a new object at a position calculated from the waves properties
	GameObject SpawnNewObject(GameObject obj, float xOff, float yOff, WaveProperties selWave){
		if(!obj){
			return null;
		}

		Vector3 spawnPos = transform.position;

		//Sin by Cos results in a circular wave, every value effects this initial wave
		spawnPos.x += GenerateSinWave(selWave.waveXYFreq, xOff, spawnRadius, selWave.xyWave, selWave.waveXYModFreq, selWave.waveXYAmp);
		spawnPos.y += GenerateCosWave(selWave.waveXYFreq, yOff, spawnRadius, selWave.xyWave, selWave.waveXYModFreq, selWave.waveXYAmp);

		//Dont do any calculations if not needed
		if(selWave.waveZFreq != 0){
			waveFunction curWave = SelectWave(selWave.zWave);
			spawnPos.z += curWave(selWave.waveZFreq, 0, selWave.waveZAmp);
		}

		return Instantiate(obj, spawnPos, Quaternion.identity) as GameObject;
	}

	//Selects the type of mathematical wave to generate - Returns a delegate to the waves function
	waveFunction SelectWave(WaveProperties.waveTypes wave){
		switch(wave){
			case WaveProperties.waveTypes.NoWave:
				return GenerateNothing;
//				break;
			case WaveProperties.waveTypes.SinWave:
				return GenerateSinWave;
//				break;
			case WaveProperties.waveTypes.CosWave:
				return GenerateCosWave;
//				break;
			case WaveProperties.waveTypes.PingPongWave:
				return GeneratePingPong;
//				break;
			case WaveProperties.waveTypes.TanWave:
				return GenerateTanWave;
//			break;
			case WaveProperties.waveTypes.PerlinWave:
				return GeneratePerlinWave;
//			break;
		}

		return GenerateSinWave;
	}

	//Functions for calculating math waves start here |V-Except this one -------------------------------------------
	float GenerateNothing(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		return 0.0f;
	}

	float GenerateSinWave(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		if(wMod == WaveProperties.waveTypes.NoWave){
			return Mathf.Sin((Time.time + offSet) * freq) * amp;
		}

		waveFunction wave = SelectWave(wMod);

		return Mathf.Sin((Time.time + offSet + (wave(xyModFreq, 0, xyAmp) * xyModFreq)) * freq) * amp;
	}

	float GenerateCosWave(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		if(wMod == WaveProperties.waveTypes.NoWave){
			return Mathf.Cos((Time.time + offSet) * freq) * amp;
		}
		
		waveFunction wave = SelectWave(wMod);

		return Mathf.Cos((Time.time + offSet + (wave(xyModFreq, 0, xyAmp) * xyModFreq))  * freq) * amp;
	}

	float GeneratePingPong(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		if(wMod == WaveProperties.waveTypes.NoWave){
			return Mathf.PingPong((Time.time + offSet) * (m_gameSpeed * freq * 10), amp * 2) - amp;
		}
		
		waveFunction wave = SelectWave(wMod);

		return Mathf.PingPong((Time.time + offSet + (wave(xyModFreq, 0, xyAmp) * xyModFreq)) * freq, amp * 2) - amp;
	}

	float GenerateTanWave(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		//Tan is clamped between -1 and 1 to avoid infinite numbers
		if(wMod == WaveProperties.waveTypes.NoWave){
			return Mathf.Tan(Mathf.Clamp((Time.time + offSet) * freq, -1.0f, 1.0f)) * amp;
		}
		
		waveFunction wave = SelectWave(wMod);

		return Mathf.Tan(Mathf.Clamp((Time.time + offSet + (wave(xyModFreq, 0, xyAmp) * xyModFreq)) * freq, -1.0f, 1.0f)) * amp;
	}

	float GeneratePerlinWave(float freq = 1, float offSet = 0, float amp = 1, WaveProperties.waveTypes wMod = WaveProperties.waveTypes.NoWave, float xyModFreq = 1, float xyAmp = 1){
		if(wMod == WaveProperties.waveTypes.NoWave){
			return Mathf.PerlinNoise(Time.time + offSet, freq) * amp;
		}

		waveFunction wave =SelectWave(wMod);

		return Mathf.PerlinNoise(Time.time + offSet + (wave(xyModFreq, 0, xyAmp) * xyModFreq), freq) * amp;
	}
	//-----------------------------------------------------------------------------------------------------------------------------------------

	//Recieves a broadcasted message for altering game speed (Look at game manager for details)
	void UpdateGameSpeed(float gameSpeed){
		m_gameSpeed = gameSpeed;
	}

	void GameOver(){
		if(m_waveHandler != null){
			m_waveHandler.RestartHandler();
		}
	}
}
