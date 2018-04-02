using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePopulationManager : MonoBehaviour {
	public event UnityAction onRestartSimulation;

	protected void TriggerRestartSimulationEvent(){
		if (onRestartSimulation != null)
			onRestartSimulation ();
	}
}
