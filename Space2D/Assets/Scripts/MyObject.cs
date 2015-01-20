using UnityEngine;
using System.Collections;

public class MyObject : MonoBehaviour {

	#region Public Properties
	public Animator Sprite;

	public bool DieWhenTapped = false;
	public bool DieAfterTime = true;

	public float TimeToLive = 5f;
	#endregion

	#region Private Properties
	private bool isAlive = true;
	#endregion

	// Use this for initialization
	void Start () {
		Debug.Log ("Hello, my name is " + this.gameObject.name + " and I am at " + this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		// update the timer by subtracting the amount of time since the last update
		// if the timer reaches zero, call the Die coroutine
		if (TimeToLive > 0f) {
			TimeToLive -= Time.deltaTime;

			if (TimeToLive <= 0f) {
				if (DieAfterTime) StartCoroutine(_Die());
			}
		}
	}

	void OnMouseDown()  {
		// The mouse was clicked over this objects box collider
		if (DieWhenTapped) StartCoroutine(_Die ());
	}

	// This is a coroutine.  Coroutines are methods that can be interrupted and halted for a bit then ran again
	// This coroutine changes the sprite animation to a death animation, and then destroys the object
	IEnumerator _Die() {
		// Only run this if the object is alive
		if (!isAlive) yield break;

		Debug.Log ("Hello, my name is " + this.gameObject.name + " and I am dying!");

		// Set a flag to make sure we don't run this coroutine twice
		isAlive = false;

		// Change to DEAD animation
		Sprite.Play("GrassBlock_Dead");

		// Wait for a half a second
		yield return new WaitForSeconds(0.5f);

		// Destroy myself
		Destroy (this.gameObject);
	}

	// Advanced: this is how to get a list of other objects with the same type.
	void FindOtherObjects() {
		MyObject[] allMyObjects = Object.FindObjectsOfType(typeof(MyObject)) as MyObject[];

		for (int i = 0; i < allMyObjects.Length; i++) {
			Debug.Log ("I found a MyObject named " + allMyObjects[i].name);
		}
	}
}
