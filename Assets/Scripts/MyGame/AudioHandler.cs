using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {
	public enum MusicState{GAME, MENU};

	//A random song will be chosen from the music provided
	public AudioClip[] GameMusic;
	public AudioClip[] MenuMusic;
	//How long it takes to start next song
	public float TransSpeed;

	//Active array of music
	AudioClip[] m_activeMusic;
	AudioClip m_curSong;
	bool m_startSong;
	bool m_stopSong;
	bool m_activeHandler;
	float m_maxVolume;
	float m_audioTimer;

	//Used to make the class a singleton as monobehaviour
	public static AudioHandler instance;

	//Used to make the class a singleton as monobehaviour
	void Awake(){
		if(instance == null){
			instance = this;
			m_activeHandler = true;
		}else{
			Debug.Log("One AudioHandler per scene");
			m_activeHandler = false;
		}
	}

	// Use this for initialization
	void Start () {
		//Add a playerPrefs.SetFloat("MusicVolume", num) to change volume of the games music (InPrep for options screen)
		m_maxVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
		//We fade music in from 0
		audio.volume = 0;

		//If we are the singleton play the music
		if(m_activeHandler){
			m_activeMusic = MenuMusic;

			m_curSong = m_activeMusic[Random.Range(0, m_activeMusic.Length)];
			audio.PlayOneShot(m_curSong);
			audio.time = 60;
			m_audioTimer = 0.0f;

			m_stopSong = false;
			m_startSong = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_activeHandler){
			m_audioTimer += Time.deltaTime;
			//Fade music in to finish staring the song
			if(m_startSong){
				if(TransitionMusic(Time.deltaTime, true)){
					m_startSong = false;
				}
			}

			//Fade music out to finish stopping the song
			if(m_stopSong){
				if(TransitionMusic(Time.deltaTime, false)){
					m_stopSong = false;
					m_startSong = true;

					//Start up next song
					m_curSong = m_activeMusic[Random.Range(0, m_activeMusic.Length)];
					audio.PlayOneShot(m_curSong);
					m_audioTimer = 0.0f;
				}
			}

			//If the current song is near the end start to stop it
			if(m_audioTimer >= m_curSong.length - 1.0f && !m_stopSong){
				m_stopSong = true;
			}
		}
	}

	public void SwitchTo(MusicState state){
		//Changes music depending on state
		switch(state){
		case MusicState.GAME:
			m_activeMusic = GameMusic;
			m_stopSong = true;
			m_startSong = false;
		break;
		case MusicState.MENU:
			m_activeMusic = MenuMusic;
			m_stopSong = true;
			m_startSong = false;
		break;
		}
	}

	bool TransitionMusic(float deltaTime, bool fadeIn){
		//Fade the music out at the transSpeed
		if(!fadeIn){
			audio.volume -= deltaTime * (m_maxVolume * (1/TransSpeed));

			//keeps volume from going negative
			if(audio.volume <= 0.01f){
				audio.Stop();
				audio.volume = 0.0f;
				m_audioTimer = 0.0f;
				return true;
			}
		}

		if(fadeIn){
			audio.volume += deltaTime * (m_maxVolume/TransSpeed);

			//Stop volume exceeding the max
			if(audio.volume >= m_maxVolume){
				audio.volume = m_maxVolume;
				return true;
			}
		}

		return false;
	}
}
