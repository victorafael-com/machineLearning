using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	private ArtificialNeuralNetwork neuralNetwork;
	public TrainingSetData trainingData;

	public int hiddenLayers = 1;
	public int neuronsPerHiddenLayer = 3;
	public Neuron.ActivationType inputLayerActivationMethod = Neuron.ActivationType.Sigmoid;
	public Neuron.ActivationType hiddenLayersActivationMethod = Neuron.ActivationType.Sigmoid;
	public Neuron.ActivationType outputLayerActivationMethod = Neuron.ActivationType.Sigmoid;
	public double alpha = 0.8;

	public bool continuousTrain;
	public bool keepTraining = true;
	public int trainingPerCycle = 150;
	// Use this for initialization
	void Start () {
		if (trainingData != null) {
			neuralNetwork = new ArtificialNeuralNetwork (
				trainingData.InputAmmount,
				trainingData.OutputAmmount,
				hiddenLayers,
				neuronsPerHiddenLayer,
				alpha,
				inputLayerActivationMethod,
				hiddenLayersActivationMethod,
				outputLayerActivationMethod
			);
			Train (trainingData.epochCount);
		}
	}
	/// <summary>
	/// Starts the neural network with the Brain default values
	/// </summary>
	/// <param name="inputAmmount">Input ammount.</param>
	/// <param name="outputAmmount">Output ammount.</param>
	public void StartNeuralNetwork(int inputAmmount, int outputAmmount){
		if (neuralNetwork != null)
			return;
		StartNeuralNetwork (inputAmmount, outputAmmount, hiddenLayers, neuronsPerHiddenLayer, alpha, inputLayerActivationMethod, hiddenLayersActivationMethod, outputLayerActivationMethod);
	}

	/// <summary>
	/// Starts the neural network with full controls of its setup.
	/// </summary>
	/// <param name="inputAmmount">Input ammount.</param>
	/// <param name="outputAmmount">Output ammount.</param>
	/// <param name="hiddenLayers">Hidden layers.</param>
	/// <param name="neuronsPerLayer">Neurons per layer.</param>
	/// <param name="alpha">Alpha.</param>
	/// <param name="inputActivationMethod">Input activation method.</param>
	/// <param name="hiddenLayerActivationMethod">Hidden layer activation method.</param>
	/// <param name="outputActivationMethod">Output activation method.</param>
	public void StartNeuralNetwork(int inputAmmount, int outputAmmount, int hiddenLayerAmmount, int neuronsInHiddenLayer, double errorAlpha, Neuron.ActivationType inputActivationMethod, Neuron.ActivationType hiddenActivationMethod, Neuron.ActivationType outputActivationMethod){
		if (neuralNetwork != null)
			return;
		neuralNetwork = new ArtificialNeuralNetwork (inputAmmount, outputAmmount, hiddenLayerAmmount, neuronsInHiddenLayer, errorAlpha, inputActivationMethod, hiddenActivationMethod, outputActivationMethod);
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
		if (continuousTrain) {
			Train (trainingPerCycle);
		}
		if (Input.GetKeyUp (KeyCode.Alpha1))
			Time.timeScale = 1;
		if (Input.GetKeyUp (KeyCode.Alpha2))
			Time.timeScale = 2;
		if (Input.GetKeyUp (KeyCode.Alpha3))
			Time.timeScale = 3;
		if (Input.GetKeyUp (KeyCode.Alpha4))
			Time.timeScale = 4;
		if (Input.GetKeyUp (KeyCode.Alpha5))
			Time.timeScale = 5;
		if (Input.GetKeyUp (KeyCode.Alpha6))
			Time.timeScale = 6;
		if (Input.GetKeyUp (KeyCode.Alpha7))
			Time.timeScale = 7;
		if (Input.GetKeyUp (KeyCode.Alpha8))
			Time.timeScale = 8;
		if (Input.GetKeyUp (KeyCode.Alpha9))
			Time.timeScale = 9;
	}

	public double[] Execute(double[] inputs){
		return neuralNetwork.Execute (inputs);
	}
	public double[] ExecuteAndTrain(double[] inputs, double[] outputs){
		return neuralNetwork.Execute (inputs, outputs);
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
