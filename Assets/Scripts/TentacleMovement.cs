using UnityEngine;
using System.Collections;

public class TentacleMovement : MonoBehaviour {

	public Joystick tentacleJoystick;
	public float speed = 1.0f;
	private GameController gameController;
	
	// Use this for initialization
	void Start () {
		gameController = GameController.sharedGameController();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameController.isPlayerDead()){ // If we're still alive, move the tentacles
			// Tentacle Movement
			// Get the horizontal and vertical axis.
			// The value is in the range -1 to 1
			float xAxisCursor = tentacleJoystick.position.x;
			float yAxisCursor = tentacleJoystick.position.y;
			
			// If we're on a PC, etc, use the standard axis
			if (gameController.isRunningInEditor()) {
				xAxisCursor = Input.GetAxis ("CursorHorizontal");
				yAxisCursor = Input.GetAxis ("CursorVertical");
			}
			
			GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
			
			if (xAxisCursor != 0.0f || yAxisCursor != 0.0f){
				
				float moveXPalm = xAxisCursor * speed * Time.deltaTime;
				float moveYPalm = yAxisCursor * speed * Time.deltaTime;
				
				GetComponent<Rigidbody2D>().AddForce (new Vector3 (moveXPalm, moveYPalm, 0.0f));
			}
		}
	}
}
