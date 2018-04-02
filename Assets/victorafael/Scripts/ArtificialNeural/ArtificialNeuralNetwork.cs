using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialNeuralNetwork {
	//public int inputs;
	//public int outputs;
	//public int hidden;
	//public int neuronPerHidden;
	//public double alpha;

	private int m_inputCount;

	//Determinates how much the new training content will affect the network;
	//1 behaves as a perceptron, everything affects everything :P
	private double m_alpha;
	NeuronLayer[] layers;

	public ArtificialNeuralNetwork(int inputCount, int outputCount, int hiddenLayers, int neuronsPerLayer, double alpha, Neuron.ActivationType firstLayerActivation, Neuron.ActivationType hiddenLayerActivation, Neuron.ActivationType outputLayerActivation){
		m_inputCount = inputCount;
		m_alpha = alpha;

		if (hiddenLayers < 0)
			hiddenLayers = 0;
		layers = new NeuronLayer[hiddenLayers+1];
		if (hiddenLayers > 0) {
			layers [0] = new NeuronLayer (neuronsPerLayer, inputCount, firstLayerActivation);
			for (int i = 1; i < hiddenLayers; i++) {
				layers [i] = new NeuronLayer (neuronsPerLayer, neuronsPerLayer, hiddenLayerActivation);
			}
			layers [hiddenLayers] = new NeuronLayer (outputCount, neuronsPerLayer, outputLayerActivation);
		} else {
			layers [0] = new NeuronLayer (outputCount, inputCount, outputLayerActivation);
		}
	}

	public double[] Execute(double[] inputs, double[] desiredResponse = null){
		if (inputs.Length != m_inputCount) {
			throw new System.Exception ("Invalid ammount of inputs. expected: " + m_inputCount);
		}

		double[] output = null;

		for (int i = 0; i < layers.Length; i++) {
			if (i > 0) {
				inputs = output;
			}
			output = layers [i].Process (inputs);
		}

		if (desiredResponse != null) {
			UpdateWeights (output, desiredResponse);
		}

		return output;
	}

	void UpdateWeights(double[] outputs, double[] desiredOutput){
		for (int i = layers.Length - 1; i >= 0; i--) {
			if (i == layers.Length - 1) {
				layers [i].UpdateWeights (outputs, desiredOutput, m_alpha);
			} else {
				layers [i].UpdateWeights (layers [i + 1], m_alpha);
			}
		}
	}
}
