using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBehaviour0327 : MonoBehaviour {
	public Throw0327 throwScript;
	public Perceptron perceptron;

	private Animator m_animator;
	private Rigidbody m_rigidBody;
	// Use this for initialization
	void Start () {
		if (perceptron == null) {
			perceptron = gameObject.AddComponent<Perceptron> ();
			perceptron.dynamicDataCount = 2;
		}
		throwScript.onThrow += OnProjectileIsThrown;

		m_animator = GetComponent<Animator> ();
		m_rigidBody = GetComponent<Rigidbody> ();
	}

	void OnProjectileIsThrown (Projectitle0327 projectile)
	{
		double result = perceptron.CalculateOutput (projectile.color, projectile.shape);
		if (result == 0) {
			m_rigidBody.isKinematic = false;
			m_animator.SetTrigger ("Crouch");
		} else {
			m_rigidBody.isKinematic = true;
		}

		perceptron.AcquireKnowledge (projectile.name, projectile.result, projectile.color, projectile.shape);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
