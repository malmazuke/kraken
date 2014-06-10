using UnityEngine;
using System.Collections;

public class Splashable : MonoBehaviour {

	// The sound to play when the player hits the water
	public AudioClip playerSplash;
	public float yPositionWater = 0.0f;
	
	// Used to calculate if the character is travelling downwards (for water splashes, etc)
	private bool shouldPlaySample = false;
	
	// Update is called once per frame
	void FixedUpdate () {
		// Figure out if we need to create a splash or not
		if (transform.position.y < yPositionWater){
			// If we're travelling down the screen, create a splash
			if (shouldPlaySample){
				AudioSource.PlayClipAtPoint(playerSplash, transform.position);
				shouldPlaySample = false;
			}
		}
		else {
			shouldPlaySample = true;
		}
	}
}
