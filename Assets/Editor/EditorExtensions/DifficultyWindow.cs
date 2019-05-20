#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DifficultyWindow : ScriptableObject {
	public string difficultyName;
	public float duration;
	public List<WaveWindow> waveWindows = new List<WaveWindow>();
}

#endif
