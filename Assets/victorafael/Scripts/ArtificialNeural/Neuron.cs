using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Events;

public class Neuron{
	private delegate double ActivationDelegate(double val);
	public enum ActivationType
	{
		Sigmoid,
		Identy,
		Tan,
		Step,
		LeakyLinear
	}

	public double bias;
	public double errorGradient;
	public double[] weights;
	private ActivationDelegate activationMethod;

	private double[] lastInput;
	public double lastOutput;


	public Neuron(int inputCount, ActivationType type = ActivationType.Sigmoid){
		weights = new double[inputCount];
		bias = Random.Range (-1f, 1f);
		for (int i = 0; i < inputCount; i++) {
			weights [i] = Random.Range (-1f, 1f);
		}

		FindActivationMethod (type);
	}


	public void UpdateErrorGradient(double output, double desired, double alpha){
		double error = desired - output;
		errorGradient = output * (1 - output) * error;
		//Error gradient calculated with Delta rule http://en.wikipedia.org/wiki/Delta_rule

		for (int i = 0; i < lastInput.Length; i++) {
			weights [i] += alpha * lastInput [i] * error;
		}
		bias += alpha * -1 * errorGradient;
	}

	public void UpdateErrorGradient(Neuron[] nextNeurons, int weightIndex, double alpha){
		double errorGrandSum = 0;
		for (int i = 0; i < nextNeurons.Length; i++) {
			errorGrandSum += nextNeurons [i].errorGradient * nextNeurons [i].weights [weightIndex];
		}
		errorGradient = lastOutput * (1 - lastOutput) * errorGrandSum;

		for (int i = 0; i < lastInput.Length; i++) {
			weights [i] += alpha * lastInput [i] * errorGradient;
		}
		bias += alpha * -1 * errorGradient;
	}

	void FindActivationMethod(ActivationType type){
		switch (type) {
		case ActivationType.Identy:
			activationMethod = ActivationIdenty;
			break;
		case ActivationType.Sigmoid:
			activationMethod = ActivationSigmoid;
			break;
		case ActivationType.Tan:
			activationMethod = ActivationTan;
			break;
		case ActivationType.Step:
			activationMethod = ActivationStep;
			break;
		case ActivationType.LeakyLinear:
			activationMethod = ActivationLeakyLinear;
			break;
		}
	}

	public double Process(double[] inputs){
		double N = 0;
		lastInput = inputs;
		for (int i = 0; i < inputs.Length; i++) {
			N += weights [i] * inputs [i];
		}
		N -= bias;
		lastOutput = activationMethod(N);
		return lastOutput;
	}

	private double ActivationStep(double value){
		return value < 0 ? 0 : 1;
	}
	private double ActivationSigmoid(double val){
		val = System.Math.Exp (val);
		return val / (1f + val);
	}
	private double ActivationIdenty(double val){
		return val;
	}
	private double ActivationLeakyLinear(double val){
		return val < 0 ? 0.05 * val : val;
	}
	private double ActivationTan(double val){
		return Mathf.Tan ((float)val);
	}
}
