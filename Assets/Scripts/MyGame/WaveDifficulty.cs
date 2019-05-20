using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("WaveDifficulty")]
public class WaveDifficulty{
	public string name = "New Difficulty";
	//How long the difficulty will last
	public float duration = 30.0f;
	[XmlArray("Waves"),XmlArrayItem("Wave")]
	public WaveProperties[] waves;

	float m_timer = 0.0f;

	//Needed for xml serialization
	public WaveDifficulty(){
	}

	public bool UpdateDifficulty(float deltaTime){
		m_timer += deltaTime;

		//Returns true if its time to change difficulty
		if(m_timer >= duration){
			return true;
		}

		return false;
	}

	//Call this before Serialzing the class
	public void OnBeforeSerialize(){
		for(int i = 0; i < waves.Length; i++){
			waves[i].OnBeforeSerialize();
		}
	}

	//Call this after Deserializing the class
	public void OnAfterDeserialize(){
		for(int i = 0; i < waves.Length; i++){
			waves[i].OnAfterDeserialize();
		}
	}
}
