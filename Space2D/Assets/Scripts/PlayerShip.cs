using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {

	#region Public Properties
	public float xSpeed;
	public float ySpeed;
	public float currentHealth = 1000;

	public Transform BulletPrefab;
	public AudioClip BulletSound;
	public Transform ExplosionPrefab;
	public AudioClip ExplosionSound;
	public Animator DamageSprite;
	public Animator ShieldSprite;
	public Transform healthBar;
	#endregion
	
	#region Private Properties
	private float maxHealth = 1000;
	private float CollisionDamageAmount = 50;		// How much health is subtracted if the player collides with something
	private float ShotDamageAmount = 100;
	private float ModerateDamage = 25;
	private float shieldTimer;
	private float FiringDelay = 0.2f;
	private float PostCollisionInvincibilityTime = 0.5f;

	private float xMinimum = -1920f;
	private float xMaximum = 1920f;
	private float yMinimum = -1080f;
	private float yMaximum = 1080f;

	private float firingDelayTimer = 0f;
	private float invincibilityTimer;

	private bool isAlive = true;

	private GameManager gameManager;
	#endregion
	
	// Use this for initialization
	void Start () {
		shieldTimer = 0f;
		// Get a reference to the game manager in the scene
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) gameManager = go.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		// Decrement the invincibility timer
		invincibilityTimer -= Time.deltaTime;

		if (shieldTimer > 0f) {
			shieldTimer -= Time.deltaTime;
			//Debug.Log(shieldTimer);
		}
		else {
			ShieldSprite.Play ("Shield_Off");
			shieldTimer = 0f;
		}

		// Check for keyboard input and move in the direction
		float newX = this.transform.position.x;
		float newY = this.transform.position.y;

		if (Input.GetKey(KeyCode.LeftArrow)) {
			newX -= Time.deltaTime * xSpeed;
		} 
		else if (Input.GetKey(KeyCode.RightArrow)) {
			newX += Time.deltaTime * xSpeed;
		} 

		if (Input.GetKey(KeyCode.UpArrow)) {
			newY += Time.deltaTime * ySpeed;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			newY -= Time.deltaTime * ySpeed;
		}

		// Make sure the newX and newY are in bounds
		if (newX < xMinimum) newX = xMinimum;
		if (newX > xMaximum) newX = xMaximum;
		if (newY < yMinimum) newY = yMinimum;
		if (newY > yMaximum) newY = yMaximum;

		// We move the ship by simply updating its position with the newX and newY
		this.transform.position = new Vector3(newX, newY, this.transform.position.z);

		// Handle firing.
		firingDelayTimer -= Time.deltaTime;
		if (Input.GetKey(KeyCode.Space)) {
			if (firingDelayTimer < 0f) {
				AudioSource.PlayClipAtPoint(BulletSound, this.transform.position);
				Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);
				firingDelayTimer = FiringDelay;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {

		if (other.transform.GetComponent<ShieldBehavior>() != null) {
			//Debug.Log ("shield on!");
			if (shieldTimer > 0f) {
				StartCoroutine(_ShieldWait());
			}
			else {
				//Debug.Log("Else stmt");
				ShieldSprite.Play("Shield_On");
				shieldTimer = 5f;
			}
		}

		if (shieldTimer > 0f)
			return;

		//Debug.Log ("I am the player ship and I just collided with " + other.name);

		if ((other.transform.GetComponent<Asteroid>() != null && other.name.ToString() == "AsteroidLarge(Clone)") || other.transform.GetComponent<UFO>()) 
			TakeCollisionDamage();		 
		
		if (other.transform.GetComponent<UFOBullet>() != null)
			TakeShotDamage();

		if (other.name.ToString() == "AsteroidMedium(Clone)")
			TakeModerateDamage();

		if (currentHealth <= 0) {
			// Notify the game manager that we died
			if (gameManager != null) {
				gameManager.PlayerDied();
				StartCoroutine(_Die());
			}
			
			if (gameManager.Lives == 0)
				StartCoroutine(_Die ());
		} 
	}

	// Health bar is 86 units halfway
	// 172 total
	// If I divide scale by half, 86
	// Move 21.5 To do half

	void TakeCollisionDamage() {
		// Take no damage if we are invincible
		if (invincibilityTimer > 0f) return;

		currentHealth -= this.CollisionDamageAmount;
		
		if (currentHealth <= 0) {
			// Notify the game manager that we died
			updateHealthBar();
			return;
		} 
		else {
			// Play sound and show damage animation
			AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
			DamageSprite.Play ("PlayerShipDamage_On");
			//DamageSprite.Play ("UFODamage_On");
			updateHealthBar();
			// Reset the invincibility timer
			invincibilityTimer = PostCollisionInvincibilityTime;
		}
	}

	void TakeShotDamage() {
		if (invincibilityTimer > 0f)
			return;

		currentHealth -= this.ShotDamageAmount;

		if (currentHealth <= 0) {
			updateHealthBar();
			return;
		}
		else {
			AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
			DamageSprite.Play ("PlayerShipDamage_On");
			//DamageSprite.Play ("UFODamage_On");
			updateHealthBar();
			invincibilityTimer = PostCollisionInvincibilityTime;
		}
	}

	void TakeModerateDamage() {
		if (invincibilityTimer > 0f)
			return;

		currentHealth -= this.ModerateDamage;

		if (currentHealth <= 0) {
			updateHealthBar();
			return;
		}
		else {
			AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
			DamageSprite.Play ("PlayerShipDamage_On");
			//DamageSprite.Play ("UFODamage_On");
			updateHealthBar();
			invincibilityTimer = PostCollisionInvincibilityTime;
		}
	}

	void updateHealthBar() {
		if (currentHealth < 0)
			currentHealth = 0;

		float scale = 2 * (currentHealth/maxHealth);
		float size = 86 * (currentHealth/maxHealth);
		float xPos = 43 - (size/2);

		healthBar.transform.localScale = new Vector3(scale,1f,1f);
		healthBar.transform.position = new Vector3(this.transform.position.x - xPos, this.transform.position.y - 50f, 0f);
	}

	IEnumerator _ShieldWait() {
		ShieldSprite.Play("Shield_Off");
		//Debug.Log ("Attempting to turn shield off.");
		yield return new WaitForSeconds(0.017f);
		ShieldSprite.Play("Shield_On");
		shieldTimer = 5f;
	}

	IEnumerator _Die() {
		if (!isAlive) yield break;

		isAlive = false;

		// Play the explosion sound
		AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);

		Instantiate(ExplosionPrefab, this.transform.position + new Vector3(0,0,-10f), Quaternion.identity);

		//this.transform.localScale = Vector3.zero;

		yield return new WaitForSeconds(0.2f);

		Destroy (this.gameObject);
	}
}
