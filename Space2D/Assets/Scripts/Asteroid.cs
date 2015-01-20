using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	#region Public Properties
	public Transform ExplosionPrefab;
	public AudioClip ExplosionSound;
	public int PointValue = 10;
	public Transform SmallerAsteroidPrefab;
	public int NumberOfAsteroidsToSpawnOnDie;
	#endregion
	
	#region Private Properties
	private bool isAlive = true;
	private GameManager gameManager;
	private float invincibilityTimer = 0.5f;
	#endregion

	// Use this for initialization
	void Start () {
		// Get a reference to the game manager in the scene
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) 
			gameManager = go.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		this.invincibilityTimer -= Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (invincibilityTimer > 0f) 
			return;

		if (other.transform.GetComponent<Asteroid>() != null || other.transform.GetComponent<PlayerShip>() != null) {
			StartCoroutine(_Die ());
		} 
		else if (other.transform.GetComponent<PlayerBullet>() != null) {
			if (gameManager != null) 
				gameManager.AddScore(this.PointValue);
			StartCoroutine(_Die ());
		}
	}

	IEnumerator _Die() {
		if (!isAlive) yield break;
		
		isAlive = false;
		
		// Play the explosion sound
		AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
		
		Instantiate(ExplosionPrefab, this.transform.position + new Vector3(0,0,-10f), Quaternion.identity);
		
		// Instantiate any smaller asteroids
		if (SmallerAsteroidPrefab != null) {
			for (int i = 0; i < NumberOfAsteroidsToSpawnOnDie; i++) {
				Instantiate(SmallerAsteroidPrefab, this.transform.position, Quaternion.identity);
			}
		}

		yield return new WaitForSeconds(0.2f);
		Destroy (this.gameObject);
	}
}
