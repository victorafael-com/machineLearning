using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Brain))]
public class PaddleScript : MonoBehaviour {
	private const int MaxEstimationBounce = 8;

	private Brain brain;

	public float minY = 8.8f;
	public float maxY = 17.4f;
	public float speed;

	private float currentInput;

	public Rigidbody2D ballRb;

	public int saved = 0;
	public int missed = 0;
	// Use this for initialization

	public LayerMask raycastMask;

	private double[] inputValues;
	private double[] outputs;

	public bool train;


	void Start () {
		
		brain = GetComponent<Brain> ();
		/* Inputs		| Outputs
		 * 				|
		 * Ball X		| Paddle Velocity Y
		 * Ball Y		|
		 * Ball Speed X |
		 * Ball Speed Y |
		 * Paddle X 	|
		 * Paddle Y		|
		 */
		inputValues = new double[6];
		outputs = new double[1];
		brain.StartNeuralNetwork (6, 1);
	}

	double[] Run(bool train = false, float value = 0){
		inputValues [0] = ballRb.position.x;
		inputValues [1] = ballRb.position.y;
		inputValues [2] = ballRb.velocity.x;
		inputValues [3] = ballRb.velocity.y;
		inputValues [4] = transform.position.x;
		inputValues [5] = transform.position.y;
		if (!train) {
			return brain.Execute (inputValues);
		} else {
			outputs [0] = value;
			return brain.ExecuteAndTrain (inputValues, outputs);
		}
	}

	void Update(){

		Vector3 pos = transform.position;
		pos.y = Mathf.Clamp (pos.y + currentInput * speed * Time.deltaTime, minY, maxY);
		transform.position = pos;

		Vector2 point;
		if (train && BounceUntilBackPos (ballRb.position, ballRb.velocity, out point)) {
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
