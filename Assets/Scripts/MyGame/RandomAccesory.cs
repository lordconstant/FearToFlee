using UnityEngine;
using System.Collections;

public class RandomAccesory : MonoBehaviour {
	[System.Serializable]
	public struct Outfit{
		public GameObject item;
		public Material outfitMat;
	}

	[Range (0, 100)]
	public float chanceForAccesory = 2.0f;
	public Outfit[] Accesories;

	// Use this for initialization
	void Start (){
		float chance = Random.Range(0.0f, 100.0f);

		if(chance < chanceForAccesory){
			//Randomly spawn in an outfit if needed and attaches it to the gameobject
			int chosen = Random.Range(0, Accesories.Length);

			if(Accesories[chosen].item){
				GameObject newAcc = Instantiate(Accesories[chosen].item, transform.position, Quaternion.identity) as GameObject;
				newAcc.transform.up = transform.parent.up;
				newAcc.transform.parent = transform;
			}

			if(Accesories[chosen].outfitMat){
				Material[] matArr = new Material[renderer.materials.Length+1];

				for(int i = 0; i < renderer.materials.Length; i++){
					matArr[i] = renderer.materials[i];
				}

				matArr[renderer.materials.Length] = Accesories[chosen].outfitMat;

				renderer.materials = matArr;
			}
		}
	}
}
