using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ScoreHandler{
	public static int[] scores;
	static int m_maxSize;
	
	public static void SaveScores(int[] scores){
		//Serializer.Save<int[]>("Scores.xml", scores);

		string scoreString = string.Empty;
		
		for(int i = 0; i < scores.Length; i++){
			scoreString += scores[i].ToString () + ";";
		}
		
		PlayerPrefs.SetString("Scores", scoreString);
	}
	
	public static int[] GetScores(){
		//Checks if the score file exists and returns an empty array if not
//		if(Serializer.FileExists("Scores.xml")){
//			return Serializer.Load<int[]>("Scores.xml");
//		}
//		
//		return new int[m_maxSize];
		string storedScores = PlayerPrefs.GetString("Scores");
		
		List<int> savedScores = new List<int>();
		string curInt = string.Empty;
		
		
		foreach(char c in storedScores){
			if(c == ';'){
				savedScores.Add (int.Parse (curInt));
				curInt = "";
			}else{
				curInt += c;
			}
		}
		
		return savedScores.ToArray();
	}
	
	public static bool AddScore(int score){
		int bestScore = score;
		
		if(Serializer.FileExists("Scores.xml")){
			int[] curScores = GetScores();
			
			if(curScores.Length >= m_maxSize){
				int lowestScore = score;
				int lowestScoreKey = 0;
				
				//Looks for the lowest score among the curent scores
				for(int i = 0; i < curScores.Length; i++){
					if(curScores[i] < lowestScore){
						lowestScore = curScores[i];
						lowestScoreKey = i;
					}
					
					if(curScores[i] > bestScore){
						bestScore = curScores[i];
					}
				}
				
				//Replaces the lowest score with the new score, if the new score is higher
				if(score != lowestScore){
					curScores[lowestScoreKey] = score;
					
					SaveScores(curScores);
					
					PlayerPrefs.SetInt("BestScore", bestScore);
					return true;
				}
				
				PlayerPrefs.SetInt("BestScore", bestScore);
				return false;
			}else{
				//Creates a new array for array resizing
				int[] newScoreArr = new int[curScores.Length+1];
				
				for(int i = 0; i < curScores.Length; i++){
					newScoreArr[i] = curScores[i];
					
					if(curScores[i] > bestScore){
						bestScore = curScores[i];
					}
				}
				
				newScoreArr[curScores.Length] = score;
				
				SaveScores(newScoreArr);
				PlayerPrefs.SetInt("BestScore", bestScore);
				
				return true;
			}
		}else{
			//Creates an array to pass through to the SaveScore function
			int[] intArr = new int[1];
			intArr[0] = score;
			SaveScores(intArr);
			PlayerPrefs.SetInt("BestScore", bestScore);
			return true;
		}
	}
	
	//Quicksort for sorting the scores from Highest to Lowest
	public static int[] SortScores(int[] scores){
		for(int i = 0; i < scores.Length; i++){
			for(int j = i+1; j < scores.Length; j++){
				if(scores[i] < scores[j]){
					int temp = scores[i];
					scores[i] = scores[j];
					scores[j] = temp;
				}
			}
		}
		
		return scores;
	}
	
	public static void setMaxSize(int size){
		m_maxSize = size;
		
		int[] curScores = GetScores();
		
		if(curScores.Length <= m_maxSize){
			return;
		}
		
		//Sorts the array from highest to lowest
		curScores = SortScores(curScores);
		
		//Creates a new array of max size and culls lowest scores
		int[] newScoreArr = new int[m_maxSize];
		
		for(int i = 0; i < m_maxSize; i++){
			newScoreArr[i] = curScores[i];
		}
		
		SaveScores(newScoreArr);
	}

	public static void CheckForReset(){
		AddScore (0);

		if(PlayerPrefs.GetInt ("BestScore") == 0){
			PlayerPrefs.SetInt ("LastScore", 0);
		}
	}
}
