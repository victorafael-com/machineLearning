using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Events;

public class PopulationManager<T> : BasePopulationManager where T : BreedableEntity {

	public float ElapsedTime{
		get{
			return Time.time - startTime;
		}
	}
	public int Generation{ get; private set; }


	public GameObject prefab;
	public int populationSize;
	public int survivorsPerGeneration;
	protected List<T> entities = new List<T>();

	public Vector3 minSpawnPos, maxSpawnPos;

	protected float startTime = 0;

	// Use this for initialization
	protected void SpawnFirstGeneration () {
		Generation = 1;
		startTime = Time.time;
		for (int i = 0; i < populationSize; i++) {
			var entity = Spawn ();
			entity.SetRandom ();
			entities.Add (entity);
		}
	}

	protected void BreedPopulation(List<T> bestIndividuals){
		Generation++;
		startTime = Time.time;

		if (survivorsPerGeneration < 2) {
			Debug.LogError ("Unnable to breed with too little survivors");
			return;
		}

		var newEntities = new List<T> ();
		for(int i = 0; i < populationSize; i++){
			int a = Random.Range (0, bestIndividuals.Count);
			int b;
			do {
				b = Random.Range (0, bestIndividuals.Count);
			} while(a == b);

			var entity = Spawn ();
			entity.SetInheritance (bestIndividuals [a].dna, bestIndividuals [b].dna);
			newEntities.Add (entity);
		}
		for (int i = 0; i < entities.Count; i++) {
			Destroy (entities [i].gameObject);
		}
		entities.Clear ();
		entities.AddRange (newEntities);

		TriggerRestartSimulationEvent ();
	}

	protected virtual T Spawn(){
		Vector3 pos = new Vector3 (
	          Random.Range (minSpawnPos.x, maxSpawnPos.x),
	          Random.Range (minSpawnPos.y, maxSpawnPos.y),
	          Random.Range (minSpawnPos.z, maxSpawnPos.z)
		);

		GameObject go = Instantiate (prefab, pos, Quaternion.identity);
		var entity = go.GetComponent<T> ();
		return entity;
	}
	void OnDrawGizmos(){
		Gizmos.DrawCube (Vector3.Lerp (minSpawnPos, maxSpawnPos, 0.5f), maxSpawnPos - minSpawnPos);
	}
}
