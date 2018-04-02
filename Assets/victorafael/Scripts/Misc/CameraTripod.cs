using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTripod : MonoBehaviour {
	public BasePopulationManager populationManager;
	Vector3 startPos;
	Vector3 targetPos;
	public float moveSpeed = 3;
	// Use this for initialization
	void Start () {
		startPos = targetPos = transform.position;
		populationManager.onRestartSimulation += PopulationManager_onRestartSimulation;
	}

	void PopulationManager_onRestartSimulation ()
	{
		transform.position = startPos;
		targetPos = startPos;
	}

	public void MoveTo(Vector3 pos){
		targetPos = pos;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * moveSpeed);
	}
}
