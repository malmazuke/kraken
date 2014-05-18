using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float jetMultiplier = 5.0f;
	
	bool inWater = false;
	float waterRadius = 0.2f;
	public Transform waterCheck;
	public LayerMask whatIsWater;
	
	public AudioClip playerHitClip;
	
	public GameObject leftTentacle;
	public GameObject rightTentacle;
	public GameObject legs;
	public ParticleSystem particleSystem;
	
	public bool isBoosting = false;
	private int timeSinceBoost = 0;
	
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
			//set our inWater bool
			inWater = Physics2D.OverlapCircle (waterCheck.position, waterRadius, whatIsWater);
			
			// Get the horizontal and vertical axis.
			// The value is in the range -1 to 1
			float xAxis = Input.GetAxis ("Horizontal");
			float yAxis = Input.GetAxis ("Vertical");
			
			if (inWater){
				float jetSpeed = speed;
				if (Input.GetButton("Boost") && gameController.GetBoost() > 0){
					jetSpeed = speed * jetMultiplier;
					isBoosting = true;
					gameController.RemoveFromBoost(1);
					timeSinceBoost = 0;
					
					particleSystem.Play();
				}
				else {
					isBoosting = false;
					particleSystem.Stop();
					if (timeSinceBoost >= 30){
						if (gameController.GetBoost() < 100){
							gameController.AddToBoost(1);
						}
					}
					else{
						timeSinceBoost += 1;
					}
				}
				float moveX = xAxis * jetSpeed * Time.deltaTime;
				float moveY = yAxis * jetSpeed * Time.deltaTime;
			
				rigidbody2D.AddForce (new Vector3 (moveX, moveY, 0.0f));
				
				SetGravityForAll(0.01f);
			}
			
			else {
				SetGravityForAll(1.0f);
				particleSystem.Stop();
			}
			
			// Rotation
			float angle = Mathf.Atan2(-xAxis, yAxis) * Mathf.Rad2Deg;
			Quaternion desiredDirection = Quaternion.AngleAxis(angle, Vector3.forward);
			
			if (xAxis != 0.0f || yAxis != 0.0f){
				rigidbody2D.angularVelocity = 0.0f;
				transform.rotation = Quaternion.Slerp(transform.rotation, desiredDirection, 0.2f);
			}
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
}
