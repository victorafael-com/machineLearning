using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {
	public float[] chromossomes;

	public DNA(float[] chromossomeValues){
		chromossomes = chromossomeValues;
	}

	public float this [int index]{
		get{
			return chromossomes [index];
		}
	}

	public int IntVal(int index){
		return Mathf.RoundToInt(chromossomes[index]);
	}
}
