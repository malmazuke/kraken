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
	public GameObject titleLabels;
	public GameObject gameoverLabels;
	public AudioClip beginGameClip;
	public AudioClip gameoverClip;
	
	public bool isTitleShowing = true;
	
	private int health;
	private int boost;
	private int score;
//	private int highScore;
	
	private bool isDead = false;
	
	// Use this for initialization
	void Start () {
		health = initialHealth;
		boost = maxBoost;
		score = 0;
//		highScore = 0;
		
		SetGameplayLabelsVisible(false);
		SetTitleLabelsVisible(true);
		SetGameoverLabelsVisible(false);
//		SetMusicPlaying(false);
		
		// Stop ships from generating
		GetComponent <ShipGenerator>().enabled = false;
	}
	
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
//			Vector2 pos = Input.GetTouch(0).position;
			
			if (isTitleShowing){
				isTitleShowing = false;
				
				//Update the labels on screen
				titleLabels.transform.position = new Vector3(-1, -1);
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
		if (isPlayerDead()){
			SetGameplayLabelsVisible(false);
			SetGameoverLabelsVisible(true);
//			SetMusicPlaying(false);
			finalScoreLabel.text = "Your Score: " + score;
		}
	}
	
	public bool isPlayerDead() {
		return isDead;
	}
	
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
	}
	
	void SetTitleLabelsVisible (bool areVisible){
		if (areVisible){
			titleLabels.transform.position = new Vector3(0.05f, 0.1f);
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
	
	void SetMusicPlaying(bool shouldPlay){
//		if (shouldPlay){
//			audio.Play();
//		}
//		else {
//			audio.Stop();
//		}
	}
}
