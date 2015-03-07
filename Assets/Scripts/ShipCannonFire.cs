using UnityEngine;
using System.Collections;

public class ShipCannonFire : MonoBehaviour {

	public GameObject projectilePrefab;
	public GameObject player;
	public float fireSpeed = 3000.0f;
	public float fireFrequency = 5.0f;
	public AudioClip projectileSound;
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("FireProjectile", fireFrequency, fireFrequency);
	}
	
	void FireProjectile () {
		if (player != null){
			// Instantiate the projectile at the position and rotation of this transform
			GameObject cannonball = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
			AudioSource.PlayClipAtPoint(projectileSound, transform.position);
			
			Vector3 dir = player.transform.position - transform.position;
			dir = dir.normalized;
			
			cannonball.GetComponent<Rigidbody2D>().AddForce(dir * fireSpeed);
			GetComponent<Rigidbody2D>().AddForce(-dir * (fireSpeed/15.0f));
			
			Destroy (cannonball, 8);
		}
	}
}
