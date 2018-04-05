using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour {
	private const int MaxEstimationBounce = 8;

	private MoveBall ball;
	public Brain brain;

	public float minY = 8.8f;
	public float maxY = 17.4f;
	public float speed;

	private float currentInput;

	public Rigidbody2D ballRb;

	public int saved = 0;
	public int missed = 0;
	// Use this for initialization

	public LayerMask raycastMask;

	public double[] normalizationValues;
	private double[] inputValues;
	private double[] outputs;

	public bool train;


	void Start () {
		/* Inputs		| Outputs
		 * 				|
		 * Ball X		| Paddle Velocity Y
		 * Ball Y		|
		 * Ball Speed X |
		 * Ball Speed Y |
		 * Paddle X 	|
		 * Paddle Y		|
		 */
		ball = ballRb.GetComponent<MoveBall> ();
		inputValues = new double[6];
		outputs = new double[1];
		brain.StartNeuralNetwork (6, 1);
	}

	double[] Run(bool train = false, float value = 0){
		inputValues [0] = ballRb.transform.localPosition.x / normalizationValues[0];
		inputValues [1] = ballRb.transform.localPosition.y / normalizationValues[1];
		inputValues [2] = ballRb.velocity.x / normalizationValues[2];
		inputValues [3] = ballRb.velocity.y / normalizationValues[3];
		inputValues [4] = transform.localPosition.x / normalizationValues[4];
		inputValues [5] = transform.localPosition.y / normalizationValues[5];
		if (!train) {
			return brain.Execute (inputValues);
		} else {
			outputs [0] = value;
			return brain.ExecuteAndTrain (inputValues, outputs);
		}
	}

	void Update(){

		Vector3 pos = transform.localPosition;
		pos.y = Mathf.Clamp (pos.y + currentInput * speed * Time.deltaTime, minY, maxY);
		transform.localPosition = pos;

		Vector2 point;

		bool canTrain = false;

		if (train && Physics2D.CircleCast (ballRb.position, 0.5f, Vector2.up, 0, raycastMask).collider == null) {
			canTrain = true;
		}

		if (canTrain && BounceUntilBackPos (ballRb.position, ballRb.velocity, out point)) {
			currentInput = (float)Run (true, point.y - transform.position.y)[0];
		} else {
			currentInput = (float)Run ()[0];
		}
		
	}

	private bool BounceUntilBackPos(Vector2 pos, Vector2 direction, out Vector2 collisionPoint, int iteration = 0, Collider2D disableCollider = null){
		if (iteration >= MaxEstimationBounce) {
			Debug.DrawLine (pos - new Vector2 (-.3f, -.3f), pos + new Vector2 (.3f, .3f), Color.red);
			Debug.DrawLine (pos - new Vector2 (-.3f, .3f), pos + new Vector2 (.3f, -.3f), Color.red);
			collisionPoint = Vector3.zero;
			return false;
		}

		if (disableCollider != null)
			disableCollider.enabled = false;
		RaycastHit2D hit = Physics2D.Raycast (pos, direction, 500, raycastMask);
		if (disableCollider != null)
			disableCollider.enabled = true;
		
		if (hit.collider != null) {
			Debug.DrawLine (pos, hit.point, Color.white);
			if (hit.collider.tag.Equals ("paddleWall")) {
				collisionPoint = hit.point;
				Debug.DrawLine (hit.point, hit.point + Vector2.right, Color.green);
				return true;
			} else {
				return BounceUntilBackPos (hit.point, Vector2.Reflect(direction, hit.normal), out collisionPoint, iteration + 1, hit.collider);
			}
		} else {
			Debug.DrawLine (pos, pos + direction, Color.red);
			collisionPoint = Vector3.zero;
			return false;
		}
	}
}
