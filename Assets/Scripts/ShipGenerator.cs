using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipGenerator : MonoBehaviour {

	public GameController gameController;
	public GameObject smallShip;
	public GameObject largeShip;
	public float smallShipFrequency = 10.0f;
	public float largeShipFrequency = 30.0f;
	public float smallShipDelay = 0.0f;
	public float largeShipDelay = 5.0f;
	private float currentFireRateModifier = 1.0f;
	private float currentShipMoveSpeedModifier = 1.0f;
	private List<GameObject> ships;
	
	// Use this for initialization
	void Start () {
		gameController = GameController.sharedGameController();
		
		InvokeRepeating("GenerateSmallShip", smallShipDelay, smallShipFrequency);
		InvokeRepeating("GenerateLargeShip", largeShipDelay, largeShipFrequency);
		InvokeRepeating ("DecreaseSmallSpawnRate", 20.0f, 40.0f);
		InvokeRepeating ("DecreaseLargeSpawnRate", 50.0f, 50.0f);
		
		ships = new List<GameObject>(100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void GenerateSmallShip() {
		if (gameController.CanGenerateShip()){
			string direction;
			float randVal = Random.value;
			if (randVal < 0.5){
				direction = "left";
			}
			else{
				direction = "right";
			}
			
			float startX = (direction == "left") ? 20.0f + transform.position.x: -20.0f + transform.position.x;
			GameObject ship = (GameObject)Instantiate(smallShip, new Vector3(startX, 0.5f, 1.75f), Quaternion.identity);
			
			ShipMovement shipMovement = ship.GetComponent <ShipMovement>();
			shipMovement.SetDirection(direction);
			shipMovement.speed = shipMovement.speed * currentShipMoveSpeedModifier;
			
			ShipCannonFire shipFire = ship.GetComponent <ShipCannonFire>();
			shipFire.player = GameObject.FindWithTag ("PlayerBody");
			shipFire.fireSpeed = shipFire.fireSpeed * currentFireRateModifier;
			
			gameController.ShipCreated();
		}
	}
	
	void GenerateLargeShip() {
		if (gameController.CanGenerateShip()){
			string direction;
			float randVal = Random.value;
			if (randVal < 0.5){
				direction = "left";
			}
			else{
				direction = "right";
			}
			
			float startX = (direction == "left") ? 20.0f + transform.position.x: -20.0f + transform.position.x;
			GameObject ship = (GameObject)Instantiate(largeShip, new Vector3(startX, 0.5f, 1.75f), Quaternion.identity);
			
			ShipMovement shipMovement = ship.GetComponent <ShipMovement>();
			shipMovement.SetDirection(direction);
			
			ShipCannonFire shipFire = ship.GetComponent <ShipCannonFire>();
			shipFire.player = GameObject.FindWithTag ("PlayerBody");
			
			gameController.ShipCreated();
		}
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
