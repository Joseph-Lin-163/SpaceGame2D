using UnityEngine;
using System.Collections;

public class UFO_Done : MonoBehaviour {

	#region Public Properties
	public Transform BulletPrefab;
	public AudioClip BulletSound;

	public Transform ExplosionPrefab;
	public AudioClip ExplosionSound;

	public float TimeToFire;
	#endregion

	#region Private Properties
	private float timeToFireTimer;
	private bool isAlive = true;
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// Fire a bullet
		timeToFireTimer -= Time.deltaTime;
		if (timeToFireTimer < 0f) {
			// Instantiate a bullet
			Instantiate(BulletPrefab, this.transform.position, Quaternion.identity);

			// Play the bullet sound
			AudioSource.PlayClipAtPoint(BulletSound, Vector3.zero);

			// Reset the timer
			timeToFireTimer += TimeToFire;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("I am UFO and I just collided with " + other.name);
		
		if (other.transform.GetComponent<PlayerShip>() != null || other.transform.GetComponent<PlayerBullet>())
			StartCoroutine(_Die ());
	}
	
	IEnumerator _Die() {
		if (!isAlive) yield break;
		
		isAlive = false;
		
		// Play the explosion sound
		AudioSource.PlayClipAtPoint(ExplosionSound, this.transform.position);
		
		Instantiate(ExplosionPrefab, this.transform.position + new Vector3(0,0,-10f), Quaternion.identity);
		
		this.transform.localScale = Vector3.zero;
		
		yield return new WaitForSeconds(0.2f);
		
		Destroy (this.gameObject);
	}
}
