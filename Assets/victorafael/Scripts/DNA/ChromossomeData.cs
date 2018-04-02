using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChromossomeData {
	public string name;
	public float minValue;
	public float maxValue;
	public float breedVariation;
	[Range(0,0.2f)]
	public float mutationChance;
	[Range(0,1)]
	public float fatherInheritanceRatio = 0.5f;

	//public float value;

	public float RandomValue{
		get{
			return Random.Range (minValue, maxValue);
		}
	}
	public float BreedValue(float father, float mother){
		if (Random.value < mutationChance)
			return RandomValue;
		else{
			float result = Random.value < fatherInheritanceRatio ? father : mother;
			result += Random.Range(-breedVariation,breedVariation);
			return Mathf.Clamp (result, minValue, maxValue);
		}
	}
}
