using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	#region Public Properties
	public Restart RestartObject;
	public int Score;
	public int Lives = 3;
	public Transform PlayerShipPrefab;
	public Transform ScoreBoardPrefab;
	public GUIText scoreText;
	public GUIText highScoreText;
	public bool transition = false;

	public bool checkForRespawn;

	#endregion
	
	#region Private Properties
	private int numUFOsDestroyed = 0;
	private int highScore;
	private float LOLDONTDIE;
	#endregion
	
	// Use this for initialization
	void Start () {
		Score = 0;
		checkForRespawn = false;
		LOLDONTDIE = 0f;
		scoreText.text = "INSERT";
		highScoreText.text = "COIN(S)"; //"High Score: " + highScore;
	}
	
	// Update is called once per frame
	void Update () {
		if (LOLDONTDIE > 0f)
			LOLDONTDIE -= Time.deltaTime;
		else
			LOLDONTDIE = 0f;

		if (Application.loadedLevelName == "Week6") {
			if (transition == true) {
				StartWeek6();
			}

			scoreText.text = "Score: " + Score;
			if (highScore < Score) {
				highScore = Score;
				highScoreText.text = "High Score: " + highScore;
			}
		}

		if (checkForRespawn == true) {
			if (Input.GetKey(KeyCode.R) == true) {
				Score = 0;
				GameObject go = GameObject.Find ("Restart");
				if (go != null) 
					RestartObject = go.GetComponent<Restart>();
				RestartObject.SetRestart(true);
			}
		}
	}



	// Other objects can call this method to increase their score
	public void AddScore(int score) {
		this.Score += score;
	}

	public void addNumUFOScore () {
		numUFOsDestroyed++;
	}

	// The playerShip calls this method to decrease the number of lives
	public void PlayerDied() {
		if (LOLDONTDIE > 0f)
			return;
		else
			LOLDONTDIE += 2f;

		Lives--;
		// If we are out of lives, reload the scene
		// This is how we load the current scene.  If we want a different scene, we load it by name or number
		if (Lives < 1) {
			//Application.LoadLevel(Application.loadedLevel);
			checkForRespawn = true;
		}
		else {
			// Spawn a new ship
			StartCoroutine(_SpawnNewPlayerShip(3f));
		}
	}

	public void StartWeek6() {
		if (Application.loadedLevelName == "Week6") {
			//StartCoroutine(_SpawnScoreBoard(0f));
			StartCoroutine(_SpawnNewPlayerShip(0f));
			highScore = 0;
			highScoreText.text = "High Score: " + highScore;
			scoreText.text = "Score: " + Score;
			transition = false;
		}
	}

	IEnumerator _SpawnScoreBoard(float delay) {
		if (delay > 0f)
			yield return new WaitForSeconds(delay);
		Instantiate(ScoreBoardPrefab);
	}

	IEnumerator _SpawnNewPlayerShip(float delay) {
		if (delay > 0f)
			yield return new WaitForSeconds(delay);
		Instantiate(PlayerShipPrefab);
	}
}
