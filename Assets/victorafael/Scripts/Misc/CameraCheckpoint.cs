using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheckpoint : MonoBehaviour {
	public CameraTripod tripod;
	public BasePopulationManager populationManager;

	void Start(){
		if (tripod == null) {
			tripod = FindObjectOfType<CameraTripod> ();
		}
		if (tripod == null) {
			gameObject.SetActive (false);
		} else {
			populationManager.onRestartSimulation += OnRestartSimulation;
		}
	}

	void OnRestartSimulation ()
	{
		gameObject.SetActive (true);
	}
	void OnTriggerEnter2D(Collider2D other){
		tripod.MoveTo (transform.position);
		gameObject.SetActive (false);
	}
}
