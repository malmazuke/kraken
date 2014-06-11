using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int initialHealth = 100;
	public int maxBoost = 100;
	public GUIText scoreDescrLabel;
	public GUIText scoreLabel;
	public GUIText healthLabel;
	public GUIText healthDescrLabel;
	public GUIText boostLabel;
	public GUIText boostDescrLabel;
	public GUIText finalScoreLabel;
	public GUIText newHighScoreLabel;
	public GUIText highScoreLabel;
	public GameObject titleLabels;
	public GameObject gameoverLabels;
	public Camera minimapCamera;
	public AudioClip beginGameClip;
	public AudioClip gameoverClip;
	
	public bool isTitleShowing = true;
	
	public GameObject playerBody;
	
	/********* SHIPS *********/
	public int maxNumberOfShips = 100;
	private int numberOfShips = 0;
	
	private int health;
	private int boost;
	private int score;
	private int highScore;
	
	private Vector3 originalMinimapPosition;
	
	private bool isDead = false;
	
	void Awake() {
		// We want the fastest possible framerate
		Application.targetFrameRate = -1;
	}
	
	// Use this for initialization
	void Start () {
		health = initialHealth;
		boost = maxBoost;
		score = 0;
		highScore = PlayerPrefs.GetInt("HighScore");
		
		SetGameplayLabelsVisible(false);
		SetTitleLabelsVisible(true);
		SetGameoverLabelsVisible(false);
//		SetMusicPlaying(false);
		
		// Stop ships from generating
		GetComponent <ShipGenerator>().enabled = false;
	}
	
	void Update () {
		// If the player touches the screen, or presses the Start button (enter/return)
		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetButtonDown("Start")){
			
			if (isTitleShowing){
				isTitleShowing = false;
								
				//Update the labels on screen
				SetTitleLabelsVisible(false);
				SetGameplayLabelsVisible(true);
				
				GetComponent <ShipGenerator>().enabled = true;
				
				if (beginGameClip != null){
				 	AudioSource.PlayClipAtPoint(beginGameClip, Vector3.zero);
				}
				
//				SetMusicPlaying(true);
			}
			else if (isPlayerDead()){
				Application.LoadLevel("DefaultScene");
			}
		}
		if (isPlayerDead() && !AreGameOverLabelsVisible()){
			SetGameplayLabelsVisible(false);
			SetGameoverLabelsVisible(true);
//			SetMusicPlaying(false);
			finalScoreLabel.text = "Your Score: " + score;
			
			// Set the highscore
			if (score > highScore) {
				highScore = score;
				PlayerPrefs.SetInt("HighScore", highScore);
				newHighScoreLabel.text = "New High Score!";
			}
			else {
				newHighScoreLabel.text = "";
			}
		}
		else if (!isPlayerDead()){
			Vector2 temp = transform.position;
			transform.position = new Vector2(playerBody.transform.position.x, temp.y);
		}
	}
	
	public bool isPlayerDead() {
		return isDead;
	}
	
	// ***************** SHIPS ***************** //	
	public bool CanGenerateShip(){
		return (numberOfShips < maxNumberOfShips);
	}
	
	public void ShipCreated(){
		numberOfShips++;
	}
	
	public void ShipDestroyed(){
		numberOfShips--;
	}
	
	public int getNumberOfTotalShips(){
		return numberOfShips;
	}
	
	// ***************** PLAYER ***************** //
	public void RemoveHealth(int healthToRemove){
		health -= healthToRemove;
		if (health <= 0){
			health = 0;
			isDead = true;
			if (gameoverClip){
				AudioSource.PlayClipAtPoint(gameoverClip, Camera.main.transform.position);
			}
		}
		UpdateHealth();
	}
	
	public void AddToBoost (int boostToAdd) {
		boost += boostToAdd;
		UpdateBoost();
	}
	
	public void RemoveFromBoost(int boostToRemove) {
		boost -= boostToRemove;
		if (boost < 0){
			boost = 0;
		}
		UpdateBoost();
	}
	
	public void AddScore(int scoreToAdd){
		score += scoreToAdd;
		UpdateScore();
	}
	
	public int GetScore() {
		return score;
	}
	
	public int GetHealth() {
		return health;
	}
	
	public int GetBoost() {
		return boost;
	}
	
	void UpdateScore () {
		scoreLabel.text = "" + score;
	}
	
	void UpdateHealth () {
		healthLabel.text = health + "%";
	}
	
	void UpdateBoost () {
		boostLabel.text = boost + "%";
	}
	
	void SetGameplayLabelsVisible(bool areVisible){
		scoreLabel.enabled = areVisible;
		scoreDescrLabel.enabled = areVisible;
		healthLabel.enabled = areVisible;
		healthDescrLabel.enabled = areVisible;
		boostLabel.enabled = areVisible;
		boostDescrLabel.enabled = areVisible;
		
		SetMinimapVisible(areVisible);
	}
	
	void SetTitleLabelsVisible (bool areVisible){
		if (areVisible){
			titleLabels.transform.position = new Vector3(0.05f, 0.1f);
			// If we've got a high score, display it
			if (highScore != 0){
				highScoreLabel.text = "High Score: " + highScore;
			}
			else {
				highScoreLabel.text = "";
			}
		}
		else {
			titleLabels.transform.position = new Vector3(-1.0f, -1.0f);
		}
	}
	
	void SetGameoverLabelsVisible (bool areVisible){
		if (areVisible){
			gameoverLabels.transform.position = new Vector3(0.05f, 0.1f);
		}
		else {
			gameoverLabels.transform.position = new Vector3(-1.0f, -1.0f);
		}
	}
	
	bool AreGameOverLabelsVisible () {
		if (gameoverLabels.transform.position == new Vector3(0.05f, 0.1f)){
			return true;
		}
		else {
			return false;
		}
	}
	
	void SetMinimapVisible (bool areVisible) {
		minimapCamera.enabled = areVisible;
	}
	
	void SetMusicPlaying(bool shouldPlay){
//		if (shouldPlay){
//			audio.Play();
//		}
//		else {
//			audio.Stop();
//		}
	}
	
	public bool isRunningInEditor() {
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor){
			return true;
		}
		return false;
	}
	
	public static GameController sharedGameController(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		GameController gameController = gameControllerObject.GetComponent <GameController>();
		
		return gameController;
	}
}
