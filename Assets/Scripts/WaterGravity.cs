﻿using UnityEngine;
using System.Collections;

public class WaterGravity : MonoBehaviour {

	bool inWater = false;
	public Transform waterCheck;
	public float waterRadius = 0.5f;
	public LayerMask whatIsWater;
	public float gravityInWater = 0.01f;
	public float gravityOutWater = 1.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		//set our inWater bool
		inWater = Physics2D.OverlapCircle (waterCheck.position, waterRadius, whatIsWater);
		
		if (inWater){
			rigidbody2D.gravityScale = gravityInWater;
		}
		
		else {
			rigidbody2D.gravityScale = gravityOutWater;
		}
	}
}
