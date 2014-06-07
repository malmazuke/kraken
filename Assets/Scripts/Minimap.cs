using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	public GameObject playerBody;
	public GUITexture minimapSky;
	public GUITexture minimapWater;
	public GUITexture minimapPlayer;
	public float waterLinePosition = 6.0f;
	public float waterScreenEdgePosition = 2.45f;
	
	private float originalWaterHeight;
	private float originalSkyHeight;
	private float totalWaterSkyHeight;
	
	void Start () {
		// Get the original offsets for both sky & water
//		originalWaterHeight = minimapWater.pixelInset.height;
//		originalSkyHeight = minimapSky.pixelInset.height;
		totalWaterSkyHeight = 58.0f;//originalWaterHeight + originalSkyHeight;
	}
	
	// Update is called once per frame
	void Update () {
		// Depending on the player's current y position, change the relative water + sky heights on the minimap
		// Calculate the mount to scale based on the y position relative to the screen size
		
		float bottom = 0.0f;
		float center = waterLinePosition - waterScreenEdgePosition;
		float top = center + waterScreenEdgePosition;
		
		float relativeYPosition = playerBody.transform.position.y - waterScreenEdgePosition;
		
		float normalizedYPosition = relativeYPosition / top;
		
		if (normalizedYPosition > 1.0f) normalizedYPosition = 1.0f;
		else if (normalizedYPosition < 0.0f) normalizedYPosition = 0.0f;
		
		float height = totalWaterSkyHeight * (1.0f - normalizedYPosition);
		minimapWater.pixelInset = new Rect(minimapWater.pixelInset.x, minimapWater.pixelInset.y, minimapWater.pixelInset.width, height);
		
		height = totalWaterSkyHeight * normalizedYPosition * -1;
		minimapSky.pixelInset = new Rect(minimapSky.pixelInset.x, minimapSky.pixelInset.y, minimapSky.pixelInset.width, height);
	}
}
