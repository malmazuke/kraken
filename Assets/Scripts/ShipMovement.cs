using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	public float speed = 600.0f;
	public string direction;
	
	// Use this for initialization
	void Start () {
		if (direction == null || (direction != "left" && direction != "right")){
			float randVal = Random.value;
			if (randVal < 0.5){
				direction = "left";
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
			else{
				direction = "right";
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Move the ship in the specified direction
		float xForce = (direction == "left") ? -speed : speed;
		xForce = xForce * Time.deltaTime;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, 0.0f));
	}
	
	public void SetDirection(string newDirection){
		direction = newDirection;
		transform.localScale = new Vector3((direction == "left") ? -transform.localScale.x : transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
