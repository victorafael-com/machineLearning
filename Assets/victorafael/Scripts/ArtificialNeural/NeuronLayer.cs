using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronLayer  {
	private Neuron[] neurons;
	double[] output;

	public NeuronLayer(int neuronCount, int inputs, Neuron.ActivationType activationType){
		neurons = new Neuron[neuronCount];
		for (int i = 0; i < neuronCount; i++) {
			neurons [i] = new Neuron (inputs, activationType);
		}
		output = new double[neuronCount];
	}


	public double[] Process(double[] inputs){
		for (int i = 0; i < neurons.Length; i++) {
			output [i] = neurons [i].Process (inputs);
		}

		return output;
	}

	public void UpdateWeights(double[] output, double[] desired, double alpha){
		for (int i = 0; i < neurons.Length; i++) {
			neurons [i].UpdateErrorGradient (output [i], desired [i], alpha);
		}
	}

	public void UpdateWeights(NeuronLayer nextLayer, double alpha){

		for (int i = 0; i < neurons.Length; i++) {
			neurons [i].UpdateErrorGradient (nextLayer.neurons, i, alpha);
		}
	}

}
