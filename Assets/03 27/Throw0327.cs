using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Throw0327 : MonoBehaviour {
	public GameObject[] prefabs;

	public event UnityAction<Projectitle0327> onThrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			GameObject g = Instantiate (prefabs [Random.Range (0, prefabs.Length)], Camera.main.transform.position, Camera.main.transform.rotation);
			g.GetComponent<Rigidbody> ().AddForce (0, 0, 500);
			Destroy (g, 5);
			if (onThrow != null) {
				onThrow (g.GetComponent<Projectitle0327> ());
			}
		}
	}
}
