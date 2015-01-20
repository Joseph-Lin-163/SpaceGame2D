using UnityEngine;
using System.Collections;

public class UFOBullet : MonoBehaviour {

	#region Public Properties

	public float maxSpeed;

	#endregion
	
	#region Private Properties
	private float xMinimum = -1920f;
	private float xMaximum = 1920f;
	private float yMinimum = -1080f;
	private float yMaximum = 1080f;
	private float xSpeed;
	private float ySpeed;
	private GameObject playerShip;
	private bool isAlive = true;
	private float ox, oy, tx, ty, alpha;
	#endregion
	
	// Use this for initialization
	void Start () {
		FindPlayerShip();

		// Find appropriate x and y Speeds
		// ox, oy are UFO's coordinates, tx, ty are playerShip's coordinates
		if (playerShip != null) {
			ox = this.transform.position.x;
			oy = this.transform.position.y;
			tx = playerShip.transform.position.x;
			ty = playerShip.transform.position.y;

			if (tx - ox != 0)
				alpha = Mathf.Abs((ty-oy)/(tx-ox))*Mathf.Abs((ty-oy)/(tx-ox));
			else
				alpha = 0.01f;

			if (ox <= tx && oy <= ty) {
				xSpeed = Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));
			}
			else if (ox >= tx && oy <= ty) {
				xSpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else if (ox >= tx && oy >= ty) {
				xSpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else if (ox <= tx && oy >= ty) {
				xSpeed = Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else {
				Debug.Log ("Error finding relative positions of playerShip and UFO");
			}
		}
		else {
			xSpeed = 0;
			ySpeed = 1000f;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (playerShip == null) {
			StartCoroutine(_Stall());
		}


		// Move the object
		float newX = this.transform.position.x;
		float newY = this.transform.position.y;
		
		newX += Time.deltaTime * xSpeed;
		newY += Time.deltaTime * ySpeed;
		
		// Make sure the newX and newY are in bounds -- if they aren't, Die
		if (newX < xMinimum) StartCoroutine(_Die ());
		if (newX > xMaximum) StartCoroutine(_Die ());
		if (newY < yMinimum) StartCoroutine(_Die ());
		if (newY > yMaximum) StartCoroutine(_Die ());
		
		// We move the object by simply updating its position with the newX and newY
		this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerShip>() != null || other.GetComponent<Asteroid>() != null || other.GetComponent<PlayerBullet>() != null)
			StartCoroutine(_Die());
	}

	IEnumerator _Stall() {

		yield return new WaitForSeconds(10f);

		FindPlayerShip();

		if (playerShip != null) {
			ox = this.transform.position.x;
			oy = this.transform.position.y;
			tx = playerShip.transform.position.x;
			ty = playerShip.transform.position.y;
			
			if (tx - ox != 0)
				alpha = Mathf.Abs((ty-oy)/(tx-ox));
			else
				alpha = 0.01f;
			
			if (ox <= tx && oy <= ty) {
				xSpeed = Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));
			}
			else if (ox >= tx && oy <= ty) {
				xSpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else if (ox >= tx && oy >= ty) {
				xSpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else if (ox <= tx && oy >= ty) {
				xSpeed = Mathf.Sqrt((maxSpeed*maxSpeed)/(1+alpha));
				ySpeed = -1 * Mathf.Sqrt((maxSpeed*maxSpeed)-(xSpeed*xSpeed));		
			}
			else {
				Debug.Log ("Error finding relative positions of playerShip and UFO");
			}
		}
		else {
			xSpeed = 0;
			ySpeed = 1000f;
		}
	}

	IEnumerator _Die() {
		if (!isAlive) yield break;
		
		isAlive = false;
		
		yield return 0;
		
		Destroy(this.gameObject);
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
