using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedableEntity : MonoBehaviour {
	public DNAStructure dnaStructure;
	public DNA dna;

	public void SetRandom(){
		dna = dnaStructure.RandomDNA ();
	}
	public void SetInheritance(DNA father, DNA mother){
		dna = dnaStructure.BreedDNA (father, mother);
	}
}
