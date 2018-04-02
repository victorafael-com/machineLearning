using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "victorafael/DNA/New Structure", order = 1, fileName = "new DNA Structure")]
public class DNAStructure : ScriptableObject {
	public ChromossomeData[] chromossomes;

	public DNA RandomDNA(){

		float[] chromossomeValues = new float[chromossomes.Length];

		for (int i = 0; i < chromossomes.Length; i++) {
			chromossomeValues [i] = chromossomes [i].RandomValue;
		}
		return new DNA (chromossomeValues);
	}

	public DNA BreedDNA(DNA father, DNA mother){
		float[] chromossomeValues = new float[chromossomes.Length];
		for (int i = 0; i < chromossomes.Length; i++) {
			chromossomeValues [i] = chromossomes [i].BreedValue (father.chromossomes [i], mother.chromossomes [i]);
		}
		return new DNA (chromossomeValues);
	}
}
