#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//The editor window for creating waves and difficulties
public class WaveEditorWindow : EditorWindow {
	static List<DifficultyWindow> m_difficultyWindows;

	WaveWindow m_curWaveWindow;
	DifficultyWindow m_curDiffWindow;
	Rect m_windowRect = new Rect(20, 10, 280, 0);
	Vector2 m_horiScrollPos = new Vector2(0, 0);
	//Used for storing inital values
	string m_waveName = "NewWave";
	string m_difficultyName = "NewDifficulty";
	float m_difficultyDuration = 15;

	[MenuItem ("Window/Wave Designer")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		WaveEditorWindow window = (WaveEditorWindow)EditorWindow.GetWindow (typeof (WaveEditorWindow));
		
		Vector2 size = new Vector2(600, 500);
		window.minSize = size;
		
		size.x = 1000;
		size.y = 1000;
		
		window.maxSize = size;

		m_difficultyWindows = new List<DifficultyWindow>();

		//Loads in existing waves
		WaveDifficulty[] waveDiff = Serializer.Load<WaveDifficulty[]>("WaveDifficulties.xml");

		if(waveDiff != null){
			for(int i = 0; i < waveDiff.Length; i++){
				DifficultyWindow tempWind = CreateInstance<DifficultyWindow>();
				waveDiff[i].OnAfterDeserialize();
				tempWind.difficultyName = waveDiff[i].name;
				tempWind.duration = waveDiff[i].duration;

				for(int j = 0; j < waveDiff[i].waves.Length; j++){
					WaveWindow waveWind = CreateInstance<WaveWindow>();

					waveWind.setPropertes(waveDiff[i].waves[j]);
					tempWind.waveWindows.Add(waveWind);
				}

				m_difficultyWindows.Add(tempWind);
			}
		}
	}

	void OnGUI () {
		float xScrollSize = 480 * m_difficultyWindows.Count;
		float yScrollSize;
		int highestCount = 0;
		for(int i = 0; i < m_difficultyWindows.Count; i++){
			if(m_difficultyWindows[i].waveWindows.Count > highestCount){
				highestCount = m_difficultyWindows[i].waveWindows.Count;
			}
		}

		yScrollSize = 385 * highestCount;

		if(yScrollSize < position.height - 45){
			yScrollSize = position.height - 45;
		}

		float scrollWidth = position.width - 325;

		if(scrollWidth > xScrollSize){
			scrollWidth = xScrollSize;
		}

		if(m_curWaveWindow){
			m_windowRect = m_curWaveWindow.WaveCreationWindow(0);
		}

		m_horiScrollPos = GUI.BeginScrollView(new Rect(305, 10, scrollWidth, position.height - 45), m_horiScrollPos, new Rect(0, 10, xScrollSize, yScrollSize), true, true);

		BeginWindows();

		//if(m_curWaveWindow){
		//	m_windowRect = GUILayout.Window(0, m_windowRect, m_curWaveWindow.WaveCreationWindow, m_curWaveWindow.getProperties().waveName);
		//}

		int windNum = 1;

		//Creates the windows in the formatted layout
		for(int i = 0; i < m_difficultyWindows.Count; i++){
			Rect waveWindRect = new Rect((i * 480), 10, 475, 40);
			GUI.Box(waveWindRect, m_difficultyWindows[i].difficultyName);
			waveWindRect.y += 20;
			GUILayout.BeginArea(waveWindRect);
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Edit")){
				m_curDiffWindow = m_difficultyWindows[i];
			}
			if(GUILayout.Button("Delete")){
				m_difficultyWindows.RemoveAt(i);
				i--;
				continue;
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.EndArea();
			waveWindRect.y += 20;

			GUILayout.BeginArea(waveWindRect);
			EditorGUILayout.BeginHorizontal();
			waveWindRect.y += 25;

			if(i != 0){
				if(GUILayout.Button("<")){
					DifficultyWindow tempWind = m_difficultyWindows[i];
					m_difficultyWindows[i] = m_difficultyWindows[i-1];
					m_difficultyWindows[i-1] = tempWind;
				}
			}

			if(i != m_difficultyWindows.Count-1){
				if(GUILayout.Button(">")){
					DifficultyWindow tempWind = m_difficultyWindows[i];
					m_difficultyWindows[i] = m_difficultyWindows[i+1];
					m_difficultyWindows[i+1] = tempWind;
				}
			}

			GUILayout.EndArea();
			EditorGUILayout.EndHorizontal();

			if(m_difficultyWindows[i].waveWindows != null){
				for(int j = 0; j < m_difficultyWindows[i].waveWindows.Count; j++){
					List<WaveWindow> curWaveWindows = m_difficultyWindows[i].waveWindows;
					waveWindRect = GUILayout.Window(windNum, waveWindRect, curWaveWindows[j].WaveDisplayWindow, curWaveWindows[j].getProperties().waveName);
					curWaveWindows[j].Focus();
					waveWindRect.y = waveWindRect.y + 320;
					waveWindRect.height = 30;
					GUILayout.BeginArea(waveWindRect);
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Edit")){
						m_curWaveWindow = curWaveWindows[j];
						curWaveWindows.RemoveAt(j);
						j--;
					}
					if(GUILayout.Button("Copy")){
						m_curWaveWindow = CreateInstance<WaveWindow>();
						WaveProperties waveProps = ClassCopier.CloneClass<WaveProperties>(curWaveWindows[j].getProperties());
						waveProps.OnAfterDeserialize();
						m_curWaveWindow.setPropertes(waveProps);
					}
					if(GUILayout.Button("Delete")){
						curWaveWindows.RemoveAt(j);
						j--;
					}
					EditorGUILayout.EndHorizontal();
					GUILayout.EndArea();
					waveWindRect.y += 30;
					windNum++;
				}
			}
		}

		EndWindows();
		GUI.EndScrollView();
		int windowHeight = 0;

		if(!m_curWaveWindow){
			windowHeight += 40;
		}

		Rect newWaveRect = new Rect(m_windowRect.x, m_windowRect.y + m_windowRect.height, m_windowRect.width, windowHeight);

		if(!m_curWaveWindow){
			newWaveWindow(newWaveRect);
		}else{
			newWaveRect.height += (m_difficultyWindows.Count * 20);
			newWaveWindow(newWaveRect);
		}

		newWaveRect.y += newWaveRect.height + 10;
		newWaveRect.height = 60;

		NewDifficultyWindow(newWaveRect);
	}

	//Creates the window for saving a wave and creating a new one
	void newWaveWindow(Rect posRect){
		GUI.Box(posRect, "");
		GUILayout.BeginArea(posRect);
		if(!m_curWaveWindow){
			m_waveName = EditorGUILayout.TextField("Name:", m_waveName);
			if(GUILayout.Button("Create Wave")){
				m_curWaveWindow = CreateInstance<WaveWindow>();
				m_curWaveWindow.OnCreate(m_waveName);
				m_curWaveWindow.Focus();
			}
		}else{
			for(int i = 0; i < m_difficultyWindows.Count; i++){
				if(GUILayout.Button("Save new " + m_difficultyWindows[i].difficultyName)){
					m_difficultyWindows[i].waveWindows.Add(m_curWaveWindow);
					m_curWaveWindow = null;
					m_windowRect = new Rect(10, 10, 280, 0);
				}
			}
		}
		GUILayout.EndArea();
	}

	//Creates the window for saving a difficulty and making a new one
	void NewDifficultyWindow(Rect posRect){
		GUI.Box(posRect, "");
		GUILayout.BeginArea(posRect);
		if(!m_curDiffWindow){
			m_difficultyName = EditorGUILayout.TextField("Name:", m_difficultyName);
			m_difficultyDuration = EditorGUILayout.FloatField("Duration:", m_difficultyDuration);
			if(GUILayout.Button("Create Difficulty")){
				DifficultyWindow newDiff = CreateInstance<DifficultyWindow>();
				newDiff.difficultyName = m_difficultyName;
				newDiff.duration = m_difficultyDuration;

				m_difficultyWindows.Add(newDiff);
			}
		}else{
			m_curDiffWindow.difficultyName = EditorGUILayout.TextField("Name:", m_curDiffWindow.difficultyName);
			m_curDiffWindow.duration = EditorGUILayout.FloatField("Duration:", m_curDiffWindow.duration);
			if(GUILayout.Button("Save Difficulty")){
				m_curDiffWindow = null;
			}
		}
		GUILayout.EndArea();
	}

	//When the window is closed
	void OnDestroy() {
		WaveDifficulty[] tempDiffs = new WaveDifficulty[m_difficultyWindows.Count];

		for(int i = 0; i < tempDiffs.Length; i++){
			WaveProperties[] tempProps = new WaveProperties[m_difficultyWindows[i].waveWindows.Count];
			WaveDifficulty newDiff = new WaveDifficulty();
			newDiff.duration = m_difficultyWindows[i].duration;
			newDiff.name = m_difficultyWindows[i].difficultyName;

			for(int j = 0; j < m_difficultyWindows[i].waveWindows.Count; j++){
				tempProps[j] = m_difficultyWindows[i].waveWindows[j].getProperties();
			}

			newDiff.waves = tempProps;
			newDiff.OnBeforeSerialize();

			tempDiffs[i] = newDiff;
		}

		Serializer.Save<WaveDifficulty[]>("WaveDifficulties.xml", tempDiffs);
	}
}
#endif
