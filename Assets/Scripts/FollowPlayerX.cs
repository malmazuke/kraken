using UnityEngine;
using System.Collections;

public class FollowPlayerX : MonoBehaviour {

	// The moving player body
	public GameObject playerBody;
	public GameController gameController;
	
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
	
	// Follow the player on the X axis
	void Update () {
		if (!gameController.isPlayerDead()){ // If we're still alive, follow
			Vector2 temp = transform.position;
			transform.position = new Vector2(playerBody.transform.position.x, temp.y);
		}
	}
}
