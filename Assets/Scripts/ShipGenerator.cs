using UnityEngine;
using System.Collections;

public class ShipGenerator : MonoBehaviour {

	public GameObject smallShip;
	public GameObject largeShip;
	public float smallShipFrequency = 10.0f;
	public float largeShipFrequency = 30.0f;
	
	private float currentFireRateModifier = 1.0f;
	private float currentShipMoveSpeedModifier = 1.0f;
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("GenerateSmallShip", 0.0f, smallShipFrequency);
		InvokeRepeating("GenerateLargeShip", 30.0f, largeShipFrequency);
		InvokeRepeating ("DecreaseSmallSpawnRate", 20.0f, 40.0f);
		InvokeRepeating ("DecreaseLargeSpawnRate", 50.0f, 50.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void GenerateSmallShip() {
		string direction;
		float randVal = Random.value;
		if (randVal < 0.5){
			direction = "left";
		}
		else{
			direction = "right";
		}
		
		float startX = (direction == "left") ? 70.0f: -70.0f;
		GameObject ship = (GameObject)Instantiate(smallShip, new Vector3(startX, 0.5f, 0.0f), Quaternion.identity);
		
		ShipMovement shipMovement = ship.GetComponent <ShipMovement>();
		shipMovement.SetDirection(direction);
		shipMovement.speed = shipMovement.speed * currentShipMoveSpeedModifier;
		
		ShipCannonFire shipFire = ship.GetComponent <ShipCannonFire>();
		shipFire.player = GameObject.FindWithTag ("PlayerBody");
		shipFire.fireSpeed = shipFire.fireSpeed * currentFireRateModifier;
	}
	
	void GenerateLargeShip() {
		string direction;
		float randVal = Random.value;
		if (randVal < 0.5){
			direction = "left";
		}
		else{
			direction = "right";
		}
		
		float startX = (direction == "left") ? 70.0f: -70.0f;
		GameObject ship = (GameObject)Instantiate(largeShip, new Vector3(startX, 0.5f, 0.0f), Quaternion.identity);
		
		ShipMovement shipMovement = ship.GetComponent <ShipMovement>();
		shipMovement.SetDirection(direction);
		
		ShipCannonFire shipFire = ship.GetComponent <ShipCannonFire>();
		shipFire.player = GameObject.FindWithTag ("PlayerBody");
	}
	
	void DecreaseSmallSpawnRate () {
		if (smallShipFrequency > 3.0f){
			smallShipFrequency -= 1.0f;
		}
		currentFireRateModifier += 0.125f;
		currentShipMoveSpeedModifier += 0.125f;
	}
	
	void DecreaseLargeSpawnRate () {
		if (largeShipFrequency > 3.0f){
			largeShipFrequency -= 1.0f;
		}
	}
}
