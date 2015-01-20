using UnityEngine;
using System.Collections;

public class UFO : MonoBehaviour {

	#region Public Properties
	public Transform BulletPrefab;
	public AudioClip BulletSound;
	public Transform Shield;
	public Transform ExplosionPrefab;
	public AudioClip ExplosionSound;
	
	public float TimeToFire;

	public int PointValue = 10;
	#endregion
	
	#region Private Properties
	private GameObject playerShip;

	private float timeToFireTimer;
	private bool isAlive = true;

	private GameManager gameManager;
	private bool shotByBullet = false;
	private int randNum;
	#endregion
	
	// Use this for initialization
	void Start () {
		randNum = Random.Range(0,100);
		FindPlayerShip();
		// Get a reference to the game manager in the scene
		GameObject go = GameObject.Find ("GameManager");
		if (go != null) gameManager = go.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// Fire a bullet
		timeToFireTimer -= Time.deltaTime;
		if (timeToFireTimer < 0f) {
			// Make correct direction for bullet
			// ox, oy are the UFO's coordinates, tx, ty are the playerShip's coordinates
			if (playerShip != null) {
				float ox, oy, tx, ty, txox, tyoy;
				ox = this.transform.position.x;
				oy = this.transform.position.y;
				tx = playerShip.transform.position.x;
				ty = playerShip.transform.position.y;

				if (tx - ox != 0)
					txox = tx - ox;
				else
					txox = 0.01f;

				if (ty - oy != 0)
					tyoy = ty - oy;
				else
					tyoy = 0.01f;

				float tempZ = 0f;
				if (ox <= tx && oy <= ty) {
					tempZ = (Mathf.Atan ((ty-oy)/(txox))*Mathf.Rad2Deg) + 90f;
				}
				else if (ox >= tx && oy <= ty) {
					tempZ = (Mathf.Atan ((ty-oy)/(txox))*Mathf.Rad2Deg) + 270f;
				}
				else if (ox >= tx && oy >= ty) {
					tempZ = 360f - (Mathf.Atan ((tx-ox)/(tyoy))*Mathf.Rad2Deg);
				}
				else if (ox <= tx && oy >= ty) {
					tempZ = -1 * (Mathf.Atan ((tx-ox)/(tyoy))*Mathf.Rad2Deg);
				}
				else {
					Debug.Log ("Error finding relative positions of playerShip and UFO");
				}

				//Debug.Log ("tempZ is: " + tempZ + ", ox is: " + ox + ", oy is: " + oy + ", tx is: " + tx + ", ty is: " + ty);			
				// Instantiate a bullet
				Quaternion temp = Quaternion.Euler(0,0, tempZ);
				Instantiate(BulletPrefab, this.transform.position, temp);
			}
			else {
				//Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);
				StartCoroutine(_Stall());
			}
			// Play the bullet sound
			AudioSource.PlayClipAtPoint(BulletSound, Vector3.zero);
			
			// Reset the timer
			timeToFireTimer += TimeToFire;
		}
	}

	IEnumerator _Stall() {
		yield return new WaitForSeconds(3.1f);
		FindPlayerShip();

		if (playerShip != null) {
			float ox, oy, tx, ty, txox, tyoy;
			ox = this.transform.position.x;
			oy = this.transform.position.y;
			tx = playerShip.transform.position.x;
			ty = playerShip.transform.position.y;
			
			if (tx - ox != 0)
				txox = tx - ox;
			else
				txox = 0.01f;
			
			if (ty - oy != 0)
				tyoy = ty - oy;
			else
				tyoy = 0.01f;
			
			float tempZ = 0f;
			if (ox <= tx && oy <= ty) {
				tempZ = (Mathf.Atan ((ty-oy)/(txox))*Mathf.Rad2Deg) + 90f;
			}
			else if (ox >= tx && oy <= ty) {
				tempZ = (Mathf.Atan ((ty-oy)/(txox))*Mathf.Rad2Deg) + 270f;
			}
			else if (ox >= tx && oy >= ty) {
				tempZ = 360f - (Mathf.Atan ((tx-ox)/(tyoy))*Mathf.Rad2Deg);
			}
			else if (ox <= tx && oy >= ty) {
				tempZ = -1 * (Mathf.Atan ((tx-ox)/(tyoy))*Mathf.Rad2Deg);
			}
			else {
				Debug.Log ("Error finding relative positions of playerShip and UFO");
			}
			
			//Debug.Log ("tempZ is: " + tempZ + ", ox is: " + ox + ", oy is: " + oy + ", tx is: " + tx + ", ty is: " + ty);			
			// Instantiate a bullet
			Quaternion temp = Quaternion.Euler(0,0, tempZ);
			Instantiate(BulletPrefab, this.transform.position, temp);
		}
		else {
			Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("I am UFO and I just collided with " + other.name);
		
		if (other.transform.GetComponent<PlayerShip>() != null)
			StartCoroutine(_Die ());
		else if (other.transform.GetComponent<PlayerBullet>() != null) {
			shotByBullet = true;
			StartCoroutine(_Die ());
		}
	}
	
	IEnumerator _Die() {
		if (!isAlive) yield break;
		
		isAlive = false;

		if (gameManager != null && shotByBullet == true)  {
			gameManager.AddScore(this.PointValue);
			gameManager.addNumUFOScore();
		}

		if ((randNum % 2) == 0 && shotByBullet == true) {
			Instantiate (Shield, this.transform.position, Quaternion.identity);
			Debug.Log ("Shield!");
		}

		// Play the explosion sound
		AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
		
		Instantiate(ExplosionPrefab, this.transform.position + new Vector3(0,0,-10f), Quaternion.identity);
		
		//this.transform.localScale = Vector3.zero;
		
		yield return new WaitForSeconds(0.2f);
		
		Destroy (this.gameObject);
	}

	void FindPlayerShip() {
		GameObject[] allObjects = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];

		for (int i = 0; i < allObjects.Length; i++) {
			if (allObjects[i].GetComponent<PlayerShip>() != null) {
				playerShip = allObjects[i];
				break;
			}
		}
	}

}
