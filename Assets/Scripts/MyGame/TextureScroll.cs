using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {
	public float speed = 1.0f;
	public float gameSpeedMultiplier = 2.0f;
	float m_gameSpeed = 1.0f;
	float m_uvOffset;

	// Use this for initialization
	void Start () {
		m_uvOffset = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Scrolls the texture by moving its UVS
		//Gives the feeling of running in the game
		m_uvOffset += Time.deltaTime * (speed * (m_gameSpeed * 2.0f));

		renderer.material.mainTextureOffset = new Vector2(0, m_uvOffset);

		//Avoids uvs getting to high and acting weird
		if(m_uvOffset > 1.0f){
			m_uvOffset = 0.0f;
		}
	}

	void UpdateGameSpeed(float gameSpeed){
		m_gameSpeed = gameSpeed;
	}
}
