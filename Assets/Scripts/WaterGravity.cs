using UnityEngine;
using System.Collections;

public class WaterGravity : MonoBehaviour {

	public bool inWater = false;
	public float waterRadius = 0.5f;
	public float gravityInWater = 0.01f;
	public float gravityOutWater = 1.0f;
	public float yPositionWater = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		// If we're in the water, set our gravity scale to less
		if (isInWater()){
			rigidbody2D.gravityScale = gravityInWater;
		}
		
		else {
			rigidbody2D.gravityScale = gravityOutWater;
		}
	}
	
	public bool isInWater() {
		return (transform.position.y <= yPositionWater);
	}
}
