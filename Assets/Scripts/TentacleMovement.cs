using UnityEngine;
using System.Collections;

public class TentacleMovement : MonoBehaviour {

	public float speed = 1.0f;
	private GameController gameController;
	
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameController.isPlayerDead()){ // If we're still alive, move the tentacles
			// Tentacle Movement
			// Get the horizontal and vertical axis.
			// The value is in the range -1 to 1
			float xAxisCursor = Input.GetAxis ("CursorHorizontal");
			float yAxisCursor = Input.GetAxis ("CursorVertical");
			
			rigidbody2D.angularVelocity = 0.0f;
			
			if (xAxisCursor != 0.0f || yAxisCursor != 0.0f){
				
				float moveXPalm = xAxisCursor * speed * Time.deltaTime;
				float moveYPalm = yAxisCursor * speed * Time.deltaTime;
				
				rigidbody2D.AddForce (new Vector3 (moveXPalm, moveYPalm, 0.0f));
			}
		}
	}
}
