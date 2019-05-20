#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//Used for drawing the wave window in the editor window
public class WaveWindow : EditorWindow {
	WaveProperties m_properties;
	bool m_showObs;
	bool m_showColls;
	bool m_showPowerUps;

	public void OnCreate(string newName){
		m_properties = new WaveProperties();
		m_properties.waveName = newName;
	}

	void OnGUI () {
	}

	//Draws the editable window
	public Rect WaveCreationWindow(int id){
		int propsInWindow = 15;

		if(m_showColls){
			propsInWindow++;
			propsInWindow += m_properties.waveCollectables.Count;
		}

		if(m_showObs){
			propsInWindow++;
			propsInWindow += m_properties.waveObstacle.Count;
		}

		if(m_showPowerUps){
			propsInWindow++;
			propsInWindow += m_properties.wavePowerUps.Count;
		}

		Rect tempRect = new Rect(20, 10, 280, propsInWindow*21);
		GUI.Box(tempRect, "");
		GUILayout.BeginArea(tempRect);
		GUILayout.BeginVertical();
		GUIStyle myStyle = new GUIStyle(GUI.skin.textField);
		myStyle.alignment = TextAnchor.MiddleCenter;
		m_properties.waveName = EditorGUILayout.TextField("", m_properties.waveName, myStyle);
		m_properties.waveDuration = EditorGUILayout.FloatField("WaveDuration:", m_properties.waveDuration);
		m_properties.waveXYFreq = EditorGUILayout.FloatField("WaveXYFrequency:", m_properties.waveXYFreq);
		m_properties.waveZFreq = EditorGUILayout.FloatField("WaveZFrequency:", m_properties.waveZFreq);
		m_properties.waveXYModFreq = EditorGUILayout.FloatField("WaveXYModFrequency:", m_properties.waveXYModFreq);
		m_properties.waveZAmp = EditorGUILayout.FloatField("WaveZAmplitude:", m_properties.waveZAmp);
		m_properties.waveXYAmp = EditorGUILayout.FloatField("WaveXYModAmplitude:", m_properties.waveXYAmp);
		m_properties.obstacleSpawnRate = EditorGUILayout.FloatField("ObstacleSpawnRate:", m_properties.obstacleSpawnRate);
		m_properties.spawnCount = EditorGUILayout.IntSlider("SpawnCount:", m_properties.spawnCount, 1, 10);
		m_properties.obstacleOffset = EditorGUILayout.Slider("ObstacleSpawnOffset:", m_properties.obstacleOffset, 1.0f, 100.0f);
		m_properties.zWave = (WaveProperties.waveTypes)EditorGUILayout.EnumPopup("ZWaveType:", m_properties.zWave);
		m_properties.xyWave = (WaveProperties.waveTypes)EditorGUILayout.EnumPopup("XYWaveType:", m_properties.xyWave);
		m_properties.collectableSpawnChance = EditorGUILayout.Slider("CollectableSpawnChance:", m_properties.collectableSpawnChance, 0.0f, 100.0f);
		m_properties.PowerUpSpawnChance = EditorGUILayout.Slider("PowerUpSpawnChance:", m_properties.PowerUpSpawnChance, 0.0f, 100.0f);
		DrawGameObjectArray(m_properties.waveObstacle, "WaveObstacle", ref m_showObs);
		DrawGameObjectArray(m_properties.waveCollectables, "WaveCollectable", ref m_showColls);
		DrawGameObjectArray(m_properties.wavePowerUps, "WavePowerUps", ref m_showPowerUps);
		GUILayout.EndVertical();
		GUILayout.EndArea();

		return tempRect;
	}

	//Draws the uneditable window
	public void WaveDisplayWindow(int id){
		GUILayout.Label("WaveDuration - " + m_properties.waveDuration.ToString());
		GUILayout.Label("WaveXyFrequency - " + m_properties.waveXYFreq.ToString());
		GUILayout.Label("WaveZFrequency - " + m_properties.waveZFreq.ToString());
		GUILayout.Label("WaveXYModFrequency - " + m_properties.waveXYModFreq.ToString());
		GUILayout.Label("WaveZAmplitude - " + m_properties.waveZAmp.ToString());
		GUILayout.Label("WaveXYModAmplitude - " + m_properties.waveXYAmp.ToString());
		GUILayout.Label("ObstacleSpawnRate - " + m_properties.obstacleSpawnRate.ToString());
		GUILayout.Label("ObstacleSpawnOffset - " + m_properties.obstacleOffset.ToString());
		GUILayout.Label("SpawnCount - " + m_properties.spawnCount.ToString());
		GUILayout.Label("ZWaveType - " + m_properties.zWave.ToString());
		GUILayout.Label("XYWaveType - " + m_properties.xyWave.ToString());
		GUILayout.Label("CollectableSpawnChance - " + m_properties.collectableSpawnChance.ToString());
		GUILayout.Label("PowerUpSpawnChance - " + m_properties.PowerUpSpawnChance.ToString());
		GUILayout.Label("WaveObstacle - " + m_properties.waveObstacle.ToString());
		GUILayout.Label("WaveCollectables - " + m_properties.waveCollectables.ToString());
		GUILayout.Label("WavePowerUps - " + m_properties.wavePowerUps.ToString());
	}

	//used for drawing an array into an editable format
	void DrawGameObjectArray(List<GameObject> goArr, string label, ref bool show){
		show = EditorGUILayout.Foldout(show, "Waves");

		if(show){
			int arrSize = goArr.Count;

			arrSize = EditorGUILayout.IntField("Array Size:", arrSize);

			while(arrSize > goArr.Count){
				goArr.Add(null);
			}
			while(arrSize < goArr.Count){
				goArr.RemoveAt(goArr.Count-1);
			}

			for(int i = 0; i < goArr.Count; i++){
				goArr[i] = EditorGUILayout.ObjectField(label, goArr[i], typeof(GameObject), false) as GameObject;
			}
		}
	}

	public void setPropertes(WaveProperties props){
		m_properties = props;
	}

	public WaveProperties getProperties(){
		return m_properties;
	}
}
#endif