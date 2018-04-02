using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "victorafael/DNA/Training Set Data", order = 1, fileName = "new Training Set")]
public class TrainingSetData : ScriptableObject {
	private HashSet<int> knowledgeHash = new HashSet<int>();

	[System.Serializable]
	public class TrainingSetItem
	{
		public string name;
		public double[] input;
		public double[] outputs;
		public double Output{
			get{
				if (outputs.Length == 1) {
					return outputs [0];
				} else {
					throw new System.Exception ("Trying to fetch Output from Training Item with "+outputs.Length+" outputs");
				}
			}
		}

		public TrainingSetItem(int inputsize){
			input = new double[inputsize];
			name = "";
			outputs = new double[1];
		}
		public TrainingSetItem(string itemName, double[] data, double result){
			name = itemName;
			input = data;
			outputs = new double[1]{result};
		}
		public TrainingSetItem(string itemName, double[] data, double[] result){
			name = itemName;
			input = data;
			outputs = result;
		}

		public float GetError(double[] results){
			float res = 0;
			for (int i = 0; i < results.Length; i++) {
				res +=  Mathf.Abs ((float)(results [i] - outputs [i]));
			}
			return res;
		}
	}

	public string[] inputNames;
	public string[] outputNames;

	[TrainingItemAttribute]
	public List<TrainingSetItem> itens;
	public int epochCount;
	public int InputAmmount{
		get{
			return itens[0].input.Length;
		}
	}
	public int OutputAmmount{
		get{
			return itens [0].outputs.Length;
		}
	}
	public TrainingSetItem this[int index]{
		get{
			return itens [index];
		}
	}

	void OnEnable(){
		if (itens == null)
			itens = new List<TrainingSetItem> ();
		for (int i = 0; i < itens.Count; i++) {
			int hash = itens [i].name.GetHashCode ();
			if (!knowledgeHash.Contains (hash))
				knowledgeHash.Add (hash);
		}
	}

	public bool HasKnowledge(int hash){
		return knowledgeHash.Contains (hash);
	}
	public bool HasKnowledge(string name){
		return knowledgeHash.Contains (name.GetHashCode ());
	}

	public void AddKnowledge(string name, double result, params double[] inputs){
		int hash = name.GetHashCode ();
		if(!HasKnowledge(hash)){
			itens.Add (new TrainingSetItem (name, inputs, result));
			knowledgeHash.Add (hash);
		}
	}
	public void AddKnowledge(int hash, double result, params double[] inputs){
		if(!HasKnowledge(hash)){
			itens.Add (new TrainingSetItem ("custom #" + hash, inputs, result));
			knowledgeHash.Add (hash);
		}
	}
}
