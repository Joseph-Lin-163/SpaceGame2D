using UnityEngine;
using System.Collections;

public class ShieldBehavior : MonoBehaviour {

	#region Private Properties
	private float ySpeed = -100f;
	private float yPos;
	#endregion

	// Use this for initialization
	void Start () {
		yPos = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		yPos += ySpeed * Time.deltaTime;
		this.transform.position = new Vector3(this.transform.position.x, yPos, this.transform.position.z);
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.transform.GetComponent<PlayerShip>() != null)
			StartCoroutine(_Die());
	}
	
	IEnumerator _Die() {
		
		yield return 0;
		Destroy(this.gameObject);
	}
}
