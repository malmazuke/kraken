using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	
	public float startingHealth = 100.0f;
	public AudioClip damageSound;
	public AudioClip bigDamageSound;
	public AudioClip deathSound;
	public GUIText healthLabel;
	public int tentacleScore = 50;
	public int bodyHitScore = 75;
	public int killScore = 250;
	
	private GameController gameController;
	private float currentHealth;
	
	// Use this for initialization
	void Start () {
		gameController = GameController.sharedGameController();
		
		currentHealth = startingHealth;
		
		// Hide the label until we are hit
		healthLabel.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (currentHealth <= 0){
			Die();
		}
	}
	
	void Die () {
		Destroy(gameObject);
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		gameController.AddScore(killScore);
		gameController.ShipDestroyed();
	}
	
//	void OnCollisionEnter (Collision col) {
//		Debug.Log("Hello");
//	}
	
	void OnTriggerEnter2D(Collider2D other){
		// If the player is dead, don't increase the score, etc
		if (!gameController.isPlayerDead()){
			// If the ship is hit by a tentacle, reduce health
			if (other.gameObject.tag == "TentaclePalm"){
				currentHealth -= 40.0f;
				rigidbody2D.AddForce(other.attachedRigidbody.velocity * 20.0f);
				AudioSource.PlayClipAtPoint(damageSound, transform.position);
				gameController.AddScore(tentacleScore);
				UpdateHealthLabel();
			}
			else if (other.gameObject.tag == "PlayerBody"){
				PlayerController player = other.GetComponent<PlayerController>();
				if (player.isBoosting){
					currentHealth -= 50.0f;
					rigidbody2D.AddForce(other.attachedRigidbody.velocity * 40.0f);
					AudioSource.PlayClipAtPoint(bigDamageSound, transform.position);
					gameController.AddScore(bodyHitScore);
					UpdateHealthLabel();
				}
			}	
		}
		// If ship goes out of bounds, wrap it to the other side
		if (other.gameObject.tag == "WrapL"){
			GameObject wrapR = GameObject.FindWithTag("WrapR");
			Vector2 tempPos = transform.position;
			transform.position = new Vector2(wrapR.transform.position.x-5.0f, tempPos.y);
		}
		else if (other.gameObject.tag == "WrapR"){
			GameObject wrapL = GameObject.FindWithTag("WrapL");
			Vector2 tempPos = transform.position;
			transform.position = new Vector2(wrapL.transform.position.x+5.0f, tempPos.y);
		}
	}
	
	void UpdateHealthLabel(){
		healthLabel.enabled = true;
		healthLabel.text = currentHealth + "%";
	}
}
