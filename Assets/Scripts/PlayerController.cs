using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Joystick moveJoystick;
	public Button boostButton;
	
	public float yPositionWater = 0.0f;
	public float speed = 15.0f;
	public float jetMultiplier = 5.0f;
	
	public AudioClip playerHitClip;
	
	public GameObject leftTentacle;
	public GameObject rightTentacle;
	public GameObject legs;
	public ParticleSystem boostParticles;
	public GameObject minimapIcon;
	
	public bool isBoosting = false;
	private int timeSinceBoost = 0;
	
	private GameController gameController;
	
	// Use this for initialization
	void Start () {
		gameController = GameController.sharedGameController();
	}
	
	void Update () {
		// Check health
		if (gameController.GetHealth() <= 0){
			Die();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// If we're still alive, do stuff
		if (!gameController.isPlayerDead()){
			
			// Get the horizontal and vertical axis.
			// The value is in the range -1 to 1
			float xAxis = moveJoystick.position.x;
			float yAxis = moveJoystick.position.y;
			
			// If we're on a PC, etc, use the standard axis
			if (gameController.isRunningInEditor()) {
				xAxis = Input.GetAxis ("Horizontal");
				yAxis = Input.GetAxis ("Vertical");
			}
			
			// If we're in the water, allow movement
			if (isInWater()){
				// Move in direction
				float moveX = xAxis * speed * Time.deltaTime;
				float moveY = yAxis * speed * Time.deltaTime;
			
				rigidbody2D.AddForce (new Vector3 (moveX, moveY, 0.0f));
				
				// Boost
				bool boostPressed = boostButton.isPressed;
				// If running in the editor, use the keyboard boost button
				if (gameController.isRunningInEditor()){
					if (Input.GetButton("Boost")){
						boostPressed = true;
					}
				}
				
				if (boostPressed && gameController.GetBoost() > 0){
					isBoosting = true;
					gameController.RemoveFromBoost(1);
					timeSinceBoost = 0;
					
					boostParticles.Play();
					
					// Add boost vector
					Vector3 boostVector = transform.up * (jetMultiplier * speed) * Time.deltaTime;
					rigidbody2D.AddForce(boostVector);
				}
				else {
					isBoosting = false;
					boostParticles.Stop();
					if (timeSinceBoost >= 30){
						if (gameController.GetBoost() < 100){
							gameController.AddToBoost(1);
						}
					}
					else{
						timeSinceBoost += 1;
					}
				}
				
				SetGravityForAll(0.01f);
			}
			
			else {
				SetGravityForAll(1.0f);
				boostParticles.Stop();
			}
			
			// Rotation
			float angle = Mathf.Atan2(-xAxis, yAxis) * Mathf.Rad2Deg;
			Quaternion desiredDirection = Quaternion.AngleAxis(angle, Vector3.forward);
			
			if (xAxis != 0.0f || yAxis != 0.0f){
				rigidbody2D.angularVelocity = 0.0f;
				transform.rotation = Quaternion.Slerp(transform.rotation, desiredDirection, 0.2f);
			}
			
			// Make sure that the minimap icon is always pointing north
			minimapIcon.transform.rotation = Quaternion.identity;
		}
	}
	
	void SetGravityForAll(float gravity) {
		rigidbody2D.gravityScale = gravity;
		// Set the gravity of the tentacles
		foreach (Transform child in leftTentacle.transform)
		{
			//child is your child transform
			child.rigidbody2D.gravityScale = gravity;
		}
		foreach (Transform child in rightTentacle.transform)
		{
			//child is your child transform
			child.rigidbody2D.gravityScale = gravity;
		}
		foreach (Transform child in legs.transform){ // Legs
			foreach (Transform legSegment in child.transform){
				legSegment.rigidbody2D.gravityScale = gravity;
			}
		}
	}	
	
	void OnTriggerEnter2D(Collider2D other){
		// If the ship is hit by a tentacle, reduce health
		if (other.gameObject.tag == "Projectile"){
			Destroy(other.gameObject, 0.1f);
			gameController.RemoveHealth(5);
			AudioSource.PlayClipAtPoint(playerHitClip, transform.position);
		}
		else if (other.gameObject.tag == "ProjectileLarge"){
			Destroy(other.gameObject, 0.1f);
			gameController.RemoveHealth(25);
			AudioSource.PlayClipAtPoint(playerHitClip, transform.position);
		}
	}
	
	void Die () {
		Destroy(gameObject, 0.1f);
	}
	
	bool isInWater() {
		return (transform.position.y <= yPositionWater);
	}
}
