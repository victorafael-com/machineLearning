using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {
	Vector3 ballStartPosition;
	Rigidbody2D m_rigidbody;
	public float speed = 20;
	public AudioSource blip;
	public AudioSource blop;

	public float resetBallInterval = 60;
	private float lastReset = 0;

	// Use this for initialization
	void Start () {
		m_rigidbody = GetComponent<Rigidbody2D> ();
		ballStartPosition = transform.position;
		ResetBall ();
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "backwall")
			blop.Play ();
		else
			blip.Play ();
	}

	void Update(){
		if (resetBallInterval > 0 && Time.time > lastReset + resetBallInterval) {
			ResetBall ();
		}
	}

	// Update is called once per frame
	void ResetBall () {
		transform.position = ballStartPosition;
		m_rigidbody.velocity = new Vector2(Random.Range(1f,3f),Random.Range(-1f,1f)).normalized * speed;
		lastReset = Time.time;
	}
}
