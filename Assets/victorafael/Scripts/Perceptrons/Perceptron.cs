using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron : MonoBehaviour {
	public TrainingSetData trainingData;
	public int dynamicDataCount = 2;

	double[] weights;
	double bias;
	double totalError;

	void Start(){
		if (trainingData != null) {
			Train (trainingData.epochCount);
		} else {
			InitialiseWeights ();
			trainingData = ScriptableObject.CreateInstance<TrainingSetData>();
		}
	}

	double DotProductBias(double[] weights, double[] inputs){
		double result = 0;
		for (int i = 0; i < weights.Length; i++) {
			result += weights [i] * inputs [i];
		}
		result += bias;
		return result;
	}

	string CalculateOutputResult(TrainingSetData.TrainingSetItem ti){
		double expected = ti.Output;
		var b = new System.Text.StringBuilder ();
		b.Append ("result for ");
		for(int i = 0; i < ti.input.Length; i++){
			b.AppendFormat ("{0} ", ti.input [i]);
		}
		double result = CalculateOutput (ti.input);
		b.AppendFormat (": {0}. Expected: {1}", result, expected);
		return (b.ToString ());
	}
	public double CalculateOutput(params double[] inputs){
		return DotProductBias (weights, inputs) > 0 ? 1 : 0;
	}

	public void AcquireKnowledge(string name, double output, params double[] data){
		if (!trainingData.HasKnowledge (name)) {
			trainingData.AddKnowledge (name, output, data);
			Train (3);
		} else {
			if (output != CalculateOutput (data)) { // Has knowledge but made something wrong. Reforce training
				Train(2);
			}
		}
	}

	// Use this for initialization
	void InitialiseWeights () {
		weights = new double[trainingData == null ? dynamicDataCount : trainingData.InputAmmount];
		for (int i = 0; i < weights.Length; i++)
			weights [i] = Random.Range (-1f, 1f);
		bias = Random.Range (-1f, 1f);
	}

	void Train(int epochs){
		InitialiseWeights ();
		int e;
		for (e = 0; e < epochs; e++) {
			totalError = 0;
			for (int t = 0; t < trainingData.itens.Count; t++) {
				UpdateWeights (trainingData[t]);
				//PrintValues (totalError);
			}
			//Debug.Log ("#"+e+" TotalError: " + totalError);
			if (totalError == 0)
				break;
		}
		Debug.Log (name + " finished on epoch " + e + " with " + totalError + " errors");
	}

	void PrintValues(float tError){
		var b = new System.Text.StringBuilder ();
		for (int i = 0; i < weights.Length; i++) {
			b.AppendFormat ("W{0}: {1}, ", i, weights [i]);
		}
		b.AppendFormat ("Bias: {0}, ", bias);
		b.AppendFormat ("TotalError: {0}", tError);
		Debug.Log (b.ToString ());
	}
	

	void UpdateWeights(TrainingSetData.TrainingSetItem t){
		double error = t.Output - CalculateOutput (t.input);
		totalError += Mathf.Abs((float)error);
		for (int i = 0; i < weights.Length; i++) {
			weights [i] = weights [i] + error * t.input [i];
		}
		bias += error;
	}
}
