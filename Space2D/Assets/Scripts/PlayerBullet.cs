using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

	#region Public Properties
	public float xSpeed;
	public float ySpeed;
	#endregion
	
	#region Private Properties
	private bool isAlive = true;

	private float xMinimum = -1920f;
	private float xMaximum = 1920f;
	private float yMinimum = -1080f;
	private float yMaximum = 1080f;
	#endregion
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnTriggerEnter() {
		StartCoroutine(_Die());
	}

	IEnumerator _Die() {
		if (!isAlive) yield break;

		isAlive = false;

		yield return 0;

		Destroy(this.gameObject);
	}
}
