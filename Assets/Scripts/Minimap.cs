using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

	public GameObject playerBody;
	public GUITexture minimapSky;
	public GUITexture minimapWater;
	public GUITexture minimapPlayer;
	public GUITexture minimapSmallShip;
	public float waterLinePosition = 6.0f;
	public float waterScreenEdgePosition = 2.45f;
	
	private float totalWaterSkyHeight;
	
	private GameController gameController;
	
	void Start () {
		totalWaterSkyHeight = 58.0f;
		
		gameController = GameController.sharedGameController();
	}
	
	// Update is called once per frame
	void Update () {
		// Depending on the player's current y position, change the relative water + sky heights on the minimap
		// Calculate the mount to scale based on the y position relative to the screen size
		
		if (!gameController.isPlayerDead()){
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
			
			if (normalizedYPosition < 1.0f || normalizedYPosition > 0.0f){
				// for each small ship, add to minimap
				GameObject[] gos;
				gos = GameObject.FindGameObjectsWithTag("SmallShip");
				
				foreach (GameObject go in gos) {
					Vector3 diff = go.transform.position - transform.position;
					float curDistance = diff.sqrMagnitude;
					
					
				}
			}
		}
	}
}
