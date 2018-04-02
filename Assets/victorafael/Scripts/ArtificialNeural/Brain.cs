using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	private ArtificialNeuralNetwork neuralNetwork;
	public TrainingSetData trainingData;

	public int hiddenLayers = 1;
	public int neuronsPerHiddenLayer = 3;
	public double alpha = 0.8;

	public bool continuousTrain;
	public bool keepTraining = true;
	public int trainingPerCycle = 150;
	// Use this for initialization
	void Start () {
		if (trainingData != null) {
			neuralNetwork = new ArtificialNeuralNetwork (trainingData.InputAmmount, trainingData.OutputAmmount, hiddenLayers, neuronsPerHiddenLayer, alpha);
			Train (trainingData.epochCount);
		}
	}

	double[] res;
	double sumError = 0;
	void Train(int times){
		int count = trainingData.itens.Count;
		TrainingSetData.TrainingSetItem item;
		for (int i = 0; i < times; i++) {
			sumError = 0;
			for (int n = 0; n < count; n++) {
				item = trainingData.itens [n];
				res = neuralNetwork.Execute(item.input, item.outputs);
				sumError += item.GetError (res);
			}
		}
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Space) || continuousTrain) {
			Train (trainingPerCycle);
		}
	}

	public double[] Execute(double[] inputs){
		return neuralNetwork.Execute (inputs);
	}

	private string DoubleArrayToString(double[] values){
		var b = new System.Text.StringBuilder ();
		b.Append ("[");
		for (var i = 0; i < values.Length; i++) {
			if (i > 0)
				b.Append (", ");
			b.Append (values [i]);
		}
		b.Append ("]");
		return b.ToString ();
	}
}
