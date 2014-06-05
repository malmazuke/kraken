using UnityEngine;
using System.Collections;

public class TileWrap : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		// If the tile is completely outside of the screen, offset it back to the top
		
		// Get the right edge of the tile
		Vector3 rightSide = new Vector3(transform.position.x + renderer.bounds.size.x/2.0f, 
		                                transform.position.y + renderer.bounds.size.y/2.0f, 
		                                transform.position.z + renderer.bounds.size.z/2.0f);
		Vector3 viewPos = Camera.main.WorldToViewportPoint(rightSide);
		
		if (viewPos.x < -1.0f){
			transform.position = new Vector3(transform.position.x + (renderer.bounds.size.x * 3), transform.position.y, transform.position.z);
		}
		
		// Get the left edge of the tile
		Vector3 leftSide = new Vector3(transform.position.x - renderer.bounds.size.x/2.0f, 
		                               transform.position.y - renderer.bounds.size.y/2.0f, 
		                               transform.position.z - renderer.bounds.size.z/2.0f);
		viewPos = Camera.main.WorldToViewportPoint(leftSide);
		
		if (viewPos.x > 1.0f){
			transform.position = new Vector3(transform.position.x - (renderer.bounds.size.x * 3), transform.position.y, transform.position.z);
		}
	}
}
